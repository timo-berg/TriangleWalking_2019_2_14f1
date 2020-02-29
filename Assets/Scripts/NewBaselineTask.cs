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

        //Translate to transit point
        SphereMovement.Instance.setTranslation(transitPoint);
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        yield return new WaitForSeconds(Random.Range(1f,2f));

        //Rotate towards target point
        Vector3 vec1 = transitPoint - startPoint;
        Vector3 vec2 = targetPoint - transitPoint;
        SphereMovement.Instance.setRotation(Vector3.SignedAngle(vec1, vec2, Vector3.up), transitPoint);
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        yield return new WaitForSeconds(Random.Range(1f,2f));

        //Translate to target point
        SphereMovement.Instance.setTranslation(targetPoint);
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        yield return new WaitForSeconds(Random.Range(1f,2f));

        baselineRunning = false;
        SphereMovement.Instance.isBaseline = false;
    }

    public void initiateLongBaseline(int runCounter, int numBaselineWaypoints){
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
                Debug.Log("New waypoint");
            } while (!waypointSecure);

            Debug.DrawLine(PlayerMovement.Instance.getPlayerPosition(), waypoint, Color.magenta, 60f);
            ExperimentManager.Instance.LogMarker(string.Format("event:baselineWaypoint;waypoint:{0}",waypoint));
            
            //Rotate towards target point
            SphereMovement.Instance.setRotation(rotationAngle, PlayerMovement.Instance.getPlayerPosition());
            yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
            yield return StartCoroutine(SphereMovement.Instance.breath());
            yield return new WaitForSeconds(Random.Range(1f,2f));

            //Translate to the waypoint
            SphereMovement.Instance.setTranslation(waypoint);
            yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
            yield return StartCoroutine(SphereMovement.Instance.breath());
            yield return new WaitForSeconds(Random.Range(1f,2f));
        }

        //Lead to target point
        //Rotate towards target point
        rotationAngle = MathHelper.getAngle(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), targetPoint);
        SphereMovement.Instance.setRotation(rotationAngle, PlayerMovement.Instance.getPlayerPosition());
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        yield return new WaitForSeconds(Random.Range(1f,2f));

        //Translate to the waypoint
        SphereMovement.Instance.setTranslation(targetPoint);
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        yield return new WaitForSeconds(Random.Range(1f,2f));




        baselineRunning = false;
        ExperimentManager.Instance.LogMarker("event:longBaselineEnd");
    }


    public bool isBaselineRunning() {
        return baselineRunning;
    }
}
