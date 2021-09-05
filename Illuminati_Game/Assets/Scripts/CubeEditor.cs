using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[SelectionBase]
public class CubeEditor : MonoBehaviour
{
    [SerializeField] [Range(.1f, 20f)] float xGridSize = 1f;
    [SerializeField] [Range(.1f, 20f)] float zGridSize = .5f;
    private Vector3 cubeLocation;
    private bool filled = false;

    public Vector3 CubeLocation { get => cubeLocation; }
    public bool Filled { get => filled; set => filled = value; }

    void Start()
    {
        cubeLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 snapPos;
        snapPos.x = Mathf.RoundToInt(transform.position.x / xGridSize) * xGridSize;
        snapPos.z = Mathf.RoundToInt(transform.position.z / zGridSize) * zGridSize;
                          
        transform.position = new Vector3(snapPos.x, transform.position.y, snapPos.z);
        cubeLocation = transform.position;          
        
    }
}
