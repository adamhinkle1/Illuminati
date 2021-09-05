using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncontrolledBoard : MonoBehaviour
{
    //Array of all the grid cubes
    [SerializeField] GameObject[] uncontrolledSection;

    //2d array for the grid cubes
    private GameObject[,] uncontrolledArray;
    //2d array for the board cards
    private GameObject[,] uncontrolledGroups;
    public GameObject[,] UncontrolledArray { get => uncontrolledArray; set => uncontrolledArray = value; }

    // Start is called before the first frame update
    void Start()
    {
        InstantiateUncontrolledArray();
        uncontrolledGroups = new GameObject [3, 3]; //change 3 to var
    }
    private void InstantiateUncontrolledArray()
    {
        uncontrolledArray = new GameObject[3, 3];
        int oneDIndex = 0;
        for (int i = 0; i < 3; ++i) //change 3 to var
        {
            for (int j = 0; j < 3; ++j)
            {
                uncontrolledArray[i,j] = uncontrolledSection[oneDIndex];
                ++oneDIndex;
            }
        }
    }

public bool AddGroup(GameObject group)
{
    GameObject boardCard = group.GetComponent<GroupInterface>().BoardCard;
    for (int i = 0; i < 3; ++i) //change 3 to var
    {
        for (int j = 0; j < 3; ++j)
        {
            //if the cube is not already filled, then add to the location of that cube
            if (!uncontrolledArray[i, j].GetComponent<CubeEditor>().Filled)
            {
                uncontrolledGroups[i, j] = boardCard;
                uncontrolledArray[i, j].GetComponent<CubeEditor>().Filled = true;
                GameObject boardInstance = Instantiate(boardCard);
                boardInstance.GetComponent<BoardCardInterface>().Cube = uncontrolledArray[i, j];  //assign cube to this card
                boardInstance.transform.position = uncontrolledArray[i, j].GetComponent<CubeEditor>().transform.position;
                boardInstance.transform.rotation = uncontrolledArray[i, j].GetComponent<CubeEditor>().transform.rotation;
                Cursor.SetCursor((Texture2D)Resources.Load("Cursors/Regular"), Vector2.zero, CursorMode.Auto);
                return true;
            }
        }

    }
    return false;
}}
