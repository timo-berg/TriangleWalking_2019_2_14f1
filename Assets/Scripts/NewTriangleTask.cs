using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTriangleTask : Singleton<NewTriangleTask>
{
    public GameObject pole;
    MeshRenderer poleMesh;

    float angle;
    float secondDistance;
    Vector3 homePoint;
    Vector3 firstWaypoint;
    Vector3 secondWaypoint;
    bool triangleRunning;


    void Start()
    {
        poleMesh = pole.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        
    }

    public void initiateTriangle(int runCounter) {
        triangleRunning = true;
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
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        poleVisibility(false);

        //Translate to first point
        SphereMovement.Instance.setSpherePosition(pole.transform.position);
        SphereMovement.Instance.setTranslation(firstWaypoint);
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        yield return new WaitForSeconds(Random.Range(1f,2f));

        //Rotate towards second point
        SphereMovement.Instance.setRotation(Mathf.Sign(angle)*(180 - Mathf.Abs(angle)), firstWaypoint);
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        yield return new WaitForSeconds(Random.Range(1f,2f));

        //Translate to second point
        SphereMovement.Instance.setTranslation(secondWaypoint);
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        yield return StartCoroutine(SphereMovement.Instance.breath());
        yield return new WaitForSeconds(Random.Range(1f,2f));

        //Show arrow
        Arrow.Instance.enablePointing(true);
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        Arrow.Instance.enablePointing(false);

        //Homing performance check
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        poleVisibility(true);
        pole.transform.position = homePoint;

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        poleVisibility(false);

        triangleRunning = false;

    }

    public bool isTriangleRunning() {
        return triangleRunning;
    }

    void poleVisibility(bool visible) {
        poleMesh.enabled = visible;
    }
}
