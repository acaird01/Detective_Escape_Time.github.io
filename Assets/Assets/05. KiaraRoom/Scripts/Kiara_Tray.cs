using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Kiara_Tray : MonoBehaviour
{
    // pos1은 감튀 또는 햄버거
    // pos2는 닭
    // pos3은 콜라 또는 커피
    public Transform[] setItemPos; // 아이템이 인스턴스 될 위치

    bool[] posCheck; // 각각의 위치에 올바른 아이템이 올라가 있는지 체크
    public bool datasave = false;
    GameObject[] instantiate_Item; // 해당 위치에 인스턴스한 아이템 들고있을 변수
    int episodeRound;
    string episode1_firstText = "어디..주문에 맞게 세팅해볼까.\n마침 음식은 다 나와있는거 같네.";
    string episode2_firstText = "이번에도 주문에 맞게 세팅해보자.\n그런데 음식은 다 나와있는건가?";
    string Wrong_answer_Text = "어라, 이게 아닌가?";
    string noitemInHotkey = "이건 음식이 아닌거 같은데?";
    public GameObject TakoKiara; // 성공했을 때 소환될 키아라 타코

    public GameObject SpawnMagic;
    Interaction_Gimics interaction;
    GameObject player;
    TextController textController;

    private void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();

        SpawnMagic.gameObject.SetActive(false);
        TakoKiara.gameObject.SetActive(false);
        episodeRound = GameManager.instance.Episode_Round;
        SetRndItem();
        StartSceneBoolSet();
        StartCoroutine(CheckCorrectItem());
        StartCoroutine(SpawnTako());

    }

    void StartSceneBoolSet()
    {
        posCheck = new bool[setItemPos.Length];
        for(int i = 0; i < posCheck.Length; i++)
        {
            posCheck[i] = false;
        }
    }

    /*void SceneStartSetting_KiaraTray() // 받아온 데이터에 따른 초기 위치 설정
    {
        if (interaction.run_Gimic)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }*/

    IEnumerator CheckCorrectItem() // 핫키에 있는 아이템이랑 놓아야 되는 아이템 비교해서 맞으면 생성, 틀리면 초기화 하는거
    {
        while (player)
        {
            instantiate_Item = new GameObject[setItemPos.Length];
            for (int i = 0; i < setItemPos.Length; i++)
            {
                if (interaction.run_Gimic == false)
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == true); // 상호작용이 들어오면
                    if (setItemPos[i].name == ItemManager._instance.hotkeyItemName) // 놓아야 하는 위치(순서)와 핫키에 등록된 아이템이 같다면
                    {
                        posCheck[i] = true; // 해당 위치 판별하는거 트루
                        instantiate_Item[i] = Instantiate(Resources.Load<GameObject>("Items/Prefabs/" + ItemManager._instance.hotkeyItemName)); // 리소스에서 핫키에 등록된 아이템 생성해서 instantiate_Item 배열에 담고                                                
                        instantiate_Item[i].gameObject.GetComponent<Kiara_SpawnItem>().dontSetactivefalse = true;
                        instantiate_Item[i].transform.position = setItemPos[i].position; // 위치 지정
                        instantiate_Item[i].transform.parent = setItemPos[i];
                        instantiate_Item[i].gameObject.GetComponent<Collider>().enabled = false;
                        if (instantiate_Item[i].gameObject.activeSelf == false)
                        {
                            instantiate_Item[i].gameObject.SetActive(true);
                        }
                    }
                    else if (ItemManager._instance.hotkeyItemName == "" || ItemManager._instance.hotkeyItemIndex != 28 && ItemManager._instance.hotkeyItemIndex != 29
                        && ItemManager._instance.hotkeyItemIndex != 30 && ItemManager._instance.hotkeyItemIndex != 31 && ItemManager._instance.hotkeyItemIndex != 32) 
                    {
                        if (i == 0)
                        {
                            if (episodeRound == 1)
                            {
                                StartCoroutine(textController.SendText(episode1_firstText));
                                i = -1;
                                break;
                            }
                            else
                            {
                                StartCoroutine(textController.SendText(episode2_firstText));
                                i = -1;
                                break;
                            }
                        }
                        else
                        {
                            StartCoroutine(textController.SendText(noitemInHotkey));
                            i--;
                        }
                    }                    
                    else
                    {
                        if (i != 0)
                        {
                            StartCoroutine(textController.SendText(Wrong_answer_Text));
                        }
                        for (int j = 0; j < i; j++) // 이때까지 반복문 돌며 들어왔던 배열들 초기화
                        {
                            Destroy(instantiate_Item[j]);
                            posCheck[j] = false;
                        }
                        break; // 싹 비운 뒤에 반복문 탈출해 처음부터 대기
                    }

                }
                else
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == false); // 상호작용이 들어오면
                    if (setItemPos[i].name == ItemManager._instance.hotkeyItemName) // 놓아야 하는 위치(순서)와 핫키에 등록된 아이템이 같다면
                    {
                        posCheck[i] = true; // 해당 위치 판별하는거 트루
                        instantiate_Item[i] = Instantiate(Resources.Load<GameObject>("Items/Prefabs/" + ItemManager._instance.hotkeyItemName)); // 리소스에서 핫키에 등록된 아이템 생성해서 instantiate_Item 배열에 담고                                                
                        instantiate_Item[i].gameObject.GetComponent<Kiara_SpawnItem>().dontSetactivefalse = true;
                        instantiate_Item[i].transform.position = setItemPos[i].position; // 위치 지정
                        instantiate_Item[i].transform.parent = setItemPos[i];
                        instantiate_Item[i].gameObject.GetComponent<Collider>().enabled = false;
                        if (instantiate_Item[i].gameObject.activeSelf == false)
                        {
                            instantiate_Item[i].gameObject.SetActive(true);
                        }
                    }
                    else if (ItemManager._instance.hotkeyItemName == "" || ItemManager._instance.hotkeyItemIndex != 28 && ItemManager._instance.hotkeyItemIndex != 29
                        && ItemManager._instance.hotkeyItemIndex != 30 && ItemManager._instance.hotkeyItemIndex != 31 && ItemManager._instance.hotkeyItemIndex != 32)
                    {
                        if (i == 0)
                        {
                            if (episodeRound == 1)
                            {
                                StartCoroutine(textController.SendText(episode1_firstText));
                                i = -1;
                                break;
                            }
                            else
                            {
                                StartCoroutine(textController.SendText(episode2_firstText));
                                i = -1;
                                break;
                            }
                        }
                        else
                        {
                            StartCoroutine(textController.SendText(noitemInHotkey));
                            i--;
                        }
                    }
                    else
                    {
                        if (i != 0)
                        {
                            StartCoroutine(textController.SendText(Wrong_answer_Text));
                        }
                        for (int j = 0; j < i; j++) // 이때까지 반복문 돌며 들어왔던 배열들 초기화
                        {
                            Destroy(instantiate_Item[j]);
                            posCheck[j] = false;
                        }
                        break; // 싹 비운 뒤에 반복문 탈출해 처음부터 대기
                    }

                }
            }
        }
    }




    IEnumerator SpawnTako() // 3개 아이템이 모두 알맞게 들어오면 타코 키아라 소환(1회차라서)
    {
        yield return new WaitUntil(() => posCheck[0] == true && posCheck[1] == true && posCheck[2] == true);
        //        Instantiate(TakoKiara, setItemPos[3]);
        TakoKiara.gameObject.SetActive(true);
        Vector3 targetPos = new Vector3(TakoKiara.transform.position.x, TakoKiara.transform.position.y + 0.5f, TakoKiara.transform.position.z);
        SpawnMagic.gameObject.SetActive(true);
        datasave = true;
        TakoKiara.GetComponent<Collider>().enabled = false;
        while (TakoKiara.transform.localPosition != targetPos)
        {
            TakoKiara.transform.localPosition = Vector3.MoveTowards(TakoKiara.transform.localPosition, targetPos, 0.004f * Time.timeScale);
            yield return null;
        }
        TakoKiara.GetComponent<Collider>().enabled = true;
        SpawnMagic.gameObject.GetComponent<ParticleSystem>().Stop();
        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        gameObject.SetActive(false);
    }

    void SetRndItem() // 랜덤한 아이템을 놓게 설정 메모 스크립트에서 이거 받아쓰기
    {
        int pos1 = Random.Range(0, 2);
        if(pos1 == 0)
        {
            setItemPos[0].name = "Item_28_Kiara_Burger";
        }
        else
        {
            setItemPos[0].name = "Item_29_Kiara_Potatoes";
        }
        setItemPos[1].name = "Item_30_Kiara_Chicken";
        int pos3 = Random.Range(0, 2);
        if (pos3 == 0)
        {
            setItemPos[2].name = "Item_31_Kiara_Coffee";
        }
        else
        {
            setItemPos[2].name = "Item_32_Kiara_Coke";
        }
    }
}
