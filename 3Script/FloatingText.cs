using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float alphaSpeed;
    [SerializeField]
    private float destoryTime;
    [SerializeField]
    private RectTransform rt;

    private Image image;
   // private RectTransform rt;
    private Text text;
    private Color color;
    private Color color2;

    // Start is called before the first frame update
    void Start()
    {
        //rt = GetComponentInParent<RectTransform>();
        image = GetComponentInParent<Image>();
        text = GetComponent<Text>();
        color = text.color;
        color2 = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + ( moveSpeed * Time.deltaTime));
        
        color.a -= alphaSpeed * Time.deltaTime;
        text.color = color;

        color2.a -= alphaSpeed * Time.deltaTime;
        image.color = color2;

        destoryTime -= Time.deltaTime;
        
        if(destoryTime <= 0)
        {
            Destroy(rt.gameObject);
           // Destroy(this.gameObject);
        }
    }
}
