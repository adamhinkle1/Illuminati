using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnify : MonoBehaviour

{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float magnifySpeed = 0.2f;
    [SerializeField] private float cardHeight = .25f;
    [SerializeField] private float cardWidth = .2f;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float yOffset = .1f;
    [SerializeField] private float zOffset = .5f;
    private Player player;
    private Vector3 playerOrientation;
   
    private GameObject spawn;
    private Vector3 fullScale;
    //private bool cardShown = false;
    
    void Start()
    {
        fullScale = new Vector3(cardWidth,cardHeight,0);   //define magnified card size
        player = GameObject.Find("Turn Manager").GetComponent<TurnManager>().Players.Peek();
        playerOrientation = player.DrawRotation;
    }

    void OnMouseEnter()
    {
        player = GameObject.Find("Turn Manager").GetComponent<TurnManager>().Players.Peek();
        playerOrientation = player.DrawRotation;
        Debug.Log("player orientation: " + playerOrientation);
        //if (!cardShown){
        Vector3  newPos = transform.position + new Vector3(xOffset,yOffset,zOffset);  //target location for new prefab
            spawn = Instantiate(cardPrefab,newPos,Quaternion.Euler(playerOrientation));   //create prefab
            spawn.transform.localScale = new Vector3(.01f,.01f,0f);  //make it very small
            StartCoroutine(magnify(spawn.transform.localScale,fullScale));  //magnify animation
    }
    IEnumerator magnify(Vector3 current, Vector3 target)
    {
        for (float t=0f; t<magnifySpeed; t += Time.deltaTime) {   // iterate by time.deltaTime
            spawn.transform.localScale = Vector3.Lerp(current, target, t / magnifySpeed);  //lerp to target vector
            yield return 0;
        }
        spawn.transform.localScale = target;  //snap to fullScale
    }
    void OnMouseExit()
    {
        //StartCoroutine(magnify(fullScale,new Vector3(.01f,.01f,0f)));
        //if (spawn != null){   //destroy created prefab
        //    Destroy(spawn);
        //}
        Destroy(spawn);

        //StartCoroutine(magnify(fullScale,new Vector3(.01f,.01f,0f)));
        //if (spawn != null){   //destroy created prefab
        //    Destroy(spawn);
        //}
      
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.name == "New Game Object" && obj.transform.parent == null)
            {
                Destroy(obj);
            }
        }
    }

    
}
