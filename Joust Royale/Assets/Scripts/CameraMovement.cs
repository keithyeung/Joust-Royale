using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //write me a simple script that moves the camera with wasd
    //and zooms in and out with the mouse wheel

    public float speed = 5f;
    public float zoomSpeed = 5f;
    public float minY = 1;
    public float maxY = 5;
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 direction = transform.forward * scroll * zoomSpeed;
        transform.position += direction;

        // Ensure the camera stays within the desired height range
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
