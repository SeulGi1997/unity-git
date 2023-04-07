using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBurn : MonoBehaviour
{
    [SerializeField]
    private GameObject burnPrefab;
    [SerializeField]
    private float maxBurnTime;
    [SerializeField]
    private float currnetBurnTime;


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Fire")
        {
            currnetBurnTime += Time.deltaTime;
            if(currnetBurnTime >= maxBurnTime)
            {
                Instantiate(burnPrefab, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Fire")
        {
            currnetBurnTime = 0f;
        }

    }
}
