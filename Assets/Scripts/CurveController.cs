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
    private TextMeshProUGUI functionText;

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
    private GameObject escapeMenu;
    private bool wasHelpActive;

    private Slider SoundSlider;
    private Toggle ShowGraphToggle;
    private Toggle ShowGridToggle;
    private Toggle ShowEquationToggle;
    private bool ShowGraph = false;
    private bool ShowEquation;


    private List<Level> levels;
    private int currentLevelIndex;
    private int fails = 0;
    private int levelScore = 0;
    private int collectedStars = 0;
    private int globalScore;
    private int globalFails;

    private GameObject helpUI;
    private TextMeshProUGUI helpUItext;

    private GameObject overlay_PI;
    private GameObject overlay_NUMBER;

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
        functionText = GameObject.Find("UI/Function").GetComponent<TextMeshProUGUI>();

        levelFinishedText = GameObject.Find("LevelFinishedUI/NonFunctional/LEVEL DONE").GetComponent<TextMeshProUGUI>();
        nextLevelButton = GameObject.Find("LevelFinishedUI/NextLevelButton");
        trackScoreText = GameObject.Find("LevelFinishedUI/TrackScoreText");

        overlay_PI = GameObject.Find("Background/Overlay1");
        overlay_NUMBER = GameObject.Find("Background/Overlay2");

        helpUI = GameObject.Find("HelpUI");
        helpUItext = GameObject.Find("HelpUI/Bubble/Text (TMP)").GetComponent<TextMeshProUGUI>();
        helpUI.SetActive(false);

        SoundSlider = GameObject.Find("EscapeMenu/Box/Sound").GetComponent<Slider>();
        ShowGraphToggle = GameObject.Find("EscapeMenu/Box/ShowGraph").GetComponent<Toggle>();
        ShowGridToggle = GameObject.Find("EscapeMenu/Box/ShowGrid").GetComponent<Toggle>();
        ShowEquationToggle = GameObject.Find("EscapeMenu/Box/ShowEquation").GetComponent<Toggle>();

        SoundSlider.value = soundSystem.GetComponent<AudioSource>().volume;
        ShowGraphToggle.isOn = TrackData.settings.showGraph;
        ShowGridToggle.isOn = TrackData.settings.showGrid;
        ShowEquationToggle.isOn = TrackData.settings.showFunction;
        SoundSlider.onValueChanged.AddListener(delegate
        {
            soundSystem.GetComponent<AudioSource>().volume = SoundSlider.value;
            TrackData.Set(new Settings(ShowGraph, ShowGridToggle.isOn, ShowEquation, SoundSlider.value));
        });
        ShowGraph = ShowGraphToggle.isOn;
        lineRenderer.enabled = ShowGraphToggle.isOn;
        ShowEquation = ShowEquationToggle.isOn;
        ShowGraphToggle.onValueChanged.AddListener(delegate {
            ShowGraph = ShowGraphToggle.isOn;
            lineRenderer.enabled = ShowGraphToggle.isOn;
            TrackData.Set(new Settings(ShowGraph, ShowGridToggle.isOn, ShowEquation, SoundSlider.value));
        });
        ShowGridToggle.onValueChanged.AddListener(delegate {
            overlay_PI.SetActive(ShowGridToggle.isOn && function == Function.Sine);   
            overlay_NUMBER.SetActive(ShowGridToggle.isOn && function != Function.Sine);
            TrackData.Set(new Settings(ShowGraph, ShowGridToggle.isOn, ShowEquation, SoundSlider.value));
        });
        ShowEquationToggle.onValueChanged.AddListener(delegate {
            ShowEquation = ShowEquationToggle.isOn;
            functionText.text = "";
            TrackData.Set(new Settings(ShowGraph, ShowGridToggle.isOn, ShowEquation, SoundSlider.value));
        });

        escapeMenu = GameObject.Find("EscapeMenu");
        escapeMenu.SetActive(false);

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
                a.minValue = -5000;
                a.maxValue = 5000;
                b.minValue = -6000;
                b.maxValue = 6000;
                c.minValue = -3142;
                c.maxValue = 3142;
                d.minValue = -4000;
                d.maxValue = 4000;
                overlay_NUMBER.SetActive(false);
                overlay_PI.SetActive(ShowGridToggle.isOn);
                break;
            case Function.Linear:
                a.minValue = -5000;
                a.maxValue = 5000;
                b.minValue = -5000;
                b.maxValue = 5000;
                c.minValue = 0;
                c.maxValue = 0;
                d.minValue = 0;
                d.maxValue = 0;
                overlay_NUMBER.SetActive(ShowGridToggle.isOn);
                overlay_PI.SetActive(false);
                break;
            case Function.Square:
                a.minValue = -5000;
                a.maxValue = 5000;
                b.minValue = -8000;
                b.maxValue = 8000;
                c.minValue = -5000;
                c.maxValue = 5000;
                d.minValue = 0;
                d.maxValue = 0;
                overlay_NUMBER.SetActive(ShowGridToggle.isOn);
                overlay_PI.SetActive(false);
                break;
            case Function.Cubic:
                a.minValue = -5000;
                a.maxValue = 5000;
                b.minValue = -10000;
                b.maxValue = 10000;
                c.minValue = -8000;
                c.maxValue = 8000;
                d.minValue = -5000;
                d.maxValue = 5000;
                overlay_NUMBER.SetActive(ShowGridToggle.isOn);
                overlay_PI.SetActive(false);
                break;
        }
        switch (function)
        {
            case Function.Sine:
                a.value = 1000;
                b.value = 1000;
                c.value = 0;
                d.value = 0;
                break;
            case Function.Linear:
            case Function.Square:
            case Function.Cubic:
                a.value = 1000;
                b.value = 0;
                c.value = 0;
                d.value = 0;
                break;
        }

        placedElements = new List<GameObject>();
        loadLevel(levels[currentLevelIndex]);
    }

    void FixedUpdate()
    {
         
    }

    private void Update()
    {
        Draw();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(escapeMenu.activeSelf)
            {
                escapeMenu.SetActive(false);
                helpUI.SetActive(wasHelpActive);
                UI.SetActive(true);
            }
            else
            {
                escapeMenu.SetActive(true);
                wasHelpActive = helpUI.activeSelf;
                helpUI.SetActive(false);
                UI.SetActive(false);
            }
        }
    }

    private void Draw()
    {
        if(ShowGraph)
        {
            lineRenderer.positionCount = Points;
            for (int i = 0; i < Points; i++)
            {
                float progress = (float)i / (Points - 1);
                float x = Mathf.Lerp(start, end, progress);
                float y = Y(x);
                lineRenderer.SetPosition(i, new(x, y, 0));
            }
        }
        if(ShowEquation)
        {
            string tb = "";
            switch (function)
            {
                case Function.Sine:
                    if (a.value != 1000) tb += (a.value / 1000f).ToString("0.00") + "*";
                    tb += "sin(";
                    if (b.value != 1000) tb += (b.value / 1000f).ToString("0.00") + "*";
                    tb += "x";
                    if (c.value > 0) tb += "+" + (c.value / 1000f).ToString("0.00");
                    if (c.value < 0) tb += (c.value / 1000f).ToString("0.00");
                    tb += ")";
                    if (d.value > 0) tb += "+" + (d.value / 1000f).ToString("0.00");
                    if (d.value < 0) tb += (d.value / 1000f).ToString("0.00");

                    break;
                case Function.Linear:
                    if (a.value != 1000) tb += (a.value / 1000f).ToString("0.00") + "*";
                    tb += "x";
                    if (b.value > 0) tb += "+" + (b.value / 1000f).ToString("0.00");
                    if (b.value < 0) tb += (b.value / 1000f).ToString("0.00");
                    break;
                case Function.Square:
                    if (a.value != 1000) tb += (a.value / 1000f).ToString("0.00") + "*";
                    tb += "x^2";
                    if (b.value > 0) tb += "+" + (b.value / 1000f).ToString("0.00") + "x";
                    if (b.value < 0) tb += (b.value / 1000f).ToString("0.00") + "x";
                    if (c.value > 0) tb += "+" + (c.value / 1000f).ToString("0.00");
                    if (c.value < 0) tb += (c.value / 1000f).ToString("0.00");
                    break;
                case Function.Cubic:
                    if (a.value != 1000) tb += (a.value / 1000f).ToString("0.00") + "*";
                    tb += "x^3";
                    if (b.value > 0) tb += "+" + (b.value / 1000f).ToString("0.00") + "x^2";
                    if (b.value < 0) tb += (b.value / 1000f).ToString("0.00") + "x^2";
                    if (c.value > 0) tb += "+" + (c.value / 1000f).ToString("0.00") + "x";
                    if (c.value < 0) tb += (c.value / 1000f).ToString("0.00") + "x";
                    if (d.value > 0) tb += "+" + (d.value / 1000f).ToString("0.00");
                    if (d.value < 0) tb += (d.value / 1000f).ToString("0.00");
                    break;
            }
            functionText.text = tb;
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
        return (float)levels[currentLevelIndex].applyLevelFunction(x, a.value / 1000f, b.value / 1000f, c.value / 1000f, d.value / 1000f);
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
        double distance = levels[currentLevelIndex].DistanceToCurve(pos, a.value / 1000f, b.value / 1000f, c.value / 1000f, d.value / 1000f);
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
        switch (function)
        {
            case Function.Sine:
                a.value = 1000;
                b.value = 1000;
                c.value = 0;
                d.value = 0;
                break;
            case Function.Linear:
            case Function.Square:
            case Function.Cubic:
                a.value = 1000;
                b.value = 0;
                c.value = 0;
                d.value = 0;
                break;
        }
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
        if (currentLevelIndex == levels.Count)
        {
            TrackData.TrackFinished(difficulty, function);
        }
    }

    public void MainMenuEscape()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single);
        TrackData.Set(new TrackInfo(difficulty, function, currentLevelIndex, globalScore, globalFails));
    }

    public void Roger()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        helpUI.SetActive(false);
        UI.SetActive(true);
    }
    public void Play()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        escapeMenu.SetActive(false);
        helpUI.SetActive(wasHelpActive);
        UI.SetActive(true);
    }
}
