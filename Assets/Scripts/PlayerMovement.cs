using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{

    public CharacterController controller;
    public float speed = 12f;
    public bool isMovementLocked = false;
    
    void Update()
    {

        if (isMovementLocked == false) {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown("left")) {
            SphereMovement.Instance.setRotation(45f, false);
        }

        if (Input.GetKeyDown("right")) {
            SphereMovement.Instance.setRotation(45f, true);
        }

        if (Input.GetKeyDown("up")) {
            SphereMovement.Instance.setTranslation(4f);
        }
       


    }

    public void lockMovement() {
        isMovementLocked = true;
    }
    
    public void unlockMovement() {
        isMovementLocked = false;
    }

    public Vector3 getPlayerPosition() {
        Vector3 position = transform.position;
        position.y = 0f;
        return position;
    }

}
