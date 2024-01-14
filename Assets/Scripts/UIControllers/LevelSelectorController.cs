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


    private GameObject soundSystem;

    private void Start()
    {
        soundSystem = GameObject.Find("SoundSystem");
        DontDestroyOnLoad(soundSystem);

        SineText = GameObject.Find("FunctionGrid/Sine/Text (TMP)").GetComponent<TextMeshProUGUI>();
        LinearText = GameObject.Find("FunctionGrid/Linear/Text (TMP)").GetComponent<TextMeshProUGUI>();
        SquareText = GameObject.Find("FunctionGrid/Square/Text (TMP)").GetComponent<TextMeshProUGUI>();
        CubicText = GameObject.Find("FunctionGrid/Cubic/Text (TMP)").GetComponent<TextMeshProUGUI>();

        TutorialText = GameObject.Find("DifficultyGrid/Tutorial/Text (TMP)").GetComponent<TextMeshProUGUI>();
        EasyText = GameObject.Find("DifficultyGrid/Easy/Text (TMP)").GetComponent<TextMeshProUGUI>();
        MediumText = GameObject.Find("DifficultyGrid/Medium/Text (TMP)").GetComponent<TextMeshProUGUI>();
        HardText = GameObject.Find("DifficultyGrid/Hard/Text (TMP)").GetComponent<TextMeshProUGUI>();
        ExpertText = GameObject.Find("DifficultyGrid/Expert/Text (TMP)").GetComponent<TextMeshProUGUI>();
}

    public void Sine()
    {
        function = Function.Sine;
        SineText.color = orange;
        LinearText.color = yellow;
        SquareText.color = yellow;
        CubicText.color = yellow;
    }

    public void Linear()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        function = Function.Linear;
        SineText.color = yellow;
        LinearText.color = orange;
        SquareText.color = yellow;
        CubicText.color = yellow;
    }

    public void Square()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        function = Function.Square;
        SineText.color = yellow;
        LinearText.color = yellow;
        SquareText.color = orange;
        CubicText.color = yellow;
    }

    public void Cubic()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        function = Function.Cubic;
        SineText.color = yellow;
        LinearText.color = yellow;
        SquareText.color = yellow;
        CubicText.color = orange;
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
        if(info == null)
        {
            LoadedFails = 0;
            LoadedScore = 0;
            LoadedLevel = 0;
        }
        else
        {
            Debug.Log(info.currentLevel);
            Debug.Log(info.score);
            Debug.Log(info.fails);
            LoadedFails = info.fails;
            LoadedScore = info.score;
            LoadedLevel = info.currentLevel;
        }
        SceneManager.LoadScene("Scenes/GameScene", LoadSceneMode.Single);


    }
}
