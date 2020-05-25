using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : Singleton<ArrowManager>
{
    
    public GameObject egoArrow;
    MeshRenderer egoArrowMesh;
    public GameObject alloArrow;
    MeshRenderer alloArrowMesh;

    

    float distance;
    bool isSelecting;
    bool hasSelected;
    bool alloIsActive;
    float direction;

    // Start is called before the first frame update
    void Start()
    {
        distance = 2f;
        egoArrowMesh = egoArrow.GetComponent<MeshRenderer>();
        alloArrowMesh = alloArrow.GetComponent<MeshRenderer>();
    }

    void Update() {
        if (isSelecting) {
            if (ExperimentManager.Instance.isVR) {
                //Get the point between the arrows
                UnityEngine.Vector3 midpoint = UnityEngine.Vector3.Lerp(egoArrow.transform.position,
                    alloArrow.transform.position, 0.5f);
                midpoint -= new UnityEngine.Vector3(0f, midpoint.y, 0f);

                UnityEngine.Vector3 midvector = midpoint - PlayerMovement.Instance.getPlayerPosition();

                UnityEngine.Vector3 gaze = PlayerMovement.Instance.getPointingGaze();

                if (UnityEngine.Vector3.SignedAngle(midvector, gaze, UnityEngine.Vector3.up) > 0)
                {
                    egoArrowMesh.material.color = new Color(2, 0, 0);
                    alloArrowMesh.material.color = new Color(1, 1, 1);
                    alloIsActive = false;
                    hasSelected = true;
                }
                else
                {
                    alloArrowMesh.material.color = new Color(2, 0, 0);
                    egoArrowMesh.material.color = new Color(1, 1, 1);
                    alloIsActive = true;
                    hasSelected = true;
                }
            }
            else {
                //Check keypress and change color according to the current gaze direction
                UnityEngine.Vector3 perp = UnityEngine.Vector3.Cross(PlayerMovement.Instance.getPlayerGaze(), alloArrow.transform.position - PlayerMovement.Instance.getPlayerPosition());
                direction = UnityEngine.Vector3.Dot(perp, UnityEngine.Vector3.up);

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (direction > 0) {
                        alloArrowMesh.material.color = new Color(2, 0, 0);
                        egoArrowMesh.material.color = new Color(1, 1, 1);
                        alloIsActive = true;
                        hasSelected = true;
                    } else {
                        alloArrowMesh.material.color = new Color(1, 1, 1);
                        egoArrowMesh.material.color = new Color(2, 0, 0);
                        alloIsActive = false;
                        hasSelected = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (direction > 0) {
                        alloArrowMesh.material.color = new Color(1, 1, 1);
                        egoArrowMesh.material.color = new Color(2, 0, 0);
                        alloIsActive = false;
                        hasSelected = true;
                    } else {
                        alloArrowMesh.material.color = new Color(2, 0, 0);
                        egoArrowMesh.material.color = new Color(1, 1, 1);
                        alloIsActive = true;
                        hasSelected = true;
                    }
                }
            }
        }
        
    }

    public IEnumerator homingVectorTask (UnityEngine.Vector3 playerGaze, UnityEngine.Vector3 homePoint) {
        //Calculate homing vectors
        UnityEngine.Vector3 playerPosition = PlayerMovement.Instance.getPlayerPosition();
        UnityEngine.Vector3 perpVector = UnityEngine.Vector3.Cross(playerGaze, UnityEngine.Vector3.up);

        UnityEngine.Vector3 alloPosition = playerPosition + playerGaze*2f + perpVector/2f;
        UnityEngine.Vector3 egoPosition = playerPosition + playerGaze*2f - perpVector/2f;

        UnityEngine.Vector3 alloDirection = alloPosition - homePoint;
        UnityEngine.Vector3 egoDirection = egoPosition - homePoint;

        //Move arrows
        alloArrow.transform.position = alloPosition + new UnityEngine.Vector3(0f, 1.5f, 0f);
        alloArrow.transform.rotation = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.down, alloDirection);

        egoArrow.transform.position = egoPosition + new UnityEngine.Vector3(0f, 1.5f, 0f);
        egoArrow.transform.rotation = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.down, egoDirection);

        //Show arrows
        alloArrowMesh.enabled = true;
        egoArrowMesh.enabled = true;

        //Select arrow
        isSelecting = true;

        //Wait until the player has selected an arrow at least once
        hasSelected = false;
        yield return new WaitUntil(() => hasSelected);

        //Wait for confirmation of currently selected arrow
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());

        yield return new WaitForSeconds(1f);

        //Log the result
        string choice;
        if (alloIsActive) {
            choice = "allocentric";
        } else {
            choice = "egocentric";
        }
        ExperimentManager.Instance.LogMarker("event:homingArrowChosen;homingArrow:" + choice);


        alloArrowMesh.enabled = false;
        egoArrowMesh.enabled = false;

        alloArrowMesh.material.color = new Color(1, 1, 1);
        egoArrowMesh.material.color = new Color(1, 1, 1);
    }


}