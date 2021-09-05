using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] List<Player> playerList;
    [SerializeField] GameObject camManager;
    [SerializeField] GameObject bank;
    [SerializeField] GameObject deckObject;
    [SerializeField] GameObject nextTurnBanner;
    [SerializeField] GameObject incomeDistribution;
    private cameraManager mCameraManger;
    private MoneyTransactions moneyTransactions;
    private Queue<Player> players;
    private Player player;
    private Deck deck;
    private bool cardDrawn = false;
    private bool firstTurn = true;



    public Queue<Player> Players { get => players; }
    public List<Player> PlayerList { get => playerList; set => playerList = value; }


    // Start is called before the first frame update
    void Start()
    {
        players = new Queue<Player>(playerList);
        mCameraManger = camManager.GetComponent<cameraManager>();
        moneyTransactions = bank.GetComponent<MoneyTransactions>();
        deck = deckObject.GetComponent<Deck>();

    }


    //public void PopulatePlayers(int numHumanPlayers)
    //{
    //    for (int i = 0; i < numHumanPlayers; ++i)
    //    {
    //        players.Enqueue(new Player());
    //    }

    //}

     void Update()
    {
        if (firstTurn)
        {
            StartTurn();
        }
    }

    private void StartTurn()
    {
        player = players.Peek();
        

        print("player: " + player.name);

        


        

        StartCoroutine(StartTurnProceedings());
        




 
        //player.LockControls();
        
        //Something to change UI stats to the current player
        //Deck.DrawCard();
        //Bank.DistributeIncome(player);
        //unlock controls;
        
        //else if (player is AIPlayer)
        //{
        //    //do something yet to be determined
        //    //Deck.DrawCard();
        //    //Bank.DistributeIncome(player);
        //}

    }

    public IEnumerator StartTurnGraphics()
    {
        yield return new WaitForSeconds(1);
        nextTurnBanner.SetActive(true);
        nextTurnBanner.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = player.name + "'s turn!";
        yield return new WaitForSeconds(1.5f);

        nextTurnBanner.SetActive(false);

    }

    public IEnumerator StartTurnProceedings()
    {
        if (firstTurn)
        {
            firstTurn = false;
            yield return StartCoroutine(StartTurnGraphics());
        }
        else
        {
            mCameraManger.rotateToNextPlayer();
            yield return StartCoroutine(StartTurnGraphics());
        }
        

        StartCoroutine(DrawCard());
    }

    public IEnumerator DrawCard()
    {
        if (deck.GetDeck.Count > 0)
        {
            for (int i = 0; i < 2; ++i)
            {
                cardDrawn = false;
                deck.DrawCard();
                yield return new WaitUntil(DrawNextCard);
            }         
            
        }
        else
        {
            print("No more cards in the deck!");
        }

        if (player.PowerStructure.GetComponent<PowerStructure>().GetPowerStructure.Count > 0)
        {
            StartCoroutine(DistributeIncome());
        }



    }

    public bool DrawNextCard()
    {
        return cardDrawn;
    }
    
    public void DrawTrue()
    {
        cardDrawn = true;
    }

    public IEnumerator DistributeIncome()
    {
        incomeDistribution.SetActive(true);
        yield return new WaitForSeconds(1);
        incomeDistribution.SetActive(false);
        StartCoroutine(moneyTransactions.collectIncome(player));
        foreach (GameObject group in player.PowerStructure.GetComponent<PowerStructure>().GetPowerStructure)
        {
            print(group.GetComponent<BoardCardInterface>().GroupData.Name);
        }
        foreach (GameObject group in player.PowerStructure.GetComponent<PowerStructure>().GetPowerStructure)
        {
            print(group.GetComponent<BoardCardInterface>().GroupData.Name + " received " + group.GetComponent<BoardCardInterface>().GroupData.Income + " mega bucks!");
            yield return new WaitForSeconds(.5f);
        }


    }

    public void EndTurn()
    {
        //EndTurnAnimation.Play();
        players.Dequeue();
        players.Enqueue(player);
        StartTurn();
    }

}
