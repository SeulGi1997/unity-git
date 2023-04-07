using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    private Image blackBackGround;

    private const float fadeSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color color = blackBackGround.color;
        color.a = 1f;
        blackBackGround.color = color;

        yield return new WaitForSeconds(0.5f);

        while (color.a > 0f)
        {
            color.a -= fadeSpeed;
            blackBackGround.color = color;

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(blackBackGround.gameObject);
    }
}
