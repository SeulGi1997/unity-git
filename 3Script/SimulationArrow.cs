using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationArrow : MonoBehaviour
{
    [SerializeField]
    private Transform rayTransform;
    [SerializeField]
    private float rayDistance;
    [SerializeField]
    private GameObject pointEffect;

    private Rigidbody rigid;

    private RaycastHit hit;

    public float yForce;
    public float zForce;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(player.transform.forward * zForce + player.transform.up * yForce);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(this.transform.position + rigid.velocity);

        if (Physics.Raycast(rayTransform.position - rayTransform.up * 0.01f, rayTransform.forward, out hit, rayDistance))
        {
            if (hit.transform.tag != "Arrow" && hit.transform.tag != "Player")
            {
                var effect = Instantiate(pointEffect, hit.point, Quaternion.identity);
                Destroy(effect, 0.2f);
                Destroy(this.gameObject);
            }
        }


    }



    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Arrow" && other.transform.tag != "Player")
        {
            var effect = Instantiate(pointEffect, hit.point, Quaternion.identity);
            Destroy(effect, Time.deltaTime);
            Destroy(this.gameObject);
        }

    }



}
