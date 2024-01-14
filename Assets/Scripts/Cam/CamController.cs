using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform pivot; //A pivot where the camera can rotate around.
    public Transform ZoomFarthest; //The transform of the camera at it's farthest point.
    public Transform ZoomClosest; //The transform of the camera at it's closest point.
    public float LerpDistance;

    //private float _distanceToPivot; //DEBUG
   /* private float _zoomAmount;
    private float _zoomSpeed = 10; */

    // Update is called once per frame
    void LateUpdate()
    {
       // _distanceToPivot = Vector3.Distance(transform.position, pivot.transform.position); //Get & Update the distance every frame. DEBUG.
       //print(Input.GetAxis("Mouse ScrollWheel")); //Print the distance relative to the pivot point.

        LerpDistance += Input.GetAxis("Mouse ScrollWheel") * 0.1f; //The value of scrolling.
        if (LerpDistance > 1)
            LerpDistance = 1;
        else if (LerpDistance < 0)
            LerpDistance = 0;


        transform.LookAt(pivot);
        transform.position = Vector3.Lerp(ZoomClosest.position, ZoomFarthest.position, LerpDistance);
        Debug.DrawLine(ZoomClosest.position, ZoomFarthest.position, Color.red);

    }
}
