using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSave : MonoBehaviour
{
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        int _index = this.gameObject.name.IndexOf("(Clone)");
        string _name = this.gameObject.name;
        if (_index > 0)
            _name = this.gameObject.name.Substring(0, _index);

        SaveNLoad.playdata.haveCraftNames.Add(_name);
        SaveNLoad.playdata.haveCraftPositions.Add(this.gameObject.transform.position);
        SaveNLoad.playdata.haveCraftRotations.Add(this.gameObject.transform.eulerAngles);
        SaveNLoad.playdata.haveCraftIndex.Add(GameManager.craftIndex);
        currentIndex = GameManager.craftIndex;
        GameManager.craftIndex++;
    }

}
