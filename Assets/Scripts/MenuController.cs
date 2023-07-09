using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayClick()
    {
        SoundManager.Instance.PlayClick();
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
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
