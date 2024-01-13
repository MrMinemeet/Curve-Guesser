using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }
    public void Credits()
    {
        SceneManager.LoadScene("Scenes/Credits", LoadSceneMode.Single);
    }
    public void Play()
    {
        SceneManager.LoadScene("Scenes/LevelSelect", LoadSceneMode.Single);
    }
}
