using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Singleton<Arrow>
{
    bool clippedToCamera = false;
    GameObject cylinder;

    // Start is called before the first frame update
    void Start()
    {
        cylinder = GameObject.Find("Cylinder");
    }

    // Update is called once per frame
    void Update()
    {
        if (clippedToCamera) {
            //Clip arrow to the camera movement
            transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane + 3) );
            //Set the height of the arrow
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            //Make the arrow face the participant
            transform.LookAt(cylinder.transform, Vector3.up);
            transform.Rotate(90f, 0f, 0f, Space.Self);
        }
    }

    void showArrow(bool show) {
        GetComponent<Renderer>().enabled = show;
    }

    void clipToCamera(bool clipToCamera) {
        clippedToCamera = clipToCamera;
    }

    public void enablePointing(bool enable) {
        showArrow(enable);
        clipToCamera(enable);
    }

    public Vector3 getPointingDirection() {
        return transform.position;
    }
}
