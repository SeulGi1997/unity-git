using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
   

    [SerializeField]
    private GameObject[] weapons;


    [SerializeField]
    private GameObject bowButton;
    [SerializeField]
    private GameObject melleButton;

    private GameObject player;
    private Animator player_Anim;

    public float readyTime;

    public static bool isWeaponChange;
    public static string currentWeapon = "Axe";

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        player_Anim = player.GetComponentInChildren<Animator>();
    }


    public void SelectBow()
    {
        if (!isWeaponChange)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].GetComponent<Weapon>().weaponName == "WoodenBow")
                {
                    //StartCoroutine("ChangeMotion", i);
                    weapons[i].SetActive(true);
                }
                else
                {
                    weapons[i].SetActive(false);
                }

            }

            player.GetComponent<PlayerAttack>().currentWeapon = "Bow";

            bowButton.SetActive(true);
            melleButton.SetActive(false);
        }
       
    }

    public void SelectAxe()
    {
        if (!isWeaponChange)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].GetComponent<Weapon>().weaponName == "StoneAxe")
                {
                    // StartCoroutine("ChangeMotion", i);
                    weapons[i].SetActive(true);
                }
                else
                {
                    weapons[i].SetActive(false);
                }

            }

            player.GetComponent<PlayerAttack>().currentWeapon = "Sword";

            bowButton.SetActive(false);
            melleButton.SetActive(true);
        }
        
    }

    private IEnumerator ChangeMotion(int i)
    {
        if (weapons[i].GetComponent<Weapon>().weaponName == "Wooden Bow")
            player_Anim.SetBool("isMirror", true);

        isWeaponChange = true;
        player_Anim.SetTrigger("doWeaponChange");
        yield return new WaitForSeconds(readyTime);
        weapons[i].SetActive(true);
        yield return new WaitForSeconds(0.2f);

        if (weapons[i].GetComponent<Weapon>().weaponName == "Bow")
            player_Anim.SetBool("isMirror", false);

        isWeaponChange = false;
    }

    public void ChangeWeapon(string _itemName)
    {

        Debug.Log(_itemName);
        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i].GetComponent<Weapon>().weaponName == _itemName)
            {
                weapons[i].SetActive(true);
                currentWeapon = _itemName;

                if(weapons[i].GetComponent<Weapon>().weaponType == Weapon.WeaponTpye.Melle)
                {
                    melleButton.SetActive(true);
                    bowButton.SetActive(false);
                }
                else
                {
                    melleButton.SetActive(false);
                    bowButton.SetActive(true);
                }

            }
            else
            {
                weapons[i].SetActive(false);
            }
        }
    }
}
