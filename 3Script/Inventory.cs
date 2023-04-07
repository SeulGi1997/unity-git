using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject inventory;
    [SerializeField]
    private GameObject[] hideGrooup;
    [SerializeField]
    public GameObject[] slots;
    [SerializeField]
    private GameObject floatingText;
    [SerializeField]
    private RectTransform startFloatingTextPosition;
    [SerializeField]
    private GameObject itemToolTip;

    [SerializeField]
    private Status playerStatus;

    [SerializeField]
    public Item[] items;
    

    // Update is called once per frame
 
    public void ChoiceSlot(Item item)
    {
        // ���Կ� �ش�������� ���� �� ���� ����
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().itemName == item.itemName)
            {

                slots[i].GetComponent<Slot>().SetItemAdd(item.itemCount);

                var clone = Instantiate(floatingText, startFloatingTextPosition);
                clone.GetComponentInChildren<Text>().text = item.itemNameKorea + " + " + item.itemCount;
                clone.GetComponentInChildren<Image>().sprite = item.itemSprite;

                return;
            }
        }
        // ���Կ� �ش�������� ���� �� �� ���Կ� �߰�
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().itemName == null || slots[i].GetComponent<Slot>().itemName == "")
            {

                slots[i].GetComponent<Slot>().ActiveSlot(item);

                var clone = Instantiate(floatingText, startFloatingTextPosition);
                clone.GetComponentInChildren<Text>().text = item.itemNameKorea + " + " + item.itemCount;
                clone.GetComponentInChildren<Image>().sprite = item.itemSprite;

                return;

            }
        }
    }

    // ������ �ѱ� �̸����� ���
    public void UsedItem(string itemNameKorea,int num)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().itemNameKorea == itemNameKorea)
            {
                if(slots[i].GetComponent<Slot>().itemtype == Item.ItemType.Used)
                {
                    playerStatus.SetCurrentHP(slots[i].GetComponent<Slot>().hp);
                    playerStatus.SetCurrentThirst(slots[i].GetComponent<Slot>().thirst);
                    playerStatus.SetCurrentHugry(slots[i].GetComponent<Slot>().hungry);
                }


                slots[i].GetComponent<Slot>().SetItemAdd(num);
                
                return;
            }
        }
    }

    // ������ ���� ���� ������ 0
    public int GetItemCount(string itemNameKorea)
    {

        int count = 0; 

        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].GetComponent<Slot>().itemNameKorea == itemNameKorea)
            {
                count = slots[i].GetComponent<Slot>().itemCount;
            }
        }


        return count;
    }

    public void AllReSetSlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponent<Slot>().RemoveSlot();
        }
    }


    public void OpenInventory()
    {
        for (int i = 0; i < hideGrooup.Length; i++)
        {
            hideGrooup[i].SetActive(false);
        }

        inventory.SetActive(true);
    }
    public void CloseInventory()
    {
        inventory.SetActive(false);

        for (int i = 0; i < hideGrooup.Length; i++)
        {
            hideGrooup[i].SetActive(true);
        }

        itemToolTip.SetActive(false);
    }

    public Item GetItemInformation(string koreaName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i].itemNameKorea == koreaName)
            {
                return items[i];
            }
        }
        return null;
    }
}
