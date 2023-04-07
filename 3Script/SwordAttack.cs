using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{

    [SerializeField]
    private int damage;
    [SerializeField]
    private float moveDistance;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody playerRigid;
    [SerializeField]
    private TrailRenderer trailEffect;
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Status theStatus;

    [SerializeField]
    private AudioClip swingClip;


    [HideInInspector]
    public bool isAttack; // 공격 중 인가

    [HideInInspector]
    public bool isdoAttack = true;  // 공격 가능 한가

    public float testNum;

    private bool attackDelay1;
    private bool attackDelay2;
    private bool attackDelay3;

    private bool isZoom;
    private float zoomTime;

    void Start()
    {
        isdoAttack = true;

    }

    void Update()
    {
        if (isZoom)
        {
            ZoomTimer();
        }
    }

    public void Attack()
    {

        if(isdoAttack)
        {
            if (!attackDelay1)
            {
                if (theStatus.currentPhysicalStrength >= 0.15f)
                    StartCoroutine("AttackCoroutine");
            }
            else if (attackDelay2)
            {
                if (theStatus.currentPhysicalStrength >= 0.15f)
                    StartCoroutine("AttackCoroutine2");
            }
            else if (attackDelay3)
            {
                if (theStatus.currentPhysicalStrength >= 0.15f)
                    StartCoroutine("AttackCoroutine3");
            }
        }
        
    }

    public void AttckStop()
    {
        StopCoroutine("AttackCoroutine");
        StopCoroutine("AttackCoroutine2");
        StopCoroutine("AttackCoroutine3");

        isAttack = false;
        attackDelay1 = false;
        attackDelay2 = false;
        attackDelay3 = false;
        trailEffect.enabled = false;

        anim.SetBool("isSword1", false);
        anim.SetBool("isSword2", false);
        anim.SetBool("isSword3", false);

        JoyStick.isMove = true;

    }

    private void ZoomTimer()
    {
        zoomTime += Time.deltaTime;

        if(zoomTime >= 5f)
        {
            zoomTime = 0;
            
            isZoom = false;
        }
    }

    IEnumerator AttackCoroutine()
    {

        // 공격 시작 초기화
        isAttack = true;
        attackDelay1 = true;
        JoyStick.isMove = false;
        playerRigid.velocity = Vector3.zero;

        playerRigid.AddForce(player.transform.forward * moveDistance * 0.5f, ForceMode.Impulse);

        isZoom = true;
        zoomTime = 0f;
      

        anim.SetBool("isSword1",true);
        anim.SetTrigger("doSword1");
       

        theStatus.currentPhysicalStrength -= 0.15f;

        // 공격
        trailEffect.enabled = true;
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Player>().AttackSound();
        AudioManager.instance.EffectPlay(swingClip);
        doAttack();

        // 공격 끝 초기화
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
        attackDelay2 = true;

        
        // 추가타 없을 시 초기화
        yield return new WaitForSeconds(0.5f);
        if(attackDelay2 != false)
        {
            anim.SetBool("isSword1", false);
            JoyStick.isMove = true;
            attackDelay1 = false;
            attackDelay2 = false;
            isAttack = false;

        }

    }

    IEnumerator AttackCoroutine2()
    {

        // 공격 시작 초기화
        attackDelay2 = false;
        JoyStick.isMove = false;
        playerRigid.velocity = Vector3.zero;
        zoomTime = 0;

        anim.SetBool("isSword2",true);
        
        theStatus.currentPhysicalStrength -= 0.15f;

        // 공격
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.EffectPlay(swingClip);
        player.GetComponent<Player>().AttackSound();
        //playerRigid.AddForce(player.transform.forward * moveDistance, ForceMode.Impulse);
        trailEffect.enabled = true;
        doAttack();


        // 공격 끝 초기화
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
        attackDelay3 = true;

       
        // 추가타 없을 시 초기화
        yield return new WaitForSeconds(0.5f);
        if (attackDelay3 != false)
        {
            JoyStick.isMove = true;
            attackDelay1 = false;
            attackDelay3 = false;
            isAttack = false;
            anim.SetBool("isSword2", false);

        }
    }

    IEnumerator AttackCoroutine3()
    {

        // 공격 시작 초기화
        attackDelay3 = false;
        JoyStick.isMove = false;
        playerRigid.velocity = Vector3.zero;
        zoomTime = 0;

        anim.SetBool("isSword3", true);
       
        theStatus.currentPhysicalStrength -= 0.15f;

        // 공격
        trailEffect.enabled = true;
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.EffectPlay(swingClip);
        player.GetComponent<Player>().AttackSound();
        playerRigid.AddForce(player.transform.forward * moveDistance, ForceMode.Impulse);
        doAttack();

        // 공격 끝 초기화
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;

        
        // 추가타 없을 시 초기화
        yield return new WaitForSeconds(0.5f);
        JoyStick.isMove = true;
        attackDelay1 = false;
        isAttack = false;
        anim.SetBool("isSword2", false);
        anim.SetBool("isSword3", false);

    }


    private void doAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(player.transform.position + player.transform.up * -0.5f, 1f, player.transform.forward, 1f);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.tag == "Animal")
                {
                    if (hits[i].transform.gameObject.layer != 16) // 16 == Dead
                        hits[i].transform.GetComponent<Animal>().Hurt(34);

                }
                else if (hits[i].transform.tag == "Enemy")
                {
                    if (hits[i].transform.GetComponent<Enemy>().isAttackInvincibility != true)
                    {
                        if(hits[i].transform.gameObject.layer != 16) // 16 == Dead
                            hits[i].transform.GetComponent<Enemy>().Hurt(damage);
                    }
                }
                else if (hits[i].transform.tag == "TestTag")
                {
                    Debug.Log("attack");
                }
            }
        }
    }


  
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(player.transform.position + player.transform.up * -0.5f + player.transform.forward * 1f, 1f);
    }

}
