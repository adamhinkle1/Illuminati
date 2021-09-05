using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStructure : MonoBehaviour
{
    [SerializeField] GameObject[] gridSpots;
    private List<GameObject> powerStructure;

    public List<GameObject> GetPowerStructure { get => powerStructure; set => powerStructure = value; }

    // Start is called before the first frame update
    void Start()
    {
        powerStructure = new List<GameObject>();
    }

    public bool AddGroup(GameObject defendingGroup)
    {

        //defendingGroup.GetComponent<BoardCardInterface>().GroupData.Master = attackingGroup.GetComponent<BoardCardInterface>().GroupData; ;
        //attackingGroup.GetComponent<BoardCardInterface>().GroupData.AddSlave(defendingGroup.GetComponent<BoardCardInterface>().GroupData);

        for (int i = 0; i < gridSpots.Length; ++i)
        {
            CubeEditor cube = gridSpots[i].GetComponent<CubeEditor>();
            if (!cube.Filled)
            {
                powerStructure.Add(defendingGroup);
                cube.Filled = true;

                defendingGroup.transform.position = cube.transform.position;
                defendingGroup.transform.rotation = cube.transform.rotation;
                //boardInstance.transform.SetParent(cube.transform, false);
                return true;
            }
        }
        defendingGroup.GetComponent<BoardCardInterface>().GroupData.ControllingPlayer = GetComponent<BoardCardInterface>().GroupData.ControllingPlayer;
        defendingGroup.GetComponent<BoardCardInterface>().GroupData.ControllingPlayer.NumControlledGroups += 1;

        return false;



    }

    public void DropGroup(GameObject defendingGroup)
    {
        List<GameObject> powerStructureCopy = new List<GameObject>();

        foreach (GameObject obj in powerStructure)
        {
            powerStructureCopy.Add(obj);
        }

        foreach (GameObject dGroup in powerStructureCopy)
        {
            if (dGroup.name == defendingGroup.name)
            {
                powerStructure.Remove(dGroup);
                print(dGroup.name + " has been removed");
            }
        }

        foreach (GameObject dGroup in powerStructure)
        {
            print(dGroup.name + "\n");
        }

        defendingGroup.GetComponent<BoardCardInterface>().GroupData.ControllingPlayer.NumControlledGroups -= 1;
    }

}


