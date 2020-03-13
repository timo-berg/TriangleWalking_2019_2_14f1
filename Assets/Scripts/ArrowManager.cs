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
    bool selecting;
    

    // Start is called before the first frame update
    void Start()
    {
        distance = 2f;
        egoArrowMesh = egoArrow.GetComponent<MeshRenderer>();
        alloArrowMesh = alloArrow.GetComponent<MeshRenderer>();
    }

    void Update() {
        if (selecting) {
            UnityEngine.Vector3 playerGaze = PlayerMovement.Instance.getPlayerGaze();
            RaycastHit hit;

            if (Physics.Raycast(transform.position, PlayerMovement.Instance.getPlayerGaze(), out hit , Mathf.Infinity)) {
                if (hit.transform.gameObject == egoArrow) {
                    egoArrowMesh.material.color = new Color(2, 0, 0);
                } else if (hit.transform.gameObject == alloArrow) {
                    alloArrowMesh.material.color = new Color(2, 0, 0);
                } else {
                    egoArrowMesh.material.color = new Color(1, 1, 1);
                    alloArrowMesh.material.color = new Color(1, 1, 1);
                }
            }
                
        }
        
    }

    public IEnumerator homingVectorTask (UnityEngine.Vector3 homePoint, UnityEngine.Vector3 firstWaypoint) {
        //Calculate homing vectors
        UnityEngine.Vector3 playerPosition = PlayerMovement.Instance.getPlayerPosition();
        
        UnityEngine.Vector3 alloPosition = playerPosition + (firstWaypoint - homePoint).normalized * distance;
        UnityEngine.Vector3 alloDirection = alloPosition - homePoint;
        
        UnityEngine.Vector3 egoPosition = playerPosition + (playerPosition - firstWaypoint).normalized * distance;
        UnityEngine.Vector3 egoDirection = egoPosition - homePoint;

        Debug.DrawLine(homePoint, alloDirection, Color.red, 20f);
        Debug.DrawLine(homePoint, egoDirection, Color.green, 20f);

        //Move arrows
        alloArrow.transform.position = alloPosition;
        alloArrow.transform.rotation.SetLookRotation(alloDirection, UnityEngine.Vector3.left);

        egoArrow.transform.position = egoPosition;
        egoArrow.transform.rotation.SetLookRotation(egoDirection, UnityEngine.Vector3.left);
        
        //Show arrows
        alloArrowMesh.enabled = true;
        egoArrowMesh.enabled = true;

        //Select arrow
        selecting = true;
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());

        alloArrowMesh.enabled = false;
        egoArrowMesh.enabled = false;
    }
}
