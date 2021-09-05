using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int numControlledGroups;
    private int numStoleGroups;
    private int numDestroyedGroups;
    [SerializeField] GameObject powerStructure;
    [SerializeField] Vector3 drawRotation;


    private int playerName;

    public GameObject PowerStructure { get => powerStructure; set => powerStructure = value; }
    public Vector3 DrawRotation { get => drawRotation; set => drawRotation = value; }
    public int NumControlledGroups { get => numControlledGroups; set => numControlledGroups = value; }

    // Start is called before the first frame update
    void Start()
    {
        print("Player created");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
