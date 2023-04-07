using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject craftButton;
    public Inventory inventory;
    public Status status;

    public GameObject pickupButton;
    public GameObject playerHandWood;
    public GameObject saveButton;

    public GameObject mainCamera;

    public static SwimScript swimScript;

    public static bool isTutorialEnd;

    public static int craftIndex;

    private void Awake()
    {
        swimScript = GameObject.FindObjectOfType<SwimScript>();
    }

}
