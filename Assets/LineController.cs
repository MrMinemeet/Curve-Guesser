using System;
using UnityEngine;
using UnityEngine.Serialization;

public class LineController : MonoBehaviour
{
	private const int POINT_COUNT = 40;
	
	
	// Add slider
	[Range(0, (float) Math.PI)]
	public float X_Value = 1.0f;
	[SerializeField]
	private String functionName;
	private LineRenderer _lr;
	private EdgeCollider2D _ec;
	[SerializeField]
	private Camera _camera;
	private Func<float, double> _oldFunc;

	private Vector3 _leftMostPos;
	private Vector3 _rightMostPos;
	private void Awake()
	{
		_lr = GetComponent<LineRenderer>();
		_lr.positionCount = POINT_COUNT;
		_ec = GetComponent<EdgeCollider2D>();
		
		_leftMostPos = _camera.ViewportToWorldPoint(new Vector3(0, 0.5f, _camera.nearClipPlane));
		_rightMostPos = _camera.ViewportToWorldPoint(new Vector3(1, 0.5f, _camera.nearClipPlane));
	}

	// Update is called once per frame
	void Update()
	{
		if(Globals.Functions.ContainsKey(functionName))
		{
			this._oldFunc = Globals.Functions[functionName];
		}
		
		float width = (_rightMostPos.x - _leftMostPos.x);
		var colliderPoints = new Vector2[POINT_COUNT];
		

		// Calculate and draw points for the function
		for (int i = 0; i < POINT_COUNT; i++)
		{
			float normalizedX = (float) i / POINT_COUNT;
			float x = normalizedX * width + _leftMostPos.x;
			
			// Calculate the y value
			var res = _oldFunc(x * X_Value);

			// Set the position of the point
			_lr.SetPosition(i, new Vector3(x, (float)res  * 2, 0));
			colliderPoints[i] = new Vector2(x, (float)res * 2);
			
			// Calculate the x value
			//var res = func(i * X_Value);
			//_lr.SetPosition(i, new Vector3(i, (float) res, 0));
		}
		_ec.points = colliderPoints;
	}
}