using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject throwItem;

    [SerializeField]
    public int maxHp;
    [SerializeField]
    public int currentHp;

    [SerializeField]
    private float nextActionTime;
    [SerializeField]
    private float destinationDistance;

    [SerializeField]
    private float castRadius;
    [SerializeField]
    private float castDistance;
    [SerializeField]
    private float knockBackDistance;

    [SerializeField]
    private BoxCollider attackArea;
    [SerializeField]
    private float attackDelayTime;

    [SerializeField]
    private GameObject hpBar;

    private float timeCount;

    private GameObject player;

    private Animator anim;
    private NavMeshAgent nav;
    private Rigidbody rigid;
    private SkinnedMeshRenderer[] meshs;

    private Vector3 destination;

    private RaycastHit[] hit;

    [HideInInspector]
    public bool isAttackInvincibility; // 공격 중 무적

    private bool isWalk;
    private bool isPlayerFollowRun;
    private bool isAttack;
    private bool isRandomActionTimer = true;
    private bool isPlayerFollowWalk;
    private bool isHurt;
    private bool isFrozenVelocity = true;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();

    }

    // Update is called once per frame
   
    private void FixedUpdate()
    {
        if (isRandomActionTimer)
        {
            NextActionTimer();
        }

        FrozenVelocity();
        Walk(); // 걷기
        PlayerFollowRun(); // 플레이어 따라가서 공격
    }

    private void FrozenVelocity()
    {
        if (isFrozenVelocity)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }

    }

    private void NextActionTimer()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= nextActionTime)
        {
            timeCount = 0f;
            RandomActionReset();
            RandomAction();
        }
    }

    private void RandomActionReset()
    {
        isRandomActionTimer = true;
        isPlayerFollowRun = false;
        isWalk = false;
        nav.speed = 2f;
        nav.ResetPath();
        destination.Set(Random.Range(-10.0f, 10.0f), 0f, Random.Range(-10.0f, 10.0f));
        destination = destination.normalized;

        if (hpBar.activeSelf == true)
            hpBar.SetActive(false);
    }

    private void RandomAction()
    {
        int random = Random.Range(1, 4);

        switch (random)
        {
            case 1:
                // idle
                anim.SetBool("isRun", false);
                anim.SetBool("isWalk",false);
                break;
            case 2:
                anim.SetBool("isRun", false);
                anim.SetBool("isWalk", false);
                break;
            case 3:
                // walk
                isWalk = true;
                anim.SetBool("isWalk", true);
                break;
        }
    }

    //걷기
    private void Walk()
    {
        if (isWalk)
        {
            nav.SetDestination(this.transform.position + destination * destinationDistance);
        }
    }


    //플레이어 따라가서 공격
    private void PlayerFollowRun()
    {
        if (isPlayerFollowRun)
        {
            nav.SetDestination(player.transform.position);
            Attack();
        }
    }

    // 공격
    private void Attack()
    {
        if (!isAttack && !isHurt)
        {
            hit = Physics.SphereCastAll(transform.position + transform.up * 0.3f, castRadius, transform.forward, castDistance, LayerMask.GetMask("Player"));

            if (hit.Length > 0)
            {
                StartCoroutine("AttackCoroutine");
            }
        }

    }

    private IEnumerator AttackCoroutine()
    {

        nav.isStopped = true;
        isAttack = true;
        isAttackInvincibility = true;
        anim.SetBool("isRun", false);

        int random = Random.Range(1, 3);

        switch (random)
        {
            case 1:
               // anim.SetTrigger("doAttack1");
               // break;
            case 2:
                anim.SetTrigger("doAttack2");
                break;
        }

        yield return new WaitForSeconds(attackDelayTime);
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.3f);
        attackArea.enabled = false;
        isAttackInvincibility = false;

        yield return new WaitForSeconds(1.5f);

        if(!isHurt)
            anim.SetBool("isRun", true);

        isAttack = false;
        nav.isStopped = false;
        timeCount = 0f;
    }

    private void StopAttack()
    {
        StopCoroutine("AttackCoroutine");
        isAttack = false;
        isAttackInvincibility = false;
        attackArea.enabled = false;
        nav.isStopped = false;
    }

    public void Hurt(int damage)
    {
        if (hpBar.activeSelf == false)
            hpBar.SetActive(true);

        StopCoroutine("HurtCoroutine");
        StopCoroutine("HurtEffectCoroutine");
        StartCoroutine("HurtCoroutine",damage);


    }

    private IEnumerator HurtCoroutine(int damage)
    {
        if(isAttack)
            StopAttack();

        nav.isStopped = true;
        isFrozenVelocity = false;
        isPlayerFollowRun = false;
        isHurt = true;

        rigid.AddForce((transform.position - player.transform.position) * knockBackDistance, ForceMode.Impulse);
        anim.SetTrigger("doHit");
        anim.SetBool("isWalk", false);
        anim.SetBool("isRun", false);
        StartCoroutine("HurtEffectCoroutine");

        timeCount = 0f;
        nextActionTime = 10f;

        currentHp -= damage;

        yield return null;

        if (currentHp <= 0)
        {
            Dead();
        }

        yield return new WaitForSeconds(1f);
        isFrozenVelocity = true;

        yield return new WaitForSeconds(1f);

        nav.speed = 5f;
        isPlayerFollowRun = true;
        nav.isStopped = false;

        isHurt = false;
        anim.SetBool("isRun", true);

    }


    private IEnumerator HurtEffectCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach(SkinnedMeshRenderer _meshs in meshs)
            {
                //Color color = _meshs.material.color;
                //color.a = 0f;
                _meshs.material.color = Color.gray;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (SkinnedMeshRenderer _meshs in meshs)
            {
               // Color color = _meshs.material.color;
                //color.a = 1f;
                _meshs.material.color = Color.white;
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }
    
    public void IsPlayerFollowRunActive()
    {
        isHurt = false;
        isPlayerFollowWalk = false;
        anim.SetBool("isRun",true);
        nav.speed = 5f;
        isPlayerFollowRun = true;

    }

    private void Dead()
    {
        StopAllCoroutines();
        nav.enabled = false;
        this.gameObject.layer = 16; // Dead == 16
        hpBar.SetActive(false);
        anim.SetTrigger("doDie");

        if(throwItem != null)
            Instantiate(throwItem, transform.position, Quaternion.identity);

        Destroy(this.gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (isPlayerFollowRun != true)
            {
                timeCount = 0f;
                nextActionTime = 10f;
                RandomActionReset();
                isPlayerFollowWalk = true;
                Invoke("IsPlayerFollowRunActive", 1f);

                if (hpBar.activeSelf == false)
                    hpBar.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if (isPlayerFollowWalk)
            {
                nav.SetDestination(player.transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + transform.forward * castDistance + transform.up * 0.3f, castRadius);
    }
}
