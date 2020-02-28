using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static float getAngle(Vector3 spherePosition, Vector3 playerPosition, Vector3 waypointPosition) {
        Vector3 differenceSpherePlayer = playerPosition - spherePosition;
        Vector3 differenceWaypointPlayer = playerPosition - waypointPosition;
        float angle = Vector3.SignedAngle(differenceSpherePlayer, differenceWaypointPlayer, Vector3.up);
        return angle;
    }

    public static float getDistance(Vector3 spherePosition, Vector3 playerPosition, Vector3 waypointPosition) {
        Vector3 differenceSpherePlayer = playerPosition - spherePosition;
        Vector3 differenceWaypointPlayer = playerPosition - waypointPosition;
        float distance = differenceWaypointPlayer.magnitude - differenceSpherePlayer.magnitude;
        return distance;
    }

    public static Vector3 generateRandomWaypoint() {        
        float xPosition = Random.Range(-2.5f, 3.5f);  
        float zPosition = Random.Range(-5f, 8f);       

        Vector3 randomWaypoint = new Vector3(xPosition, 0f, zPosition);

        return randomWaypoint;
    }

    public static bool proofWaypoint(float angle, float distance) {
        if (Mathf.Abs(angle) <= 180f && Mathf.Abs(angle) >= 10f && distance >= 3f) {
            return true;
        } else {
            return false;
        }
    }

    public static float sineScaleFactor(float currentValue, float totalValue) {
        //Returns the value of a sine curve between 1.5pi (minimum) and 3.5pi (next minimum)
        //The factor is then scaled to be ranging from 0 to 2
        //Returns 0 at the start and the end and 2 in the middle
        float scaleFactor = Mathf.Sin(Mathf.PI * currentValue/totalValue) + .3f;
        //return scaleFactor;
        return scaleFactor*2;
    }

    public static Vector3 getHomePoint(float angle) {
        //Calculates home point. The anchor point of
        //the triangle is the first waypoint and is
        //fixed (one for pos and one for neg angles)
        Vector3 home;
        float firstDistance = ConfigValues.firstDistance;
        float absAngleRad = Mathf.Abs(angle) * Mathf.Deg2Rad;


        if(angle < 0) {
            home = ConfigValues.anchorPoints[0] + new Vector3(
                    -Mathf.Cos(absAngleRad/2f)*firstDistance,
                    0, 
                    -Mathf.Sin(absAngleRad/2f)*firstDistance);
            return home;
        } else {
            home = ConfigValues.anchorPoints[1] + new Vector3(
                    Mathf.Cos(absAngleRad/2f)*firstDistance,
                    0, 
                    Mathf.Sin(absAngleRad/2f)*firstDistance);

            return home;
        }
    }

    public static Vector3 getSecondWaypoint(float angle, float distance) {
        //Calculates second waypoint. The anchor point of
        //the triangle is the first waypoint and is
        //fixed (one for pos and one for neg angles)
        Vector3 secondWaypoint;
        float absAngleRad = Mathf.Abs(angle) * Mathf.Deg2Rad;

        if(angle < 0) {
            secondWaypoint = ConfigValues.anchorPoints[0] + new Vector3(
                    -Mathf.Cos(absAngleRad/2)*distance,
                    0, 
                    Mathf.Sin(absAngleRad/2)*distance);

            return secondWaypoint;
        } else {
            secondWaypoint = ConfigValues.anchorPoints[1] + new Vector3(
                    Mathf.Cos(absAngleRad/2)*distance,
                    0, 
                    -Mathf.Sin(absAngleRad/2)*distance);

            return secondWaypoint;
        }
    }
}
