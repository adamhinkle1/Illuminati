using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] GameObject[] sides;
    public GameObject[] Sides { get => sides; set => sides = value; }
    // Start is called before the first frame update

 
    public void EnableDice()
    {
        print("active now");
        foreach (GameObject side in sides)
        {
            print("activating sides");
            side.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void DisableDice()
    {

        foreach (GameObject side in sides)
        {
            side.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

}
