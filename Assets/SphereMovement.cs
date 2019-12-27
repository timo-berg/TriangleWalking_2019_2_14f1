using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : Singleton<SphereMovement>
{
    //Rotation variables
    bool isRotating = true;
    float remainingAngle = 0f;
    float totalAngle;
    bool rightRotation = false;
    float angularSpeed = 10f;

    //Translation variables
    bool isTranslating = true;
    float remainingDistance = 0f;
    float totalDistance;
    float translationSpeed = 4f;
    Vector3 translationDirection;

        
    void Update()
    {
        //Call rotation and translation in every update.
        //If the sphere actually translates or rotates depends on the
        //variables isTranslating and isRotating
        rotateSphere();
        translateSphere();

    }

    public void setRotation(float angle, bool rotateRight) {
        //Public function to let other scripts rotate the sphere
        //Sets all necessary factors to enable the rotation

        //angle:        angle that is to be rotated
        //rotateRight:  true for clockwise rotation, false for counterclockwise rotation

        isRotating = true;
        rightRotation = rotateRight;
        remainingAngle = angle;
        totalAngle = angle;
    }

    public void setTranslation(float distance) {
        //Public function to let other scripts translate the sphere
        //Sets all necessary factors to enable the translation

        //dsitance:     distance that is to be translated

        isTranslating = true;
        remainingDistance = distance;
        totalDistance = distance;
        translationDirection = transform.position - PlayerMovement.Instance.getPlayerPosition();
    }

    void translateSphere() {
        //Translates the sphere forward as long as there is remaining distance left

        //Stop the translation if there is no remaining distance
        if (remainingDistance <= 0.1f) {
            isTranslating = false;
        }

        if (isTranslating) {
            //Get sine scaling factor
            float scalingFactor = sineScaleFactor(remainingDistance, totalDistance);
            //Translation vector with scaled length
            Vector3 translationVector = translationDirection.normalized * Time.deltaTime * scalingFactor * translationSpeed;
            //Move sphere forward
            transform.Translate(translationVector);
            //Decrement by the travelled distance
            remainingDistance -= Time.deltaTime * translationSpeed;
        }
        
    }

    void rotateSphere(){
        //Rotates the sphere at a fixed distance around the player as long as there is a remaining angle left

        //Stop the rotation if there is no remaining angle
        if (remainingAngle <= 0.1f) {
            isRotating = false;
        }
           
        if (isRotating) {
            //Set the rotation direction
            if (!rightRotation) {
                angularSpeed = -1 * angularSpeed;
            }
            //Get sine scaling factor
            float scalingFactor = sineScaleFactor(remainingAngle, totalAngle);
            //Rotation increment
            float roationIncrement = angularSpeed * Time.deltaTime * scalingFactor;
            // Spin the object around the Player.
            transform.RotateAround(PlayerMovement.Instance.getPlayerPosition(), Vector3.up, roationIncrement);  
            
            //Decrement by the travelled rotation
            if (rightRotation) {
                remainingAngle -= Time.deltaTime * angularSpeed;
            } else {
                remainingAngle += Time.deltaTime * angularSpeed;
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

    float sineScaleFactor(float currentValue, float totalValue) {
        //Returns the value of a sine curve between 1.5pi (minimum) and 3.5pi (next minimum)
        //The factor is then scaled to be ranging from 0 to 2
        //Returns 0 at the start and the end and 2 in the middle
        float scaleFactor = Mathf.Sin(1.5f*Mathf.PI + 2*Mathf.PI * currentValue/totalValue) + 1;
        return scaleFactor;
    }

}
