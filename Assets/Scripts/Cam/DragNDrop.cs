using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragNDrop : MonoBehaviour
{
    //Declaring
    private Camera _camera;
    private Vector3 _mousePosition; //Location in 3D world.
    private Vector3 _mousePos; //For mouse input.
    private Ray _ray;
    private RaycastHit _hit;
    [HideInInspector]
    public GameObject _object;
    public float timer = 1.5f; //Pickup delay.
    private bool _dragging, _buildingPickedUp;
    [HideInInspector]
    public bool ShowUI = false;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main; //Have a Camera w/ the MainCamera TAG be the _camera.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Left Mouse button.
            _dragging = true; 
        else if (Input.GetMouseButtonUp(0))
        {
            if (_object != null)
            {
                _object.GetComponent<Collider>().enabled = true;
                _object.GetComponent<Rigidbody>().useGravity = true;
            }
            //Negate booleans
            _dragging = false;
            _buildingPickedUp = false;
            timer = 1.5f; //Reset the timer.
        }

        _mousePos = Input.mousePosition; //Update the mouse position every frame.
        _ray = _camera.ScreenPointToRay(_mousePos); //Same as above, but to be used the ray.
        if (Physics.Raycast(_ray, out _hit)) //Do raycast.
        {
            _mousePosition = _hit.point; //Gets the location of where the ray hit.
            if (_dragging && _hit.transform.gameObject.CompareTag("Building")) //If the target is a building.
            {
                _object = _hit.transform.gameObject;
                ShowUI = true;
                timer -= Time.deltaTime; //Delay.
                if (timer <= 0) //After delay.
                {
                    if (!_buildingPickedUp) //If building has NOT been picked up.
                    {
                        _buildingPickedUp = true; //Building has been picked up.
                    } 
                }
            }
            if (_object != null && _buildingPickedUp == true) //If not null, e.g picked up building.
            {
                _object.transform.position = new Vector3(_mousePosition.x, 1f, _mousePosition.z); //Move the object to mouse location.
                _object.GetComponent<Collider>().enabled = false;
                _object.GetComponent<Rigidbody>().useGravity = false;
            }
        }

  

        //DEBUG
        //Debug.Log(_mousePosition); //Tells the pointer location in console.
        Debug.DrawRay(_camera.transform.position, _ray.direction * 20f, Color.red); //Same as above but graphically.
    }
}
