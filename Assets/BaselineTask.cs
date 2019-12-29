using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaselineTask : Singleton<BaselineTask>
{
        int remainingWaypoints;
        bool baselineRunning;

        public void initiateBaseline(int numberWaypoints) {      
            remainingWaypoints = numberWaypoints; 
            baselineRunning = true;
            StartCoroutine(executeBaseline());
        }



        IEnumerator executeBaseline() {
            while(remainingWaypoints > 0) {
                //Create a waypoint
                bool waypointSecure = false;
                float rotationAngle = 0f;
                float translationDistance = 0f;
                Vector3 waypoint;
                //Sample a random waypoint and check if its in the room bounds
                do {
                waypoint = MathHelper.generateRandomWaypoint();
                rotationAngle = MathHelper.getAngle(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), waypoint);
                translationDistance = MathHelper.getDistance(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), waypoint);
                waypointSecure = MathHelper.proofWaypoint(rotationAngle, translationDistance);
                } while (!waypointSecure);

                Debug.Log(waypoint);
                //Set rotation parameters and start rotation
                bool rotateRight = rotationAngle >= 0;
                SphereMovement.Instance.setRotation(Mathf.Abs(rotationAngle), rotateRight);
                //Wait until rotation is completed
                yield return new WaitWhile(() => SphereMovement.Instance.isSphereRotating());
                Debug.Log("Rotation done!");

                //Translate to the waypoint
                SphereMovement.Instance.setTranslation(translationDistance);
                //Debug.Log(String.Format("Translate {0} meter", translationDistance));
                yield return new WaitWhile(() => SphereMovement.Instance.isSphereTranslating());
                Debug.Log("Translation done!");

                //Repeat until no waypoints are left
                yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
                Debug.Log("Waypoint reached!");
                remainingWaypoints -= 1;

            }

            baselineRunning = false;
            Debug.Log("Baseline done!");
        }

        public bool isBaselineRunning() {
            return baselineRunning;
        }
}