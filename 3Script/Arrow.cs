using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private Transform rayTransform;
    [SerializeField]
    private float rayDistance;

    private Rigidbody rigid;

    private RaycastHit hit;

    public float yForce;
    public float zForce;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        rigid = GetComponent<Rigidbody>();
       rigid.AddForce(player.transform.forward *zForce + player.transform.up * yForce);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(this.transform.position + rigid.velocity) ;
       
        if (Physics.Raycast(rayTransform.position - rayTransform.up * 0.01f, rayTransform.forward, out hit, rayDistance))
        {
            if (hit.transform.tag != "Arrow" && hit.transform.tag != "Player")
            {
                rigid.velocity = Vector3.zero;
                rigid.isKinematic = true;
                this.transform.SetParent(hit.transform);
            }
        }

        
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(rayTransform.position - rayTransform.up * 0.01f, rayTransform.forward * rayDistance, Color.red, 0.1f);
    }

   
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Arrow" && other.transform.tag != "Player")
        {
            rigid.velocity = Vector3.zero;
            rigid.isKinematic = true;
            this.transform.SetParent(hit.transform);
        }

    }



}
