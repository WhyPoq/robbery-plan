using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentListener : MonoBehaviour
{
    private void Start()
    {
        if (SceneLoader.instance != null)
        {
            SceneLoader.instance.RemoveTransitionCamera();
        }
    }

}
