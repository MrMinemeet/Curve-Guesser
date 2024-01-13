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
    private Coroutine rocketAnimation;

    private List<Level> levels;
    private int currentLevelIndex;

    private Slider a;
    private Slider b;
    private Slider c;
    private Slider d;

    private bool isLaunched = false;

    private List<GameObject> placedElements;

    private GameObject soundSystem;

    private Sprite[] pointSprites;
    private Sprite[] obstacleSprites;
    private GameObject pointPrefab;
    private GameObject obstaclePrefab;

    private GameObject levelFinishedUI;
    private GameObject UI;

    void Start()
    {
        soundSystem = GameObject.Find("SoundSystem");
        DontDestroyOnLoad(soundSystem);

        pointSprites = Resources.LoadAll<Sprite>("Sprites/stars");
        obstacleSprites = Resources.LoadAll<Sprite>("Sprites/asteroids");
        pointPrefab = Resources.Load<GameObject>("Prefabs/Point");
        obstaclePrefab = Resources.Load<GameObject>("Prefabs/Obstacle");

        levelFinishedUI = GameObject.Find("LevelFinishedUI");
        UI = GameObject.Find("UI");
        levelFinishedUI.SetActive(false);

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
        placedElements = new List<GameObject>();
        loadLevel(levels[currentLevelIndex]);
    }

    void FixedUpdate()
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
            soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/rocketlaunch"));
            isLaunched = true;
            rocketAnimation = StartCoroutine(animate());
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
        rocketAnimation = null;
        loadNextScreen();
    }

    private float Y(float x)
    {
        return (float)levels[currentLevelIndex].applyLevelFunction(x, a.value, b.value, c.value, d.value);
    }

    private void loadLevel(Level level)
    {

        foreach(var go in placedElements)
        {
            Destroy(go);
        }
        placedElements.Clear();

        foreach(var obstacle in level.obstacles)
        {
            var go = Instantiate(obstaclePrefab, obstacle, Quaternion.Euler(0,0, Random.Range(0,360)));
            go.GetComponent<SpriteRenderer>().sprite = obstacleSprites[Random.Range(0, obstacleSprites.Length)];
            placedElements.Add(go);
        }
        foreach (var point in level.points)
        {
            var go = Instantiate(pointPrefab, point, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            go.GetComponent<SpriteRenderer>().sprite = pointSprites[Random.Range(0, pointSprites.Length)];
            placedElements.Add(go);
        }
        if(rocketAnimation != null) StopCoroutine(rocketAnimation);
        Rocket.transform.position = new Vector3(15, 0);
        isLaunched = false;
        rocketAnimation = null;
    }

    private void loadNextScreen()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/winning"));
        levelFinishedUI.SetActive(true);
        UI.SetActive(false);
    }

    public void OnPointCollision(GameObject gameObject)
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/collectstar"));
        Vector3 pos = gameObject.transform.position;
        Debug.Log(pos);
        Destroy(gameObject);
    }

    public void OnObstacleCollision()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/rockethit"));
        loadLevel(levels[currentLevelIndex]);
    }
}
