using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreView : MonoBehaviour
{
    [SerializeField]
    private float previewDestance; // 프리뷰와 플레이어 사이 거리
    [SerializeField]
    private GameObject thisCanvas;
    [SerializeField]
    private GameObject[] materialText; // Canvas Text
    [SerializeField]
    private int[] materialCount; //필요한 재료 개수
    [SerializeField]
    private string[] materialName; //재료 이름
    [SerializeField]
    private int[] currentMaterialCount; // 현재 가지고 있는 재료 개수 // 배열 수만 나타내고 초깃값 0으로

    [SerializeField]
    private GameObject craftObject; // 제작할 실제 오브젝트

    private GameObject craftButton; 
    private GameManager gameManager;
    private Inventory inventory;
    private GameObject mainCamera;
    private ImageWorldToScreen imageWorldToScreen;

    [SerializeField]
    private Collider lockBefore; // 위치 고정 전 콜라이더
    [SerializeField]
    private Collider lockAfter; // 위치 고정 후 콜라이더

    [SerializeField]
    private Material redMaterial; // 위치 고정 불가 색
    [SerializeField]
    private Material greenMaterial; // 위치 고정 가능 색


    private GameObject player;

    private RaycastHit hit;

    private bool isRed; 
    private bool isCraft; // 재료가 다 있어서 제작 가능한가
    private bool isPositionLock; // preview상태에서 지형으로 고정되었는가

    static private bool isCraftPreView; // 다른 preView 가 만들어 지고 있는 가 ( 지형에 고정하지 않은 상태)

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
        craftButton.GetComponentInChildren<Text>().text = "고정";
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

    // Canvas Text 를 카메라 정면으로 보이게 설정
    private void TextRotationUpdate()
    {
            thisCanvas.transform.rotation = Quaternion.LookRotation( thisCanvas.transform.position - mainCamera.transform.position);
    }

    // 위치고정
    private void PositionLock()
    {
        if (!isRed)
        {
            isPositionLock = true;
            craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
            craftButton.GetComponentInChildren<Text>().text = "제작";
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

    // 제작 가능 한가 true 가능 false 불가능 or Canvas Text 색 변경
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

    // 제작
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

    // 데이터 불러올 때 위치 고정 시키기
    public void LoadPositionLock()
    {

        StartCoroutine(LoadPositionLockCoroutine());
      

    }

    private IEnumerator LoadPositionLockCoroutine()
    {
        isPositionLock = true;

        yield return null;

        craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
        craftButton.GetComponentInChildren<Text>().text = "제작";
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
    
    // preview 에서 완료오브젝트로 전환 될 시 기존 preview를 저장데이터에서 제거
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
