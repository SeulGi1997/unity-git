using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemCraft : MonoBehaviour
{
    [SerializeField]
    private string[] materialNameKr;
    [SerializeField]
    private int[] materialCount;
    [SerializeField]
    private GameObject craftButton;
    [SerializeField]
    private GameObject finishText;
    [SerializeField]
    private Item item;
    [SerializeField]
    private Image backGround;
    [SerializeField]
    private Text[] materialText;

    private Inventory inventory;

    [HideInInspector]
    public bool isCraftFinish;




    // Start is called before the first frame update
    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        craftButton.GetComponent<Button>().onClick.AddListener(CraftItem);
    }

    private void OnEnable()
    {
        CheckMaterial();
    }


    public void CheckMaterial()
    {
        bool isCraftCan = true;

        for (int i = 0; i < materialNameKr.Length; i++)
        {
            materialText[i].text = materialNameKr[i] + " " + inventory.GetItemCount(materialNameKr[i]) + "/" + materialCount[i];
            materialText[i].color = Color.white;

            if (inventory.GetItemCount(materialNameKr[i]) < materialCount[i])
            {
                materialText[i].color = Color.red;
                isCraftCan = false;
            }
        }

        if (isCraftCan)
        {
            craftButton.SetActive(true);
        }
        else
        {
            craftButton.SetActive(false);
        }

    }

    public void CraftItem()
    {
        for (int i = 0; i < materialNameKr.Length; i++)
        {
            inventory.UsedItem(materialNameKr[i], -materialCount[i]);
        }

        inventory.ChoiceSlot(item);
        isCraftFinish = true;
        CraftFinish();
    }

    public void CraftFinish()
    {
        backGround.color = Color.gray;

        for (int i = 0; i < materialText.Length; i++)
        {
            Destroy(materialText[i].gameObject);
        }

        finishText.SetActive(true);

        Destroy(craftButton);
        Destroy(this);


    }

 
   
}
