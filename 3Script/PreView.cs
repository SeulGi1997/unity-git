using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreView : MonoBehaviour
{
    [SerializeField]
    private float previewDestance; // ������� �÷��̾� ���� �Ÿ�
    [SerializeField]
    private GameObject thisCanvas;
    [SerializeField]
    private GameObject[] materialText; // Canvas Text
    [SerializeField]
    private int[] materialCount; //�ʿ��� ��� ����
    [SerializeField]
    private string[] materialName; //��� �̸�
    [SerializeField]
    private int[] currentMaterialCount; // ���� ������ �ִ� ��� ���� // �迭 ���� ��Ÿ���� �ʱ갪 0����

    [SerializeField]
    private GameObject craftObject; // ������ ���� ������Ʈ

    private GameObject craftButton; 
    private GameManager gameManager;
    private Inventory inventory;
    private GameObject mainCamera;
    private ImageWorldToScreen imageWorldToScreen;

    [SerializeField]
    private Collider lockBefore; // ��ġ ���� �� �ݶ��̴�
    [SerializeField]
    private Collider lockAfter; // ��ġ ���� �� �ݶ��̴�

    [SerializeField]
    private Material redMaterial; // ��ġ ���� �Ұ� ��
    [SerializeField]
    private Material greenMaterial; // ��ġ ���� ���� ��


    private GameObject player;

    private RaycastHit hit;

    private bool isRed; 
    private bool isCraft; // ��ᰡ �� �־ ���� �����Ѱ�
    private bool isPositionLock; // preview���¿��� �������� �����Ǿ��°�

    static private bool isCraftPreView; // �ٸ� preView �� ����� ���� �ִ� �� ( ������ �������� ���� ����)

    private int currentIndex;



    // Start is called before the first frame update
    private void Awake()
    {
       // craftButton = GameObject.Find("Canvas").transform.Find("CraftButton").gameObject;
    }
    void Start()
    {
        isCraftPreView = true;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = gameManager.inventory;
        craftButton = gameManager.craftButton;
        craftButton.SetActive(true);
        craftButton.GetComponentInChildren<Text>().text = "����";
        craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
        craftButton.GetComponent<Button>().onClick.AddListener(PositionLock);
        imageWorldToScreen = GetComponent<ImageWorldToScreen>();

        player = GameObject.Find("Player");
        mainCamera = gameManager.mainCamera;
        Debug.Log(GameObject.Find("PlayGroup").transform.Find("CraftButton").gameObject.name);

    }



    // Update is called once per frame
    void Update()
    {
        if (!isPositionLock)
        {
           
            if (Physics.Raycast(player.transform.position + player.transform.up * 3f + player.transform.forward * previewDestance, Vector3.down, out hit, 10f, LayerMask.GetMask("Terrain")))
            {

                this.transform.position = hit.point;
            }
        }
        else
        {
            isCraft = IsCraft();
        }

        if (isCraftPreView)
        {
            for (int i = 0; i < materialText.Length; i++)
            {
                materialText[i].gameObject.SetActive(false);
            }
        }

        TextRotationUpdate();
    }

    // Canvas Text �� ī�޶� �������� ���̰� ����
    private void TextRotationUpdate()
    {
            thisCanvas.transform.rotation = Quaternion.LookRotation( thisCanvas.transform.position - mainCamera.transform.position);
    }

    // ��ġ����
    private void PositionLock()
    {
        if (!isRed)
        {
            isPositionLock = true;
            craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
            craftButton.GetComponentInChildren<Text>().text = "����";
            craftButton.SetActive(false);
            isCraftPreView = false;
            lockBefore.enabled = false;
            lockAfter.enabled = true;
            imageWorldToScreen.enabled = true;
            this.gameObject.tag = "Craft";

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

    // ���� ���� �Ѱ� true ���� false �Ұ��� or Canvas Text �� ����
    private bool IsCraft()
    {
        bool _isCraft = true;

        for (int i = 0; i < materialName.Length; i++)
        { 
           if( currentMaterialCount[i] < materialCount[i])
            {
                _isCraft = false;
                materialText[i].GetComponent<Text>().color = Color.red;
            }
            else
            {
                materialText[i].GetComponent<Text>().color = Color.white;
            }
        }

        return _isCraft;
    }

    // ����
    private void Craft()
    {

        for (int i = 0; i < materialName.Length; i++)
        {
            inventory.UsedItem(materialName[i], -(materialCount[i]));
        }


        Instantiate(craftObject, this.transform.position, Quaternion.identity);

        craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
        craftButton.SetActive(false);

        Destroy(this.gameObject);


    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isCraftPreView)
        {
            if (other.tag == "Player")
            {
                for (int i = 0; i < materialText.Length; i++)
                {
                    materialText[i].gameObject.SetActive(true);
                }

                craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
                craftButton.GetComponent<Button>().onClick.AddListener(Craft);
                
                
            }
        }
        
          
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isCraftPreView)
        {
            if (other.tag == "Player")
            {
                for (int i = 0; i < materialText.Length; i++)
                {
                    materialText[i].gameObject.SetActive(true);
                    currentMaterialCount[i] = inventory.GetItemCount(materialName[i]);
                    materialText[i].GetComponent<Text>().text = materialName[i] + " " + inventory.GetItemCount(materialName[i]) + " / " + materialCount[i];
                }

                if (isCraft)
                {

                    craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    craftButton.GetComponent<Button>().onClick.AddListener(Craft);
                    craftButton.SetActive(true);
                }
                else
                {
                    craftButton.SetActive(false);
                }

            }

            
        }

        if (!isPositionLock)
        {
           if(other.tag != "Terrain" && other.tag != "Item")
            {
                if(other.isTrigger == false)
                {
                    isRed = true;

                    GetComponent<Renderer>().material = redMaterial;
                }
               
            }
            
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isCraftPreView)
        {
            if (other.tag == "Player")
            {
                for (int i = 0; i < materialText.Length; i++)
                {
                    materialText[i].gameObject.SetActive(false);
                }

                craftButton.SetActive(false);
                craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
            }

           
        }

        if (!isPositionLock)
        {
            if (other.tag != "Terrain" && other.tag != "Item")
            {
                if(other.isTrigger == false)
                {
                    isRed = false;
                    GetComponent<Renderer>().material = greenMaterial;
                }
                
            }
        }
       
    }

    // ������ �ҷ��� �� ��ġ ���� ��Ű��
    public void LoadPositionLock()
    {

        StartCoroutine(LoadPositionLockCoroutine());
      

    }

    private IEnumerator LoadPositionLockCoroutine()
    {
        isPositionLock = true;

        yield return null;

        craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
        craftButton.GetComponentInChildren<Text>().text = "����";
        craftButton.SetActive(false);
        isCraftPreView = false;
        lockBefore.enabled = false;
        lockAfter.enabled = true;
        imageWorldToScreen.enabled = true;

        this.gameObject.tag = "Craft";

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
    
    // preview ���� �Ϸ������Ʈ�� ��ȯ �� �� ���� preview�� ���嵥���Ϳ��� ����
    private void OnDestroy()
    {
        for (int i = 0; i < SaveNLoad.playdata.haveCraftIndex.Count; i++)
        {
            if (SaveNLoad.playdata.haveCraftIndex[i] == currentIndex)
            {
                SaveNLoad.playdata.haveCraftIndex.RemoveAt(i);
                SaveNLoad.playdata.haveCraftNames.RemoveAt(i);
                SaveNLoad.playdata.haveCraftPositions.RemoveAt(i);
                SaveNLoad.playdata.haveCraftRotations.RemoveAt(i);
            }
        }

        Debug.Log("onDestroy");
    }

}
