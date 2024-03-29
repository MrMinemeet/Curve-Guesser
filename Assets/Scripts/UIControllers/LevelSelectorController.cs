using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public static Difficulty difficulty = Difficulty.Tutorial;
    public static Function function = Function.Sine;
    public static int LoadedScore;
    public static int LoadedFails;
    public static int LoadedLevel;

    private static GameObject SineButton;
    private static GameObject LinearButton;
    private static GameObject SquareButton;
    private static GameObject CubicButton;

    private static GameObject TutorialButton;
    private static GameObject EasyButton;
    private static GameObject MediumButton;
    private static GameObject HardButton;
    private static GameObject ExpertButton;

    private static TextMeshProUGUI SineText;
    private static TextMeshProUGUI LinearText;
    private static TextMeshProUGUI SquareText;
    private static TextMeshProUGUI CubicText;

    private static TextMeshProUGUI TutorialText;
    private static TextMeshProUGUI EasyText;
    private static TextMeshProUGUI MediumText;
    private static TextMeshProUGUI HardText;
    private static TextMeshProUGUI ExpertText;

    private static Color yellow = new(0.925f, 1f, 0f);
    private static Color orange = new(1f, 0.5f, 0f);

    private GameObject loadGameUI;


    private GameObject soundSystem;

    private void Start()
    {
        difficulty = Difficulty.Tutorial;
        function = Function.Sine;
        soundSystem = GameObject.Find("SoundSystem");
        DontDestroyOnLoad(soundSystem);

        SineButton = GameObject.Find("FunctionGrid/Sine");
        LinearButton = GameObject.Find("FunctionGrid/Linear");
        SquareButton = GameObject.Find("FunctionGrid/Square");
        CubicButton = GameObject.Find("FunctionGrid/Cubic");

        TutorialButton = GameObject.Find("DifficultyGrid/Tutorial");
        EasyButton = GameObject.Find("DifficultyGrid/Easy");
        MediumButton = GameObject.Find("DifficultyGrid/Medium");
        HardButton = GameObject.Find("DifficultyGrid/Hard");
        ExpertButton = GameObject.Find("DifficultyGrid/Expert");

        SineText = GameObject.Find("FunctionGrid/Sine/Text (TMP)").GetComponent<TextMeshProUGUI>();
        LinearText = GameObject.Find("FunctionGrid/Linear/Text (TMP)").GetComponent<TextMeshProUGUI>();
        SquareText = GameObject.Find("FunctionGrid/Square/Text (TMP)").GetComponent<TextMeshProUGUI>();
        CubicText = GameObject.Find("FunctionGrid/Cubic/Text (TMP)").GetComponent<TextMeshProUGUI>();

        TutorialText = GameObject.Find("DifficultyGrid/Tutorial/Text (TMP)").GetComponent<TextMeshProUGUI>();
        EasyText = GameObject.Find("DifficultyGrid/Easy/Text (TMP)").GetComponent<TextMeshProUGUI>();
        MediumText = GameObject.Find("DifficultyGrid/Medium/Text (TMP)").GetComponent<TextMeshProUGUI>();
        HardText = GameObject.Find("DifficultyGrid/Hard/Text (TMP)").GetComponent<TextMeshProUGUI>();
        ExpertText = GameObject.Find("DifficultyGrid/Expert/Text (TMP)").GetComponent<TextMeshProUGUI>();

        SetActiveDifficulties();

        loadGameUI = GameObject.Find("Canvas/LoadGame");
        loadGameUI.SetActive(false);
    }

    private void SetActiveDifficulties()
    {
        LinearButton.SetActive(TrackData.finishedTracks.Contains((Difficulty.Tutorial, Function.Sine)));
        SquareButton.SetActive(TrackData.finishedTracks.Contains((Difficulty.Tutorial, Function.Linear)));
        CubicButton.SetActive(TrackData.finishedTracks.Contains((Difficulty.Tutorial, Function.Square)));

        EasyButton.SetActive(TrackData.finishedTracks.Contains((Difficulty.Tutorial, function)));
        MediumButton.SetActive(TrackData.finishedTracks.Contains((Difficulty.Easy, function)));
        HardButton.SetActive(TrackData.finishedTracks.Contains((Difficulty.Medium, function)));
        ExpertButton.SetActive(TrackData.finishedTracks.Contains((Difficulty.Hard, function)));
    }

    public void Sine()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        function = Function.Sine;
        SineText.color = orange;
        LinearText.color = yellow;
        SquareText.color = yellow;
        CubicText.color = yellow;

        SetActiveDifficulties();
    }

    public void Linear()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        function = Function.Linear;
        SineText.color = yellow;
        LinearText.color = orange;
        SquareText.color = yellow;
        CubicText.color = yellow;

        SetActiveDifficulties();
    }

    public void Square()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        function = Function.Square;
        SineText.color = yellow;
        LinearText.color = yellow;
        SquareText.color = orange;
        CubicText.color = yellow;

        SetActiveDifficulties();
    }

    public void Cubic()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        function = Function.Cubic;
        SineText.color = yellow;
        LinearText.color = yellow;
        SquareText.color = yellow;
        CubicText.color = orange;

        SetActiveDifficulties();
    }

    public void Tutorial()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        difficulty = Difficulty.Tutorial;
        TutorialText.color = orange;
        EasyText.color = yellow;
        MediumText.color = yellow;
        HardText.color = yellow;
        ExpertText.color = yellow;
    }

    public void Easy()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        difficulty = Difficulty.Easy;
        TutorialText.color = yellow;
        EasyText.color = orange;
        MediumText.color = yellow;
        HardText.color = yellow;
        ExpertText.color = yellow;
    }

    public void Medium()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        difficulty = Difficulty.Medium;
        TutorialText.color = yellow;
        EasyText.color = yellow;
        MediumText.color = orange;
        HardText.color = yellow;
        ExpertText.color = yellow;
    }

    public void Hard()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        difficulty = Difficulty.Hard;
        TutorialText.color = yellow;
        EasyText.color = yellow;
        MediumText.color = yellow;
        HardText.color = orange;
        ExpertText.color = yellow;
    }

    public void Expert()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        difficulty = Difficulty.Expert;
        TutorialText.color = yellow;
        EasyText.color = yellow;
        MediumText.color = yellow;
        HardText.color = yellow;
        ExpertText.color = orange;
    }

    public void Back()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single);
    }

    public void StartGame()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        TrackInfo info = TrackData.Get(difficulty, function);
        if(info == null || Level.levels.Where(level => level.difficulty == difficulty && level.function == function).ToList().Count == info.currentLevel)
        {
            TrackData.Set(new TrackInfo(difficulty, function, 0, 0, 0));
            LoadedFails = 0;
            LoadedScore = 0;
            LoadedLevel = 0;
            SceneManager.LoadScene("Scenes/GameScene", LoadSceneMode.Single);
        }
        else
        {
            loadGameUI.SetActive(true);
        }
    }

    public void Continue()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        TrackInfo info = TrackData.Get(difficulty, function);
        LoadedFails = info.fails;
        LoadedScore = info.score;
        LoadedLevel = info.currentLevel;
        SceneManager.LoadScene("Scenes/GameScene", LoadSceneMode.Single);
    }

    public void NewGame()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        LoadedFails = 0;
        LoadedScore = 0;
        LoadedLevel = 0;
        SceneManager.LoadScene("Scenes/GameScene", LoadSceneMode.Single);
    }
}
