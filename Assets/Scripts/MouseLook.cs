using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    bool isCameraLocked = false;
    float x;
    float scaleFactor;
    float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // if (isCameraLocked == false) {
        //     float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity + Time.deltaTime;
        //     float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity + Time.deltaTime;

        //     if (Mathf.Abs(mouseY) > 0.1) {
        //         xRotation -= mouseY;
        //         xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //         transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //     }
            
        //     if (Mathf.Abs(mouseX) > 0.1) {
        //         playerBody.Rotate(Vector3.up * mouseX);
        //     }
            
        // }

        if (isCameraLocked == false) {    
            x = Input.GetAxis("Horizontal");

            if (SphereMovement.Instance.isSphereRotating()) {
                scaleFactor = SphereMovement.Instance.rotationScalingFactor;
            } else {
                scaleFactor = 1.5f;
            }

            playerBody.Rotate(Vector3.up * x * rotationSpeed * scaleFactor * Time.deltaTime);  // 
        }


    }

    public void lockCamera() {
        isCameraLocked = true;
    }
    
    public void unlockCamera() {
        isCameraLocked = false;
    }
}
