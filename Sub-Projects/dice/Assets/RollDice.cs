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

    private bool coroutineAllowed = true;
    private int diceValue = 0;
 
    
    
    // Use this for initialization
    private void Start () {
        
        //load die sprites from resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");

        //initialize array for sides of dies
        spriteSides1 = new SpriteRenderer[5];
        spriteSides2 = new SpriteRenderer[5]; 
        
        //Fill array with each sprite renderer of each side of each die
        for (int i = 0; i < 5; ++i)
        {
            spriteSides1[i] = die1.GetComponent<Dice>().Sides[i].GetComponent<SpriteRenderer>();
            spriteSides2[i] = die2.GetComponent<Dice>().Sides[i].GetComponent<SpriteRenderer>();
        }
    }

    private void StartRoll()
    {
        print("Roll");

        //start coroutine for roll
        if (coroutineAllowed)
        {
            StartCoroutine("Roll");
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
            for (int k = 0; k < 5; ++k)
            {
                sideRolls1[k] = Random.Range(0, 6);
                sideRolls2[k] = Random.Range(0, 6);
            }
     
            //change the sprite for each side of each die according to roll results
            for (int j = 0; j < 5; ++j)
            {
                spriteSides1[j].sprite = diceSides[sideRolls1[j]];
                spriteSides2[j].sprite = diceSides[sideRolls2[j]];
            }

            yield return new WaitForSeconds(speed);
        }

        //add the two results of the two sides viewed by camera
        diceValue = sideRolls1[2] + sideRolls2[2] + 2;

        Debug.Log(diceValue);
        coroutineAllowed = true;
    }

    private void OnMouseDown()
    {
        //roll on mouse click
        StartRoll();
    }
}
