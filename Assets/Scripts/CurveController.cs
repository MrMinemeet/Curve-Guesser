using System.Collections;
using UnityEngine;
using static Level;

public class CurveController : MonoBehaviour
{
    public GameObject Rocket;
    public LineRenderer lineRenderer;
    public float RocketSpeed = 1;
    public int Points;
    
    private Difficulty difficulty;
    private Function function;

    private float start = -10f;
    private float end = 10f;
    // Start is called before the first frame update
    void Start()
    {
        difficulty = LevelSelector.difficulty;
        function = LevelSelector.function;
        lineRenderer = GetComponent<LineRenderer>();
        Debug.Log("Loaded");
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
            float x = Mathf.Lerp(start, end, progress);
            float y = Mathf.Sin(x);
            lineRenderer.SetPosition(i, new(x, y, 0));
        }
    }

    public void Launch()
    {
        StartCoroutine(animate());
    }

    IEnumerator animate() {
        Rocket.transform.position = new Vector3(-15, 0);
        float x = start;
        float y = Mathf.Sin(x);
        for (int i = 0; i < Points; i++)
        {
            float progress = (float)i / (Points - 1);
            float x_next = Mathf.Lerp(start,end, progress);
            float y_next = Mathf.Sin(x_next);
            
            Vector3 thisPos = new Vector3(x, y);
            Vector3 nextPos = new Vector3(x_next, y_next);
            Vector3 direction = (nextPos - thisPos).normalized;

            Rocket.transform.position = new Vector3(x, y);
            Rocket.transform.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI) -90);

            x = x_next;
            y = y_next;
            yield return new WaitForSeconds(1 / (RocketSpeed * (Points / 3)));
        }
        Rocket.transform.position = new Vector3(15,0);
    }
}
