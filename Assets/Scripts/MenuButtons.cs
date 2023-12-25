using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadSceneAdd(int scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    public void UnloadScene(int scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }
    public void DisplayPanel(GameObject panel)
    {
        if (panel.active)
            panel.SetActive(false);
        else if(!panel.active)
            panel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
