using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TaskManager :  Singleton<TaskManager>
{
    //public GameObject controller;

    // Start is called before the first frame update
    void Start()
    {
        //Push an initial marker with experiment values
        string marker = string.Format("event:experimentStart;angularSpeed:{0};translationSpeed:{1}", 
                                        SphereMovement.Instance.angularSpeed, SphereMovement.Instance.translationSpeed);
        ExperimentManager.Instance.LogMarker(marker);
        StartCoroutine(taskQueue());

        //controller.GetComponent<Hand>().Show();
        //other.DoSomething();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator taskQueue() {
        
        //Wait for user input
        HMDMessageManager.Instance.ShowMessage("Herzlich Willkommen! \n Zum Start bitte klicken.");
        yield return new WaitUntil(() => getKeyDown());
        HMDMessageManager.Instance.HideMessage();
        
        TriangleTask.Instance.initiateTriangle(0, 2);
        yield return new WaitWhile(() =>  TriangleTask.Instance.isTriangleRunning());

        HMDMessageManager.Instance.ShowMessage("Aufgabe geschafft! \n Zum Fortfahren bitte klicken!");
        yield return new WaitUntil(() => getKeyDown());
        HMDMessageManager.Instance.HideMessage();

        BaselineTask.Instance.initiateBaseline(3);
        yield return new WaitWhile(() => BaselineTask.Instance.isBaselineRunning());
    }

    public bool getKeyDown() {
        if (ExperimentManager.Instance.isVR) {
            return SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any);
        } else {
            return Input.GetKeyDown(KeyCode.Mouse0);
        }
    }
}
