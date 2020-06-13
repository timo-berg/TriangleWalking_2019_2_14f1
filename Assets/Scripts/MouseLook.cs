using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    bool isCameraLocked = false;
    float x;
    float scaleFactor;
    float rotationSpeed;
    public bool cameraTilt;
    float y;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotationSpeed = ConfigValues.angularSpeed;
        cameraTilt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTilt == true) {
            y = Input.GetAxis("VerticalLook");

            scaleFactor = Mathf.Clamp(scaleFactor * 1.01f, 1f, 3f);

            yRotation -= y * rotationSpeed * scaleFactor * Time.deltaTime;
            yRotation = Mathf.Clamp(yRotation, 0f, 45f);

            transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        }

        if (isCameraLocked == false) {    
            x = Input.GetAxis("Horizontal");

            if (SphereMovement.Instance.isSphereRotating()) {
                scaleFactor = SphereMovement.Instance.rotationScalingFactor;
            } else {
               //Short accelertation period after switch from sphere following rotation
                scaleFactor = Mathf.Clamp(scaleFactor*1.01f, 1f, 3f);
            }

            playerBody.Rotate(Vector3.up * x * rotationSpeed * scaleFactor * Time.deltaTime, Space.World);  // 
        }


    }

    public void lockCamera() {
        isCameraLocked = true;
    }
    
    public void unlockCamera() {
        isCameraLocked = false;
    }
}
