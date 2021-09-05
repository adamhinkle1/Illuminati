using HyperCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class is an interface used to get and set different GroupData values for various reasons.
public class BoardCardInterface : MonoBehaviour
{
    //reference to the groupData for this specific card
    [SerializeField] GroupData groupData;
    [SerializeField] GameObject menu;

    private GameObject groupMenuManager;
    private GameObject turnManager;
    
    private GameObject canvas;
    private GameObject menuInstance;
  

    private int income;
    //GameObject uncontrolledBoard;
    private GameObject cube = null;
    //enum to determine whether a group is in the controlled or uncontrolled section
    public enum BoardCardState { uncontrolled, controlled };
    public enum AttackState { neutral, vulnerable, defending, attacking};
    AttackState attackState;
    BoardCardState boardCardState;

    //getters and setters
    public BoardCardState BoardCardStateSet { get => boardCardState; set => boardCardState = value; }
    public GroupData GroupData { get => groupData; set => groupData = value; }
    public int Income { get => income; set => income = value; }
    public GameObject Cube{ get => cube; set => cube = value;}
    public AttackState AttackStateSet { get => attackState; set => attackState = value; }

    // Start is called before the first frame update
    void Start()
    {
        //default state is to be uncontrolled
        boardCardState = BoardCardState.uncontrolled;
        attackState = AttackState.neutral;
        income = groupData.Income;
        canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        groupMenuManager = GameObject.Find("GroupMenuManager");
        turnManager = GameObject.Find("Turn Manager");
     

        //uncontrolledBoard = GameObject.Find("Uncontrolled Section");
    }

     void OnMouseDown()
    {
        //if boardc ard is in the uncontrolled section
        if (boardCardState == BoardCardState.uncontrolled && gameObject.tag != "Illuminati")
        {
            ControlBoardGroup();

        }
    }

    public void ControlBoardGroup()
    {
        cube.GetComponent<CubeEditor>().Filled = false;
        //find the turn manager and determine which player is currently playing based on who is at front of queue.
        groupData.ControllingPlayer = GameObject.Find("Turn Manager").GetComponent<TurnManager>().Players.Peek();

        boardCardState = BoardCardState.controlled;
        GameObject powerStructure = groupData.ControllingPlayer.PowerStructure;
        //add to players power structure
        powerStructure.GetComponent<PowerStructure>().AddGroup(gameObject);
    }

    void OnMouseOver()
    {

    
        if (Input.GetMouseButtonDown(1))
        {
           
            if (boardCardState == BoardCardState.controlled || gameObject.tag == "Illuminati")
            {
 
                if (menuInstance == null)
                {

                    menuInstance = Instantiate(menu);
                }

                //make right click menu

                menuInstance.transform.SetParent(canvas.transform);
                menuInstance.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.Find("MenuSpawn").position);
                Button btn = menuInstance.GetComponentInChildren<Button>();
                btn.onClick.AddListener(AttackOnClick);
            }
            
        }

        if (GetComponent<BoardCardInterface>().AttackStateSet == AttackState.vulnerable)
        {
            Cursor.SetCursor((Texture2D) Resources.Load("Cursors/Fist"), Vector2.zero, CursorMode.Auto);
        }
        else if (GetComponent<BoardCardInterface>().AttackStateSet == AttackState.defending)
        {
            Cursor.SetCursor((Texture2D)Resources.Load("Cursors/Fist"), Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor((Texture2D)Resources.Load("Cursors/Click"), Vector2.zero, CursorMode.Auto);
        }

    }

    void AttackOnClick()
    {
        GetComponent<AttackController>().SelectCard();
        Destroy(menuInstance);

        GameObject[] boardCards = GameObject.FindGameObjectsWithTag("Board Card");

        foreach (GameObject boardCard in boardCards)
        {
            if (boardCard.GetComponent<BoardCardInterface>().GroupData.ControllingPlayer != turnManager.GetComponent<TurnManager>().Players.Peek() && !(boardCard.gameObject.tag == "Illuminati"))
            {
                print(":LFKJS:DFLKSJDF:KFLDS");
                boardCard.GetComponent<Card>().Properties.IsOutlineEnabled = true;
                boardCard.GetComponent<Card>().Properties.OutlineEndColor = Color.blue;
                boardCard.GetComponent<Card>().Properties.FaceSide.Redraw();
                boardCard.GetComponent<BoardCardInterface>().AttackStateSet = AttackState.vulnerable;
            }
            
        }

        if (!(gameObject.tag == "Illuminati"))
        {
            GetComponent<Card>().Properties.IsOutlineEnabled = true;
            GetComponent<Card>().Properties.OutlineEndColor = Color.red;
            GetComponent<Card>().Properties.FaceSide.Redraw();
            GetComponent<BoardCardInterface>().AttackStateSet = AttackState.attacking;
        }
        else
        {
            GetComponent<ParticleSystem>().Play();
            GetComponent<BoardCardInterface>().AttackStateSet = AttackState.attacking;
        }
        


    }

    void OnMouseExit()
    {
        if (boardCardState == BoardCardState.controlled || gameObject.tag == "Illuminati")
        {
            if (menuInstance != null)
            {
                
                Destroy(menuInstance);
            }
           
        }

        Cursor.SetCursor((Texture2D)Resources.Load("Cursors/Regular"), Vector2.zero, CursorMode.Auto);

    }
}
