using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreePickup : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider triggercollider;
    [SerializeField]
    private Sprite pickupImg;

    private GameManager gameManager;
    private GameObject pickupButton;

    private GameObject playerHandWood;

    private Animator playerAnim;


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pickupButton = gameManager.pickupButton;
        playerHandWood = gameManager.playerHandWood;
        playerAnim = GameObject.Find("Player").GetComponentInChildren<Animator>();
    }

    private void SetCollider()
    {
        triggercollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pickupButton.SetActive(true);
            pickupButton.transform.GetChild(0).GetComponent<Image>().sprite = pickupImg;
            pickupButton.GetComponent<Button>().onClick.RemoveAllListeners();
            pickupButton.GetComponent<Button>().onClick.AddListener(Pickup);
        }
    }

   

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pickupButton.SetActive(false);
        }
    }

    private void Pickup()
    {
        playerHandWood.SetActive(true);
        playerAnim.SetBool("isCarry", true);  
        triggercollider.enabled = false;
        playerAnim.gameObject.GetComponentInParent<Player>().isCarry = true;

        JoyStick.isWalk = true;

        Destroy(this.gameObject);

    }
    private void OnDestroy()
    {
        try
        {
            pickupButton.SetActive(false);
        }
        catch
        {

        }
    }


}
