using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotController : MonoBehaviour
{
    public float RotationSpeed = 0.5f;

    public float movementSpeed = 10f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime, 0);
        transform.Translate(-Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime, 0, Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime);
    }
}
