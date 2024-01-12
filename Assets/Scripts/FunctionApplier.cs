using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionApplier : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public int Points;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        lineRenderer.positionCount = Points;
        for(int i = 0; i < Points; i++)
        {
            float progress = (float) i / (Points - 1);
            float x = Mathf.Lerp(-8.888889f, 8.888889f, progress);
            float y = 5* Mathf.Sin(x);
            lineRenderer.SetPosition(i, new(x, y, 0));
           
            
        }
    }
}
