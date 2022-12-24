using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PositionClamp : MonoBehaviour
{

    void Update()
    {
        transform.localPosition = new Vector3(
            Math.Clamp(transform.localPosition.x, -5, 5),
            Math.Clamp(transform.localPosition.y, -5, 5),
            Math.Clamp(transform.localPosition.z, -5, 5)
        );
        
    }
}

