﻿using System.Collections;
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
    float[] distances = {3f, 3.5f, 4f};
    
    

    Vector3[] anchorPoints = new [] { new Vector3(-0.5f,0f,1.5f), new Vector3(1.5f,0f,1.5f) };
    Vector3[] firstWaypoints = new [] { new Vector3(3f,0f,3.5f), new Vector3(-2f,0f,-0.5f) };
    Vector3 anchorPoint;
    Vector3 firstWaypoint;
    Vector3 rotationVector;
    Vector3 pointingDirection;

    public void initiateTriangle(int angleID, int secondDistanceID) {
        triangleRunning = true;
        //Set triangle parameters
        angle = angles[angleID];
        secondDistance = distances[secondDistanceID];
        if (angle > 0) { 
            anchorPoint = anchorPoints[0];
            firstWaypoint = firstWaypoints[0];
        } else { 
            anchorPoint = anchorPoints[1];
            firstWaypoint = firstWaypoints[1];
        }
        rotationVector = anchorPoint - firstWaypoint;
        ExperimentManager.Instance.LogMarker(string.Format("event:triangleTaskStart;angle:{0};secondDistance:{1}",angle,secondDistance));
        //Start the tasks
        StartCoroutine(executeTriangle());
    }

    IEnumerator executeTriangle() {
        
        //Lead participant to anchor point
        ExperimentManager.Instance.LogMarker(string.Format("event:walkToAnchor;waypoint:{0}",anchorPoint));
        StartCoroutine(walkToWaypoint(anchorPoint));

        Debug.DrawLine(PlayerMovement.Instance.getPlayerPosition(), anchorPoint, Color.green, 60f);

        yield return new WaitWhile(() => isCoroutineRunning());
        ExperimentManager.Instance.LogMarker("event:anchorReached");

        //Lead participant to the second point
        Debug.DrawLine(anchorPoint, firstWaypoint, Color.blue, 60f);
        
        ExperimentManager.Instance.LogMarker(string.Format("event:walkToFirstWaypoint;waypoint:{0}",firstWaypoint));
        StartCoroutine(walkToWaypoint(firstWaypoint));
        yield return new WaitWhile(() => isCoroutineRunning()); 
        ExperimentManager.Instance.LogMarker("event:firstWaypointReached");

        //Lead participant to the third point
        Vector3 secondWaypoint = firstWaypoint + (Quaternion.Euler(0f, Mathf.Abs(angle), 0f) * rotationVector.normalized) * secondDistance;
        
        Debug.DrawLine(firstWaypoint, secondWaypoint, Color.red, 60f);
        
        ExperimentManager.Instance.LogMarker(string.Format("event:walkToSecondWaypoint;waypoint:{0}",secondWaypoint));
        StartCoroutine(walkToWaypoint(secondWaypoint));
        yield return new WaitWhile(() => isCoroutineRunning());
        ExperimentManager.Instance.LogMarker("event:secondWaypointReached");
        

        yield return new WaitForSeconds(1f);
        //Let the participant point towards the origin. Confirmation via click
        Arrow.Instance.enablePointing(true);
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        Arrow.Instance.enablePointing(false);
        //Get the pointed direction
        pointingDirection = Arrow.Instance.getPointingDirection() - PlayerMovement.Instance.getPlayerPosition();
        pointingDirection.y = 0f;
        Debug.DrawRay(PlayerMovement.Instance.getPlayerPosition(), pointingDirection*20, Color.green, 20f);
        yield return new WaitForSeconds(0.5f);

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
