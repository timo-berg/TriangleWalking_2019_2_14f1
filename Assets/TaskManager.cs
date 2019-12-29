using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager :  Singleton<TaskManager>
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(taskQueue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator taskQueue() {
        BaselineTask.Instance.initiateBaseline(3);
        yield return new WaitWhile(() => BaselineTask.Instance.isBaselineRunning());
        BaselineTask.Instance.initiateBaseline(3);
        yield return new WaitWhile(() => BaselineTask.Instance.isBaselineRunning());
    }
}
