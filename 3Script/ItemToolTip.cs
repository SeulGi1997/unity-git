using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject toolTip;
    [SerializeField]
    private Text itemNameKr;
    [SerializeField]
    private Text itemDesc;
    [SerializeField]
    private GameObject usedButton;
    [SerializeField]
    private GameObject changeWeaponButton;
  

    [SerializeField]
    private Inventory inventory;

    private WeaponController weaponController;

    private GameObject itemPrefab;

    private string itemNameEng;


    private GameObject player;
    private RaycastHit hit;
    private void Start()
    {
        player = GameObject.Find("Player");
        weaponController = FindObjectOfType<WeaponController>();
    }

    public void OpenToolTip(Vector3 position,Item _item)
    {
        toolTip.SetActive(true);
        toolTip.transform.position = new Vector3(position.x,position.y- toolTip.GetComponent<RectTransform>().rect.width *0.3f);
        this.itemNameKr.text = _item.itemNameKorea;
        this.itemDesc.text = _item.itemDesc;

        this.itemPrefab = _item.itemPrefab;
        this.itemNameEng = _item.itemName;


        if (_item.itemtype == Item.ItemType.Used)
        {
            usedButton.SetActive(true);
            changeWeaponButton.SetActive(false);
        }
        else if(_item.itemtype == Item.ItemType.Weapon)
        {
            usedButton.SetActive(false);
            changeWeaponButton.SetActive(true);
        }
        else
        {
            usedButton.SetActive(false);
            changeWeaponButton.SetActive(false);
        }


    }

    public void CloseToolTip()
    {
        toolTip.SetActive(false);
    }

    public void UseButton()
    {
        inventory.UsedItem(itemNameKr.text,-1);
        CloseToolTip();

    }

   
    public void WeaponCaangeButton()
    {
        weaponController.ChangeWeapon(itemNameEng);
        CloseToolTip();
    }


}
