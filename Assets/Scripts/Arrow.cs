using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Singleton<Arrow>
{
    bool clippedToCamera = false;
    bool clippedToController= false;
    GameObject desktopPlayer;
    GameObject hand;
    GameObject VRplayer;

    // Start is called before the first frame update
    void Start()
    {
        desktopPlayer = GameObject.Find("Cylinder");
        hand = GameObject.Find("RighHand");
        VRplayer = GameObject.Find("VRCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (clippedToCamera) {
            //Clip arrow to the camera movement
            transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane + 1) );
            //Set the height of the arrow
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            //Make the arrow face the participant
            transform.LookAt(desktopPlayer.transform, Vector3.up);
            transform.Rotate(110f, 180f, 0f, Space.Self);
        } else if (clippedToController) {
            transform.position = hand.transform.position;
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            transform.LookAt(VRplayer.transform, Vector3.up);
            transform.Rotate(110f, 180f, 0f, Space.Self);
            
        }
    }

    void showArrow(bool show) {
        GetComponent<Renderer>().enabled = show;
    }

    public void enablePointing(bool enable) {
        showArrow(enable);
        if (ExperimentManager.Instance.isVR) {
            clippedToController = enable;
        } else {
            clippedToCamera = enable;
        }
        
        if (enable) {
            ExperimentManager.Instance.LogMarker("event:pointingStart");
        } else {
            ExperimentManager.Instance.LogMarker("event:pointingStop");
        }
    }

    public Vector3 getPointingDirection() {
        return transform.position;
    }
}
