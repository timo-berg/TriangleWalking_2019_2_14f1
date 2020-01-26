using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;

public class ExperimentManager : Singleton<ExperimentManager>
{
    public float nearDistance = 1f;
    public GameObject sphere;
    public GameObject VRplayer;
    public GameObject DesktopPlayer;
    public GameObject TrackerLeft;
    public GameObject TrackerRight;
    public GameObject TrackerTorso;
    public bool isVR;

    public int participantID;
    float participantHeight = 1.7f;
    
    protected override void Awake() {
        base.Awake();
        isVR = StartButtonClick.Instance.isVR;
    }
    void Start() {
        sphere.transform.Translate(0f, participantHeight - 0.2f, 0f, Space.World);
        
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
    }

    void Update() {
        updateSphereColor();        
    }

    public bool isTaskFinished() {
        bool playerNearSphere = false;
        bool sphereMoving = true;

        //Checks if the player is close to the sphere
        if (distancePlayerSphere() < nearDistance)
            playerNearSphere = true;

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

    void updateSphereColor() {
        //Updates the sphere color based on the proximity of the player
        //to the sphere
        float currentDistance = distancePlayerSphere();
        sphere.GetComponent<Renderer>().material.color = proximityColor(currentDistance, nearDistance);
        
    }
    Color proximityColor(float currentDistance, float preferredDistance) {
        //Returns red, if the player is really close or really far away
        //Returns green, if the player is at the preferred distance
        //Returns a gradient in between otherwise
        float x;
        if (currentDistance < preferredDistance) {
            x = 5f*Mathf.Pow(currentDistance-preferredDistance,2);
        } else {
            x = Mathf.Abs(currentDistance/preferredDistance-1);
        }
        
        Color color = new Color(2.0f * x, 2.0f * (1 - x), 0); 
        return color;
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
