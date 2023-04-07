using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewArrow : MonoBehaviour
{

    private void Update()
    {
        if(this.transform.position.y <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Animal")
        {
            other.GetComponent<Animal>().Hurt(100);
        }
        else if(other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Hurt(50);
        }

        Destroy(this.gameObject);
    }
}
