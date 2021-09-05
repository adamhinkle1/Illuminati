using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupInterface : MonoBehaviour
{
    //used for interfacing with the main card while drawing from deck
    [SerializeField] GroupData groupData;
    [SerializeField] GameObject boardCard;

    public GameObject BoardCard { get => boardCard; set => boardCard = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public string GetName()
    {
        return groupData.Name;
    }

    void OnMouseOver()
    {
    
        Cursor.SetCursor((Texture2D)Resources.Load("Cursors/Click"), Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor((Texture2D)Resources.Load("Cursors/Regular"), Vector2.zero, CursorMode.Auto);
    }

}
