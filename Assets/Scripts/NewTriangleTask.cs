using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTriangleTask : Singleton<NewTriangleTask>
{
    public GameObject pole;
    MeshRenderer poleMesh;
    public GameObject targetCircle;
    MeshRenderer circleMesh;

    float angle;
    float secondDistance;
    Vector3 homePoint;
    Vector3 firstWaypoint;
    Vector3 secondWaypoint;
    bool triangleRunning;


    void Start()
    {
        poleMesh = pole.GetComponent<MeshRenderer>();
        circleMesh = targetCircle.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        
    }

    public void initiateTriangle(int runCounter) {
        //Initiates the triangle task. The triangle is determined by the angle alone.

        triangleRunning = true;
        
        //Get and calculate the triangle parameters
        angle = ConfigValues.anglePermutation[ExperimentManager.Instance.participantID, runCounter];
        homePoint = MathHelper.getHomePoint(angle);
        if (angle < 0) {firstWaypoint = ConfigValues.anchorPoints[0];} else {firstWaypoint = ConfigValues.anchorPoints[1];}
        secondWaypoint = MathHelper.getSecondWaypoint(angle, 4f);

        Debug.DrawLine(homePoint, firstWaypoint, Color.blue, 120f);
        Debug.DrawLine(firstWaypoint, secondWaypoint, Color.blue, 120f);

        StartCoroutine(executeTriangle());
    }

    IEnumerator executeTriangle() {
        //Show pole
        poleVisibility(true);
        pole.transform.position = homePoint + (firstWaypoint - homePoint).normalized * 2f;
        ExperimentManager.Instance.LogMarker("event:triangleTaskPoleSpotted");
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        poleVisibility(false);

        //Lead to first point
        ExperimentManager.Instance.LogMarker(string.Format("event:triangleTaskFirstpoint;waypoint:{0}",firstWaypoint));
        //Translate to first point
        SphereMovement.Instance.setSpherePosition(pole.transform.position);
        SphereMovement.Instance.setTranslation(firstWaypoint);
        yield return StartCoroutine(subtaskFinish());

        //Lead to second point
        ExperimentManager.Instance.LogMarker(string.Format("event:triangleTaskSecondpoint;waypoint:{0}",secondWaypoint));
        //Rotate towards second point
        SphereMovement.Instance.setRotation(Mathf.Sign(angle)*(180 - Mathf.Abs(angle)), firstWaypoint);
        yield return StartCoroutine(subtaskFinish());
        //Translate to second point
        SphereMovement.Instance.setTranslation(secondWaypoint);
        yield return StartCoroutine(subtaskFinish());

        //Homing task
        ExperimentManager.Instance.LogMarker("event:triangleTaskHomingtaskStart;waypoint");
        //Show arrow
        Arrow.Instance.enablePointing(true);
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        ExperimentManager.Instance.LogMarker("event:triangleTaskHomingtaskAngleConfirmed;waypoint");
        Arrow.Instance.enablePointing(false);

        //Homing task and performance check
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        ExperimentManager.Instance.LogMarker("event:triangleTaskHomingtaskLocationConfirmed;waypoint");
        pole.transform.position = homePoint;
        targetCircle.transform.position = new Vector3(homePoint.x,-1.09f ,homePoint.z);
        poleVisibility(true);
        targetCircleVisibility(true);
        
        //End the task
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        poleVisibility(false);
        targetCircleVisibility(false);

        triangleRunning = false;
    }

    IEnumerator subtaskFinish() {
        //Coroutine that checks and ends a subtask
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        ExperimentManager.Instance.LogMarker("event:subtaskFinished");
        yield return new WaitForSeconds(Random.Range(1f,2f));
    }

    public bool isTriangleRunning() {
        return triangleRunning;
    }

    void poleVisibility(bool visible) {
        poleMesh.enabled = visible;
    }

    void targetCircleVisibility(bool visible) {
        circleMesh.enabled = visible;
    }
}
