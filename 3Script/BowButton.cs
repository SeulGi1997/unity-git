using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BowButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{

    [SerializeField]
    private float CameraSpeed;
    [SerializeField]
    private GameObject bowCamera;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject playerBodyUpper;

    private bool isTouch;

    private Vector2 bowCameraRotation;



    private void LateUpdate()
    {
        if (isTouch)
        {
            // 플레이어 상체 움직임
            playerBodyUpper.transform.localEulerAngles = new Vector3(playerBodyUpper.transform.localPosition.x, bowCamera.transform.localEulerAngles.x, playerBodyUpper.transform.localPosition.z);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 카메라 상하 움직임
        bowCameraRotation = eventData.delta * Time.deltaTime * CameraSpeed;

        Vector3 currentBowCameraRotation = bowCamera.transform.localEulerAngles;
        Vector3 currentBowCameraPositon = bowCamera.transform.localPosition;

        bowCamera.transform.RotateAround(player.transform.position, player.transform.right, -bowCameraRotation.y);

        float _x = bowCamera.transform.localEulerAngles.x > 180 ? bowCamera.transform.localEulerAngles.x - 360 : bowCamera.transform.localEulerAngles.x;

        if (_x <= -40 || _x >= 40)
        {
            bowCamera.transform.localEulerAngles = currentBowCameraRotation;
            bowCamera.transform.localPosition = currentBowCameraPositon;
        }


        // 카메라(플레이어) 좌우 움직임
        player.transform.Rotate(Vector3.up, eventData.delta.x * Time.deltaTime * CameraSpeed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        bowCamera.transform.RotateAround(player.transform.position, player.transform.right, -bowCamera.transform.localEulerAngles.x);
    }
}
