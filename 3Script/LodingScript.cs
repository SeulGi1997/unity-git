using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class LodingScript : MonoBehaviour
{
    [SerializeField]
    private Image lodingBarImg;
    [SerializeField]
    private GameObject lodingBackground;
    [SerializeField]
    private Image FadeBackGround;
    [SerializeField]
    private Text hint;

    private SaveNLoad saveNLoad;
    private TutorialScript tutorialScript;

    public static LodingScript loding;

    private void Awake()
    {

        if (loding == null)
        {
            loding = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lodingBarImg.fillAmount = 0f;
        SetHitText();
    }

    public void NewGameStart()
    {
        StartCoroutine(NewGameStartCoroutine());
    }

    public void LoadGameStart()
    {
        StartCoroutine(LoadGameStartCoroutine());
    }


    private IEnumerator NewGameStartCoroutine()
    {
        yield return new WaitForSeconds(0.3f);

        AsyncOperation ao = SceneManager.LoadSceneAsync("MainScene");
        ao.allowSceneActivation = false;

        while (!ao.isDone && ao.allowSceneActivation == false)
        {
            yield return null;

            if (ao.progress < 0.9f)
            {
                lodingBarImg.fillAmount = ao.progress;
            }
            else
            {
                lodingBarImg.fillAmount += 0.1f * Time.deltaTime;

                if (lodingBarImg.fillAmount >= 0.99f)
                {
                    ao.allowSceneActivation = true;
                }
            }

        }

        yield return null;
        tutorialScript = FindObjectOfType<TutorialScript>();
        tutorialScript.StartTutorial();

        StartCoroutine(FadeOut());
    }

    private IEnumerator LoadGameStartCoroutine()
    {
        yield return new WaitForSeconds(0.3f);

        AsyncOperation ao = SceneManager.LoadSceneAsync("MainScene");
        ao.allowSceneActivation = false;
        
        while (!ao.isDone )
        {
            yield return null;

            if(ao.progress < 0.9f)
            {
                lodingBarImg.fillAmount = ao.progress;
            }
            else
            {
                lodingBarImg.fillAmount += 0.1f * Time.deltaTime;

                if (lodingBarImg.fillAmount >= 0.99f)
                {
                    ao.allowSceneActivation = true;

                }
            }
            
        }

        yield return null;
        saveNLoad = FindObjectOfType<SaveNLoad>();
        saveNLoad.PlayDataLoad();


        StartCoroutine(FadeOut());

    }

    IEnumerator FadeOut()
    {
        hint.text = "";
        lodingBackground.SetActive(false);

        Color color = FadeBackGround.color;
        color.a = 1f;
        FadeBackGround.color = color;

        yield return new WaitForSeconds(0.5f);

        while (color.a > 0f)
        {
            color.a -= 0.2f;
            FadeBackGround.color = color;

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(this.gameObject);
    }

    private void SetHitText()
    {
        int num = Random.Range(0, 2);

        string _text = "";

        switch (num)
        {
            case 0:
                _text = "힌트 : 불을 피워 고기를 익혀드세요";
                break;
            case 1:
                _text = "힌트 : 쉼터를 지어 저장할 수 있습니다";
                break;
       
        }

        hint.text = _text;
    }

}
