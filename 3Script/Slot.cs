using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler
{
    //공통
    public Item item;

    public string itemName; //아이템 이름
    public int itemCount; // 아이템 개수
    public Sprite itemSprite; // 아이템 이미지
    public string itemNameKorea; // 아이템 한글 이름
    [TextArea]
    public string itemDesc; // 설명
    public Item.ItemType itemtype; // 아이템 타입
    public GameObject itemPrefab; // 아이템 오브젝트
    // 아이템 효과
    public int hp; 
    public int thirst;
    public int hungry;

    // 무기
    public int itemDamage;

    [SerializeField]
    private Image image;
    [SerializeField]
    private Text countText;
    [SerializeField]
    private ItemToolTip itemToolTip;


   

    public void ActiveSlot(Item item)
    {
        this.item = item;
        itemName = item.itemName;
        itemCount = item.itemCount;
        itemSprite = item.itemSprite;
        itemNameKorea = item.itemNameKorea;
        itemDesc = item.itemDesc;
        itemtype = item.itemtype;
        hp = item.hp;
        thirst = item.thirst;
        hungry = item.hungry;
        itemDamage = item.itemDamage;
        itemPrefab = item.itemPrefab;

        // 이미지 셋팅
        image.sprite = itemSprite;
        Color color = image.color;
        color.a = 1f;
        image.color = color;

        // 개수 셋팅
        if(item.itemtype == Item.ItemType.Weapon)
        {
            countText.text = "";
        }
        else
        {
            countText.text = itemCount.ToString();
        }

    }
    
    public void SetItemAdd(int num)
    {
        itemCount += num;
        countText.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            RemoveSlot();
        }
    }

    public void SetItemCount(int num)
    {
        itemCount = num;
        countText.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            RemoveSlot();
        }

    }



    public void RemoveSlot()
    {
        itemName = "";
        itemCount = 0;
        Color color = image.color;
        color.a = 0f;
        image.color = color;
        countText.text = "";
        itemNameKorea = "";
        item = null;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemName != "")
        {
            itemToolTip.OpenToolTip(this.transform.position,this.item);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemName != "")
        {
            DragSlot.instance.slot = this;
            DragSlot.instance.SetImage(itemSprite);
            DragSlot.instance.SetItemCount(itemCount);
            DragSlot.instance.transform.position = eventData.position;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetImageColor(0f);
        DragSlot.instance.slot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.slot != null)
        {
            Item _itme = this.item;
            int _itemCount = this.itemCount;
            this.ActiveSlot(DragSlot.instance.slot.item);
            this.SetItemCount(DragSlot.instance.itemCount);

            if (_itme != null )
            {
                DragSlot.instance.slot.ActiveSlot(_itme);
                DragSlot.instance.slot.SetItemCount(_itemCount);
            }
            else
            {
                DragSlot.instance.slot.RemoveSlot();
            }
 
        }
       
    }
}
