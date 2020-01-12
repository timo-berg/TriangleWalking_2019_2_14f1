using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class TrackerManager : MonoBehaviour
{
    public GameObject leftFootTracker;
    public GameObject rightFootTracker;
    public GameObject torsoTracker;

    // Start is called before the first frame update
    void Start()
    {
        for (int index=0;index<16;index++) {
            ETrackedDeviceClass deviceClass = OpenVR.System.GetTrackedDeviceClass((uint) index);
            if(deviceClass == ETrackedDeviceClass.GenericTracker) {
                Debug.Log("GenericTracker got connected at index:" + index);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
