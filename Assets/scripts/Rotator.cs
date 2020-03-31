using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    Quaternion originRotation;
    float angle;

    void Start()
    {
        originRotation = transform.rotation;
    }
    void FixedUpdate()
    {
        angle++;

    	Quaternion rotationX = Quaternion.AngleAxis(angle, Vector3.up);


        transform.rotation = originRotation * rotationX;
    }
}
