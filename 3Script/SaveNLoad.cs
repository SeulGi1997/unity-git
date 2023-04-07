using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayData
{
    // player
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public string currentWeapon;
    public float currentHP;
    public float currentThirst;
    public float currentHungry;

    //TIme
    public float hour;
    public float minute;
    public float second;

    
    // Inventory
    public List<string> haveItemNames = new List<string>();
    public List<int> haveItemCount = new List<int>(); // 아이템 갯수
    public List<int> haveItemSlotNum = new List<int>(); // 아이템 슬롯 번호

    // Craft
    public List<string> haveCraftNames = new List<string>();
    public List<Vector3> haveCraftPositions = new List<Vector3>();
    public List<Vector3> haveCraftRotations = new List<Vector3>();
    public List<int> haveCraftIndex = new List<int>();

    //Tutorial
    public bool isTutorialEnd;

}


public class SaveNLoad : MonoBehaviour
{

    [SerializeField]
    private GameObject[] craftPrefabs;

    private GameObject player;
    private Inventory inventory;
    private GameTimeScript gameTime;
    private TutorialScript tutorialScript;
    private WeaponController weaponController;
    private Status status;

    private string FILE_DIRECTORY;
    private string FILE_NAME;

    public static PlayData playdata;

    private void Awake()
    {
        playdata = new PlayData();
    }


    // Start is called before the first frame update
    void Start()
    {
        
        FILE_DIRECTORY = Application.persistentDataPath + "/SaveFile/";
        FILE_NAME = "SaveTxt";

        player = GameObject.Find("Player");
        gameTime = FindObjectOfType<GameTimeScript>();
        inventory = FindObjectOfType<Inventory>();
        tutorialScript = FindObjectOfType<TutorialScript>();
        weaponController = FindObjectOfType<WeaponController>();
        status = player.GetComponent<Status>();


        if (!Directory.Exists(FILE_DIRECTORY))
            Directory.CreateDirectory(FILE_DIRECTORY);

    

    }
    
    public void PlayDataSave()
    {
        //player
        playdata.playerPosition = player.transform.position;
        playdata.playerRotation = player.transform.eulerAngles;
        playdata.currentWeapon = WeaponController.currentWeapon;
        playdata.currentHP = status.currentHP;
        playdata.currentThirst = status.currentThirst;
        playdata.currentHungry = status.currentHungry;

        //Time
        playdata.hour = gameTime.currentHour;
        playdata.minute = gameTime.currentMinute;
        playdata.second = gameTime.currentSecond;
        
        //Inventory
        InventoryData(playdata.haveItemNames, playdata.haveItemCount, playdata.haveItemSlotNum);

        //tutorial
        playdata.isTutorialEnd = GameManager.isTutorialEnd;


        string json = JsonUtility.ToJson(playdata);

        File.WriteAllText(FILE_DIRECTORY + FILE_NAME, json);

        Debug.Log(json);

    }

    public void PlayDataLoad()
    {

        if (File.Exists(FILE_DIRECTORY + FILE_NAME))
        {
            string txt = File.ReadAllText(FILE_DIRECTORY + FILE_NAME);

            PlayData playData = JsonUtility.FromJson<PlayData>(txt);

            // player 정보 
            player.transform.position = playData.playerPosition;
            player.transform.eulerAngles = playData.playerRotation;
            if(playData.currentWeapon != "Hand")
                weaponController.ChangeWeapon(playData.currentWeapon);
            status.SetStatus(playData.currentHP, playData.currentThirst, playData.currentHungry);

            //Time
            gameTime.SetTime(playData.hour, playData.minute, playData.second);


            // Inventory 정보 
            LoadInventory(playData);

            // Craft 정보 
            LoadCraftData(playData);

            // Tutorial
            if (playData.isTutorialEnd == true)
                tutorialScript.EndTutorial();
            else
                tutorialScript.StartTutorial();


        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다");
        }

    }


    public void InventoryData(List<string> haveItemNames, List<int> haveItemCount, List<int> haveItemSlotNum)
    {

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i].GetComponent<Slot>().itemName != "" || inventory.slots[i].GetComponent<Slot>().itemName != null)
            {
                haveItemNames.Add(inventory.slots[i].GetComponent<Slot>().itemName);
                haveItemCount.Add(inventory.slots[i].GetComponent<Slot>().itemCount);
                haveItemSlotNum.Add(i);
            }
        }
    }

    public void LoadInventory(PlayData data)
    {

        inventory.AllReSetSlot();

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            for(int j=0; j < data.haveItemSlotNum.Count; j++)
            {
                if(i== data.haveItemSlotNum[j])
                {
                    for(int x=0; x < inventory.items.Length; x++)
                    {
                        if(data.haveItemNames[j] == inventory.items[x].itemName)
                        {
                            inventory.slots[i].GetComponent<Slot>().ActiveSlot(inventory.items[x]);
                            inventory.slots[i].GetComponent<Slot>().SetItemCount(data.haveItemCount[j]);
                        }
                    }
                    
                }
            }
        }
    }
    
    public void LoadCraftData(PlayData playData)
    {

        for(int i = 0; i < playData.haveCraftNames.Count; i++)
        {
            for(int j = 0; j < craftPrefabs.Length; j++)
            {
                if(playData.haveCraftNames[i] == craftPrefabs[j].name)
                {
                    var clone = Instantiate(craftPrefabs[j], playData.haveCraftPositions[i],Quaternion.Euler(playData.haveCraftRotations[i]));
                    PreView preview = clone.gameObject.GetComponent<PreView>();
                    PreViewHouse previewhouse = clone.gameObject.GetComponent<PreViewHouse>();

                    if(preview != null)
                    {
                        preview.LoadPositionLock();
                    }
                    else if(previewhouse !=null)
                    {
                        previewhouse.LoadPositionLock();
                    }


                   
                }
            }
     
        }
    }
}
