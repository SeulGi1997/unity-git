using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField]
    private GameObject tree;
    [SerializeField]
    public List<GameObject> itemWood;
    [SerializeField]
    private int destroyCount;
   
    private CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyCount == 0)
        {
            DestroyTree();
            destroyCount--;
        }

        if(itemWood.Count <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void DestroyTree()
    {
        Destroy(tree,0.5f);

        for(int i=0; i < itemWood.Count; i++)
        {
            itemWood[i].SetActive(true);
            itemWood[i].GetComponent<Rigidbody>().isKinematic = false;
        }

        capsuleCollider.isTrigger = true;
    }

    public void Hurt(int damage)
    {
        destroyCount -= damage;
     
    }
}
