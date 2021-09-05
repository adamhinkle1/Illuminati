using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHoverManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnMouseOver()
    {
        Cursor.SetCursor((Texture2D) Resources.Load("Curors/Click"), Vector2.zero, CursorMode.Auto);
    }

     void OnMouseExit()
    {
        Cursor.SetCursor((Texture2D)Resources.Load("Curors/Regular"), Vector2.zero, CursorMode.Auto);
    }
}
