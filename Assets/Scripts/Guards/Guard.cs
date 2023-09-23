using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Guard : MonoBehaviour
{
    public GameManager gameManager;
    FOV fov;

    private void Start()
    {
        fov = GetComponent<FOV>();
        fov.curRotation = -transform.eulerAngles.z;
    }

    private void Update()
    {
        fov.curRotation = -transform.eulerAngles.z;

        if (GameManager.gamePaused) return;

        if(fov.targets.Count > 0)
        {
            fov.targets.Clear();
            gameManager.GotCaught();
        }
    }
}
