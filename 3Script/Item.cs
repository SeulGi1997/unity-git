using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="New Item/item")]
public class Item : ScriptableObject
{
    public string itemName; //������ �̸�
    public int itemCount; // ������ ����
    public Sprite itemSprite;
    public string itemNameKorea;
    [TextArea]
    public string itemDesc; // ������ ����
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
