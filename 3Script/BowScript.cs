using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour
{
    [SerializeField]
    private GameObject showArrow;
    [SerializeField]
    private GameObject starterArrowPosition;

    [SerializeField]
    private Camera bowCamera; // 서브카메라
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private float arrowSpeed;

    private MainCamera mainCamera; // 메인 카메라
    [SerializeField]
    private float cameraZoomSpeed;
    [SerializeField]
    private GameObject crossHair;
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private AudioClip bowReadyClip;
    [SerializeField]
    private AudioClip bowShotClip;
    

    private float readyTime;

    public static bool isBow;

    private bool isAttack;
    private bool isTouch;
    private bool isUpKey;


    [SerializeField]
    private GameObject arrow;



    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>();
        readyTime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouch)
        {
            readyTime -= Time.deltaTime;
            if (readyTime <= 0)
            {
                isAttack = true;
            }

        }

    }

  

    public void BowShot()
    {
        AudioManager.instance.EffectPlay(bowShotClip,0.5f);
        //var clone = Instantiate(arrow, bowCamera.transform.position + bowCamera.transform.forward, bowCamera.transform.rotation);
        var clone = Instantiate(arrow, starterArrowPosition.transform.position + bowCamera.transform.forward, bowCamera.transform.rotation);

        clone.GetComponent<Rigidbody>().AddForce(bowCamera.transform.forward * arrowSpeed);


        Debug.Log("Soht");
    }

    public void ButtonDown()
    {
        if (!isTouch)
        {

            isBow = true;
            isUpKey = true;
            isTouch = true;
            bowCamera.enabled = true;
            showArrow.SetActive(true);

            AudioManager.instance.EffectPlay(bowReadyClip,0.5f);

            playerBody.transform.localEulerAngles = new Vector3(0f, 40f, 0f);

            playerAnim.SetBool("isShot", true);
            crossHair.SetActive(true);
            player.GetComponent<Player>().PlayerRotateValueCamera();    

        }

    }

    public void ButtonUp()
    {
        if (isUpKey)
        {
            isBow = false;
            isUpKey = false;
           

            if (isAttack)
            {
                BowShot();
            }

            isAttack = false;
            readyTime = 1f;
            bowCamera.enabled = false;
            crossHair.SetActive(false);
            showArrow.SetActive(false);
            playerBody.transform.localEulerAngles = Vector3.zero;
            playerAnim.SetBool("isShot", false);

            JoyStick.isMove = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            playerAnim.SetBool("isRun", false);
            playerAnim.SetBool("isWalk", false);

            Invoke("ActiveTouch", 0.5f);



            mainCamera.BowShotCameraReset();

        }

    }

   

    private void ActiveTouch()
    {
        isTouch = false;
        JoyStick.isMove = true;
    }

   

}
