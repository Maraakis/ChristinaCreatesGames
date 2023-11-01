using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Christina
{
    public class TrackerAtMousePosition : MonoBehaviour
    {
        private void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }
}
