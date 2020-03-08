using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBaselineTask : Singleton<NewBaselineTask>
{
    Vector3 startPoint;
    Vector3 targetPoint;
    Vector3 transitPoint;
    bool baselineRunning;
    int numWaypoints;

    public void initiateInterTrialBaseline(int runCounter){
        //Initiates the intertrial baseline in which one random waypoint is travelled to.
        //Afterwards the sphere travels to the start point of the next maze
        baselineRunning = true;
        SphereMovement.Instance.isBaseline = true;

        //Get current position
        startPoint = PlayerMovement.Instance.getPlayerPosition();

        //Get start point of the next trial
        targetPoint = MathHelper.getHomePoint(
            ConfigValues.anglePermutation[ExperimentManager.Instance.participantID, 
            runCounter]);
        
        //Set sphere for secure transit point sampling
        SphereMovement.Instance.setSpherePosition(
            PlayerMovement.Instance.getPlayerPosition() 
            + (targetPoint - PlayerMovement.Instance.getPlayerPosition()).normalized * 2f);

        //Generate random transit point
        bool waypointSecure = false;
        float rotationAngle = 0f;
        float translationDistance = 0f;
        do {
            transitPoint = MathHelper.generateRandomWaypoint();
            rotationAngle = MathHelper.getAngle(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), transitPoint);
            translationDistance = MathHelper.getDistance(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), transitPoint);
            waypointSecure = MathHelper.proofWaypoint(rotationAngle, translationDistance);
        } while (!waypointSecure);

        Debug.DrawLine(startPoint, transitPoint, Color.red, 60f);
        Debug.DrawLine(transitPoint, targetPoint, Color.red, 60f);

        StartCoroutine(executeInterTrialBaseline());
    }

    IEnumerator executeInterTrialBaseline() {
        //Place Sphere and wait for player to spot it
        SphereMovement.Instance.setSpherePosition(
            PlayerMovement.Instance.getPlayerPosition() 
            + (transitPoint - PlayerMovement.Instance.getPlayerPosition()).normalized * 2f);
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        ExperimentManager.Instance.LogMarker("event:intertrialBaselineSphereSpotted");

        //Lead to transit point
        ExperimentManager.Instance.LogMarker(string.Format("event:intretrialBaselineTransitpoint;waypoint:{0}",transitPoint));
        //Translate to transit point
        SphereMovement.Instance.setTranslation(transitPoint);
        yield return StartCoroutine(subtaskFinish());

        //Lead to target point
        ExperimentManager.Instance.LogMarker(string.Format("event:intretrialBaselineTargetpoint;waypoint:{0}",targetPoint));
        //Rotate towards target point
        Vector3 vec1 = transitPoint - startPoint;
        Vector3 vec2 = targetPoint - transitPoint;
        SphereMovement.Instance.setRotation(Vector3.SignedAngle(vec1, vec2, Vector3.up), transitPoint);
        yield return StartCoroutine(subtaskFinish());

        //Translate to target point
        SphereMovement.Instance.setTranslation(targetPoint);
        yield return StartCoroutine(subtaskFinish());

        //Hide sphere
        SphereMovement.Instance.toggleVisibility(false);

        baselineRunning = false;
        SphereMovement.Instance.isBaseline = false;
    }

    public void initiateLongBaseline(int runCounter, int numBaselineWaypoints){
        //Initiates the long baseline in which the sphere travels to a given 
        //number of random waypoints. Travels to the start point of the next 
        //Triangle task at the end
        baselineRunning = true;
        SphereMovement.Instance.isBaseline = true;

        //Get start point of the next trial
        targetPoint = MathHelper.getHomePoint(
            ConfigValues.anglePermutation[ExperimentManager.Instance.participantID, 
            runCounter]);

        numWaypoints = numBaselineWaypoints;

        //Set sphere for secure transit point sampling
        SphereMovement.Instance.setSpherePosition(
            PlayerMovement.Instance.getPlayerPosition() 
            + (targetPoint - PlayerMovement.Instance.getPlayerPosition()).normalized * 2f);

        StartCoroutine(executeLongBaseline());        
    }

    IEnumerator executeLongBaseline() {
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        ExperimentManager.Instance.LogMarker("event:longBaselineSphereSpotted");
        //Travel to random waypoints
        float rotationAngle;        
        for (int count = numWaypoints; count > 0; count--) {
            //Create a waypoint
            bool waypointSecure = false;
            
            float translationDistance = 0f;
            Vector3 waypoint;
            //Sample a random waypoint and check if its in the room bounds
            do {
                waypoint = MathHelper.generateRandomWaypoint();
                rotationAngle = MathHelper.getAngle(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), waypoint);
                translationDistance = MathHelper.getDistance(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), waypoint);
                waypointSecure = MathHelper.proofWaypoint(rotationAngle, translationDistance);
            } while (!waypointSecure);

            Debug.DrawLine(PlayerMovement.Instance.getPlayerPosition(), waypoint, Color.magenta, 60f);
            ExperimentManager.Instance.LogMarker(string.Format("event:longBaselineWaypoint;waypoint:{0}",waypoint));
            
            //Rotate towards target point
            SphereMovement.Instance.setRotation(rotationAngle, PlayerMovement.Instance.getPlayerPosition());
            yield return StartCoroutine(subtaskFinish());

            //Translate to the waypoint
            SphereMovement.Instance.setTranslation(waypoint);
            yield return StartCoroutine(subtaskFinish());
        }

        //Lead to target point
        ExperimentManager.Instance.LogMarker(string.Format("event:longBaselineTargetpoint;waypoint:{0}",targetPoint));
        //Rotate towards target point
        rotationAngle = MathHelper.getAngle(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), targetPoint);
        SphereMovement.Instance.setRotation(rotationAngle, PlayerMovement.Instance.getPlayerPosition());
        yield return StartCoroutine(subtaskFinish());

        //Translate to the waypoint
        SphereMovement.Instance.setTranslation(targetPoint);
        yield return StartCoroutine(subtaskFinish());

        //Hide sphere
        SphereMovement.Instance.toggleVisibility(false);

        //End the long baseline
        baselineRunning = false;
        SphereMovement.Instance.isBaseline = false;
    }

    IEnumerator subtaskFinish() {
        //Coroutine that checks and ends a subtask
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        ExperimentManager.Instance.LogMarker("event:subtaskFinished");
        yield return new WaitForSeconds(Random.Range(1f,2f));
    }


    public bool isBaselineRunning() {
        return baselineRunning;
    }
}
