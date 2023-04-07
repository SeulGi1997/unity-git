using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="New Item/item")]
public class Item : ScriptableObject
{
    public string itemName; //아이템 이름
    public int itemCount; // 아이템 개수
    public Sprite itemSprite;
    public string itemNameKorea;
    [TextArea]
    public string itemDesc; // 아이템 설명
    public ItemType itemtype;
    public int hp;
    public int thirst;
    public int hungry;
    public int itemDamage;
    public GameObject itemPrefab;

    public enum ItemType
    {
        Used,
        UnUsed,
        Weapon
    }


}
