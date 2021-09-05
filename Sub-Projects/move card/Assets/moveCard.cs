using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCard : MonoBehaviour
{
    private float restingHeight;
    [SerializeField] private float hoveringHeight = 1.5f;   //height card will hover at
    [SerializeField] private float hoverSpeed = .2f;   //number of seconds to complete hover animation
    [SerializeField] private float snapSpeed = .2f; // number of seconds to complete snap animation
    [SerializeField] private float xBoundary = 5f;   //x axis boundary of panning
    [SerializeField] private float zBoundary = 5f;  //z axis boundary of panning

    private Vector3 newPos;   //vector will hold next position for camera, while checking for boundary constraints
    private bool isHovering = false;  //keep track of when card is hovering/being dragged
    
    void Start()
    {
        restingHeight = transform.position.y;
        
    }
    void Update()
    {
        dragCard();
    }
    void OnMouseDown()  //when the card is clicked on
    {
        StartCoroutine(hover(restingHeight, hoveringHeight));  // hover the card
        isHovering = true;  //and let system know that the card is hovering(being moved)
    }
    void OnMouseUp()
    {
        StartCoroutine(hover(hoveringHeight, restingHeight));
        isHovering = false;
    }
    void dragCard()   //this function is updated every frame
    {
        if (Input.GetMouseButton(0) && isHovering)  //check if mouse is held down and that the card is hovering
        {
            // create a plane which will serve as reference point for cursor to find point along x-z axis
            Plane plane=new Plane(Vector3.up,new Vector3(0, 1, 0));
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);   //this creates a ray from screen to a 3-d point in space
            float distance;
            if(plane.Raycast(ray, out distance)) {  //find the distance of the ray to the plane
                newPos = ray.GetPoint(distance);  //capture the point in 3D to find targeted x-z coordinates along the plane
                newPos.x = Mathf.Clamp(newPos.x,-1*xBoundary,xBoundary);  //create x axis boundary
                newPos.z = Mathf.Clamp(newPos.z,-1*zBoundary,zBoundary);  //z axis boundary
                transform.position=new Vector3(newPos.x,hoveringHeight,newPos.z);  //update object position
            }
        }
    }
    IEnumerator hover(float h1, float h2)//function will smoothly raise/lower the card to/from the table to/from the desired hovering height
    {
        Vector3 pos1 = new Vector3(transform.position.x,h1,transform.position.z);  //starting position
        Vector3 pos2 = new Vector3(transform.position.x,h2,transform.position.z);  // ending position
        for (float t=0f; t<hoverSpeed; t += Time.deltaTime) {   // iterate by time.deltaTime
            transform.position = Vector3.Lerp(pos1, pos2, t / hoverSpeed);  //lerp is a smooth linear transition from one position to another. here its using constant x/z-axis and changing from h1- h2
            yield return 0;
        }
        transform.position = pos2;  //snap to desired end point height. need this to prevent small inaccuracies from lerp
    }
    IEnumerator snapTo(Vector3 target)
    {
        for (float t=0f; t<snapSpeed; t += Time.deltaTime) {   // iterate by time.deltaTime
            transform.position = Vector3.Lerp(transform.position, target, t / snapSpeed);  //lerp to target vector
            yield return 0;
        }
        transform.position = target; //snap to desired end point height. need this to prevent small inaccuracies from lerp
    }
}


