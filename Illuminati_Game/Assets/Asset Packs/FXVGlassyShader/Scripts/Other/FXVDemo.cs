using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FXVDemo : MonoBehaviour
{
    public GameObject[] objectRoots;

    public Text fpsLabel;

    private int currentObject = 0;

	void Start ()
    {
        UpdateObject();
    }
	
	void Update ()
    {
        if (fpsLabel)
            fpsLabel.text = "FPS: " + ((int)(1.0f / Time.smoothDeltaTime)).ToString();

        if (Input.GetKeyDown("left"))
        {
            PrevObject();
        }

        if (Input.GetKeyDown("right"))
        {
            NextObject();
        }

        if (Input.GetKeyDown("space"))
        {
            DoShine();
        }
    }

    void UpdateObject()
    {
        int countShields = 0;
        for (int i = 0; i < objectRoots.Length; ++i)
        {
            if (i == currentObject)
            {
                objectRoots[i].SetActive(true);
                countShields += objectRoots[i].transform.childCount;
            }
            else
                objectRoots[i].SetActive(false);
        }
    }

    IEnumerator DoShineOBject(FXVGlassyShineControl shine, float delay)
    {
        yield return new WaitForSeconds(delay);

        shine.DoShine(1.0f);
    }

    public void DoShine()
    {
        FXVGlassyShineControl[] objects = GameObject.FindObjectsOfType<FXVGlassyShineControl>();

        float delay = 0.0f;
        foreach (FXVGlassyShineControl ctrl in objects)
        {
            StartCoroutine(DoShineOBject(ctrl, delay));
            delay += 0.1f;
        }
    }

    public void NextObject()
    {
        if (currentObject < objectRoots.Length-1)
            currentObject++;

        UpdateObject();
    }

    public void PrevObject()
    {
        if (currentObject > 0)
            currentObject--;

        UpdateObject();
    }
}
