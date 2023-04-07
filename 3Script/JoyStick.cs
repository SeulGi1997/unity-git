using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{

    [SerializeField]
    private RectTransform rect_Background;
    [SerializeField]
    private RectTransform rect_Joystick;

    private float radius;

    [SerializeField]
    private Player player;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float swimSpeed;

    [SerializeField]
    private Animator Player_animator;

    [SerializeField]
    private GameObject theCamera;

    public static bool isMove = true; // 움직임 제한
    public static bool isWalk = true;

    private bool isTouch = false;

    private Vector3 movePosition;

    [SerializeField]
    private Rigidbody playerRigid;

    [SerializeField]
    private float testNum;
    [SerializeField]
    private GameObject swimCamera;

    private Status theStatus;



    // Start is called before the first frame update
    void Start()
    {
        theStatus = FindObjectOfType<Status>();
        radius = rect_Background.rect.width * 0.5f;
    
    }

    private void FixedUpdate()
    {
        if (isTouch)
        {
            if (isMove)
            {
                if (SwimScript.isSwim == false && BowScript.isBow == false)
                {
                    player.transform.LookAt(player.transform.position + theCamera.transform.right * movePosition.x + theCamera.transform.forward * movePosition.z); // 플레이어 방향 rotation
                    playerRigid.velocity = player.transform.forward * (isWalk ? (player.isCarry ? 4f : 5f) : (player.isCarry ? 8f : 10f)) + player.transform.up * playerRigid.velocity.y; // 이동 position
                }
                else
                {

                    if(SwimScript.isSwim == true)
                    {
                        playerRigid.velocity = swimCamera.transform.forward * movePosition.z * Time.deltaTime + swimCamera.transform.right * movePosition.x * Time.deltaTime;

                        if (SwimScript.isSwimOnWater == true)
                        {
                            if (playerRigid.velocity.y > 0)
                                playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0f, playerRigid.velocity.z);

                        }
                    }
                    else if (BowScript.isBow == true)
                    {
                        playerRigid.velocity = player.transform.forward * movePosition.z * Time.deltaTime *0.3f+ player.transform.right * movePosition.x * Time.deltaTime * 0.3f;
                    }

                }

                if (isWalk)
                {
                    Player_animator.SetBool("isRun", false);
                    Player_animator.SetBool("isWalk", true);
                }
                else
                {
                    if (!player.isCarry)
                    {
                        Player_animator.SetBool("isWalk", false);
                        Player_animator.SetBool("isRun", true);
                    }
                    else
                    {
                        Player_animator.SetBool("isRun", false);
                        Player_animator.SetBool("isWalk", true);
                    }
                   
                }

                RunnigSubStatus();
            }


        }
    }

    public void RunnigSubStatus()
    {
        if(isWalk == false)
        {
            if(theStatus.currentPhysicalStrength >= 0.1f)
                theStatus.currentPhysicalStrength -= 0.1f * Time.deltaTime;
            else
            {
                isWalk = true;
            }
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {

        
    

    }

    public void OnDrag(PointerEventData eventData)
    {
        isTouch = true;

        Vector2 value = eventData.position- (Vector2)rect_Background.position;
        value = Vector2.ClampMagnitude(value, radius);

        rect_Joystick.localPosition = value;

        float distance = Vector2.Distance(rect_Background.position, rect_Joystick.position) / radius; // Distanve(a,b) a에서 b 사이의 거리값 리턴

        value = value.normalized;
       
        movePosition = new Vector3(value.x * moveSpeed * distance * 0.5f , 0f, value.y * moveSpeed * distance * 0.5f);

        if (SwimScript.isSwim)
        {
            if (SwimScript.isSwimOnWater)
            {
                playerRigid.useGravity = false;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        isTouch = false;
        rect_Joystick.localPosition = Vector3.zero;
        movePosition = Vector3.zero;
        playerRigid.velocity = Vector3.zero;

        Player_animator.SetBool("isRun", false);
        Player_animator.SetBool("isWalk", false);

        if (SwimScript.isSwim)
        {
            if (SwimScript.isSwimOnWater)
            {
                playerRigid.useGravity = true;
            }
        }

    }
    


}
