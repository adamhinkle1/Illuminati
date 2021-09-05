using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] GameObject placeholderCard;
    [SerializeField] GameObject topCardPlaceholder;
    [SerializeField] GameObject deckObject;
    [SerializeField] GameObject uncontrolledBoard;
    [SerializeField] GameObject cameraView;
    [SerializeField] float drawSpeed;
   
    private Stack<GameObject> deck;

    private GameObject topCard;
    
    private int deckCount;

    public Stack<GameObject> GetDeck { get => deck; set => deck = value; }

    // Start is called before the first frame update
    void Start()
    {
        //deck represented by stack
        deck = new Stack<GameObject>();

        //Load all the cards into the deck. are just prefabs, not instances
        PopulateDeck();  
    }

    // Update is called once per frame

    private void PopulateDeck()
    {
        foreach (GameObject card in Resources.LoadAll("Cards", typeof(GameObject)))
        {
            deck.Push(card);
            deckCount = deck.Count;
            print(card.GetComponent<GroupInterface>().GetName());
        }

        //topCard = InstantiateCard(deck.Peek());

    }

    public void DrawCard()
    {
        //draw top card
        GameObject card = deck.Pop();

      //print("drew card: " + card.GetComponent<GroupInterface>().GetName());

        //if there are no cards left, do something
        if (deck.Count < 1)
        {
            placeholderCard.SetActive(false);
            //do something to handle this
        }


        //Instantiate the card
        GameObject topCard = InstantiateCard(card);


        //print("instantiated: " + topCard.GetComponent<GroupInterface>().GetName());
        //print("camera view location: " + cameraView.transform.position);

        //Move card to center view
        topCard.GetComponent<CardMovementController>().DrawCardMovement(cameraView, drawSpeed);
    }

    private GameObject InstantiateCard(GameObject card)
    {

        GameObject topCard = Instantiate(card);

        topCard.transform.parent = deckObject.transform;

        topCard.transform.localPosition = topCardPlaceholder.transform.localPosition;

        topCard.transform.localRotation = topCardPlaceholder.transform.localRotation;

        return topCard;
    }

    // void OnMouseDown()
    //{
    //    DrawCard();
    //}

}
