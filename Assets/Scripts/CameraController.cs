using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 0.5f;
    public Camera mainCamera = Camera.main;  // assign in the inspector

    private float cameraSize;

    void Start()
    {
        cameraSize = mainCamera.orthographicSize;
    }


    void Update()
    {
        float xAxisValue = Input.GetAxis("Horizontal") * speed * cameraSize / 100;
        float yAxisValue = Input.GetAxis("Vertical") * speed * cameraSize / 100;
        float zAxisValue = 0.0f;

        if (Input.GetKey(KeyCode.Q))
        {
            if (cameraSize < 100)
            {
                cameraSize += speed * cameraSize / 100;
                mainCamera.orthographicSize = cameraSize;
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (cameraSize > 5)
            {
                cameraSize -= speed * cameraSize / 100;
                mainCamera.orthographicSize = cameraSize;
            }
        }

        transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y + yAxisValue, transform.position.z + zAxisValue);
    }
}
