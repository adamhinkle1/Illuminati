using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    [SerializeField] private float duration = 2;   //number of seconds to complete the rotation
    [SerializeField] private float shiftAngle = -90;   //control angle of rotation. positive value rotates ccw
    
    [SerializeField] private float ZoomSensitivity = 2f;   //speed of zoom
    [SerializeField] private float zoomMin = 3f;    //closest we can zoom in
    [SerializeField] private float zoomMax = 20f;   //furthest we can zoom out
    
    [SerializeField] private float snapSpeed = .2f;
    [SerializeField] private float panLimit = 10f;
    [SerializeField] private float xBoundary = 5f;   //x axis boundary of panning
    [SerializeField] private float zBoundary = 5f;  //z axis boundary of panning
    private bool panAllowed = false;
    
    [SerializeField] private Camera cam;   //attach main camera
    private Vector3 location;
    private Vector3 clickSpot;  //position where user clicks on screen
    private Vector3 newPos;   //vector will hold next position for camera, while checking for boundary constraints
    void Start()
    {
        location = transform.position;
        
    }
    void Update()
    {
        Zoom();  //will update zoom method incrementally each frame
        Panning();  //update panning method each frame
    }
    
    private void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)  //whenever sense scroll used
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity;  //take input from scroll and multiply by sensitivity factor
            newPos = Camera.main.transform.position + Camera.main.transform.forward * ScrollAmount;  //calculate next location for camera to travel
            newPos.y = Mathf.Clamp(newPos.y,zoomMin,zoomMax);   //this ensures that the next location is clamped between our desired zoom range
            Camera.main.transform.position = newPos;   //update camera position.
        }
    }
    private void Panning()
    {
        if (cam.transform.position.y > panLimit && panAllowed)  //this condition catches the first frame in which we cross the panLimit
        {
            panAllowed = false;
            StartCoroutine(smoothSnap(cam.transform.position, new Vector3(transform.position.x, cam.transform.position.y, transform.position.z), snapSpeed) );
        }
        else if (cam.transform.position.y > panLimit)  //this condition catches the range which the camera is over the panLimit
        {
            panAllowed = false;
        }
        else{  //otherwise, we are within the panLimit
            panAllowed = true;  //set to allow panning
            if (Input.GetMouseButtonDown(1)){   //check for right click
                clickSpot = GetWorldPosition(0);  //capture location clicked on
            }
            if (Input.GetMouseButton(1)){ //see if holding down right click
                newPos = cam.transform.position;   //capture current camera position in holding variable
                Vector3 direction = clickSpot - GetWorldPosition(0);   //find direction of click/cursor
                newPos += direction;  //update holding variable towards direction of cursor
                newPos.x = Mathf.Clamp(newPos.x,location.x - xBoundary,location.x + xBoundary);  //bound x-axis
                newPos.z = Mathf.Clamp(newPos.z,location.z - zBoundary,location.z + xBoundary);   //bound z-axis
                cam.transform.position = newPos;  //update camera position with the bounded holding variable position
            }
        }
    }
    IEnumerator smoothSnap(Vector3 pos1, Vector3 pos2, float speed) {
        for (float t=0f; t<speed; t += Time.deltaTime) {
            cam.transform.position = Vector3.Lerp(pos1, pos2, t / speed);
            yield return 0;
        }
        cam.transform.position = pos2;
    }
    private Vector3 GetWorldPosition(float y){   //function is necessary for proper panning. must have a reference point for the ground when panning 3d
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0,y,0));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }

    public void rotateToNextPlayer()
    {
        StartCoroutine(rotate(Vector3.up * shiftAngle, duration));
    }
    
    public IEnumerator rotate(Vector3 byAngles, float inTime)
    {
        //capture current position
        var currentAngle = transform.rotation;
        //calculate target camera by current position + rotation amount
        var targetAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        //smooth through specified rotation(shiftAngle) in "inTime"(duration) seconds
        for (var t = 0f; t < 1f; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(currentAngle, targetAngle, t);
            yield return null;
        }
        transform.rotation = targetAngle;  //snap frame to target position. this is critical to fix framerate bugs
        
    }
}
