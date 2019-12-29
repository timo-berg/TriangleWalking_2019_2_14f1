using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager :  Singleton<TaskManager>
{
    // Start is called before the first frame update
    void Start()
    {
        BaselineTask.Instance.initiateBaseline(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
