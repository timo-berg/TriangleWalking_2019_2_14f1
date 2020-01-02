using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleTask : Singleton<TriangleTask>
{
    bool triangleRunning;
    bool coroutineRunning;

    //Triangle parameters
    float angle;
    //NOTE: positive angles are clockwise rotation and negative angles are counterclockwise
    float[] angles = {-120f, -90f, -60f, -30f, 30f, 60f, 90f, 120f};
    float firstDistance = 4f;
    float secondDistance;
    //TODO: decide on distances
    float[] distances = {3f, 4f, 5f};
    
    

    Vector3[] anchorPoints = new [] { new Vector3(-4f,0f,-2.5f), new Vector3(-4f,0f,2.5f) };
    Vector3 anchorPoint;

    public void initiateTriangle(int angleID, int secondDistanceID) {
        triangleRunning = true;
        //Set triangle parameters
        angle = angles[angleID];
        secondDistance = distances[secondDistanceID];
        if (angle < 0) { anchorPoint = anchorPoints[0]; } else { anchorPoint = anchorPoints[1]; }
        //Start the tasks
        StartCoroutine(executeTriangle());
    }

    IEnumerator executeTriangle() {
        //Lead participant to anchor point
        StartCoroutine(walkToWaypoint(anchorPoint));
        yield return new WaitWhile(() => isCoroutineRunning()); 

        //Lead participant to the second point
        Vector3 firstWaypoint = anchorPoint + Vector3.right * firstDistance;
        Debug.DrawLine(anchorPoint, firstWaypoint, Color.red, 20f);
        StartCoroutine(walkToWaypoint(firstWaypoint));
        yield return new WaitWhile(() => isCoroutineRunning()); 

        //Lead participant to the third point
        Vector3 secondWaypoint = firstWaypoint + (Quaternion.Euler(0f, angle, 0f) * Vector3.right) * secondDistance;
        Debug.DrawLine(firstWaypoint, secondWaypoint, Color.red, 20f);
        StartCoroutine(walkToWaypoint(secondWaypoint));
        yield return new WaitWhile(() => isCoroutineRunning()); 
        
        //TODO: Let the participant point
        //TODO: Let the participant walk
        triangleRunning = false;
    }

    IEnumerator walkToWaypoint(Vector3 waypoint) {
        coroutineRunning = true;
        //Lead the participant to the waypoint 
        //Calculate rotation parameters
        float rotationAngle = 0f;
        float translationDistance = 0f;
        rotationAngle = MathHelper.getAngle(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), waypoint);
        translationDistance = ExperimentManager.Instance.nearDistance + MathHelper.getDistance(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), waypoint);
        //Set rotation parameters and start rotation
        bool rotateRight = rotationAngle >= 0;
        SphereMovement.Instance.setRotation(Mathf.Abs(rotationAngle), rotateRight);
        //Wait until rotation is completed
        yield return new WaitWhile(() => SphereMovement.Instance.isSphereRotating());
        Debug.Log("Rotation done!");

        //Translate to the waypoint
        SphereMovement.Instance.setTranslation(translationDistance);
        yield return new WaitWhile(() => SphereMovement.Instance.isSphereTranslating());
        Debug.Log("Translation done!");

        //Wait until the participant is near the sphere
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        Debug.Log("Waypoint reached!");
        coroutineRunning = false;
    }

    /*
    IEnumerator pointToOrigin() {
        //Display an arrow that rotates with the view of the camera
        //Wait for mouse click
        //
    }

    IEnumerator walkToOrigin() {

    }
    */
    public bool isTriangleRunning() {
        return triangleRunning;
    }
    bool isCoroutineRunning() {
        return coroutineRunning;
    }

}
