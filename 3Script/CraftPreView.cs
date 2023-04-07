using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftPreView : MonoBehaviour
{
    [SerializeField]
    private GameObject preViewPrefab;
    [SerializeField]
    private CraftController craftController;


    

    public void CraftPreViewMethod()
    {
        craftController.CraftPreView(preViewPrefab);
    }
}
