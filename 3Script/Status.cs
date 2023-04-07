using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float maxThirst;
    public float currentThirst;
    public float maxHugry;
    public float currentHungry;
    public float maxPhysicalStrength = 1f;
    public float currentPhysicalStrength = 1f;

    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Text thirstText;
    [SerializeField]
    private Text hungryText;

    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Slider thirstSlider;
    [SerializeField]
    private Slider hungrySlider;

    [SerializeField]
    private Image physicalStrength; // Ã¼·Â ¹Ù
    [SerializeField]
    private GameObject physicalStrength_GO;

    [SerializeField]
    private Animator anim;

    private float hungryTimer;
    private float thirstTimer;


    void Update()
    {
        CanvasStatusUpdate();
        TImeDecHungry();
        TImeDecThirst();

        if (currentHP <= 0)
        {
            Dead();
        }

        if (currentPhysicalStrength < maxPhysicalStrength)
        {
            physicalStrength_GO.SetActive(true);
            currentPhysicalStrength += 0.05f * Time.deltaTime;
        }
        else
        {
            physicalStrength_GO.SetActive(false);
        }
    }
    
    private void CanvasStatusUpdate()
    {
        hpText.text = Mathf.CeilToInt(currentHP) + " / " + maxHP;
        thirstText.text = Mathf.CeilToInt(currentThirst) + " / " + maxThirst;
        hungryText.text = Mathf.CeilToInt(currentHungry) + " / " + maxHugry;

        hpSlider.value = currentHP;
        thirstSlider.value = currentThirst;
        hungrySlider.value = currentHungry;

        physicalStrength.fillAmount = currentPhysicalStrength;

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if(currentThirst > maxThirst)
        {
            currentThirst = maxThirst;
        }
        if (currentHungry > maxHugry)
        {
            currentHungry = maxHugry;
        }
        if(currentPhysicalStrength > maxPhysicalStrength)
        {
            currentPhysicalStrength = maxPhysicalStrength;
        }

    }

    private void Dead()
    {
        currentHP = 0;

    }

    public void SetCurrentHP(int num)
    {
        this.currentHP += num;
    }
    public void SetCurrentThirst(int num)
    {
        this.currentThirst += num;

    }
    public void SetCurrentHugry(int num)
    {
        this.currentHungry += num;

    }

   

    private void TImeDecHungry()
    {
      
        hungryTimer += Time.deltaTime * 100f;

        if (hungryTimer >= 60f)
        {
            hungryTimer = 0f;
            currentHungry -= maxHugry / (60f * 8f * 3f);

            if (currentHungry <= 0)
            {
                currentHungry = 0f;
                currentHP -= maxHP / (60f * 8f);

            }

        }
        
    }

    private void TImeDecThirst()
    {

        thirstTimer += Time.deltaTime * 100f;

        if (thirstTimer >= 60f)
        {
            thirstTimer = 0f;
            currentThirst -= maxHugry / (60f * 8f * 1.8f);

            if (currentThirst <= 0)
            {
                currentThirst = 0f;
                currentHP -= maxHP / (60f * 8f);

            }

        }

    }

    public void SetStatus(float hp,float thrist, float hungry)
    {
        currentHP = hp;
        currentThirst = thrist;
        currentHungry = hungry;
    }

}
