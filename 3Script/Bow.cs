using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    private Animator bowAnim;
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private GameObject fakeArrow;

    private float readyTime;

    private bool isAttack;
    private bool isTouch;
    private bool isUpKey;

    [SerializeField]
    private Transform arrowStartTransform;
    [SerializeField]
    private GameObject arrow;



    // Start is called before the first frame update
    void Start()
    {
        readyTime = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouch)
        {
            readyTime -= Time.deltaTime;
            if (readyTime <= 0)
            {
                isAttack = true;
            }

        }
      
            
    }

    public void BowShot()
    {
        var arrow_prefab = Instantiate(arrow, arrowStartTransform.position, Quaternion.identity);
       
        Debug.Log("Soht");
    }

    public void ButtonDown()
    {
        if (!isTouch)
        {
            isUpKey = true;
            isTouch = true;
            JoyStick.isWalk = true;
            bowAnim.SetTrigger("doReady");
            playerAnim.SetBool("isShot",true);

            fakeArrow.SetActive(true);
        }
       
    }

    public void ButtonUp()
    {
        if (isUpKey)
        {
            isUpKey = false;
            JoyStick.isWalk = false;
            bowAnim.SetTrigger("doShot");
            playerAnim.SetBool("isShot", false);

            if (isAttack)
            {
                BowShot();
            }

            isAttack = false;
            readyTime = 0.3f;
            fakeArrow.SetActive(false);

            Invoke("ActiveTouch", 0.3f);
        }
       
    }

    private void ActiveTouch()
    {
        isTouch = false;
    }

    private void MagicBowShot()
    {
        
    }

}
