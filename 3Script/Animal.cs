using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private int maxHP;
    [SerializeField]
    private int currentHP;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float moveDistince; //목적지 거리곱

    [SerializeField]
    private AudioClip hurtClip;
    [SerializeField]
    private AudioClip deadClip;

    private float nextActionTime;
    private float currentTime;

    private Vector3 distination;
    private Vector3 runDirection; 

    private NavMeshAgent nav;
    private Rigidbody rigid;
    private AudioSource theAudio;

    private GameObject player;


    private bool isWalk;
    private bool isDead;
    private bool isRun;

    // Start is called before the first frame update
    void Start()
    {
        nextActionTime = 1f;
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Dead();
            NextTimer();
        }
       
        Walk();
        Run();
        FrozenVelocity();
     
    }

    private void FrozenVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    private void NextTimer()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= nextActionTime)
        {
            ActionReset();
            Action();    
            currentTime = 0;
        }
    }

    private void ActionReset()
    {
        // Walk 초기화
        isWalk = false;
        isRun = false;
        nav.ResetPath();
        distination.Set(Random.Range(-10.0f, 10.0f), 0f, Random.Range(-10.0f, 10.0f));

        distination = distination.normalized;

    }

    private void Action()
    {

        int random = Random.Range(1, 5);

        switch (random)
        {
            case 1:
                //Idle_A
                nextActionTime = 10f;
                anim.CrossFade("Eyes_Blink", 0.1f, -1, 0f);
                anim.CrossFade("Idle_A", 0.1f, -1, 0f);
                break;
            case 2:
                //Sit
                nextActionTime = 10f;
                anim.CrossFade("Eyes_Sleep", 0.1f, -1, 0f);
                anim.CrossFade("Sit", 0.1f, -1, 0f);
                break;
            case 3:
            case 4:
                //Walk
                nextActionTime = 2f;
                isWalk = true;
                anim.CrossFade("Eyes_Blink", 0.1f, -1, 0f);
                anim.CrossFade("Walk", 0.1f, -1, 0f);
                break;      
        }
            

    }
    private void Walk()
    {
        if (isWalk)
        {
            nav.SetDestination(this.transform.position + distination * moveDistince);
        }
       
    }

    private void Run()
    {
        if (isRun)
        {
            runDirection += new Vector3(Random.Range(-0.2f, 0.2f), 0f, Random.Range(-0.2f, 0.2f));
            nav.SetDestination(this.transform.position + runDirection);
        }
       
    }
    private void RunReady()
    {
        if (!isDead)
        {
            runDirection = this.transform.position - player.transform.position;
            runDirection.Set(runDirection.x, 0f, runDirection.z);
            runDirection = runDirection.normalized;
            anim.CrossFade("Run", 0.1f, -1, 0f);
            anim.CrossFade("Eyes_Trauma", 0.1f, -1, 0f);
            isRun = true;
        }
        
    }

    public void Hurt(int damage)
    {
            currentTime = 0;
            nextActionTime = 8f;
            ActionReset();
            anim.CrossFade("Hit", 0.1f, -1, 0f);
            anim.CrossFade("Eyes_Cry", 0.1f, -1, 0f);
            AudioPlay(hurtClip);
            currentHP -= damage;
            Dead();

      
           Invoke("RunReady",0.5f);
 
        
    }



    private void Dead()
    {
        if (currentHP <= 0)
        {
            anim.CrossFade("Death", 0.1f, -1, 0f);
            anim.CrossFade("Eyes_Dead", 0.1f, -1, 0f);
            AudioPlay(deadClip);
            isDead = true;
            nav.enabled = false;
            rigid.isKinematic = true;
            gameObject.layer = 16; // Dead == 16
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 2f);
        }
        
    }

    private void AudioPlay(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
