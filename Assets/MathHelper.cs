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
        float zPosition = Random.Range(-3.5f, 3.5f);  
        float xPosition = Random.Range(-6.5f, 6.5f);       

        Vector3 randomWaypoint = new Vector3(xPosition, 0f, zPosition);

        return randomWaypoint;
    }

    public static float generateRandomAngle() {
        bool sign  = (Random.value > 0.5f);
        float angle = Random.Range(10f, 90f);
        if (!sign) {angle = -angle;}
        return angle;
    }

    public static float generateRandomDistance() {
        float distance = Random.Range(2f,5f);
        return distance;
    }

    public static bool proofWaypoint(float angle, float distance) {
        if (angle <= 180f && angle >= -180f && distance >= 2f) {
            return true;
        } else {
            return false;
        }
    }
}
