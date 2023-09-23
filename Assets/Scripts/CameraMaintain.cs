using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMaintain : MonoBehaviour
{
    [SerializeField]
    private int defaultW;
    [SerializeField]
    private int defaultH;
    [SerializeField]
    private float defaultSize;

    private float defaultResolution;
    Camera cam;

    private void Awake()
    {
        defaultResolution = (float)defaultW / defaultH;
        cam = GetComponent<Camera>();

        cam.orthographicSize = Mathf.Max(defaultSize, defaultSize * defaultResolution / cam.aspect);
    }

    private void Update()
    {
        cam.orthographicSize = Mathf.Max(defaultSize, defaultSize * defaultResolution / cam.aspect);
    }
}
