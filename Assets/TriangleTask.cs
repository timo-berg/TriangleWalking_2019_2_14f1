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
    Vector3 pointingDirection;

    public void initiateTriangle(int angleID, int secondDistanceID) {
        triangleRunning = true;
        //Set triangle parameters
        angle = angles[angleID];
        secondDistance = distances[secondDistanceID];
        if (angle < 0) { anchorPoint = anchorPoints[0]; } else { anchorPoint = anchorPoints[1]; }
        ExperimentManager.Instance.LogMarker(string.Format("event:triangleTaskStart;angle:{0};secondDistance:{1}",angle,secondDistance));
        //Start the tasks
        StartCoroutine(executeTriangle());
    }

    IEnumerator executeTriangle() {
        //Lead participant to anchor point
        ExperimentManager.Instance.LogMarker(string.Format("event:walkToAnchor;waypoint:{0}",anchorPoint));
        StartCoroutine(walkToWaypoint(anchorPoint));
        yield return new WaitWhile(() => isCoroutineRunning());
        ExperimentManager.Instance.LogMarker("event:anchorReached");

        //Lead participant to the second point
        Vector3 firstWaypoint = anchorPoint + Vector3.right * firstDistance;
        ExperimentManager.Instance.LogMarker(string.Format("event:walkToFirstWaypoint;waypoint:{0}",firstWaypoint));
        StartCoroutine(walkToWaypoint(firstWaypoint));
        yield return new WaitWhile(() => isCoroutineRunning()); 
        ExperimentManager.Instance.LogMarker("event:firstWaypointReached");

        //Lead participant to the third point
        Vector3 secondWaypoint = firstWaypoint + (Quaternion.Euler(0f, angle, 0f) * Vector3.right) * secondDistance;
        ExperimentManager.Instance.LogMarker(string.Format("event:walkToSecondWaypoint;waypoint:{0}",secondWaypoint));
        StartCoroutine(walkToWaypoint(secondWaypoint));
        yield return new WaitWhile(() => isCoroutineRunning());
        ExperimentManager.Instance.LogMarker("event:secondWaypointReached");
        
        //Let the participant point towards the origin. Confirmation via click
        Arrow.Instance.enablePointing(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        Arrow.Instance.enablePointing(false);
        //Get the pointed direction
        pointingDirection = Arrow.Instance.getPointingDirection() - PlayerMovement.Instance.getPlayerPosition();
        pointingDirection.y = 0f;
        Debug.DrawRay(PlayerMovement.Instance.getPlayerPosition(), pointingDirection*20, Color.green, 20f);
        yield return new WaitForSeconds(0.5f);

        //Let the participant walk. Confirmation via click
        SphereMovement.Instance.enablePushSphere(pointingDirection);
        ExperimentManager.Instance.LogMarker("event:walkToOriginStart");
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        SphereMovement.Instance.disablePushSphere();
        Vector3 finalPosition = PlayerMovement.Instance.getPlayerPosition();
        ExperimentManager.Instance.LogMarker(string.Format("event:walkToOriginStop,finalPosition:{0}",finalPosition));
        ExperimentManager.Instance.LogMarker("event:triangleTaskStop");
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

        //Translate to the waypoint
        SphereMovement.Instance.setTranslation(translationDistance);
        yield return new WaitWhile(() => SphereMovement.Instance.isSphereTranslating());

        //Wait until the participant is near the sphere
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());

        coroutineRunning = false;
    }
    public bool isTriangleRunning() {
        return triangleRunning;
    }
    bool isCoroutineRunning() {
        return coroutineRunning;
    }

}
