using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;

    private Image image;

    public Slot slot;
    public int itemCount;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        image = GetComponent<Image>();
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
        Color color = image.color;
        color.a = 0.8f;
        image.color = color;
    }
    public void SetImageColor(float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void SetItemCount(int _count)
    {
        itemCount = _count;
    }
    
}
