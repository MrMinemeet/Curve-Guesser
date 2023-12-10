using System;
using UnityEngine;
using UnityEngine.Serialization;

public class LineController : MonoBehaviour
{
	// Add slider
	[Range(0, (float) Math.PI)]
	public float X_Value = 1.0f;
	[SerializeField]
	private String functionName;
	private LineRenderer _lr;
	[SerializeField]
	private Camera _camera;

	private Vector3 _leftMostPos;
	private Vector3 _rightMostPos;

	private const int POINT_COUNT = 200;
	private void Awake()
	{
		_lr = GetComponent<LineRenderer>();
		_lr.positionCount = POINT_COUNT;
		
		_leftMostPos = _camera.ViewportToWorldPoint(new Vector3(0, 0.5f, _camera.nearClipPlane));
		_rightMostPos = _camera.ViewportToWorldPoint(new Vector3(1, 0.5f, _camera.nearClipPlane));
	}

	// Update is called once per frame
	void Update()
	{
		var func = Globals.Functions[functionName];
		float width = (_rightMostPos.x - _leftMostPos.x);


		// Calculate and draw points for the function
		for (int i = 0; i < POINT_COUNT; i++)
		{
			float normalizedX = (float) i / POINT_COUNT;
			float x = normalizedX * width + _leftMostPos.x;
			
			// Calculate the y value
			var res = func(x * X_Value);

			// Set the position of the point
			_lr.SetPosition(i, new Vector3(x, (float)res, 0));
		
			
			// Calculate the x value
			//var res = func(i * X_Value);
			//_lr.SetPosition(i, new Vector3(i, (float) res, 0));
		}
	}
}