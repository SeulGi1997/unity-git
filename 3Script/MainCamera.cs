using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Vector3 startOffset;

    [SerializeField]
    private float CameraSpeed;

    [SerializeField]
    private GameObject cameraDirection;



    private Vector3 distanceOffset;

    private Vector3 currentPosition;
    private Vector3 currentRoation;


    public float lerpSpeed;


    // private Vector3 refVector = Vector3.zero;

    private Vector2 swimCameraRotation;
    private Vector2 bowCameraRotation;

    [SerializeField]
    private GameObject swimCamera;
    [SerializeField]
    private GameObject bowCamera;

    // Start is called before the first frame update
    void Start()
    {

        this.transform.position = player.transform.position + startOffset;

        distanceOffset = this.transform.position - player.transform.position;

    }




    // Update is called once per frame
    void Update()
    {

        transform.position = player.transform.position + distanceOffset;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
        CameraMove();

        cameraDirection.transform.localEulerAngles = new Vector3(-(this.transform.localEulerAngles.x), cameraDirection.transform.localEulerAngles.y, cameraDirection.transform.localEulerAngles.z);

    }

    private void CameraMove()
    {

        if (Input.touchCount > 0)
        {

            for (int i = 0; i < Input.touchCount; i++)
            {

                if (EventSystem.current.IsPointerOverGameObject(i) == false)
                {

                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began)
                    {

                        //터치시작

                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {

                        Vector2 movePosition = touch.deltaPosition;

                        currentPosition = this.transform.position;
                        currentRoation = this.transform.localEulerAngles;

                        // 카메라 상하
                        this.transform.RotateAround(player.transform.position, cameraDirection.transform.right, -movePosition.y * CameraSpeed * Time.deltaTime);

                        if (this.transform.localEulerAngles.x >= 80 || this.transform.localEulerAngles.x <= 1) // 카메라 상하 각도 제한
                        {
                            this.transform.position = currentPosition;
                            this.transform.localEulerAngles = currentRoation;
                        }
                        distanceOffset = transform.position - player.transform.position;

                        // 카메라 좌우
                        this.transform.RotateAround(player.transform.position, Vector3.up, movePosition.x * CameraSpeed * Time.deltaTime);
                        distanceOffset = transform.position - player.transform.position;






                        if (SwimScript.isSwim == true )
                        {
                            // 카메라 상하 움직임
                            swimCameraRotation -= touch.deltaPosition * Time.deltaTime * CameraSpeed;
                            float moveX = Mathf.Clamp(swimCameraRotation.y , -90f, 90f);
                            swimCamera.transform.localEulerAngles = new Vector3(moveX, 0, 0);


                            // 카메라(플레이어) 좌우 움직임
                            player.transform.Rotate(Vector3.up, touch.deltaPosition.x * Time.deltaTime * CameraSpeed);
                        }
                        else
                        {
                            swimCamera.transform.localEulerAngles = Vector3.zero;
                        }


                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {

                        //터치 종료

                    }

                }

            }

        }
    }

    public void BowShotCameraReset()
    {
        this.transform.position = player.transform.position + startOffset;
        this.transform.eulerAngles = new Vector3(0f, 0f, this.transform.eulerAngles.z);
        this.transform.RotateAround(player.transform.position, player.transform.up, player.transform.eulerAngles.y);
        //float moveX = Mathf.Clamp((swimCamera.transform.localEulerAngles.x > 180) ? swimCamera.transform.localEulerAngles.x -360: swimCamera.transform.localEulerAngles.x, 0f, 90f);
        //this.transform.RotateAround(player.transform.position, player.transform.right, moveX);
        this.transform.RotateAround(player.transform.position, player.transform.right, 15f);

        distanceOffset = this.transform.position - player.transform.position;
    }

 
}
