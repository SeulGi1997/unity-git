using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwimScript : MonoBehaviour
{

    public static bool isSwim;
    public static bool isSwimOnWater; // 물 위 수영 중인가;

    private float maxSwimHP = 100;
    private float currentSwimHP;

    [SerializeField]
    private Slider swimHpBar;
    [SerializeField]
    private float swimHpSubSpeed;
    [SerializeField]
    private Status playerStatus;

    private float playerHpSubTimer;

    [SerializeField]
    private GameObject[] cavasGroup;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera swimCamera;
    [SerializeField]
    private float cameraZoomSpeed;
    [SerializeField]
    private float swimDrag;

    [SerializeField]
    private GameObject strengthBar;

    private float originDrag;

    private bool changeSwimOnWater;

    private void Awake()
    {
        originDrag = player.GetComponent<Rigidbody>().drag;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSwimHP = maxSwimHP;    
    }

    private void OnEnable()
    {
        isSwim = true;
        isSwimOnWater = false;
        swimHpBar.gameObject.SetActive(true);
        strengthBar.gameObject.SetActive(false);
        player.GetComponent<Rigidbody>().drag = swimDrag;

        StopCoroutine("CameraZoomOut");
        StartCoroutine("CameraZoomIn");

        swimCamera.enabled = true;

        for (int i = 0; i < player.transform.childCount; i++)
        {
            if(player.transform.GetChild(i).gameObject.name != "SwimCamera")
                player.transform.GetChild(i).gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in cavasGroup)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        swimHpBar.value = currentSwimHP;
        Swim();

        if (isSwimOnWater == true && player.GetComponent<Player>().isGround == true)
        {
            this.enabled = false;
        }

        SwimOnWater();
    }

    private void OnDisable()
    {
        
        isSwim = false;
        currentSwimHP = maxSwimHP;
        swimHpBar.gameObject.SetActive(false);
        strengthBar.gameObject.SetActive(true);
        player.GetComponent<Rigidbody>().drag = originDrag;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().useGravity = true;

        swimCamera.enabled = false;

        StopCoroutine("CameraZoomIn");
        StartCoroutine("CameraZoomOut");

        for (int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in cavasGroup)
        {
            gameObject.SetActive(true);
        }
        
       
    }

    private void SwimOnWater()
    {
        if(isSwimOnWater == true)
        {
            currentSwimHP = maxSwimHP;
            swimHpBar.gameObject.SetActive(false);
        }
        else
        {
            swimHpBar.gameObject.SetActive(true);
        }
    }


    private void Swim()
    {
        if (isSwim)
        {
            if(currentSwimHP >= 0)
            {
                currentSwimHP -= Time.deltaTime * swimHpSubSpeed;
            }
            else
            {
                playerHpSubTimer += Time.deltaTime;
                if (playerHpSubTimer >= 1)
                {
                    playerStatus.SetCurrentHP(-10);
                    playerHpSubTimer = 0f;
                }
            }

        }
    }

    private IEnumerator CameraZoomOut()
    {
        while (mainCamera.fieldOfView <= 50)
        {
            mainCamera.fieldOfView = mainCamera.fieldOfView + (cameraZoomSpeed * Time.deltaTime);
            yield return null;
        }

    }
    private IEnumerator CameraZoomIn()
    {
        while (mainCamera.fieldOfView >= 30)
        {
            mainCamera.fieldOfView = mainCamera.fieldOfView - (cameraZoomSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
