using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftController : MonoBehaviour
{

    [SerializeField]
    private GameObject play_UI;
    [SerializeField]
    private GameObject craft_UI;

    [SerializeField]
    private GameObject equipmentCraftScroll;
    [SerializeField]
    private GameObject itemCraftScrool;

    [SerializeField]
    private GameObject equipmentCraftButton;
    [SerializeField]
    private GameObject itemCraftButton;

    private GameObject player;


    void Start()
    {
        player = GameObject.Find("Player");
    }



    public void OpenCraftMenual()
    {
        craft_UI.SetActive(true);
        play_UI.SetActive(false);
    }
    public void CloseCraftMenual()
    {
        craft_UI.SetActive(false);
        play_UI.SetActive(true);
    }

    public void CraftPreView(GameObject preViewPrefab)
    {
        Instantiate(preViewPrefab, player.transform.position, Quaternion.identity);
        CloseCraftMenual();
    }

    public void EquipmentCraftButton()
    {
        equipmentCraftScroll.SetActive(true);
        itemCraftScrool.SetActive(false);
        equipmentCraftButton.GetComponent<RectTransform>().SetAsLastSibling();

    }

    public void ItemCraftButton()
    {
        equipmentCraftScroll.SetActive(false);
        itemCraftScrool.SetActive(true);
        itemCraftButton.GetComponent<RectTransform>().SetAsLastSibling();

    }

}
