using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Crop : MonoBehaviour
{
    
    [SerializeField]
    private GameObject[] crops;
    [SerializeField]
    private float cropAddTime; // �۹� ���� �ֱ�

    public Item item; 

    private float addTimer = 0; // ���� Ÿ�̸� 

    private int maxCropCount; // �ִ� ���� ����
    private int currentCropCount; // ���� ���� ����

    
   

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
