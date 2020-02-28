using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTriangleTask : MonoBehaviour
{
    public GameObject pole;
    MeshRenderer poleMesh;

    float angle;
    float secondDistance;


    void Start()
    {
        poleMesh = pole.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        
    }

    public void initiateTriangle(int runCounter) {
        
    }

    IEnumerator executeTriangle() {
        //Show pole

        //Translate to first point
        
        //Rotate towards second point
        //Translate to second point
        //Show arrow
        //Wait for homing confirmation
        yield return new WaitForSeconds(1f);
    }

    void showPole(Vector3 position) {
        poleMesh.enabled = true;
        pole.transform.position = position;
    }

    void hidePole() {
        poleMesh.enabled = false;
    }
}
