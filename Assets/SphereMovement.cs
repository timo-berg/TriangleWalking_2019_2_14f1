using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : Singleton<SphereMovement>
{
    //Rotation variables
    bool isRotating = true;
    float remainingAngle = 0f;
    bool rightRotation = false;
    int angleIncrement;
    //Translation variables
    bool isTranslating = true;
    float remainingDistance = 0f;
    int distanceIncrement;
    Vector3 translationDirection;

        
    void Update()
    {
        rotateSphere();
        translateSphere();
    }

    public void setRotation(float angle, bool rotateRight) {
        isRotating = true;
        rightRotation = rotateRight;
        remainingAngle = angle;
    }

    public void setTranslation(float distance) {
        isTranslating = true;
        remainingDistance = distance;
        translationDirection = transform.position - PlayerMovement.Instance.getPlayerPosition();
    }

    void translateSphere() {
        //Stop the translation if there is no remaining distance
        if (remainingDistance <= 0.1f) {
            isTranslating = false;
        }

        if (isTranslating) {           
            //Move sphere forward
            transform.Translate(translationDirection.normalized * Time.deltaTime);
            //Decrement by the travelled distance
            remainingDistance -= Time.deltaTime * 1;
        }
    }

    void rotateSphere(){
        //Stop the rotation if there is no remaining angle
        if (remainingAngle <= 0.1f && rightRotation == true) {
            isRotating = false;
        }
        if (remainingAngle <= 0.1f && rightRotation == false) {
            isRotating = false;
        }
           
        if (isRotating) {
            //Set the rotation direction
            if (rightRotation) {
                angleIncrement = 30;
            } else {
                angleIncrement = -30;
            }

            // Spin the object around the Player.
            transform.RotateAround(PlayerMovement.Instance.getPlayerPosition(), Vector3.up, angleIncrement * Time.deltaTime);  //)
            
            //Decrement by the travelled rotation
            if (rightRotation) {
                remainingAngle -= Time.deltaTime * angleIncrement;
            } else {
                remainingAngle += Time.deltaTime * angleIncrement;
            }
        }
    }
    public bool isSphereRotating() {
        return isRotating;
    }
    public bool isSphereTranslating() {
        return isTranslating;
    }

    public Vector3 getSpherePosition() {
        return transform.position;
    }

}
