using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private GameObject soundSystem;

    private void Start()
    {
        soundSystem = GameObject.Find("SoundSystem");
        DontDestroyOnLoad(soundSystem);
    }
    public void Exit()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        Application.Quit();
    }
    public void Credits()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        SceneManager.LoadScene("Scenes/Credits", LoadSceneMode.Single);
    }
    public void Play()
    {
        soundSystem.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/buttonpress"));
        SceneManager.LoadScene("Scenes/LevelSelect", LoadSceneMode.Single);
    }
}
