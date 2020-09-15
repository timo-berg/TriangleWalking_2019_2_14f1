using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{

    public CharacterController movementController;
    public GameObject pointingController;
    public GameObject VRCamera;
    public float translationSpeed;
    public bool isMovementLocked = false;
    float z;
    float scaleFactor;
    Vector3 move;

    void Start() {
        translationSpeed = ConfigValues.translationSpeedSlow;
    }

    void Update()
    {
        if (isMovementLocked == false) {           
            z = Input.GetAxis("Vertical");
            move = Vector3.Scale(transform.forward, new Vector3(1,0,1)) * z;

            if (SphereMovement.Instance.isSphereTranslating()) {
                scaleFactor = SphereMovement.Instance.translationScalingFactor;
            } else {
                scaleFactor = Mathf.Clamp(scaleFactor * 1.01f, 1f, 3f);
            }

            move = move * translationSpeed * scaleFactor * Time.deltaTime;

            movementController.Move(move);
        }
    


    }

    public void lockMovement() {
        isMovementLocked = true;
    }
    
    public void unlockMovement() {
        isMovementLocked = false;
    }

    public Vector3 getPlayerPosition() {
        if (ExperimentManager.Instance.isVR) {
            Vector3 VRposition = VRCamera.transform.position;
            VRposition.y = 0f; 
            return VRposition;
        } else {
            Vector3 position = transform.position;
            position.y = 0f;
            return position;
        }

    }

    public Vector3 getPlayerGaze() {
        if (ExperimentManager.Instance.isVR) {
            return  VRCamera.transform.TransformDirection(UnityEngine.Vector3.forward);;
        } else {
            return transform.TransformDirection(UnityEngine.Vector3.forward);
        }
    }

    public Vector3 getPointingGaze()
    {
        return pointingController.transform.TransformDirection(UnityEngine.Vector3.forward);
    }

}
