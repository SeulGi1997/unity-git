using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;



    public string currentWeapon;

    [HideInInspector]
    public bool isAttack;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }



    public void Attack()
    {
        if (!isAttack)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
      
        JoyStick.isMove = false;

        anim.SetTrigger("doOneHand");
        //µµÇÕ 0.8
        yield return new WaitForSeconds(0.3f);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.up * -0.5f , 0.5f, transform.forward, 1f);

        if(hits.Length > 0)
        {
            for(int i = 0; i < hits.Length; i++)
            {
                if(hits[i].transform.tag == "Tree")
                {
                    hits[i].transform.GetComponent<Tree>().Hurt(1);
                    hits[i].transform.GetComponent<Rigidbody>().AddTorque(transform.forward * 10f, ForceMode.Impulse);
                }
              

            }
        }


        yield return new WaitForSeconds(0.5f);


        JoyStick.isMove = true;
        isAttack = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.up * -0.5f+ transform.forward *1f , 0.5f);
    }

}
