using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : Singleton<SphereMovement>
{
    //Rotation variables
    bool isRotating = false;
    float remainingAngle = 0f;
    float totalAngle;
    bool rightRotation = false;
    float angularSpeed = 10f;

    //Translation variables
    bool isTranslating = false;
    float remainingDistance = 0f;
    float totalDistance;
    float translationSpeed = 2f;
    Vector3 translationDirection;

        
    void Update()
    {
        //Call rotation and translation in every update.
        //If the sphere actually translates or rotates depends on the
        //variables isTranslating and isRotating
        if (isRotating) {
            rotateSphere();
        }

        if (isTranslating) {
            translateSphere();
        }
        

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
        if (!rightRotation) {
            angularSpeed = -1 * angularSpeed;
        } else {
            angularSpeed = Mathf.Abs(angularSpeed);
        }
    }

    public void setTranslation(float distance) {
        //Public function to let other scripts translate the sphere
        //Sets all necessary factors to enable the translation

        //dsitance:     distance that is to be translated

        isTranslating = true;
        remainingDistance = distance;
        totalDistance = distance;
        translationDirection = getSpherePosition() - PlayerMovement.Instance.getPlayerPosition();
    }

    void translateSphere() {
        //Translates the sphere forward as long as there is remaining distance left

        //Stop the translation if there is no remaining distance
        if (remainingDistance <= 0.1f) {
            isTranslating = false;
        }

        //Get sine scaling factor
        float scalingFactor = MathHelper.sineScaleFactor(remainingDistance, totalDistance);
        //Debug.Log(scalingFactor);
        //Translation vector with scaled length
        Vector3 translationVector = translationDirection.normalized * (Time.deltaTime * scalingFactor *  translationSpeed);
        //Move sphere forward
        transform.Translate(translationVector, Space.World);
        //Decrement by the travelled distance
        remainingDistance -= Time.deltaTime * translationSpeed * scalingFactor;

        
    }

    void rotateSphere(){
        //Rotates the sphere at a fixed distance around the player as long as there is a remaining angle left

        //Stop the rotation if there is no remaining angle
        if (Mathf.Abs(remainingAngle) <= 0.1f) {
            isRotating = false;
        }
        //Get sine scaling factor
        float scalingFactor = MathHelper.sineScaleFactor(remainingAngle, totalAngle);
        //Rotation increment
        float roationIncrement = angularSpeed * Time.deltaTime * scalingFactor;
        // Spin the object around the Player.
        transform.RotateAround(PlayerMovement.Instance.getPlayerPosition(), Vector3.up, roationIncrement);  
        
        //Decrement by the travelled rotation
        remainingAngle -= Mathf.Abs(roationIncrement);

        
    }
    public bool isSphereRotating() {
        return isRotating;
    }
    public bool isSphereTranslating() {
        return isTranslating;
    }

    public Vector3 getSpherePosition() {
        Vector3 position = transform.position;
        position.y = 0f;
        return position;
    }



}
