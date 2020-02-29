using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : Singleton<SphereMovement>
{
    //Rotation variables
    bool isRotating = false;
    float remainingAngle = 0f;
    float totalAngle;
    public float angularSpeed { get; private set; }
    Vector3 vortex;

    //Translation variables
    bool isTranslating = false;
    float remainingDistance = 0f;
    float totalDistance;
    public float translationSpeed { get; private set; }
    Vector3 translationDirection;

    
    
    

    //Other
    MeshRenderer meshRenderer;
    public bool isBaseline = false;

    protected override void Awake() {
        base.Awake();
        angularSpeed = 10f;
        translationSpeed = 0.3f; //0.3f;
    }

    void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
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

    }

    public void setRotation(float angle, Vector3 rotationVortex) {
        //Public function to let other scripts rotate the sphere
        //Sets all necessary factors to enable the rotation

        //angle:        angle that is to be rotated
        //rotateRight:  true for clockwise rotation, false for counterclockwise rotation

        vortex = rotationVortex;
        string direction;
        isRotating = true;
        remainingAngle = Mathf.Abs(angle);
        totalAngle = Mathf.Abs(angle);
        if (angle < 0) {
            angularSpeed = -1 * Mathf.Abs(angularSpeed);
            direction = "left";
        } else {
            angularSpeed = Mathf.Abs(angularSpeed);
            direction = "right";
        }
        
        ExperimentManager.Instance.LogMarker(string.Format("event:rotationStart;angle:{0}:direction:{1}", angle,direction));
    }

    public void setTranslation(Vector3 targetPosition) {
        //Public function to let other scripts translate the sphere
        //Sets all necessary factors to enable the translation

        Vector3 translationVector = targetPosition - transform.position;
        translationVector -= new Vector3(0f, translationVector.y, 0f);
        isTranslating = true;
        totalDistance = remainingDistance = translationVector.magnitude + ConfigValues.nearDistance;
        translationDirection = translationVector;

        ExperimentManager.Instance.LogMarker(string.Format("event:translationStart;distance:{0}", translationVector.magnitude));
    }

    void translateSphere() {
        //Translates the sphere forward as long as there is remaining distance left

        //Get sine scaling factor
        float scalingFactor = MathHelper.sineScaleFactor(remainingDistance, totalDistance);
        //Translation vector with scaled length
        Vector3 translationVector = translationDirection.normalized * (Time.deltaTime * scalingFactor *  translationSpeed);
        //Move sphere forward
        transform.Translate(translationVector, Space.World);
        //Decrement by the travelled distance
        remainingDistance -= Time.deltaTime * translationSpeed * scalingFactor;

        updateSphereColor(remainingDistance);
        //Stop the translation if there is no remaining distance
        if (remainingDistance <= 0.1f) {
            isTranslating = false;
            ExperimentManager.Instance.LogMarker("event:translationStop");
        }
        
    }

    void rotateSphere(){
        //Rotates the sphere at a fixed distance around the player as long as there is a remaining angle left

        //Get sine scaling factor
        float scalingFactor = MathHelper.sineScaleFactor(remainingAngle, totalAngle);
        //Rotation increment
        float roationIncrement = angularSpeed * Time.deltaTime * scalingFactor;
        // Spin the object around the Player.
        transform.RotateAround(vortex, Vector3.up, roationIncrement);  
        //Decrement by the travelled rotation
        remainingAngle -= Mathf.Abs(roationIncrement);
        //Turn color slowly to red when remaining angle is below 30 deg
        updateSphereColor(remainingAngle/30);
        //Stop the rotation if there is no remaining angle
        if (Mathf.Abs(remainingAngle) <= 0.1f) {
            isRotating = false;
            ExperimentManager.Instance.LogMarker("event:rotationStop");
        }      
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

    public void setSpherePosition(Vector3 position) {
        transform.position = position + new Vector3(0f, ExperimentManager.Instance.participantHeight, 0f);
    }


    void updateSphereColor(float remainingAmount) {
        //Updates the sphere color based on the remaining travel amount
        //Color transition starts at remaining amount of 1 and stops at 0
        if (isBaseline) {
            meshRenderer.material.color = new Color(2.0f * (1 - remainingAmount), 0, 2.0f * remainingAmount); 
        } else {
            meshRenderer.material.color = new Color(2.0f * (1 - remainingAmount), 2.0f * remainingAmount, 0); 
        }
        
        
    }

    public IEnumerator breath()
    {
        //Coroutine that scales up the sphere and then down again
        //to indicate an event
        int FramesCount = 10;
        float AnimationTimeSeconds = 0.1f;

        float TargetScale = 0.15f;
        float InitScale = transform.localScale.y;
        float _currentScale = InitScale;
        
        float _deltaTime = AnimationTimeSeconds/FramesCount;
        float _dx = (TargetScale - InitScale)/FramesCount;
        bool  _upScale = true;

        while (_upScale)
        {
            _currentScale += _dx;
            if (_currentScale > TargetScale)
            {
                _upScale = false;
                _currentScale = TargetScale;
            }
            transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds(_deltaTime);
        }

        while (!_upScale)
        {
            _currentScale -= _dx;
            if (_currentScale < InitScale)
            {
                _upScale = true;
                _currentScale = InitScale;
            }
            transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds(_deltaTime);
        }
    }

}
