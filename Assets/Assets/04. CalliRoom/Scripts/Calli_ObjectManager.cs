using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_ObjectManager : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    
    #endregion

    #region 해당씬에서 사용할 변수 모음
    private int checkEndingEpisode_Num = 1; // 엔딩회차 확인용 변수. 현재 테스트를 위해 1로 초기화
    public int CheckEndingEpisode_Num   // 복도씬의 다른 오브젝트에서 현재 몇회차인지 확인하기 위한 프로퍼티
    {
        get
        {
            return checkEndingEpisode_Num;
        }
        set
        {
            if (checkEndingEpisode_Num < 2)    // 최대 회차보다 작을때만 할당 가능
            {
                checkEndingEpisode_Num = value;
            }
        }
    }
    #endregion

    // 데이터 받아와서 10진법으로 만들어서 각각 오브젝트에게 뿌려주기
    // 각각 오브젝트들 상태 받아서 2진법으로 만들어서넘겨주기

    public GameObject[] SceneGimics;  // 각 씬에 배치할 때 데이터 주고받을 기믹들 여기에 저장
    string binarySystem_forDataSave = "1"; // 제일 앞에 1으로 고정해 자릿수 보정

    int binarySystem_LoadData = 0;

    GameObject player;
    [SerializeField]
    Transform playerSpawnPos;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        ChangeGameManager_To_SceneData(); // 씬 시작했을 때 데이터 불러와서 뿌려주기
        player = GameObject.Find("Player");
        player.transform.position = playerSpawnPos.position;
        player.transform.rotation = Quaternion.Euler(0, 90, 0);


        Init();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 초기화 함수
    private void Init()
    {
        // 씬 진입 대사 출력
        // 만약 저장데이터가 0이라면 들어온적이 없다는 것이므로 안내문구 출력
        if (GameManager.instance.CalliScene_Save == 0)
        {
            if (GameManager.instance.Episode_Round == 1)    // 1회차
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("너무 어두운데..\n성에서 얻은 헤일로의 빛으로 밝힐 수 있지 않을까?"));
            }
            else if (GameManager.instance.Episode_Round == 2)    // 2회차
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("동굴 분위기가.. 일단 헤일로를 이용해 주변을 밝혀야겠어."));
            }
            else
            {
                // 잘못된 엔딩 설정인 경우
                Debug.Log("엔딩 설정 오류(Calli_ObjectManager)");
            }
        }
    }

    public void ChangeSceneData_To_GameManager() // 데이터 저장 버튼 or 씬 이동시 이거 호출해서 게임매니저에 저장
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
        GameManager.instance.CalliScene_Save = tempSave;

        GameManager.instance.SaveData();
    }

    void ChangeGameManager_To_SceneData()
    {
        binarySystem_LoadData = GameManager.instance.CalliScene_Save;

        string tempSave = Convert.ToString(binarySystem_LoadData, 2);
        //받아온 int값 string으로 변환 후 맨 앞 1 제거
        string binarySystem_LoadData_String = tempSave;
        binarySystem_LoadData_String = binarySystem_LoadData_String.Substring(1);

        //string을 char[]로 변환 후 각각의 char을 int로 변환하여 배열에 저장
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
