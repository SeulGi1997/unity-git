using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageWorldToScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject imagePrefab;
    [SerializeField]
    private float pivot_y; 
    [SerializeField]
    private float Pivot_x;
    [SerializeField]
    private float angleRaidus; // 플레이어 시야각도

    private GameObject imageClone;

    private GameObject cavasTrasform;
    private Camera mainCamera;
    private GameObject mainCameraDirection;


    // Start is called before the first frame update
    void Start()
    {
        cavasTrasform = GameObject.Find("ImageWorldToScreen");
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        mainCameraDirection = mainCamera.transform.GetChild(0).gameObject;

        imageClone = Instantiate(imagePrefab, this.gameObject.transform.position, Quaternion.identity, cavasTrasform.transform);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection  = (this.transform.position - mainCameraDirection.transform.position).normalized;

        float angle = Vector3.Angle(targetDirection, mainCameraDirection.transform.forward);

        if (angle <= angleRaidus)
        {
            imageClone.SetActive(true);
            imageClone.transform.position = mainCamera.WorldToScreenPoint(this.transform.position + new Vector3(Pivot_x, pivot_y, 0f));
        }
        else
        {
            imageClone.SetActive(false);
        }

    }

    private void OnDestroy()
    {
        Destroy(imageClone);
    }
}
