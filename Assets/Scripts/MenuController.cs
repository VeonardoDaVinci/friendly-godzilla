using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    
    // :)
    public void PlayClick()
    {
        SoundManager.Instance.PlayClick();
    }
    public void LoadLevel()
    {
        LevelManager.Instance.LoadLevel();
    }

    public void LoadMenu()
    {
        LevelManager.Instance.LoadMenu();
    }

    public void LoadFinishScreen()
    {
        LevelManager.Instance.LoadFinishScreen();
    }
}
