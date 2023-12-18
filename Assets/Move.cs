using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	private const float MAX_THRUST = 10.0f;

	[SerializeField] private float _thrust = 2.0f;

	// Update is called once per frame
	void Update()
	{
		Debug.Log(transform.localEulerAngles.z + "\t\t" + transform.localRotation.z);

		// FIXME: The detection of rotation doesn't work properly yet. Because inspector rotation != transform.rotation
		// Check if object is rotated to the bottom left corner 
		if (transform.localEulerAngles.z > 350)
		{
			_thrust = 2f;
			Debug.Log($"Reset thrust to {_thrust}");
		}
		// Check if object is rotated to the top right corner 
		else if (transform.localEulerAngles.z > 20)
		{
			// Add more thrust
			_thrust = Math.Max(_thrust + (0.01f * Time.deltaTime), MAX_THRUST);
			Debug.Log($"Increased thrust to {_thrust}");

			// Apply some on the x axis with respect to time and rotation
			//transform.position += transform.right * _thrust * Time.deltaTime;
		}
	}
}