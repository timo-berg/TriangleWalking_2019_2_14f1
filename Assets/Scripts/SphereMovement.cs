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
    public float angularSpeed { get; private set; }

    //Translation variables
    bool isTranslating = false;
    float remainingDistance = 0f;
    float totalDistance;
    public float translationSpeed { get; private set; }
    Vector3 translationDirection;

    //Pushing variables
    Vector3 pushAxis;
    bool isPushing = false;
    float sphereHeight = 1.5f;

    protected override void Awake() {
        base.Awake();
        angularSpeed = 15f;
        translationSpeed = 0.3f;
    }
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

        if (isPushing) {
            pushSphere();
        }
        

    }

    public void setRotation(float angle, bool rotateRight) {
        //Public function to let other scripts rotate the sphere
        //Sets all necessary factors to enable the rotation

        //angle:        angle that is to be rotated
        //rotateRight:  true for clockwise rotation, false for counterclockwise rotation

        string direction;
        isRotating = true;
        rightRotation = rotateRight;
        remainingAngle = Mathf.Abs(angle);
        totalAngle = Mathf.Abs(angle);
        if (!rightRotation) {
            angularSpeed = -1 * Mathf.Abs(angularSpeed);
            direction = "left";
        } else {
            angularSpeed = Mathf.Abs(angularSpeed);
            direction = "right";
        }
        
        ExperimentManager.Instance.LogMarker(string.Format("event:rotationStart;angle:{0}:direction:{1}", angle,direction));
    }

    public void setTranslation(float distance) {
        //Public function to let other scripts translate the sphere
        //Sets all necessary factors to enable the translation

        //dsitance:     distance that is to be translated

        isTranslating = true;
        remainingDistance = distance;
        totalDistance = distance;
        translationDirection = getSpherePosition() - PlayerMovement.Instance.getPlayerPosition();

        ExperimentManager.Instance.LogMarker(string.Format("event:translationStart;distance:{0}", distance));
    }

    void translateSphere() {
        //Translates the sphere forward as long as there is remaining distance left

        //Stop the translation if there is no remaining distance
        if (remainingDistance <= 0.1f) {
            isTranslating = false;
            ExperimentManager.Instance.LogMarker("event:translationStop");
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
        if (remainingAngle <= 0.1f) {
            isRotating = false;
            ExperimentManager.Instance.LogMarker("event:rotationStop");
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

    public void enablePushSphere(Vector3 axis) {
        pushAxis = axis;
        isPushing = true;
        transform.position = PlayerMovement.Instance.getPlayerPosition() + axis + new Vector3(0, sphereHeight, 0);

        ExperimentManager.Instance.LogMarker(string.Format("event:spherePushStart;axis:{0}",axis));
    }

    void pushSphere() {
        if (ExperimentManager.Instance.distancePlayerSphere() < 2f) {
            float pushAmount = 2f - ExperimentManager.Instance.distancePlayerSphere();
            transform.Translate(pushAxis.normalized * pushAmount, Space.World);
        }
        
    }

    public void disablePushSphere() {
        isPushing = false;
        ExperimentManager.Instance.LogMarker("event:spherePushStop");
    }



}
