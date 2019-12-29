using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaselineTask : Singleton<BaselineTask>
{
        int remainingWaypoints;

        public void initiateBaseline(int numberWaypoints) {      
            remainingWaypoints = numberWaypoints; 
            StartCoroutine(executeBaseline());
        }



        IEnumerator executeBaseline() {
            while(remainingWaypoints > 0) {
                //Create a waypoint
                /*
                float rotationAngle = MathHelper.generateRandomAngle();
                float translationDistance = MathHelper.generateRandomDistance();
                MathHelper.proofWaypoint(rotationAngle,translationDistance);
                Debug.Log(rotationAngle);
                Debug.Log(translationDistance);
                */
                bool waypointSecure = false;
                float rotationAngle = 0f;
                float translationDistance = 0f;
                Vector3 waypoint;

                do {
                waypoint = MathHelper.generateRandomWaypoint();
                rotationAngle = MathHelper.getAngle(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), waypoint);
                translationDistance = MathHelper.getDistance(SphereMovement.Instance.getSpherePosition(), PlayerMovement.Instance.getPlayerPosition(), waypoint);
                waypointSecure = MathHelper.proofWaypoint(rotationAngle, translationDistance);
                Debug.Log(waypointSecure);
                } while (!waypointSecure);

                Debug.DrawLine(PlayerMovement.Instance.getPlayerPosition(),waypoint, Color.green, 10f);
                Debug.Log(rotationAngle);
                Debug.Log(translationDistance);
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
        }
}