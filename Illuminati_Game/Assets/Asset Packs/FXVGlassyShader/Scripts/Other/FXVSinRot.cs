using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXVSinRot : MonoBehaviour
{
    public float amplitude = 2.0f;

    public Vector3 rotationSpeed = Vector3.up;

    private Vector3 currentRotation;

    public Vector3 vector = Vector3.up;

    private Vector3 startRot;

    private Vector3 angle = Vector3.zero;

    void Start ()
    {
        Camera.main.transparencySortMode = TransparencySortMode.Perspective;

        startRot = transform.rotation.eulerAngles;
    }
	
	void Update ()
    {
        angle += Time.deltaTime * rotationSpeed;
        if (angle.x > Mathf.PI * 2.0f)
            angle.x -= Mathf.PI * 2.0f;
        if (angle.y > Mathf.PI * 2.0f)
            angle.y -= Mathf.PI * 2.0f;
        if (angle.z > Mathf.PI * 2.0f)
            angle.z -= Mathf.PI * 2.0f;

        Vector3 currentRotation = startRot + new Vector3(Mathf.Sin(angle.x) * amplitude, Mathf.Sin(angle.y) * amplitude, Mathf.Sin(angle.z) * amplitude);
        transform.rotation = Quaternion.identity;
        transform.Rotate(currentRotation);
    }
}
