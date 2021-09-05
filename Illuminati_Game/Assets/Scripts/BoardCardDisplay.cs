using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class BoardCardDisplay : MonoBehaviour
{
    [SerializeField] GroupData groupData;

    [SerializeField] GameObject power;
    [SerializeField] GameObject resistance;

    TextMeshPro powerText;
    TextMeshPro resistanceText;

    // Start is called before the first frame update
    void Start()
    {
        //instatiate attributes
        powerText = power.GetComponent<TextMeshPro>();
        resistanceText = resistance.GetComponent<TextMeshPro>();       
    }

   void Update()
    {
        SetStats();
    }

    private void SetStats()
    {
        powerText.text = groupData.Power.ToString();
        resistanceText.text = groupData.Resistance.ToString();
    }
}
