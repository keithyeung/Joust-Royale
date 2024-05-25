using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMotion : MonoBehaviour
{
    public float speed = 1.0f;
    public float radius = 1.0f;
    private float angle = 0.0f;

    void Update()
    {
        angle += speed * Time.deltaTime; // if you want to switch direction, use -= instead of +=

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        transform.localPosition = new Vector3(x, transform.localPosition.y, z);
    }
}
