using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FollowMouse : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private AILerp aiPath;

    private void Start()
    {
        aiPath = GetComponent<AILerp>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(aiPath.destination);
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            aiPath.destination = pos;
        }
    }
}
