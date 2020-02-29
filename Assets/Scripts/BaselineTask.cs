using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaselineTask : Singleton<BaselineTask>
{
        int remainingWaypoints;
        bool baselineRunning;
        /*
        public void initiateLongBaseline(int numberWaypoints) {      
            remainingWaypoints = numberWaypoints; 
            baselineRunning = true;
            ExperimentManager.Instance.LogMarker(string.Format("event:longBaselineStart;numWaypoints:{0}",numberWaypoints));
            StartCoroutine(executeLongBaseline());
        }

        public void initiateShortBaseline(Vector3 nextHomePoint) {      
            baselineRunning = true;
            ExperimentManager.Instance.LogMarker(string.Format("event:shortBaselineStart;targetPoint:{0}",nextHomePoint));
            StartCoroutine(executeShortBaseline());
        }


        
        IEnumerator executeLongBaseline() {
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

                Debug.DrawLine(PlayerMovement.Instance.getPlayerPosition(), waypoint, Color.blue, 3f);
                ExperimentManager.Instance.LogMarker(string.Format("event:baselineWaypoint;waypoint:{0}",waypoint));
                //Set rotation parameters and start rotation
                bool rotateRight = rotationAngle >= 0;
                SphereMovement.Instance.setRotation(Mathf.Abs(rotationAngle));
                //Wait until rotation is completed
                yield return new WaitWhile(() => SphereMovement.Instance.isSphereRotating());

                //Translate to the waypoint
                SphereMovement.Instance.setTranslation(translationDistance);
                //Debug.Log(String.Format("Translate {0} meter", translationDistance));
                yield return new WaitWhile(() => SphereMovement.Instance.isSphereTranslating());

                //Repeat until no waypoints are left
                yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
                ExperimentManager.Instance.LogMarker("event:baselineWaypointReached");
                remainingWaypoints -= 1;

            }

            baselineRunning = false;
            ExperimentManager.Instance.LogMarker("event:longBaselineEnd");
        }

        IEnumerator executeShortBaseline() {
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

            Debug.DrawLine(PlayerMovement.Instance.getPlayerPosition(), waypoint, Color.blue, 3f);
            ExperimentManager.Instance.LogMarker(string.Format("event:baselineWaypoint;waypoint:{0}",waypoint));
            //Set rotation parameters and start rotation
            bool rotateRight = rotationAngle >= 0;
            SphereMovement.Instance.setRotation(Mathf.Abs(rotationAngle), rotateRight);
            //Wait until rotation is completed
            yield return new WaitWhile(() => SphereMovement.Instance.isSphereRotating());

            //Translate to the waypoint
            SphereMovement.Instance.setTranslation(translationDistance);
            //Debug.Log(String.Format("Translate {0} meter", translationDistance));
            yield return new WaitWhile(() => SphereMovement.Instance.isSphereTranslating());

            //Repeat until no waypoints are left
            yield return new WaitUntil(() => ExperimentManager.Instance.isTaskFinished());
            ExperimentManager.Instance.LogMarker("event:baselineWaypointReached");
            remainingWaypoints -= 1;

            baselineRunning = false;
            ExperimentManager.Instance.LogMarker("event:longBaselineEnd");
        }

        public bool isBaselineRunning() {
            return baselineRunning;
    */
}