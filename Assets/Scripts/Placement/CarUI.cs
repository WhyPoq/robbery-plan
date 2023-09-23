using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUI : MonoBehaviour
{
    public GameObject canvas;

    public void Placed(float zRotation)
    {
        canvas.transform.Rotate(new Vector3(0, 0, -zRotation));
    }
}
