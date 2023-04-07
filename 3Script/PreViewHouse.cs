using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreViewHouse : MonoBehaviour
{
    [SerializeField]
    private float previewDestance;
    [SerializeField]
    private GameObject thisCanvas;

    [SerializeField]
    private GameObject[] materialText;

    private GameObject player;

    private RaycastHit hit;

    [SerializeField]
    private int[] materialCount; //�ʿ��� ��� ����
    [SerializeField]
    private string[] materialName; //��� �̸�
    [SerializeField]
    private int[] currentMaterialCount; // ���� ������ �ִ� ��� ���� // �迭 ���� ��Ÿ���� �ʱ갪 0����

    [SerializeField]
    private GameObject craftObject; // �����䰡 �ƴ� ������ ���� ������Ʈ

    private GameObject craftButton;
    private GameManager gameManager;
    private Inventory inventory;

    [SerializeField]
    private Material greenMaterial;
    [SerializeField]
    private Material redMaterial;

    [SerializeField]
    private Collider lockBefore;
    [SerializeField]
    private Collider lockAfter;

    private MeshRenderer[] renderers;

    private bool isRed;


    private GameObject mainCamera;
    private ImageWorldToScreen imageWorldToScreen;

    private bool isCraft; // ��ᰡ �� �־ ���� �����Ѱ�


    private bool isPositionLock; // preview���¿��� �������� �����Ǿ��°�


    static private bool isCraftPreView; // �ٸ� preView �� ����� ���� �ִ� �� ( ������ �������� ���� ����)

    private int currentIndex;


    // Start is called before the first frame update
    void Start()
    {
        isCraftPreView = true;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        craftButton = gameManager.craftButton;
        inventory = gameManager.inventory;

        craftButton.SetActive(true);
        craftButton.GetComponentInChildren<Text>().text = "����";
        craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
        craftButton.GetComponent<Button>().onClick.AddListener(PositionLock);

        player = GameObject.Find("Player");

        renderers = GetComponentsInChildren<MeshRenderer>();
        imageWorldToScreen = GetComponent<ImageWorldToScreen>();

        mainCamera = gameManager.mainCamera;
    }



    // Update is called once per frame
    void Update()
    {
        if (!isPositionLock)
        {
            transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

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
    private void TextRotationUpdate()
    {
        thisCanvas.transform.rotation = Quaternion.LookRotation(thisCanvas.transform.position - mainCamera.transform.position);
    }

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


    private bool IsCraft()
    {
        bool _isCraft = true;

        for (int i = 0; i < materialName.Length; i++)
        {
            if (currentMaterialCount[i] < materialCount[i])
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

    private void Craft()
    {

       Instantiate(craftObject, this.transform.position, this.transform.rotation);

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
            
            if (other.tag == "ItemTree")
            {
                if(other.isTrigger == false)
                {
                    Destroy(other.gameObject);
                    currentMaterialCount[0] += 1;
                }
                
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
                    materialText[i].GetComponent<Text>().text = materialName[i] + " " + currentMaterialCount[i] + " / " + materialCount[i];
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
            if (other.tag != "Terrain" && other.tag != "Item")
            {
                if(other.isTrigger == false)
                {
                    isRed = true;

                    for (int i = 0; i < renderers.Length; i++)
                    {
                        renderers[i].material = redMaterial;
                    }
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
                if (other.isTrigger == false)
                {
                    isRed = false;

                    for (int i = 0; i < renderers.Length; i++)
                    {
                        renderers[i].material = greenMaterial;
                    }
                }
            }

        }
    }

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
