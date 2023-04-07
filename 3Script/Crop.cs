using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Crop : MonoBehaviour
{
    
    [SerializeField]
    private GameObject[] crops;
    [SerializeField]
    private float cropAddTime; // 작물 생성 주기

    public Item item; 

    private float addTimer = 0; // 생성 타이머 

    private int maxCropCount; // 최대 열매 갯수
    private int currentCropCount; // 현재 열매 갯수

    
   

    void Start()
    {
        maxCropCount = crops.Length;
        currentCropCount = maxCropCount;    
    }

    void Update()
    {
        CropsActive();
        CropAdd();

        if (currentCropCount == 0 )
        {
                   
        }
       
    }

    private void CropAdd()
    {
        if(maxCropCount > currentCropCount)
        {
            addTimer += Time.deltaTime;

            if (cropAddTime <= addTimer)
            {
                currentCropCount++;
                addTimer = 0f;
            }
        }

    }

    private void CropsActive()
    {
        for(int i = currentCropCount; i< maxCropCount ; i++)
        {
            crops[i].gameObject.SetActive(false);

        }

        for(int i = 0; i<currentCropCount ; i++)
        {
            crops[i].gameObject.SetActive(true);
        }

       
    }


    public void DownCurrentCropCount()
    {
        currentCropCount--;
    }

    // Getter Setter


    public GameObject[] GetCrops()
    {
        return crops;
    }
    public int GetCurrentCropCount()
    {
        return currentCropCount;
    }
    
   

}
