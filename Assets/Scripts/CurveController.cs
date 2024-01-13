using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private GameObject parA;
    private GameObject parB;
    private GameObject parC;
    private GameObject parD;

    private GameObject parAName;
    private GameObject parBName;
    private GameObject parCName;
    private GameObject parDName;

    private TextMeshProUGUI starsText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI failsText;

    private bool isLaunched = false;

    private List<GameObject> placedElements;

    private GameObject soundSystem;

    private Sprite[] pointSprites;
    private Sprite[] obstacleSprites;
    private GameObject pointPrefab;
    private GameObject obstaclePrefab;

    private GameObject levelFinishedUI;
    private GameObject UI;

    private int fails = 0;
    private int collectedStars = 0;

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

        starsText = GameObject.Find("LevelFinishedUI/StarsText").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("LevelFinishedUI/ScoreText").GetComponent<TextMeshProUGUI>();
        failsText = GameObject.Find("LevelFinishedUI/FailsText").GetComponent<TextMeshProUGUI>();

        levelFinishedUI.SetActive(false);

        difficulty = LevelSelector.difficulty;
        function = LevelSelector.function;
        lineRenderer = GetComponent<LineRenderer>();
        levels = Level.levels.Where(level => level.difficulty == difficulty && level.function == function).ToList();
        currentLevelIndex = 0;


        parA = GameObject.Find("UI/SliderGroup/Parameter A");
        parB = GameObject.Find("UI/SliderGroup/Parameter B");
        parC = GameObject.Find("UI/SliderGroup/Parameter C");
        parD = GameObject.Find("UI/SliderGroup/Parameter D");

        parAName = GameObject.Find("UI/ParameterNameGroup/A");
        parBName = GameObject.Find("UI/ParameterNameGroup/B");
        parCName = GameObject.Find("UI/ParameterNameGroup/C");
        parDName = GameObject.Find("UI/ParameterNameGroup/D");

        a = parA.GetComponent<Slider>();
        b = parB.GetComponent<Slider>();
        c = parC.GetComponent<Slider>();
        d = parD.GetComponent<Slider>();

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
        parA.SetActive(level.parameters.Contains(Parameter.a));
        parAName.SetActive(level.parameters.Contains(Parameter.a));
        parB.SetActive(level.parameters.Contains(Parameter.b));
        parBName.SetActive(level.parameters.Contains(Parameter.b));
        parC.SetActive(level.parameters.Contains(Parameter.c));
        parCName.SetActive(level.parameters.Contains(Parameter.c));
        parD.SetActive(level.parameters.Contains(Parameter.d));
        parDName.SetActive(level.parameters.Contains(Parameter.d));

        foreach (var go in placedElements)
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
        collectedStars = 0;
    }

    private void loadNextScreen()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/winning"));
        levelFinishedUI.SetActive(true);
        UI.SetActive(false);
        starsText.text = "Stars: " + collectedStars + "/" + levels[currentLevelIndex].points.Count;
        failsText.text = "Fails: " + fails;
    }

    public void OnPointCollision(GameObject gameObject)
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/collectstar"));
        Vector3 pos = gameObject.transform.position;
        Debug.Log(pos);
        Destroy(gameObject);
        collectedStars++;
    }

    public void OnObstacleCollision()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/rockethit"));
        loadLevel(levels[currentLevelIndex]);
        fails++;
    }


    //NextLevelUI

    public void NextLevel()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        loadLevel(levels[++currentLevelIndex]);
        levelFinishedUI.SetActive(false);
        UI.SetActive(true);
    }

    public void Retry()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        loadLevel(levels[currentLevelIndex]);
        levelFinishedUI.SetActive(false);
        UI.SetActive(true);
        fails++;
    }

    public void MainMenu()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single);
    }
}
