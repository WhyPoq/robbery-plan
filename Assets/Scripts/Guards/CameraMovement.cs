using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float time = 5f;
    public float angle1 = -10;
    public float angle2 = 30;

    public float curTime;
    private bool forward = true;
    private bool playing = false;
    public bool loop = false;

    public void Start()
    {
        curTime = time / 2;
        Vector3 angles = transform.eulerAngles;
        angles.z = angle1 + (angle2 - angle1) * (curTime / time);
        transform.eulerAngles = angles;
    }

    public void Play()
    {
        curTime = time / 2;
        playing = true;
    }

    public void Stop()
    {
        curTime = time / 2;
        playing = false;
        forward = true;
        Vector3 angles = transform.eulerAngles;
        angles.z = angle1 + (angle2 - angle1) * (curTime / time);
        transform.eulerAngles = angles;
    }

    private void Update()
    {
        if (!playing) return;

        curTime += Time.deltaTime;
        if(curTime > time)
        {
            curTime = 0;
            if (!loop)
            {
                forward = !forward;
            }
        }

        Vector3 angles = transform.eulerAngles;
        if (forward)
        {
            angles.z = angle1 + (angle2 - angle1) * (curTime / time);
        }
        else
        {
            angles.z = angle2 + (angle1 - angle2) * (curTime / time);
        }

        transform.eulerAngles = angles;
    }
}
