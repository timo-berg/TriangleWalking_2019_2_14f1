using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBaselineTask : Singleton<NewBaselineTask>
{
    Vector3 startPoint;
    Vector3 targetPoint;
    Vector3 transitPoint;
    bool baselineRunning;

    public void initiateInterTrialBaseline(int runCounter){
        baselineRunning = true;
        SphereMovement.Instance.isBaseline = true;

        //Get current position
        startPoint = PlayerMovement.Instance.getPlayerPosition();

        //Get start point of the next trial
        targetPoint = MathHelper.getHomePoint(
            ConfigValues.anglePermutation[ExperimentManager.Instance.participantID, 
            runCounter]);
        
        //Generate random transit point
        bool waypointSecure = false;
        do {
            transitPoint = MathHelper.generateRandomWaypoint();
            waypointSecure = MathHelper.proofWaypoint(
                MathHelper.getAngle(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), transitPoint),
                MathHelper.getDistance(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), transitPoint));
        } while (!waypointSecure);

        StartCoroutine(executeInterTrialBaseline());
    }

    IEnumerator executeInterTrialBaseline() {
        //Place Sphere
        SphereMovement.Instance.setSpherePosition(
            PlayerMovement.Instance.getPlayerPosition() 
            + (transitPoint - PlayerMovement.Instance.getPlayerPosition()).normalized * 2f);

        //Translate to transit point
        SphereMovement.Instance.setTranslation(transitPoint);
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        yield return new WaitForSeconds(Random.Range(1f,2f));

        //Rotate towards target point
        Vector3 vec1 = transitPoint - startPoint;
        Vector3 vec2 = transitPoint - targetPoint;
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

    //public void initiateLongBaseline(){

    //}


    public bool isBaselineRunning() {
        return baselineRunning;
    }
}
