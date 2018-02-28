using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    Camera zoom;
    float defaultFOV;
    bool isZoom;
    // Use this for initialization
    void Start()
    {
        zoom = Camera.main;
        defaultFOV = zoom.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Camera.main.fieldOfView = defaultFOV / 1.5f;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView = defaultFOV;
        }
    }
}
