using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    private static TitleScript title;
    

    private SaveNLoad saveNLoad;
    private LodingScript loding;
                

    private void Awake()
    {

        if(title == null)
        {
            title = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void NewGameStart()
    {
        StartCoroutine(NewGameStartCoroutine());
    }

    public void LoadGameStart()
    {
        StartCoroutine(LoadGameStartCoroutine());    
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator NewGameStartCoroutine()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("LodingScene");

        while(!ao.isDone)
        {
            yield return null;
        }

        loding = FindObjectOfType<LodingScript>();
        loding.NewGameStart();

        Destroy(this.gameObject);

    }


    private IEnumerator LoadGameStartCoroutine()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("LodingScene");

        while(!ao.isDone)
        {
            yield return null;
        }

        loding = FindObjectOfType<LodingScript>();
        loding.LoadGameStart();

        /*saveNLoad = FindObjectOfType<SaveNLoad>();
        saveNLoad.PlayDataLoad();*/

        Destroy(this.gameObject);

    }

}
