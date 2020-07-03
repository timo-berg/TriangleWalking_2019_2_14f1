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

    public GameObject Short1;
    public GameObject Short2;
    public GameObject Long1;
    public GameObject Long2;
    MeshRenderer short1Mesh;
    MeshRenderer short2Mesh;
    MeshRenderer long1Mesh;
    MeshRenderer long2Mesh;

    Collider short1Coll;
    Collider short2Coll;
    Collider long1Coll;
    Collider long2Coll;

    public bool isVR;

    public int participantID = 0;
    public float participantHeight;

    public int reward;
    public float translationSpeed;
    
    protected override void Awake() {
        base.Awake();
        isVR = StartButtonClick.Instance.isVR;
        participantID = int.Parse(StartButtonClick.Instance.ID);
    }
    void Start() {
        participantHeight = 1.5f;
        reward = 0;
        translationSpeed = ConfigValues.translationSpeedSlow;
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

        //Room bound mesh
        short1Mesh = Short1.GetComponent<MeshRenderer>();
        short2Mesh = Short2.GetComponent<MeshRenderer>();
        long1Mesh = Long1.GetComponent<MeshRenderer>();
        long2Mesh = Long2.GetComponent<MeshRenderer>();

        short1Coll = Short1.GetComponent<Collider>();
        short2Coll = Short2.GetComponent<Collider>();
        long1Coll  = Long1.GetComponent<Collider>();
        long2Coll  = Long2.GetComponent<Collider>();

    }

    void Update() {
        playerOutOfBounds();
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

    void playerOutOfBounds() {
        if (isVR) {
        Vector3 position = PlayerMovement.Instance.getPlayerPosition();

        float minDist = Mathf.Min(Vector3.Distance(short1Coll.ClosestPoint(position), position),
                                  Vector3.Distance(short2Coll.ClosestPoint(position), position),
                                  Vector3.Distance(long1Coll.ClosestPoint(position), position),
                                  Vector3.Distance(long2Coll.ClosestPoint(position), position)) - 0.5f;


        if (minDist > 0.5f) {
            short1Mesh.enabled = false;
            short2Mesh.enabled = false;
            long1Mesh.enabled = false;
            long2Mesh.enabled = false;
        } else {
            short1Mesh.enabled = true;
            short2Mesh.enabled = true;
            long1Mesh.enabled = true;
            long2Mesh.enabled = true;

            short1Mesh.material.color = new Color(1, 0, 0, MathHelper.wallFadeSigmoid(Vector3.Distance(short1Coll.ClosestPoint(position), position)-0.5f));
            short2Mesh.material.color = new Color(1, 0, 0, MathHelper.wallFadeSigmoid(Vector3.Distance(short2Coll.ClosestPoint(position), position) - 0.5f));
            long1Mesh.material.color = new Color(1, 0, 0, MathHelper.wallFadeSigmoid(Vector3.Distance(long1Coll.ClosestPoint(position), position) - 0.5f));
            long2Mesh.material.color = new Color(1, 0, 0, MathHelper.wallFadeSigmoid(Vector3.Distance(long2Coll.ClosestPoint(position), position) - 0.5f));
        }

        } else {
            short1Mesh.enabled = false;
            short2Mesh.enabled = false;
            long1Mesh.enabled = false;
            long2Mesh.enabled = false;
        }
    }
}
