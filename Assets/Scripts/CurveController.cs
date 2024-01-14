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
    private TextMeshProUGUI globalScoreText;
    private TextMeshProUGUI globalFailsText;

    private TextMeshProUGUI levelFinishedText;
    private GameObject nextLevelButton;
    private GameObject trackScoreText;

    private bool isLaunched = false;

    private List<GameObject> placedElements;

    private GameObject soundSystem;

    private Sprite[] pointSprites;
    private Sprite[] obstacleSprites;
    private GameObject pointPrefab;
    private GameObject obstaclePrefab;

    private GameObject levelFinishedUI;
    private GameObject UI;


    private List<Level> levels;
    private int currentLevelIndex;
    private int fails = 0;
    private int levelScore = 0;
    private int collectedStars = 0;
    private int globalScore;
    private int globalFails;

    private GameObject helpUI;
    private TextMeshProUGUI helpUItext;

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
        globalScoreText = GameObject.Find("UI/ScoreText").GetComponent<TextMeshProUGUI>();
        globalFailsText = GameObject.Find("UI/FailsText").GetComponent<TextMeshProUGUI>();

        levelFinishedText = GameObject.Find("LevelFinishedUI/NonFunctional/LEVEL DONE").GetComponent<TextMeshProUGUI>();
        nextLevelButton = GameObject.Find("LevelFinishedUI/NextLevelButton");
        trackScoreText = GameObject.Find("LevelFinishedUI/TrackScoreText");

        helpUI = GameObject.Find("HelpUI");
        helpUItext = GameObject.Find("HelpUI/Bubble/Text (TMP)").GetComponent<TextMeshProUGUI>();
        helpUI.SetActive(false);

        levelFinishedUI.SetActive(false);

        difficulty = LevelSelector.difficulty;
        function = LevelSelector.function;
        lineRenderer = GetComponent<LineRenderer>();
        levels = Level.levels.Where(level => level.difficulty == difficulty && level.function == function).ToList();
        currentLevelIndex = LevelSelector.LoadedLevel;
        globalScore = LevelSelector.LoadedScore;
        globalFails = LevelSelector.LoadedFails;
        globalFailsText.text = "Fails: " + globalFails;
        globalScoreText.text = "Score: " + globalScore;


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
            parA.SetActive(false);
            parAName.SetActive(false);
            parB.SetActive(false);
            parBName.SetActive(false);
            parC.SetActive(false);
            parCName.SetActive(false);
            parD.SetActive(false);
            parDName.SetActive(false);
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
        levelScore = 0;
        TrackData.Set(new TrackInfo(difficulty, function, currentLevelIndex, globalScore, globalFails));
        if(level.hint != null)
        {
            helpUI.SetActive(true);
            helpUItext.text = level.hint;
            UI.SetActive(false);
        }
    }

    private void loadNextScreen()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/winning"));
        
        levelFinishedUI.SetActive(true);
        UI.SetActive(false);
        if (currentLevelIndex == levels.Count - 1)
        {
            levelFinishedText.text = "Track Complete";
            nextLevelButton.SetActive(false);
            starsText.text = "Stars: " + collectedStars + "/" + levels[currentLevelIndex].points.Count;
            failsText.text = "Fails: " + fails;
            scoreText.text = "Score: " + levelScore;
            trackScoreText.SetActive(true);
            trackScoreText.GetComponent<TextMeshProUGUI>().text = "Track Score\n" + globalScore;
        }
        else
        {
            nextLevelButton.SetActive(true);
            starsText.text = "Stars: " + collectedStars + "/" + levels[currentLevelIndex].points.Count;
            failsText.text = "Fails: " + fails;
            scoreText.text = "Score: " + levelScore;
            trackScoreText.SetActive(false);
        }
    }

    public void OnPointCollision(GameObject gameObject)
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/collectstar"));
        Vector3 pos = gameObject.transform.position;
        double distance = levels[currentLevelIndex].DistanceToCurve(pos, a.value, b.value, c.value, d.value);
        Destroy(gameObject);
        collectedStars++;
        int starpoints = Mathf.Max(100 - (int)(100 * distance), 0);
        levelScore += starpoints;
        globalScore += starpoints;
        globalScoreText.text = "Score: " + globalScore;
    }

    public void OnObstacleCollision()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/rockethit"));
        fails++;
        globalFails++;
        globalScore -= levelScore;
        globalScoreText.text = "Score: " + globalScore;
        globalFailsText.text = "Fails: " + globalFails;
        loadLevel(levels[currentLevelIndex]);
    }


    //NextLevelUI

    public void NextLevel()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        levelFinishedUI.SetActive(false);
        UI.SetActive(true);
        loadLevel(levels[++currentLevelIndex]);
        fails = 0;
    }

    public void Retry()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        levelFinishedUI.SetActive(false);
        UI.SetActive(true);
        fails++;
        globalFails++;
        globalFailsText.text = "Fails: " + globalFails;
        globalScore -= levelScore;
        globalScoreText.text = "Score: " + globalScore;
        loadLevel(levels[currentLevelIndex]);
    }

    public void MainMenu()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single);
        TrackData.Set(new TrackInfo(difficulty, function, ++currentLevelIndex, globalScore, globalFails));
    }

    public void Roger()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        helpUI.SetActive(false);
        UI.SetActive(true);
    }
}
