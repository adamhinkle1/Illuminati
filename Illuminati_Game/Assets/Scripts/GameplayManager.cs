using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    //Will add more methods here when relevant.
    public static int numHumanPlayers;
    public static int numAIPlayers;
    [SerializeField] GameObject [] illuminati;
    // Start is called before the first frame update


    void Awake()
    {
        foreach (GameObject illum in illuminati)
        {
            illum.GetComponent<BoardCardInterface>().GroupData.ControllingPlayer =  GameObject.Find("Turn Manager").GetComponent<TurnManager>().Players.Peek();
            Player player = GameObject.Find("Turn Manager").GetComponent<TurnManager>().Players.Dequeue();
            GameObject.Find("Turn Manager").GetComponent<TurnManager>().Players.Enqueue(player);
        }
    }
    void Start()
    {
        Cursor.SetCursor((Texture2D) Resources.Load("Cursors/Regular"), Vector2.zero, CursorMode.Auto);
        //GetPlayerNumbers();
    }

    //private void GetPlayerNumbers()
    //{
    //    turnManager.PopulatePlayers(4);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
