using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Gura_ObjectManager : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음

    #endregion

    // 데이터 받아와서 10진법으로 만들어서 각각 오브젝트에게 뿌려주기
    // 각각 오브젝트들 상태 받아서 2진법으로 만들어서넘겨주기

    public GameObject[] SceneGimics;  // 각 씬에 배치할 때 데이터 주고받을 기믹들 여기에 저장
    string binarySystem_forDataSave = "1"; // 제일 앞에 1으로 고정해 자릿수 보정

    int binarySystem_LoadData = 0;

    GameObject player;
    [SerializeField]
    Transform playerSpawnPos;

    [SerializeField]
    GameObject Trident;
    [SerializeField]
    GameObject TakoGura;

    [SerializeField]
    GameObject KroniiSword;
    [SerializeField]
    GameObject BaelzDice;
    [SerializeField]
    GameObject TakoMumei;
    [SerializeField]
    GameObject TakoBell;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        ChangeGameManager_To_SceneData(); // 씬 시작했을 때 데이터 불러와서 뿌려주기
        player = GameObject.Find("Player");
        player.transform.position = playerSpawnPos.position;

        player.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (GameManager.instance.Episode_Round == 1)
        {
            if (ItemManager._instance.inventorySlots[11].GetComponent<IItem>().isGetItem == true)
            {
                Trident.SetActive(false);
            }

            if (ItemManager._instance.inventorySlots[6].GetComponent<IItem>().isGetItem == true)
            {
                TakoGura.SetActive(false);
            }
        }
        else if (GameManager.instance.Episode_Round == 2)
        {
            if (ItemManager._instance.inventorySlots[1].GetComponent<IItem>().isGetItem == true)
            {
                TakoMumei.SetActive(false);
            }

            if (ItemManager._instance.inventorySlots[5].GetComponent<IItem>().isGetItem == true)
            {
                TakoBell.SetActive(false);
            }

            if (ItemManager._instance.inventorySlots[14].GetComponent<IItem>().isGetItem == true)
            {
                BaelzDice.SetActive(false);
            }

            if (ItemManager._instance.inventorySlots[16].GetComponent<IItem>().isGetItem == true)
            {
                KroniiSword.SetActive(false);
            }
        }

        // 씬 진입 대사 출력
        // 만약 저장데이터가 0이라면 들어온적이 없다는 것이므로 안내문구 출력
        if (GameManager.instance.GuraScene_Save == 0)
        {
            if (GameManager.instance.Episode_Round == 1)    // 1회차
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("여긴 진짜 바닷속인가?\n일단 눈 앞의 구조물을 확인해봐야겠어."));
            }
            else if (GameManager.instance.Episode_Round == 2)    // 2회차
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("분명 지난번에도 있었던 구조물인거 같은데..\n뭔가 다른거 같으니 신중하게 확인해 봐야겠어."));
            }
            else
            {
                // 잘못된 엔딩 설정인 경우
                Debug.Log("엔딩 설정 오류(Gura_ObjectManager)");
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void ChangeSceneData_To_GamaManager() // 데이터 저장 버튼 or 씬 이동시 이거 호출해서 게임매니저에 저장
    {
        int tempSave = 0;

        binarySystem_forDataSave = "1";
        for (int i = 0; i < SceneGimics.Length; i++)
        {
            if (SceneGimics[i].GetComponent<Interaction_Gimics>().run_Gimic == true)
            {
                binarySystem_forDataSave += "1";
            }
            else
            {
                binarySystem_forDataSave += "0";
            }
        }
        tempSave = Convert.ToInt32(binarySystem_forDataSave, 2);
        GameManager.instance.GuraScene_Save = tempSave;
        GameManager.instance.SaveData();
    }
    void ChangeGameManager_To_SceneData()
    {
        binarySystem_LoadData = GameManager.instance.GuraScene_Save;
        string tempSave = Convert.ToString(binarySystem_LoadData, 2);
        //받아온 int값 string으로 변환 후 맨 앞 1 제거
        string binarySystem_LoadData_String = tempSave;
        binarySystem_LoadData_String = binarySystem_LoadData_String.Substring(1);

        char[] binarySystem_LoadData_Char = binarySystem_LoadData_String.ToCharArray();
        //char[]로 변환한 데이터를 각각의 오브젝트에게 뿌려주기
        for (int i = 0; i < binarySystem_LoadData_Char.Length; i++)
        {
            if (binarySystem_LoadData_Char[i] == '1')
            {
                SceneGimics[i].GetComponent<Interaction_Gimics>().Setting_Scene_Gimic(true);
            }
            else
            {
                SceneGimics[i].GetComponent<Interaction_Gimics>().Setting_Scene_Gimic(false);
            }
        }

        GameObject.FindAnyObjectByType<Number_if_Gimic>().FindObjManager();
        // 씬 시작하고 기믹들 진행상황 반영
        GameObject.FindAnyObjectByType<Number_if_Gimic>().FirstTextSet();
    }
}
