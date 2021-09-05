using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//allows the script to run while in edit mode
[ExecuteInEditMode]
public class MainCardDisplay : MonoBehaviour
{
    [SerializeField] GroupData groupData;

    [Header("Descriptions")]
    [SerializeField] new GameObject name;
    [SerializeField] GameObject specialPower;

    [Header("Main Attributes")]
    [SerializeField] GameObject income;
    [SerializeField] GameObject power;
    [SerializeField] GameObject resistance;

    [Header("Sub-Attributes")]
    [SerializeField] GameObject attributes;
    [SerializeField] GameObject corruption;
    [SerializeField] GameObject diplomacy;
    [SerializeField] GameObject credibility;
    [SerializeField] GameObject influence;
    [SerializeField] GameObject followers;
    [SerializeField] GameObject wealth;

    [Header("Alignments")]
    [SerializeField] GameObject[] alignmentObjects;

    TextMeshPro nameText;
    TextMeshPro specialPowerText;

    TextMeshPro powerText;
    TextMeshPro resistanceText;
    TextMeshPro incomeText;
    TextMeshPro corruptionText;
    TextMeshPro diplomacyText;
    TextMeshPro credibilityText;
    TextMeshPro influenceText;
    TextMeshPro followersText;
    TextMeshPro wealthText;

    TextMeshPro [] alignmentTexts;

    //how much the alignment text will move when the format of the card is changed.
    [SerializeField] float alignmentOffset;

    //where the attributes should start at
    Vector3 attributePosition;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateStats();

        //set to current position of attributes
        attributePosition = attributes.transform.localPosition;
    }

    void Update()
    {
        SetStats();
    }

    private void SetStats()
    {
        SetAttributes();
        SetAlignments();
    }

    private void SetAttributes()
    {
        nameText.text = groupData.Name;
        powerText.text = groupData.Power.ToString();
        resistanceText.text = groupData.Resistance.ToString();
        incomeText.text = groupData.Income.ToString();
        corruptionText.text = "Corruption: " + groupData.Corruption.ToString();
        diplomacyText.text = "Diplomacy: " + groupData.Diplomacy.ToString();
        credibilityText.text = "Credibility: " + groupData.Credibility.ToString();
        influenceText.text = "Influence:   " + groupData.Influence.ToString();
        followersText.text = "Followers: " + groupData.Followers.ToString();
        wealthText.text = "Wealth:     " + groupData.Wealth.ToString();

        //if group has special power checked, reformat card.
        if (groupData.HasSpecialPower)
        {
            //attributes.transform.localPosition = attributePosition + new Vector3(0f, alignmentOffset, 0f);
            specialPower.SetActive(true);
            specialPowerText.text = groupData.SpecialPower;
        }
        //if reformat and set special power to be inactive
        else
        {
            specialPower.SetActive(false);
            
            //attributes.transform.localPosition = attributePosition;
        }   
    }

    private void SetAlignments()
    {
        for (int i = 0; i < alignmentObjects.Length; ++i)
        {
            if (groupData.Alignments[i].ToString() != "None")
            {
                alignmentTexts[i].text = groupData.Alignments[i].ToString();
            }
            else
            {
                alignmentTexts[i].text = "";
            }
        }
        
    }

    private void InstantiateStats()
    {
        nameText = name.GetComponent<TextMeshPro>();
        specialPowerText = specialPower.GetComponent<TextMeshPro>();

        incomeText = income.GetComponent<TextMeshPro>();
        powerText = power.GetComponent<TextMeshPro>();
        resistanceText = resistance.GetComponent<TextMeshPro>();

        corruptionText = corruption.GetComponent<TextMeshPro>();
        diplomacyText = diplomacy.GetComponent<TextMeshPro>();
        credibilityText = credibility.GetComponent<TextMeshPro>();
        influenceText = influence.GetComponent<TextMeshPro>();
        followersText = followers.GetComponent<TextMeshPro>();
        wealthText = wealth.GetComponent<TextMeshPro>();

        alignmentTexts = new TextMeshPro[alignmentObjects.Length];

        for (int i = 0; i < alignmentObjects.Length; ++i)
        {
            alignmentTexts[i] = alignmentObjects[i].GetComponent<TextMeshPro>();
        }

    }
}
