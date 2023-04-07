using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackArea : MonoBehaviour
{

    [SerializeField]
    private int damage;  

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.tag == "Player")
        {
            player.GetComponent<Player>().Hurt(-damage,this.gameObject);
        }
    }
}
