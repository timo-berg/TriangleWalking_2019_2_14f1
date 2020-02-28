using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTaskManager : MonoBehaviour
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
        //Start long baseline
            //intiateLongBaseline(7);
        //Start trial
            //initiateTriangle()
        
        
        
        yield return new WaitForSeconds(1f);
    }
}
