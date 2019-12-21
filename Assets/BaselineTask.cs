using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaselineTask : Singleton<BaselineTask>
{

        struct waypoint {
            float distance;
            float angle;
            bool rotateRight;
        }

        int remainingWaypoints;
        bool taskAtHand = false;

        void Update() {
            //Start a new waypoint task if the previous task is finished and there are still waypoints left
            if (ExperimentManager.Instance.isTaskFinished() && remainingWaypoints > 0 && !taskAtHand) {
                waypoint newWaypoint = generateRandomWaypoint();
            }

            if ()

            

        }

        public void createBaseline(int numberWaypoints) {      
            remainingWaypoints = numberWaypoints;    
            for (int waypointCounter = 0; waypointCounter < numberWaypoints; waypointCounter++) {
                waypoint newWaypoint = generateRandomWaypoint();
                waypointList.Add(newWaypoint);
            }
        }

        waypoint generateRandomWaypoint() {        
            float distance = Random.Range(10f, 20f);  
            float angle = Random.Range(0f, 180f);       
            bool rotateRight = rand.Next(2) == 0;

            waypoint randomWaypoint = new waypoint(distance, angle, rotateRight);

            return randomWaypoint;
        }
}
