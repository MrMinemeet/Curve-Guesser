using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float thrust = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // Apply some on the x axis with respect to time and rotation
        transform.position += transform.right * thrust * Time.deltaTime;
    }
}
