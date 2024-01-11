using System;
using UnityEngine;

public class Move : MonoBehaviour
{
	private const float MIN_THRUST = 1.0f;
	private const float MAX_THRUST = 8f;

	private float _thrust = MIN_THRUST;

	// Update is called once per frame
	void Update()
	{
		float eulerZ = transform.localEulerAngles.z;
		//Debug.Log($"Current rotation: {eulerZ}");
		
		// Basically straight right is 0 and then counter clockwise the angle gets larger to 360
		if (10 < eulerZ && eulerZ <= 90) 
		{
			// Add more thrust
			_thrust = Math.Min(_thrust + (5f * Time.deltaTime), MAX_THRUST);
			//Debug.Log($"Increased thrust to {_thrust}");
		}
		else
		{
			// Decrease thrust
			_thrust = Math.Max(_thrust - (10f * Time.deltaTime), MIN_THRUST);
			//Debug.Log($"Reduced thrust to {_thrust}");
		}
		
		// Apply thrust
		transform.position += transform.right * (_thrust * Time.deltaTime);
	}
}