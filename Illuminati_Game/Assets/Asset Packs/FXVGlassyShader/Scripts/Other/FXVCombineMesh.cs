using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXVCombineMesh : MonoBehaviour
{
	void Start ()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combine = new List<CombineInstance>();
        int i = 0;
        while (i < meshFilters.Length)
        {
            if (meshFilters[i].sharedMesh)
            {
                CombineInstance c = new CombineInstance();
                c.mesh = meshFilters[i].sharedMesh;
                c.transform = meshFilters[i].transform.localToWorldMatrix;
                combine.Add(c);
            } 
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine.ToArray());
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
    }

    void Update ()
    {
		
	}
}
