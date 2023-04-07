using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{

    [SerializeField]
    private GameObject lightObject;

    [SerializeField]
    private GameObject sunObject;

    [SerializeField]
    private Color dayColor;
    [SerializeField]
    private Color nightColor;
    [SerializeField]
    private Color sunsetColor;

    [SerializeField]
    private AnimationCurve colorAC;
    [SerializeField]
    private AnimationCurve sunSetAC;


    [SerializeField]
    private Material skyDay;
    [SerializeField]
    private Material skyNight;

    [SerializeField]
    private float maxFog;
    [SerializeField]
    private float minFog;
    private float cuurentFog;

    [SerializeField]
    private float fogDensitySpeed;

    private GameTimeScript gameTime;

    private void Start()
    {
        gameTime = FindObjectOfType<GameTimeScript>();
    }
    private void FixedUpdate()
    {
        LightSetColor();
        TurnSun();
        SetFog();
    }

    private void TurnSun()
    {
        float x = gameTime.currentDaySeconds / 240;
        //240초에 1도씩 증가

        sunObject.transform.rotation = Quaternion.Euler(new Vector3(x + 250f, 0f, 0f));
        // 00시 위치 초기값 +250
    }

    private void LightSetColor()
    {
        float curveValue = colorAC.Evaluate(gameTime.currentHour);

        Color color = Color.Lerp(dayColor, nightColor, curveValue);
        lightObject.GetComponent<Light>().color = color;

        curveValue = sunSetAC.Evaluate(gameTime.currentHour);
        color = Color.Lerp(dayColor, sunsetColor, curveValue);
        sunObject.GetComponent<Light>().color = color;
    }

    private void SetFog()
    {
        if(gameTime.isNight == true)
        {
            if(cuurentFog <= maxFog)
            {
                cuurentFog += Time.deltaTime * fogDensitySpeed;
                RenderSettings.fogDensity = cuurentFog;
            }
           
        }
        else if (gameTime.isNight == false)
        {
            if (cuurentFog >= minFog)
            {
                cuurentFog -= Time.deltaTime * fogDensitySpeed;
                RenderSettings.fogDensity = cuurentFog;
            }

        }


        /*if (GameTimeScript.currentHour >= 21f || (GameTimeScript.currentHour >=0 && GameTimeScript.currentHour < 06))
            RenderSettings.fogDensity = 0.05f;
        else if (GameTimeScript.currentHour >= 20f)
            RenderSettings.fogDensity = 0.03f;
        else if (GameTimeScript.currentHour >= 19f)
            RenderSettings.fogDensity = 0.01f;

        if (GameTimeScript.currentHour >= 08 && GameTimeScript.currentHour < 19f)
            RenderSettings.fogDensity = 0f;
        else if (GameTimeScript.currentHour >= 07 && GameTimeScript.currentHour < 19f)
            RenderSettings.fogDensity = 0.01f;
        else if (GameTimeScript.currentHour >= 06 && GameTimeScript.currentHour < 19f)
            RenderSettings.fogDensity = 0.03f;*/


    }

}
