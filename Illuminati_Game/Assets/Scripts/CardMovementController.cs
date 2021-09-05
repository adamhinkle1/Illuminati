using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovementController : MonoBehaviour
{
  
    private GameObject finalLocation;
    private bool startMove = false;
    private float speed;
    private float startTime;
    private bool moveStarted = true;
    private float journeyDistance;
    private Vector3 startPos;
    private Vector3 endPos;
    private Quaternion startRos;
    private Quaternion endRos;
    private float distCovered;
    private float fractionOfJourney;
    private bool drawCardCompleted = false;
    private GameObject uncontrolledBoard;
    private Player player;
    private GameObject discardPile;

    // Start is called before the first frame update
    void Start()
    {
        finalLocation = new GameObject();
        startPos = transform.position;
        startRos = transform.rotation;
        uncontrolledBoard = GameObject.Find("Uncontrolled Section");
        discardPile = GameObject.Find("/Discard Pile/Discard Location/Position");
        player = GameObject.Find("Turn Manager").GetComponent<TurnManager>().Players.Peek();
    }

    // Update is called once per frame
    void Update()
    {
        if (startMove)
        {

            if (moveStarted)
            {
                startTime = Time.time;
                journeyDistance = Vector3.Distance(transform.position, finalLocation.transform.position);
                moveStarted = false;
            }
               
           
            DeckDrawMovement();

            if ((journeyDistance - distCovered) < Mathf.Epsilon)
            {
                startMove = false;
                drawCardCompleted = true;
            }
        }

    }

    public void DrawCardMovement(GameObject finalLocation, float speed)
    {
        this.finalLocation = finalLocation;
        endPos = finalLocation.transform.position;
        endRos = finalLocation.transform.rotation;
        this.speed = speed;

        startMove = true;
    }

    private void DeckDrawMovement()
    {
        distCovered = (Time.time - startTime) * speed;
        fractionOfJourney = distCovered / journeyDistance;
        transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
      
        transform.rotation = Quaternion.Slerp(startRos, Quaternion.Euler(player.DrawRotation), fractionOfJourney);

    }

     void OnMouseDown()
    {

            if (drawCardCompleted)
            {
                if (uncontrolledBoard.GetComponent<UncontrolledBoard>().AddGroup(gameObject))
                {
                    print("Successfully added");
                    GameObject.Find("Turn Manager").GetComponent<TurnManager>().DrawTrue();
                    
                    Destroy(gameObject);
                }
                else
                {
                    GameObject.Find("Turn Manager").GetComponent<TurnManager>().DrawTrue();
                    discardPile.GetComponent<DiscardPile>().AddGroup(gameObject);
                    print("Card Discarded");
                    Cursor.SetCursor((Texture2D)Resources.Load("Cursors/Regular"), Vector2.zero, CursorMode.Auto);
            }
            }

        

    }
}
