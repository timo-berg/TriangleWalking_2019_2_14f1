using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : Singleton<ExperimentManager>
{
        float nearDistance = 10f;
        

        void Update() {
            //Debug.Log(isTaskFinished());
        }

        bool isTaskFinished() {
            bool playerNearSphere = false;
            bool sphereMoving = true;

            //Checks if the player is close to the sphere
            Vector3 distance = PlayerMovement.Instance.getPlayerPosition() - SphereMovement.Instance.getSpherePosition();
            if (distance.magnitude < nearDistance)
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
}
