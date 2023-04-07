using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeThrow : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject pickupButton;
    private Animator playerAnim;

    [SerializeField]
    private GameObject itemTreePrefab;
    [SerializeField]
    private Sprite throwImg;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pickupButton = gameManager.pickupButton;
        playerAnim = GameObject.Find("Player").GetComponentInChildren<Animator>();

        Debug.Log(this.gameObject.name);
    }

    private void OnEnable()
    {
        Invoke("startRate", 0.5f);
    }

    private void startRate()
    {
        pickupButton.SetActive(true);
        pickupButton.transform.GetChild(0).GetComponent<Image>().sprite = throwImg;
        pickupButton.GetComponent<Button>().onClick.RemoveAllListeners();
        pickupButton.GetComponent<Button>().onClick.AddListener(ThrowTree);
    }

    public void ThrowTree()
    {
        
        var clone = Instantiate(itemTreePrefab, playerAnim.transform.position + playerAnim.transform.forward + playerAnim.transform.up, Quaternion.Euler(new Vector3(0f,0f,-90f)));

        RaycastHit hit;

        if (Physics.Raycast(playerAnim.transform.position + playerAnim.transform.up * 3f + playerAnim.transform.forward + playerAnim.transform.right * -0.5f, Vector3.down, out hit, 10f, LayerMask.GetMask("Terrain")))
        {
            clone.transform.position = hit.point + Vector3.up * 0.5f;
        }

            pickupButton.SetActive(false);
        playerAnim.SetBool("isCarry", false);
        playerAnim.gameObject.GetComponentInParent<Player>().isCarry = false;


        this.gameObject.SetActive(false);


    }
}
