using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public  class MoneyTransactions : MonoBehaviour
{
    [SerializeField] GameObject incomeBoostText;
    [SerializeField] float incomeBoostOffset;
    [SerializeField] float incomeBoostHeightOffset;
    [SerializeField] float incomeBoostSizeFactor;
    [SerializeField] float scaleSpeed;


    private Vector3 incomeBoostSize;
    private Vector3 fullIncomeBoostSize;
    public  IEnumerator collectIncome(Player player)
    {
        foreach (GameObject group in player.PowerStructure.GetComponent<PowerStructure>().GetPowerStructure)
        {
            if (group != null)
            {
                recieveMoney(group, group.GetComponent<BoardCardInterface>().GroupData.Income);
                yield return new WaitForSeconds(.5f);
            }
            
      
        }
    }
    public void spendMoney(GameObject group, int amount)
    {
        if (group.GetComponent<BoardCardInterface>().Income >= amount)
        {
            group.GetComponent<BoardCardInterface>().Income -= amount;
        }
    }
    public  void recieveMoney(GameObject group, int amount)
    {
        group.GetComponent<BoardCardInterface>().Income += amount;
        IncomeTextAnimation(group, amount);

    }

    private void IncomeTextAnimation(GameObject group, int amount)
    {
        GameObject incomeBoost = Instantiate(incomeBoostText);
        incomeBoost.GetComponent<TextMeshPro>().text = "+" + amount;
        List<Player> players = GameObject.Find("Turn Manager").GetComponent<TurnManager>().PlayerList;
        Player player = GameObject.Find("Turn Manager").GetComponent<TurnManager>().Players.Peek();

        if (player == players[0])
        {
            incomeBoost.transform.localPosition = group.transform.localPosition + new Vector3(-incomeBoostOffset, incomeBoostHeightOffset, 0);
        }
        else if (player == players[1])
        {
            incomeBoost.transform.localPosition = group.transform.localPosition + new Vector3(0, incomeBoostHeightOffset, incomeBoostOffset);
        }
        else if (player == players[2])
        {
            incomeBoost.transform.localPosition = group.transform.localPosition + new Vector3(incomeBoostOffset, incomeBoostHeightOffset, 0);
        }
        else if (player == players[3])
        {
            incomeBoost.transform.localPosition = group.transform.localPosition + new Vector3(0, incomeBoostHeightOffset, -incomeBoostOffset);
        }

        incomeBoost.transform.localRotation = group.transform.localRotation;
        incomeBoostSize = incomeBoost.transform.localScale;
        incomeBoost.transform.parent = group.transform.parent;
        fullIncomeBoostSize = incomeBoost.transform.localScale * incomeBoostSizeFactor;
        StartCoroutine(ScaleMoney(incomeBoost));
    }

    public IEnumerator ScaleMoney(GameObject incomeBoost)
    {
        for (float t = 0f; t < scaleSpeed; t += Time.deltaTime)
        {   // iterate by time.deltaTime
            incomeBoost.transform.localScale = Vector3.Lerp(incomeBoostSize, fullIncomeBoostSize, t / scaleSpeed);  //lerp to target vector
            yield return 0;
        }

        incomeBoost.SetActive(false);

        
    }
  
}