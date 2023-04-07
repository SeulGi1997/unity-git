using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPbar : MonoBehaviour
{
    [SerializeField]
    private Enemy thisEnemy;
    [SerializeField]
    private Slider hpSlider;

    private GameManager gameManager;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCamera = gameManager.mainCamera.GetComponent<Camera>();
    }

    private void Update()
    {
        this.transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        HPbarValueUpdate();
    }

    private void HPbarValueUpdate()
    {
        hpSlider.value = thisEnemy.currentHp;
    }
}
