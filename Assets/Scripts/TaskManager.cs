using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TaskManager :  Singleton<TaskManager>
{
    bool isFast;

    // Start is called before the first frame update
    void Start()
    {
        //Push an initial marker with experiment values
        StartCoroutine(taskQueue(ExperimentManager.Instance.startTrial));



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator taskQueue(int startTrial) {
        //TEST
        Debug.Log(ExperimentManager.Instance.participantID);

        //Skip the intro when experiment is continued from a later trial
        if(startTrial == 0) {
            //Welcome the player
            yield return StartCoroutine(message("Herzlich Willkommen! \n Zum Start bitte klicken."));
            ExperimentManager.Instance.logMarker("event:experimentStart;");
            ExperimentManager.Instance.hideTrackers();
            
            yield return StartCoroutine(longBaseline(22));
            //Test trials
            yield return StartCoroutine(triangleTask(22, true));
            yield return StartCoroutine(interTrialBaseline(23));
            yield return StartCoroutine(triangleTask(23, true));
        }

        //Real trials
        for (int trial = startTrial; trial<24; trial++) {
            yield return StartCoroutine(interTrialBaseline(trial));
            yield return StartCoroutine(triangleTask(trial));
        }

        yield return StartCoroutine(longBaseline(0));

        yield return StartCoroutine(message(string.Format("Danke für die Teilnahme \n Sie haben {0} Punkte erreicht!", ExperimentManager.Instance.reward)));
        ExperimentManager.Instance.logMarker(string.Format("event:experimentEnd;score:{0};", ExperimentManager.Instance.reward));
    }

    public IEnumerator message(string message) {
        HMDMessageManager.Instance.ShowMessage(message);
        yield return new WaitUntil(() => getKeyDown());
        HMDMessageManager.Instance.HideMessage();
    }

    IEnumerator triangleTask(int trial, bool isTest=false) {
        isFast = MathHelper.getRandomBoolean();
        
        //Start triangle
        if(isTest) {
            ExperimentManager.Instance.logMarker(string.Format("event:triangleStart;trial:{0};fastTrial:{1};isTest:{2};", 99, isFast, isTest));
            yield return StartCoroutine(message("Testaufgabe \n Suchen sie den Marker und richten sich aus! \n Wenn Sie den Marker gefunden haben klicken Sie!"));
        } else {
            ExperimentManager.Instance.logMarker(string.Format("event:triangleStart;trial:{0};fastTrial:{1};;isTest:{2};", trial, isFast, isTest));
            yield return StartCoroutine(message(string.Format("Aufgabe Nr {0} \n Suchen sie den Marker und richten sich aus! \n Zum Fortfahren bitte klicken!",trial+1)));
        }
        
        NewTriangleTask.Instance.initiateTriangle(trial, isFast);
        //Wait for end
        yield return new WaitWhile(() =>  NewTriangleTask.Instance.isTriangleRunning());
        yield return StartCoroutine(message(string.Format("Aufgabe geschafft! \n Sie haben {0} Punkte \n Zum Fortfahren bitte klicken!", ExperimentManager.Instance.reward)));

        ExperimentManager.Instance.logMarker(string.Format("event:triangleEnd;trial{0};", trial));
    }

    IEnumerator interTrialBaseline(int trial) {
        ExperimentManager.Instance.logMarker(string.Format("event:intertrialBaselineStart;trial:{0};", trial));
        //Start baseline
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(message("Desorientierung! \n Bitte suchen Sie dem Ball. \n Zum Fortfahren bitte klicken!"));
        NewBaselineTask.Instance.initiateInterTrialBaseline(trial);
        //Wait for end
        yield return new WaitWhile(() =>  NewBaselineTask.Instance.isBaselineRunning());

        ExperimentManager.Instance.logMarker(string.Format("event:intertrialBaselineEnd;trial:{0};", trial));
    }

    IEnumerator longBaseline(int trial) {
        ExperimentManager.Instance.logMarker(string.Format(
            "event:longBaselineStart;trial:{0};numberWaypoints:{1};", 
            trial, ConfigValues.longBaselineWaypointNumber));
        //Start baseline
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(message("Desorientierung! \n Bitte suchen Sie dem Ball. \n Zum Fortfahren bitte klicken!"));
        NewBaselineTask.Instance.initiateLongBaseline(trial, ConfigValues.longBaselineWaypointNumber);
        //Wait for end
        yield return new WaitWhile(() =>  NewBaselineTask.Instance.isBaselineRunning());
        
        ExperimentManager.Instance.logMarker(string.Format("event:longBaselineEnd;trial:{0};", trial));
    }

    public bool getKeyDown() {
        if (ExperimentManager.Instance.isVR) {
            return SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any);
        } else {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    
}
