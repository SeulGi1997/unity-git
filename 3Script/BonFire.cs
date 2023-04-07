using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonFire : MonoBehaviour
{
    [SerializeField]
    private GameObject particleObject;

    [SerializeField]
    private float offTime;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private string[] bornItemNames;
    [SerializeField]
    private Image bornItemImage;
    [SerializeField]
    private Sprite burningSprite;
    [SerializeField]
    private Sprite burnFinishSprite;
    [SerializeField]
    private GameObject[] hides;


    private float currentTime;
    private float burningTime;

    private bool isTimer = true;

    private GameManager gameManager;
    private Camera mainCamera;
    private Inventory inventory;

    private List<Item> haveItems;

    private int haveItemIndex;
    private Item craftItem;

    private bool isBurning;
    private bool isBurnFinish;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCamera = gameManager.mainCamera.GetComponent<Camera>();
        canvas.worldCamera = mainCamera;
        inventory = gameManager.inventory;

        haveItems = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTimer)
            OffTimer();

        Burning();
    }

    private void OffTimer()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= offTime)
        {
            Off();
            isTimer = false;
        }
    }

    private void Off()
    {
        particleObject.SetActive(false);
    }

    private void LockCanvasRotation()
    {
        canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - mainCamera.transform.position);
    }

    private bool CheckBorn()
    {
        bool isBorn = false;
        haveItems.Clear();

        for (int i = 0; i < bornItemNames.Length; i++)
        {
            if(inventory.GetItemCount(bornItemNames[i]) > 0)
            {
                isBorn = true;

                if (inventory.GetItemInformation(bornItemNames[i]) != null)
                {
                    haveItems.Add(inventory.GetItemInformation(bornItemNames[i]));
                }
                    
            }
        }
        if (isBorn)
        {
            canvas.gameObject.SetActive(true);
            bornItemImage.sprite = haveItems[0].itemSprite;
            haveItemIndex = 0;
        }
        else 
        {
            canvas.gameObject.SetActive(false);
        }

        return isBorn;
    }

    public void RightButton()
    {
        if(haveItemIndex +1 < haveItems.Count)
        {
            haveItemIndex++;
            bornItemImage.sprite = haveItems[haveItemIndex].itemSprite;
        }
        
    }
    public void LeftButton()
    {
        if (haveItemIndex - 1 >= 0)
        {
            haveItemIndex--;
            bornItemImage.sprite = haveItems[haveItemIndex].itemSprite;
        }

    }
    public void BornButton()
    {
        for (int i = 0; i < hides.Length; i++)
        {
            hides[i].SetActive(false);
        }

        bornItemImage.sprite = burningSprite;

        if (haveItems[haveItemIndex].itemNameKorea == "생고기")
        {
            craftItem = inventory.GetItemInformation("고기");
            inventory.UsedItem("생고기", -1);

        }

        bornItemImage.fillAmount = 0f;
        isBurning = true;

    }

    public void Burning()
    {
        if (isBurning)
        {
            bornItemImage.fillAmount += Time.deltaTime * 0.1f;
            burningTime += Time.deltaTime;
            if (burningTime >= 10f)
            {
                isBurning = false;
                bornItemImage.fillAmount = 1f;
                burningTime = 0f;
                
                BurnFinish();
            }
        }
        
    }

    public void BurnFinish()
    {
        isBurnFinish = true;
        bornItemImage.sprite = burnFinishSprite;
    }

    public void BurnFinishGetButton()
    {
        if (isBurnFinish)
        {
            isBurnFinish = false;
            inventory.ChoiceSlot(craftItem);
            CheckBorn();

            for (int i = 0; i < hides.Length; i++)
            {
                hides[i].SetActive(true);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(!isBurnFinish && !isBurning)
                CheckBorn();
            else
                canvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            LockCanvasRotation();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.gameObject.SetActive(false);
        }
    }

}
