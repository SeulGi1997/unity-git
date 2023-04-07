using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test002 : MonoBehaviour
{

    public float float_x;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();    
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.RotateAround(player.transform.position, player.transform.right, float_x);
    }
}
