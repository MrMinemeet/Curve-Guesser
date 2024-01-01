using System;
using System.Linq;
using UnityEngine;

public class LineController : MonoBehaviour
{
	private const int POINT_COUNT = 500;
	private const float TOLERANCE = 0.0001f;
	
	
	// Add slider
	[Range(0, (float) Math.PI)]
	public float X_Value = 1.0f;
	private float _oldXValue = 1.0f;
	[SerializeField]
	private String functionName;
	private LineRenderer _lr;
	private EdgeCollider2D _ec;
    private PolygonCollider2D _pc;
	private Camera _camera;
	private String _oldFuncName = "";

	private readonly Vector3 _leftMostPos = new Vector3(-50f, 0f, 0f);
	private readonly Vector3 _rightMostPos = new Vector3(50f, 0f, 0f);
	private void Awake()
	{
		_lr = GetComponent<LineRenderer>();
		_lr.positionCount = POINT_COUNT;
		_ec = GetComponent<EdgeCollider2D>();
		_pc = GetComponent<PolygonCollider2D>();
	}

	// Update is called once per frame
	void Update()
	{
		// Skip if the function and X-Value didn't change
		if (functionName == _oldFuncName && Math.Abs(_oldXValue - X_Value) < TOLERANCE)
		{
			return;
		}
		
		if (!Globals.Functions.ContainsKey(functionName))
		{
			Debug.Log($"Unknown function name: {functionName}");
			return;
		}
		_oldXValue = X_Value;
		_oldFuncName = functionName;
		
		// Get the function
		var func = Globals.Functions[functionName];
		
		
		
		float width = (_rightMostPos.x - _leftMostPos.x);
		var colliderPoints = new Vector2[POINT_COUNT + 2];
		

		// Calculate and draw points for the function
		for (int i = 0; i < POINT_COUNT; i++)
		{
			float normalizedX = (float) i / POINT_COUNT;
			float x = normalizedX * width + _leftMostPos.x;
			
			// Calculate the y value
			var res = func(x * X_Value);

			// Set the position of the point
			_lr.SetPosition(i, new Vector3(x, (float)res  * 2, 0));
			colliderPoints[i + 1] = new Vector2(x, (float)res * 2);
			
			// Calculate the x value
			//var res = func(i * X_Value);
			//_lr.SetPosition(i, new Vector3(i, (float) res, 0));
		}
		colliderPoints[0] = new Vector2(colliderPoints.Min(v => v.x), colliderPoints.Min(v => v.y));
		colliderPoints[POINT_COUNT + 1] = new Vector2(colliderPoints.Max(v => v.x), colliderPoints.Min(v => v.y));
		_pc.points = colliderPoints;
	}
}