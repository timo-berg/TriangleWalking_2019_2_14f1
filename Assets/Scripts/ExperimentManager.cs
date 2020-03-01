using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;

public class ExperimentManager : Singleton<ExperimentManager>
{
    public GameObject sphere;
    public GameObject VRplayer;
    public GameObject DesktopPlayer;
    public GameObject TrackerLeft;
    public GameObject TrackerRight;
    public GameObject TrackerTorso;
    public bool isVR;

    public int participantID = 0;
    public float participantHeight;

    public int reward;
    
    protected override void Awake() {
        base.Awake();
        isVR = false;//StartButtonClick.Instance.isVR;
    }
    void Start() {
        participantHeight = 1.5f;
        reward = 0;
        //sphere.transform.Translate(0f, participantHeight - 0.2f, 0f, Space.World);
        
        //UnityEngine.XR.XRSettings.enabled = true;

        if (XRSettings.enabled && XRSettings.isDeviceActive && isVR) {
            VRplayer.SetActive(true);  
            DesktopPlayer.SetActive(false);
            Debug.Log("Using VR");
        } else {
            VRplayer.SetActive(false);  
            DesktopPlayer.SetActive(true);
            isVR = false;
            UnityEngine.XR.XRSettings.enabled = false;
            Debug.Log("Using Desktop");

        }

        /*
        float angle = 150f;
        float distance = ConfigValues.secondDistances[1];
        Vector3 homePoint = MathHelper.getHomePoint(angle);
        Vector3 secondWaypoint = MathHelper.getSecondWaypoint(angle, distance);
        Vector3 firstWaypoint = ConfigValues.anchorPoints[1];
        Debug.Log(homePoint);
        Debug.Log(firstWaypoint);
        Debug.Log(secondWaypoint);
        Debug.DrawLine(homePoint, firstWaypoint, Color.white, 20f);
        Debug.DrawLine(firstWaypoint, secondWaypoint, Color.black, 20f);
        */
    }

    void Update() {
    }

    public bool isTaskFinished() {
        bool playerNearSphere = false;
        bool sphereMoving = true;

        //Checks if the player is close to the sphere
        if (distancePlayerSphere() < ConfigValues.nearDistance) {
            playerNearSphere = true;
        }

        //Checks if the sphere is in motion
        if (!SphereMovement.Instance.isSphereRotating() && !SphereMovement.Instance.isSphereTranslating()) {
            sphereMoving = false;
        }

        //Returns only true if the player is near the sphere and if the sphere is not moving
        if (playerNearSphere && !sphereMoving) {
            return true;
        } else {
            return false;
        }
    }

    public float distancePlayerSphere() {
        Vector3 distance = PlayerMovement.Instance.getPlayerPosition() - SphereMovement.Instance.getSpherePosition();
        return distance.magnitude;
    }

    public void LogMarker(string marker) {
        if ( string.IsNullOrEmpty(marker) ) {
            return;
        }

        // DEBUG
        print("LSL marker out: " + marker);
        Assets.LSL4Unity.Scripts.LSLMarkerStream.Instance.Write(marker, LSL.liblsl.local_clock());
    }


    public void hideTrackers() {
            TrackerLeft.GetComponent<MeshRenderer>().enabled = false;
            TrackerRight.GetComponent<MeshRenderer>().enabled = false;
            TrackerTorso.GetComponent<MeshRenderer>().enabled = false;
    }
}
