using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXVGlassyRenderQueue : MonoBehaviour 
{
    public static int currentIndex = 1;

	void Start () 
	{
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();

        currentIndex++;

        mr.material.renderQueue = mr.material.renderQueue + currentIndex;
    }

    void Update () 
	{
		
	}
}
