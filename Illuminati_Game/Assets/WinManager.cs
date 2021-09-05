using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinManager : MonoBehaviour
{

    [SerializeField] GameObject win;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Win(GameObject group)
    {
        if (group.GetComponent<BoardCardInterface>().GroupData.ControllingPlayer.NumControlledGroups > 3)
        {
            win.SetActive(true);
            win.GetComponentInChildren<TextMeshProUGUI>().text = "Player" + group.GetComponent<BoardCardInterface>().GroupData.ControllingPlayer.name + " wins!";
            
        }
        
    }


}
