using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : PersistentSingleton<LevelManager>
{
    public static event Action LevelLoaded;
    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
        LevelLoaded?.Invoke();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadFinishScreen()
    {
        SceneManager.LoadScene(2);
    }
}
