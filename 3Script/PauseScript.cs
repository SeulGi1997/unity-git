using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject[] setActiveUI;

    public void GamePause()
    {
        for(int i = 0; i < setActiveUI.Length; i++)
        {
            setActiveUI[i].SetActive(false);
        }

        pauseUI.SetActive(true);
    }

    public void GameContinu()
    {
        for (int i = 0; i < setActiveUI.Length; i++)
        {
            setActiveUI[i].SetActive(true);
        }

        pauseUI.SetActive(false);
    }

    public void TitleMove()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GameQuit()
    {
        Application.Quit();
    }

}
