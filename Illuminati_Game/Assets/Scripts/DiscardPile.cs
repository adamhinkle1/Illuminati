using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    //private GameObject finalLocation;
    private Vector3 startPos;
    private Vector3 endPos;
    private Quaternion startRos;
    private Quaternion endRos;
    
    void Start()
    {
        endPos = transform.position;
        endRos = transform.rotation;
    }
    public void AddGroup(GameObject card)
    {
        startPos = card.transform.position;
        startRos = card.transform.rotation;
        StartCoroutine(discard(card));
        //Destroy(card)

    }
    IEnumerator discard(GameObject card)
    {
        for (var t = 0f; t < 1f; t += Time.deltaTime / speed)
        {
            card.transform.position = Vector3.Lerp(startPos,endPos,t);
            card.transform.rotation = Quaternion.Slerp(startRos, endRos, t);
            yield return null;
        }
        card.transform.position = endPos;
        card.transform.rotation = endRos;
        //card.transform.localScale = new Vector3(.444f,.444f,.444f);
    }
    
}
