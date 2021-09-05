using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice : MonoBehaviour
{
    [Header("Dice")]
    [SerializeField] GameObject die1;
    [SerializeField] GameObject die2;

    [Header("Properties")]
    [SerializeField] int numRolls = 10;
    [SerializeField] float speed = 0.05f;

    //sprite arrays
    private Sprite[] diceSides;
    private SpriteRenderer[] spriteSides1;
    private SpriteRenderer[] spriteSides2;

    private int value;
    private bool valueAvailable;

    private bool coroutineAllowed = true;
    private int diceValue = 0;

    public int NumRolls { get => numRolls; set => numRolls = value; }
    public int Value { get => value; set => this.value = value; }
   
    public GameObject Die1 { get => die1; }
    public GameObject Die2 { get => die2; }

     void Awake()
    {
        
    }
    // Use this for initialization
    public void Start()
    {
        valueAvailable = false;
        //load die sprites from resources folder
        diceSides = Resources.LoadAll<Sprite>("Dice/");

        //initialize array for sides of dies
        spriteSides1 = new SpriteRenderer[6];
        spriteSides2 = new SpriteRenderer[6];

        //Fill array with each sprite renderer of each side of each die
        for (int i = 0; i < 6; ++i)
        {
            spriteSides1[i] = die1.GetComponent<Dice>().Sides[i].GetComponent<SpriteRenderer>();
            spriteSides2[i] = die2.GetComponent<Dice>().Sides[i].GetComponent<SpriteRenderer>();
        }
    }

    public IEnumerator StartRoll(int tempResult, GameObject dice, GameObject dice1, GameObject dice2, GameObject target)
    {
        print("Roll");

        //start coroutine for roll
        if (coroutineAllowed)
        {
            yield return StartCoroutine("Roll");
          
        }

        GameObject[] tempObject = GameObject.FindGameObjectsWithTag("Board Card");
        GameObject[] tempObject2 = GameObject.FindGameObjectsWithTag("Illuminati");

        foreach (GameObject boardCard in tempObject)
        {
            if (boardCard.GetComponent<BoardCardInterface>().AttackStateSet == BoardCardInterface.AttackState.attacking)
            {
                boardCard.GetComponent<AttackController>().ContinueRoll(tempResult, dice, dice1, dice2, target);
            }
        }

        foreach (GameObject illumCard in tempObject2)
        {
            if (illumCard.GetComponent<BoardCardInterface>().AttackStateSet == BoardCardInterface.AttackState.attacking)
            {
                illumCard.GetComponent<AttackController>().ContinueRoll(tempResult, dice, dice1, dice2, target);
            }
        }






    }

    private IEnumerator Roll()
    {
        coroutineAllowed = false;

        //initialize array to hold result for each side
        int[] sideRolls1 = new int[6];
        int[] sideRolls2 = new int[6];

        //Loop for each roll
        for (int i = 0; i <= numRolls; i++)
        {

            //choose a random number for each side of each die
            for (int k = 0; k < 6; ++k)
            {

                sideRolls1[k] = Random.Range(1, 6);
                sideRolls2[k] = Random.Range(1, 6);
            }

            //change the sprite for each side of each die according to roll results
            for (int j = 0; j < 6; ++j)
            {
      
                spriteSides1[j].sprite = diceSides[sideRolls1[j] - 1];
                spriteSides2[j].sprite = diceSides[sideRolls2[j] - 1];
            }

   
            yield return new WaitForSeconds(speed);
            valueAvailable = true;
        }

        //add the two results of the two sides viewed by camera
        diceValue = sideRolls1[4] + sideRolls2[4];

     
        value = diceValue;
        coroutineAllowed = true;
    }


}
