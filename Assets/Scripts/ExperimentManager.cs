using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : Singleton<ExperimentManager>
{
        public float nearDistance = 1.5f;
        GameObject sphere;
        
        void Start() {
            sphere = GameObject.Find("Sphere");
        }

        void Update() {
            updateSphereColor();
            //Debug.Log(distancePlayerSphere());
            
            
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
            /* if (SphereMovement.Instance.isSphereTranslating()) {
                float currentDistance = distancePlayerSphere();
                sphere.GetComponent<Renderer>().material.color = proximityColor(currentDistance, nearDistance);
            } else {
                sphere.GetComponent<Renderer>().material.color = new Color(1,1,1);
            }
            */
            float currentDistance = distancePlayerSphere();
            sphere.GetComponent<Renderer>().material.color = proximityColor(currentDistance, nearDistance);
            
        }
        Color proximityColor(float currentDistance, float preferredDistance) {
            //Returns red, if the player is really close or really far away
            //Returns green, if the player is at the preferred distance
            //Returns a gradient in between otherwise
            currentDistance -= preferredDistance;
            float x = Mathf.Abs(Mathf.Clamp(currentDistance/preferredDistance, -1, 1));
            
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

        public void setStartParameters(string ID, string height = "1.6") {
            participantID = ID;
            participantHeight = height;
        }
}
