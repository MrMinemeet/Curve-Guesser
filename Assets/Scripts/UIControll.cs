using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControll: MonoBehaviour
{

    //Main Menu
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
        SceneManager.LoadScene("Scenes/GameScene", LoadSceneMode.Single);
    }

    //Credits

    public void BackToMenu() 
    {
        SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single);
    }


    //Game

    public void Launch()
    {

    }
}
