using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager :  Singleton<TaskManager>
{
    // Start is called before the first frame update
    void Start()
    {
        //Push an initial marker with experiment values
        string marker = string.Format("event:experimentStart;angularSpeed:{0};translationSpeed:{1}", 
                                        SphereMovement.Instance.angularSpeed, SphereMovement.Instance.translationSpeed);
        ExperimentManager.Instance.LogMarker(marker);
        StartCoroutine(taskQueue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator taskQueue() {
        
        BaselineTask.Instance.initiateBaseline(3);
        yield return new WaitWhile(() => BaselineTask.Instance.isBaselineRunning());

        TriangleTask.Instance.initiateTriangle(2, 2);
        yield return new WaitWhile(() =>  TriangleTask.Instance.isTriangleRunning());

        BaselineTask.Instance.initiateBaseline(3);
        yield return new WaitWhile(() => BaselineTask.Instance.isBaselineRunning());
    }
}
