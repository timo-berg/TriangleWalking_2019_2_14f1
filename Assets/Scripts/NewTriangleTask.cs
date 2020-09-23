using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTriangleTask : Singleton<NewTriangleTask>
{
    public GameObject pole;
    MeshRenderer poleMesh;
    public GameObject targetCircle;
    MeshRenderer circleMesh;
    public MouseLook DesktopCamera;

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

    public void initiateTriangle(int runCounter, bool isFast) {
        //Initiates the triangle task. The triangle is determined by the angle alone.

        triangleRunning = true;
        
        if (isFast) {
            PlayerMovement.Instance.translationSpeed = ConfigValues.translationSpeedFast;
            SphereMovement.Instance.translationSpeed = ConfigValues.translationSpeedFast;
        } else {
            PlayerMovement.Instance.translationSpeed = ConfigValues.translationSpeedSlow;
            SphereMovement.Instance.translationSpeed = ConfigValues.translationSpeedSlow;
        }

        

        //Get and calculate the triangle parameters
        angle = ConfigValues.anglePermutation[ExperimentManager.Instance.participantID, runCounter];
        homePoint = MathHelper.getHomePoint(angle);
        
        Debug.Log("Triangle home point");
        Debug.Log(homePoint);
        Debug.Log("Run count" + runCounter);

        if (angle < 0) {firstWaypoint = ConfigValues.anchorPoints[0];} else {firstWaypoint = ConfigValues.anchorPoints[1];}
        secondWaypoint = MathHelper.getSecondWaypoint(angle, 4f);

        Debug.DrawLine(homePoint, firstWaypoint, Color.blue, 120f);
        Debug.DrawLine(firstWaypoint, secondWaypoint, Color.blue, 120f);

        StartCoroutine(executeTriangle());
    }

    IEnumerator executeTriangle() {
        //Show pole
        yield return new WaitForSeconds(0.5f);
        pole.transform.position = homePoint + (firstWaypoint - homePoint).normalized * 2f + new Vector3(0f, -1f, 0f);
        pole.transform.rotation = Quaternion.Euler(-90f, 0f, 
                                        Vector3.SignedAngle(Vector3.right, firstWaypoint - homePoint, Vector3.up));
        ExperimentManager.Instance.logMarker("event:triangleTaskPoleVisible;");                                
        poleVisibility(true);

        //Wait for player to find pole and confirm
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        ExperimentManager.Instance.logMarker("event:triangleTaskPoleSpotted;");
        poleVisibility(false);
        SphereMovement.Instance.toggleVisibility(true);
        ExperimentManager.Instance.logMarker("event:miniBaselineStart;");
        yield return new WaitForSeconds(1f);
        ExperimentManager.Instance.logMarker("event:miniBaselineStop;");

        //Lead to first point
        ExperimentManager.Instance.logMarker(string.Format("event:triangleTaskFirstpoint;waypoint:{0};",firstWaypoint));
        //Translate to first point
        SphereMovement.Instance.setSpherePosition(pole.transform.position + new Vector3(0f, 1f, 0f));
        SphereMovement.Instance.setTranslation(firstWaypoint);
        yield return StartCoroutine(subtaskFinish());

        //Lead to second point
        ExperimentManager.Instance.logMarker(string.Format("event:triangleTaskSecondpoint;waypoint:{0};",secondWaypoint));
        //Rotate towards second point
        SphereMovement.Instance.setRotation(secondWaypoint);
        yield return StartCoroutine(subtaskFinish());
        //Translate to second point
        SphereMovement.Instance.setTranslation(secondWaypoint);
        yield return StartCoroutine(subtaskFinish());
        SphereMovement.Instance.toggleVisibility(false);

        //Homing task
        ExperimentManager.Instance.logMarker("event:triangleTaskHomingtaskStart;waypoint;");
        yield return new WaitUntil(() => TaskManager.Instance.getKeyDown());
        ExperimentManager.Instance.logMarker("event:triangleTaskHomingtaskLocationConfirmed;waypoint;");
        pole.transform.position = homePoint;
        targetCircle.transform.position = new Vector3(homePoint.x,-1.09f ,homePoint.z);
        targetCircleVisibility(true);

        DesktopCamera.cameraTilt = true;
        
        yield return new WaitForSeconds(0.5f);
        float distanceError = Mathf.Round((homePoint - PlayerMovement.Instance.getPlayerPosition()).magnitude*100)/100;
        yield return StartCoroutine(TaskManager.Instance.message(string.Format("{0} m vom Ziel entfernt!", distanceError)));
        addReward(distanceError);
        ExperimentManager.Instance.logMarker(string.Format("event:triangleTaskHomingtaskDistanceerror;error:{0};",distanceError));

        DesktopCamera.cameraTilt = false;
        targetCircleVisibility(false);
        SphereMovement.Instance.toggleVisibility(true);
        triangleRunning = false;
        PlayerMovement.Instance.translationSpeed = ConfigValues.translationSpeedSlow;
        SphereMovement.Instance.translationSpeed = ConfigValues.translationSpeedSlow;
    }
    IEnumerator subtaskFinish() {
        //Coroutine that checks and ends a subtask
        yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
        ExperimentManager.Instance.logMarker("event:miniBaselineStart;");
        yield return StartCoroutine(SphereMovement.Instance.breath());
        ExperimentManager.Instance.logMarker("event:subtaskFinished;");
        yield return new WaitForSeconds(Random.Range(1f,1.25f));
        ExperimentManager.Instance.logMarker("event:miniBaselineStop;");
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
    void addReward(float error) {
        if (error < 1) {
            ExperimentManager.Instance.reward += 10;
        } else if (error < 2) {
            ExperimentManager.Instance.reward += 8;
        } else if (error < 3) {
            ExperimentManager.Instance.reward += 6;
        } else if (error < 4) {
            ExperimentManager.Instance.reward += 4;
        } else if (error < 5) {
            ExperimentManager.Instance.reward += 2;
        }
    }
}
