using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [TextArea]
    [SerializeField]
    private string[] leve1Texts;
    [TextArea]
    [SerializeField]
    private string[] leve2Texts;
    [TextArea]
    [SerializeField]
    private string[] leve3Texts;
    [SerializeField]
    private GameObject tutorialBackGround;
    [SerializeField]
    private Text tutorialText;
    [SerializeField]
    private Image craftButtonImage;
    [SerializeField]
    private Image inventoryButtonImage;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private CraftController craftController;
    [SerializeField]
    private GameObject[] hideObject;

    private int currentTextIndex;

    private bool isTyping;

    private int currentLevle = 1;



    
    void Update()
    {
        if(currentLevle == 1)
        {
            CheckHaveAxe();
        }
        else if(currentLevle == 2)
        {
            CheckMountAxe();
        }
        
    }

    private IEnumerator TypingCoroutine()
    {
        isTyping = true;
        tutorialText.text = "";

        yield return new WaitForSeconds(0.5f);
        tutorialBackGround.SetActive(true);

        if (currentLevle == 1)
        {

            for (int i = 0; i <= leve1Texts[currentTextIndex].Length; i++)
            {
                tutorialText.text = leve1Texts[currentTextIndex].Substring(0, i);
                yield return new WaitForSeconds(0.10f);
            }
 
        }
        else if(currentLevle == 2)
        { 
            for (int i = 0; i <= leve2Texts[currentTextIndex].Length; i++)
            {
                tutorialText.text = leve2Texts[currentTextIndex].Substring(0, i);
                yield return new WaitForSeconds(0.10f);
            }

        }
        else if (currentLevle == 3)
        {
 
            for (int i = 0; i <= leve3Texts[currentTextIndex].Length; i++)
            {
                tutorialText.text = leve3Texts[currentTextIndex].Substring(0, i);
                yield return new WaitForSeconds(0.10f);
            }
        
        }

        isTyping = false;
        currentTextIndex++;
    }

    public void OnClickNextTiping()
    {
        if (!isTyping)
        {
            if(currentLevle == 1)
            {
                if (currentTextIndex < leve1Texts.Length)
                    StartCoroutine(TypingCoroutine());
                else
                {
                    tutorialBackGround.SetActive(false);
                    StartCoroutine(CrafImageEffct());
                }
            }
            else if (currentLevle == 2)
            {
                if (currentTextIndex < leve2Texts.Length)
                    StartCoroutine(TypingCoroutine());
                else
                {
                    tutorialBackGround.SetActive(false);
                    StartCoroutine(InventoryImageEffct());
                }
            }
            else if (currentLevle == 3)
            {
                if (currentTextIndex < leve2Texts.Length)
                    StartCoroutine(TypingCoroutine());
                else
                {
                    tutorialBackGround.SetActive(false);
                    EndTutorial();
                }
            }


        }
    }

  

    private IEnumerator CrafImageEffct()
    {
        Color color = craftButtonImage.color;
        float color_a = color.a;

        while (currentLevle == 1)
        {
            color.a = 0f;
            craftButtonImage.color = color;

            yield return new WaitForSeconds(0.5f);

            color.a = color_a;
            craftButtonImage.color = color;

            yield return new WaitForSeconds(0.5f);
        }

       
   
    }

    private IEnumerator InventoryImageEffct()
    {
        Color color = inventoryButtonImage.color;
        float color_a = color.a;

        while (currentLevle == 2)
        {
            color.a = 0f;
            inventoryButtonImage.color = color;

            yield return new WaitForSeconds(0.5f);

            color.a = color_a;
            inventoryButtonImage.color = color;

            yield return new WaitForSeconds(0.5f);
        }
    }


    private void CheckHaveAxe()
    {
        if(inventory.GetItemCount("µ¹µµ³¢") != 0)
        {
            craftController.CloseCraftMenual();
            currentLevle++;
            currentTextIndex = 0;
            StartCoroutine(TypingCoroutine());
            
        }
    }
    

    private void CheckMountAxe()
    {
        if(WeaponController.currentWeapon == "StoneAxe")
        {
            currentLevle++;
            currentTextIndex = 0;

            for (int i = 0; i < hideObject.Length; i++)
            {
                hideObject[i].SetActive(true);
            }

            inventory.CloseInventory();
            StartCoroutine(TypingCoroutine());

        }
    }


    public void EndTutorial()
    {
        Destroy(tutorialBackGround.gameObject);
        Destroy(this.gameObject);
        GameManager.isTutorialEnd = true;
    }

    public void StartTutorial()
    {
        StartCoroutine(TypingCoroutine());

        for (int i = 0; i < hideObject.Length; i++)
        {
            hideObject[i].SetActive(false);
        }
    }
}
