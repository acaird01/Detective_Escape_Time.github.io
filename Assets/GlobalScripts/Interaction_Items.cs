using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction_Items : MonoBehaviour
{
    public bool run_Gimic; // 기믹 작동 여부
    GameObject player; // 플레이어
    GameObject interaction_F;

    public int interactionRange; // 상호작용 가능 거리

    TextController textConteroller;
    [TextArea]
    public string _text = ""; // 아이템 or text가 나올 오브젝트가 들고있는 string 텍스트 값 저장할 변수

    public GameObject audioSource_Object = null; // 오디오 소스가 들어있을 경우 여기에 해당 오브젝트를 넣어주자

    [SerializeField]
    GameObject ItemEffect;

    int itemIndex = 0;
    IItem itemData; 

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        Init();

        textConteroller = player.GetComponentInChildren<TextController>();

        Setting_Scene_Gimic();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (itemIndex == 0)
        {
            if (gameObject.GetComponent<IItem>() != null)
            {
                itemIndex = gameObject.GetComponent<IItem>().Index;
                itemData = ItemManager._instance.inventorySlots[itemIndex].GetComponent<IItem>();
            }
        }

        if (ItemEffect != null)
        {
            FindNearPlayer();
        }
    }

    void Setting_Scene_Gimic()
    {
        // 여기서 게임 매니저의 정보를 받아서 정보를 토대로
        run_Gimic = false; // true false 세팅
        // 상호작용할 오브젝트에게 bool값을 던져줘서 상태에 맞는 텍스처 세팅해줌
    }

    void OnMouseOver()
    {
        if (player)
        {
            // 플레이어와 상호작용하는 오브젝트 사이의 거리
            float dist = Vector3.Distance(player.transform.position, transform.position);

            // 거리가 5보다 작을 경우 실행
            if (dist < interactionRange)
            {
                interaction_F.gameObject.SetActive(true);

                if (run_Gimic == false)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (gameObject.tag == "ITEM")
                        {
                            StartCoroutine(running());
                        }
                        else
                        {
                            SendText();
                            AudioSourcePlay();
                        }
                    }
                }
            }
            else
            {
                if(gameObject.GetComponent<IItem>() != null)
                {
                    itemIndex = itemData.Index;
                }
                interaction_F.gameObject.SetActive(false);
            }
        }
    }

    void FindNearPlayer()
    {
        if (itemIndex != 0)
        {
            if (itemData.isGetItem == false)
            {
                float dist = Vector3.Distance(player.transform.position, transform.position);

                if (dist < interactionRange)
                {
                    ItemEffect.gameObject.SetActive(true);
                }
                else
                {
                    ItemEffect.gameObject.SetActive(false);
                }
            }
            else
            {                
                ItemEffect.gameObject.SetActive(false);
            }
        }
    }


    private void OnMouseExit()
    {
        interaction_F.gameObject.SetActive(false);
    }

    void SendText() // 아이템(상호작용 하는 오브젝트)에서 텍스트 받아와서 텍스트 출력
    {
        if (_text != "")
        {
            StartCoroutine(textConteroller.SendText(_text));
        }
    }

    void AudioSourcePlay() // 사운드 출력 하고싶으면 오디오 소스 들고있는 오브젝트에 interaction_item스크립트 넣고, getcomponent해서 audioSource_Object = gameObject.this << 이거 start에 너주면됨
    {
        if (audioSource_Object != null)
        {
            if (audioSource_Object.GetComponent<AudioSource>() != null)
            {
                AudioSource audioSource = audioSource_Object.GetComponent<AudioSource>();

                if (audioSource)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
            }
        }
    }
    IEnumerator running() // 기믹 작동했을 때 
    {
        ItemManager._instance.CollectItem(this.gameObject);
        // Debug.Log("상호작용: " + this.gameObject.name);

        run_Gimic = true;
        yield return new WaitForSeconds(.1f);

        if (interaction_F)
        {
            interaction_F.gameObject.SetActive(false); // 켜져있으면 비활성화
        }

        this.gameObject.SetActive(false);
    }

    public void Init()
    {
        player = GameObject.Find("Player");
        interaction_F = ItemManager._instance.interaction_F;    // 상호작용 가능 표시 이미지를 아이템 매니저에서 가져오기  

        if (gameObject.GetComponent<IItem>() != null)
        {
            //itemIndex = ItemManager._instance.GetComponent<IItem>().Index;
            //itemIndex = gameObject.GetComponent<IItem>().Index;
            itemIndex = gameObject.GetComponent<IItem>().Index;
            itemData = ItemManager._instance.inventorySlots[itemIndex].GetComponent<IItem>();
        }
        if(ItemEffect != null)
        {
            ItemEffect.gameObject.SetActive(false);
        }

        if (GameManager.instance.nowSceneName == "04. CalliScene_1" || GameManager.instance.nowSceneName == "07. CalliScene_2")
        {
            interactionRange = 10;
        }
        else
        {
            interactionRange = 5;
        }

        //StartCoroutine(FindNearPlayer());
    }
}
