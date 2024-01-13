using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    private List<Level> levels;
    private int currentLevelIndex;

    private Slider a;
    private Slider b;
    private Slider c;
    private Slider d;

    private bool isLaunched = false;

    void Start()
    {
        difficulty = LevelSelector.difficulty;
        function = LevelSelector.function;
        lineRenderer = GetComponent<LineRenderer>();
        levels = Level.levels.Where(level => level.difficulty == difficulty && level.function == function).ToList();
        currentLevelIndex = 0;

        a = GameObject.Find("UI/SliderGroup/Parameter A").GetComponent<Slider>();
        b = GameObject.Find("UI/SliderGroup/Parameter B").GetComponent<Slider>();
        c = GameObject.Find("UI/SliderGroup/Parameter C").GetComponent<Slider>();
        d = GameObject.Find("UI/SliderGroup/Parameter D").GetComponent<Slider>();

        switch (function)
        {
            case Function.Sine:
                a.minValue = -5;
                a.maxValue = 5;
                a.value = 1;
                b.minValue = -Mathf.PI;
                b.maxValue = Mathf.PI;
                b.value = 1;
                c.minValue = -Mathf.PI;
                c.maxValue = Mathf.PI;
                c.value = 0;
                d.minValue = -4;
                d.maxValue = 4;
                d.value = 0;
                break;
            case Function.Linear: 
                break;
            case Function.Square: 
                break;
            case Function.Cubic: 
                break;
        }
    }

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
            float y = Y(x);
            lineRenderer.SetPosition(i, new(x, y, 0));
        }
    }

    public void Launch()
    {
        if (!isLaunched)
        {
            isLaunched = true;
            StartCoroutine(animate());
        }
    }

    IEnumerator animate() {
        Rocket.transform.position = new Vector3(-15, 0);
        float x = start;
        float y = Y(x);
        for (int i = 0; i < Points; i++)
        {
            float progress = (float)i / (Points - 1);
            float x_next = Mathf.Lerp(start,end, progress);
            float y_next = Y(x_next);

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
        isLaunched = false;
    }

    private float Y(float x)
    {
        return (float)levels[currentLevelIndex].applyLevelFunction(x, a.value, b.value, c.value, d.value);
    }
}
