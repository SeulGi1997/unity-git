using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeScript : MonoBehaviour
{

    public float setCurrentHour;


    [SerializeField]
    private float timeSpeed;

    public float currentHour;

    public float currentMinute;

    public float currentSecond;

    public float currentDaySeconds;

    public bool isNight;

    

    private void Awake()
    {
        currentHour = setCurrentHour;
    }

  
    private void FixedUpdate()
    {
        IsNight();
    }

    // Update is called once per frame
    void Update()
    {
        

        currentSecond += (Time.deltaTime * timeSpeed);
        if(currentSecond >= 60f)
        {

            currentSecond = 0f;
            currentMinute += 1f;

            if(currentMinute >= 60f)
            {
                currentMinute = 0f;
                currentHour += 1f;

                if(currentHour >= 24f)
                {
                    currentHour = 0f;
                }
            }
        }

       // testText.text = currentHour.ToString("00") + " : " + currentMinute.ToString("00") + " : " + currentSecond.ToString("00");

        currentDaySeconds = (currentHour * 60 * 60) + (currentMinute * 60) + (currentSecond);

    }

    private void IsNight()
    {
        if (currentHour >= 20f || currentHour <= 06)
            isNight = true;
        else
            isNight = false;

    }

    public void SetTime(float h,float m,float s)
    {
        this.currentHour = h;
        this.currentMinute = m;
        this.currentSecond = s;
    }

}
