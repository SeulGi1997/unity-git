using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseScript : MonoBehaviour
{
    private GameObject saveButton;

    void Start()
    {
        saveButton = GameObject.Find("GameManager").GetComponent<GameManager>().saveButton;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(other.isTrigger == false)
            {
                saveButton.SetActive(true);
                saveButton.GetComponent<Button>().onClick.AddListener(CloseButton);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.isTrigger == false)
            {
                saveButton.GetComponent<Button>().onClick.RemoveListener(CloseButton);
                saveButton.SetActive(false);
            }
        }
    }

    private void CloseButton()
    {
        saveButton.gameObject.SetActive(false);
    }

  
}
