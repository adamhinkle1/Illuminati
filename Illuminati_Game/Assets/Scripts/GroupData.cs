using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Group", menuName = "Cards/Group")]
public partial class GroupData : ScriptableObject
{
    [Header("Descriptions")]
    [SerializeField] new string name;
    [SerializeField] string specialPower;

    [Header("Main Attributes")]
    [SerializeField] int income;
    [SerializeField] int power;
    [SerializeField] int resistance;

    [Header("Sub-Attributes")]
    [SerializeField] int corruption;
    [SerializeField] int diplomacy;
    [SerializeField] int credibility;
    [SerializeField] int influence;
    [SerializeField] int followers;
    [SerializeField] int wealth;

    [Header("Status")]
    [SerializeField] bool conspiracy = false;

    [Header("Alignments")]
    [SerializeField] Alignment[] alignments;

    [Header("Card Type")]
    [SerializeField] bool hasSpecialPower = false;

    [Header("Control Stats")]
    [SerializeField] int masterSpots;
    [SerializeField] int slaveSpots;

    public enum Alignment { Government, Communist, Liberal, Conservative, Peaceful, Violent, Straight, Weird, Criminal, Fanatic, None };

    //Relationships with other groups

    private GroupData master;
    private List<GroupData> slaves;
    private Player controllingPlayer;

    void Awake()
    {
        master = null;
        slaves = new List<GroupData>();
        controllingPlayer = null;
    }

    public void AddSlave(GroupData slave)
    {
        //if there are still open slave spots left
        if (slaves.Count <= slaveSpots)
        {
            slaves.Add(slave);
        }
        else
        {
            //Notify("All Control Spots are Taken");
        }
    }

    //getters and settings. This is how it is done in C#
    public string Name { get => name; set => name = value; }
    public string SpecialPower { get => specialPower; set => specialPower = value; }
    public int Income { get => income; set => income = value; }
    public int Power { get => power; set => power = value; }
    public int Resistance { get => resistance; set => resistance = value; }
    public int Corruption { get => corruption; set => corruption = value; }
    public int Credibility { get => credibility; set => credibility = value; }
    public int Influence { get => influence; set => influence = value; }
    public int Followers { get => followers; set => followers = value; }
    public int Wealth { get => wealth; set => wealth = value; }
    public bool Conspiracy { get => conspiracy; set => conspiracy = value; }
    public int Diplomacy { get => diplomacy; set => diplomacy = value; }
    public bool HasSpecialPower { get => hasSpecialPower; set => hasSpecialPower = value; }
    public Alignment[] Alignments { get => alignments; set => alignments = value; }
    public GroupData Master { get => master; set => master = value; }
    public List<GroupData> Slaves { get => slaves;}
    public Player ControllingPlayer { get => controllingPlayer; set => controllingPlayer = value; }
}


