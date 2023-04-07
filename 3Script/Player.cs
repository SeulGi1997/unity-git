using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject mainButton;
    [SerializeField]
    private GameObject itemPickupButton;
    [SerializeField]
    private Text mainButtonText;
    [SerializeField]
    private float jumpFloat;
    [SerializeField]
    private GameObject[] Weapons;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private float knockBackDistance;
    [SerializeField]
    private SwordAttack swordAttack;
    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private AudioClip[] attackClip;
    [SerializeField]
    private AudioClip hurtClip;
    [SerializeField]
    private AudioClip jumpClip;

    [SerializeField]
    private GameObject[] hideDeadObject;
    [SerializeField]
    private GameObject openDeadObject;


 

    private enum WeaponType { Fishing = 0 };

    private string activateName;

    private float activateTime = 0;

    private bool isTime;
    [SerializeField]
    private bool isJump;
    private bool isFishing;
    private bool isItemPicup;
    private bool isDead;

    public bool isGround;



    public bool isHurt;
    [HideInInspector]
    public bool isCarry;

    private GameObject activateObject; // 현재 작물 오브젝트

    private Rigidbody rigid;
    private Animator anim;
    private PlayerAttack playerAttack;
    private Status status;


    void Start()
    {
        activateObject = null;
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
        status = GetComponent<Status>();


    }

    // Update is called once per frame
    void Update()
    {
        if (isTime)
        {
            Timer();
        }


        Dead();
    }

   
    // 타이머 (일정 시간 지날 시 작업실행)
    private void Timer()
    {
        activateTime += Time.deltaTime;

        if (activateTime >= 2f)
        {
            Activate();
        }
    }

    // 작업버튼 누를 시 실제 작업
    private void Activate()
    {
        switch (activateName)
        {
            case "Crop":

                if(activateObject.GetComponent<Crop>().GetCurrentCropCount() > 0)
                {
                    activateObject.GetComponent<Crop>().DownCurrentCropCount();

                    inventory.ChoiceSlot(activateObject.GetComponent<Crop>().item);

                    

                }

                if (activateObject.GetComponent<Crop>().GetCurrentCropCount() <= 0)
                {
                    mainButton.SetActive(false);
                    mainButtonText.text = "";
                    activateObject = null;
                    activateName = null;
                    anim.SetBool("isGather", false);
                    activateTime = 0;
                    JoyStick.isMove = true;
                    isTime = false;
                }
                break;

            case "Fishing":

                inventory.ChoiceSlot(activateObject.GetComponent<Fishing>().item);
                Debug.Log("물고기 획득");
                // 물고기 획득

                break;

        }

        activateTime = 0;
    }

    // 작업버튼 다운 초기화
    public void MainButtonDown()
    {

        switch (activateName)
        {
            case "Crop":
                anim.SetBool("isGather", true);
                break;
            case "Fishing":
                Weapons[(int)WeaponType.Fishing].SetActive(true);
                anim.SetBool("isFishing", true);
                isFishing = true;
                break;

        }

        isTime = true;
        JoyStick.isMove = false;

    }

    // 작업버튼 업 초기화
    public void MainButtonUp()
    {

        switch (activateName)
        {
            case "Crop":
                anim.SetBool("isGather", false);
                break;
            case "Fishing":
                Weapons[(int)WeaponType.Fishing].SetActive(false);
                anim.SetBool("isFishing", false);
                isFishing = false;
                break;
        }

        isTime = false;
        activateTime = 0;
        JoyStick.isMove = true;
    }

    public void ItemPickup()
    {
        if (!isItemPicup && !isCarry)
        {
            StartCoroutine(ItemPickupCoroutine());
        }
        
    }

    private IEnumerator ItemPickupCoroutine()
    {
        isItemPicup = true;
        JoyStick.isMove = false;

        anim.SetTrigger("doPickup");

        if(activateObject.GetComponent<Rigidbody>() != null)
            activateObject.GetComponent<Rigidbody>().isKinematic = true;

        Item _item = activateObject.GetComponent<ItemPickup>().item;


        yield return new WaitForSeconds(1.5f);

        inventory.ChoiceSlot(_item);

        try
        {
            activateObject.GetComponentInParent<Tree>().itemWood.Remove(activateObject);
        }catch
        {

        }


        Destroy(activateObject);
        itemPickupButton.SetActive(false);

        isItemPicup = false;
        JoyStick.isMove = true;

    }
  

    // 점프버튼 실행
    public void Jump()
    {
        
        if (!isJump && !isFishing && !playerAttack.isAttack &&!isHurt && !isCarry )
        {
            isJump = true;
            rigid.AddForce(Vector3.up * jumpFloat, ForceMode.Impulse);
            anim.SetTrigger("doJump");
            AudioManager.instance.EffectPlay(jumpClip);
        }
       
    }

    
    public void SwordAttack()
    {
   
        if (WeaponController.currentWeapon.Contains("Axe"))
        {
        
            RaycastHit hit;

            if (Physics.SphereCast(transform.position + transform.up * -0.5f, 0.5f, transform.forward, out hit, 1f))
            {
            
                if (hit.transform.tag == "Tree")
                {
                    if (!isJump && !isHurt && !isItemPicup && !isCarry && !isFishing)
                        playerAttack.Attack();
                }
                else
                {
                  
                    if (!isJump && !isHurt && !isItemPicup && !isCarry && !isFishing)
                        swordAttack.Attack();
                }

            }
            else
            {
             
                if (!isJump && !isHurt && !isItemPicup && !isCarry && !isFishing)
                    swordAttack.Attack();
            }
        }
        else
        {
           
            if (!isJump && !isHurt && !isItemPicup && !isCarry && !isFishing)
                swordAttack.Attack();
        }


    }

    // 공격버튼 실행
    public void AttackButton()
    {
        if(!isJump && !isFishing && !isHurt && !isCarry)
        {
            rigid.velocity = Vector3.zero;
            playerAttack.Attack();
        }
    }
    
    public void Hurt(int damage, GameObject gameObject)
    {
        if (!isHurt &&!isDead)
        {
            StartCoroutine(HurtCoroutine(damage,gameObject));
        }
  
    }

    private IEnumerator HurtCoroutine(int damage, GameObject gameObject)
    {
        swordAttack.AttckStop();
        //swordAttack.isdoAttack = false;

        yield return null;

        isHurt = true;
        JoyStick.isMove = false;

        status.SetCurrentHP(damage);
        rigid.velocity = Vector3.zero;
        rigid.AddForce((transform.position - gameObject.transform.position).normalized * knockBackDistance, ForceMode.Impulse);
        anim.SetTrigger("doHurt");
        anim.SetBool("isHurt", true);
        AudioManager.instance.EffectPlay(hurtClip);

        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isHurt", false);
        yield return new WaitForSeconds(0.1f);
        isHurt = false;
        JoyStick.isMove = true;
       // swordAttack.isdoAttack = true;
    }
    
    private void Dead()
    {
        if (!isDead)
        {
            if(status.currentHP <= 0)
            {
                isDead = true;
                anim.SetTrigger("doDead");
                
                for(int i =0; i < hideDeadObject.Length; i++)
                {
                    hideDeadObject[i].SetActive(false);
                }

                openDeadObject.SetActive(true);
            }
        }
    }

    public void RunButtonDown()
    {
        JoyStick.isWalk = false;

    }

    public void RunButtonUp()
    {
        JoyStick.isWalk = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Crop" )
        {
            if(other.GetComponent<Crop>().GetCurrentCropCount() > 0)
            {
                mainButton.SetActive(true);
                mainButtonText.text = "채집";
                activateName = "Crop";
                activateObject = other.gameObject;
      
            } 

        }else if(other.tag == "Fishing")
        {
            mainButton.SetActive(true);
            mainButtonText.text = "낚시";
            activateName = "Fishing";
            activateObject = other.gameObject;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
        {
            activateName = "Item";
            activateObject = other.gameObject;
            itemPickupButton.SetActive(true);
        }

    }



    private void OnTriggerExit(Collider other)
    {
        mainButton.SetActive(false);
        mainButtonText.text = "";
        activateObject = null;
        activateName = null;
        itemPickupButton.SetActive(false);

    }


  

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            isJump = false;
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }


    public void PlayerRotateValueCamera()
    {
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, this.transform.eulerAngles.z);
    }

    public void AttackSound()
    {
        int _index = Random.Range(0, 5);
        if(_index < 3)
            AudioManager.instance.EffectPlay(attackClip[_index],0.5f);

    }

}
