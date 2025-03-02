using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHall_ObjectManager : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    // private MainHall_StartStoryAndTutorial StartStoryAndTutorial;   // 복도 씬에서 스토리와 튜토리얼을 보여줄 스크립트를 할당할 변수
    private GameObject player;                                      // 플레이어
    private GuraScene_1 guraScene_1;                                // 1회차 구라씬으로 넘어갈 문
    private GuraScene_2 guraScene_2;                                // 2회차 구라씬으로 넘어갈 문
    private CalliScene_1 calliScene_1;                              // 1회차 칼리씬으로 넘어갈 문
    private CalliScene_2 calliScene_2;                              // 2회차 칼리씬으로 넘어갈 문
    private KiaraScene_1 kiaraScene_1;                              // 1회차 키아라씬으로 넘어갈 문
    private KiaraScene_2 kiaraScene_2;                              // 2회차 키아라씬으로 넘어갈 문

    [Header("2회차 획득용 타코들")]
    [SerializeField]
    private GameObject[] takos; 

    [Header("플레이어의 스폰지역을 저장할 배열")]
    public GameObject[] SponPoints;        // 플레이어의 스폰지역을 저장할 배열(0: 시작방 위치, 1: 구라방 위치, 2: 칼리방 위치, 3: 키아라방 위치, 4: 첫 스토리 진행을 위한 위치)
    #endregion

    #region 해당씬에서 사용할 변수 모음
    private int checkEndingEpisode_Num = 1; // 엔딩회차 확인용 변수. 현재 테스트를 위해 1로 초기화
    public int CheckEndingEpisode_Num       // 복도씬의 다른 오브젝트에서 현재 몇회차인지 확인하기 위한 프로퍼티
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

    // private bool isGameStart;
    #endregion

    #region 게임시작할 때 함수 실행할 Action(현재 미사용)
    private Action StartScene = null;
    public void SetStartSceneCallback(Action _StartScene)   // 매개변수로 실행할 함수를 받아서 action에 추가
    {
        StartScene += _StartScene;
    }
    #endregion
    // 데이터 받아와서 10진법으로 만들어서 각각 오브젝트에게 뿌려주기
    // 각각 오브젝트들 상태 받아서 2진법으로 만들어서넘겨주기

    public GameObject[] SceneGimics;  // 각 씬에 배치할 때 데이터 주고받을 기믹들 여기에 저장
    string binarySystem_forDataSave = "1"; // 제일 앞에 1으로 고정해 자릿수 보정

    int binarySystem_LoadData = 0;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        Init(); // 초기화 함수 호출

        if (GameManager.instance.PrevSceneName != null)
        {
            if (String.Equals(GameManager.instance.PrevSceneName, guraScene_1.name) || String.Equals(GameManager.instance.PrevSceneName, guraScene_2.name))
            {
                // 구라씬에서 복도로 되돌아온 경우
                player.GetComponent<Transform>().position = SponPoints[1].GetComponent<Transform>().position;
                player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 270f, 0f);  // 정면을 보고 있게끔 카메라 조정
            }
            else if (String.Equals(GameManager.instance.PrevSceneName, calliScene_1.name) || String.Equals(GameManager.instance.PrevSceneName, calliScene_2.name))
            {
                // 칼리씬에서 복도로 되돌아온 경우
                player.GetComponent<Transform>().position = SponPoints[2].GetComponent<Transform>().position;
                player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 180f, 0f);  // 정면을 보고 있게끔 카메라 조정
            }
            else if (String.Equals(GameManager.instance.PrevSceneName, kiaraScene_1.name) || String.Equals(GameManager.instance.PrevSceneName, kiaraScene_2.name))
            {
                // 키아라씬에서 복도로 되돌아온 경우
                player.GetComponent<Transform>().position = SponPoints[3].GetComponent<Transform>().position;
                player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 90f, 0f);  // 정면을 보고 있게끔 카메라 조정
            }
        }

        ChangeGameManager_To_SceneData(); // 씬 시작했을 때 데이터 불러와서 뿌려주기
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 해당 씬에서 사용할 변수들 할당 및 초기화하기 위한 함수
    private void Init()
    {
        // StartStoryAndTutorial = GameObject.FindAnyObjectByType<MainHall_StartStoryAndTutorial>();   // 스토리 및 튜토리얼 총괄 스크립트를 찾아와서 할당
        player = GameObject.Find("Player"); // 플레이어를 찾아와서 할당
        checkEndingEpisode_Num = GameManager.instance.Episode_Round;    // 현재 회차 정보를 게임매니저에 저장된 회차 정보로 변경

        // StartStoryAndTutorial.Init();   // 스토리 및 튜토리얼 총괄 스크립트 초기화

        // Debug.Log("현재 회차(복도) : " + checkEndingEpisode_Num); // 테스트 용 

        // 1회차 진행시 사용할 문들 찾아와서 할당
        guraScene_1 = GameObject.FindAnyObjectByType<GuraScene_1>();
        calliScene_1 = GameObject.FindAnyObjectByType<CalliScene_1>();
        kiaraScene_1 = GameObject.FindAnyObjectByType<KiaraScene_1>();
        // 2회차 진행시 사용할 문들 찾아와서 할당
        guraScene_2 = GameObject.FindAnyObjectByType<GuraScene_2>();
        calliScene_2 = GameObject.FindAnyObjectByType<CalliScene_2>();
        kiaraScene_2 = GameObject.FindAnyObjectByType<KiaraScene_2>();

        // 각 문의 대사 출력을 위해 초기화 함수 호출
        guraScene_1.GetComponent<MainHall_PrintDoorInfo>().Init();
        calliScene_1.GetComponent<MainHall_PrintDoorInfo>().Init();
        kiaraScene_1.GetComponent<MainHall_PrintDoorInfo>().Init();
        guraScene_2.GetComponent<MainHall_PrintDoorInfo>().Init();
        calliScene_2.GetComponent<MainHall_PrintDoorInfo>().Init();
        kiaraScene_2.GetComponent<MainHall_PrintDoorInfo>().Init();

        // 1회차인 경우의 문 활성화, 2회차 문은 비활성화
        // 2회차인 경우 반대
        if (checkEndingEpisode_Num == 1)
        {
            guraScene_1.gameObject.SetActive(true);
            calliScene_1.gameObject.SetActive(true);
            kiaraScene_1.gameObject.SetActive(true);

            guraScene_2.gameObject.SetActive(false);
            calliScene_2.gameObject.SetActive(false);
            kiaraScene_2.gameObject.SetActive(false);

            for (int i = 0; i < takos.Length; i++)
            {
                Destroy(takos[i]);  // 1회차는 필요없으니 삭제
            }
        }
        else if (checkEndingEpisode_Num == 2)
        {
            guraScene_1.gameObject.SetActive(false);
            calliScene_1.gameObject.SetActive(false);
            kiaraScene_1.gameObject.SetActive(false);

            guraScene_2.gameObject.SetActive(true);
            calliScene_2.gameObject.SetActive(true);
            kiaraScene_2.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 데이터 저장 버튼 or 씬 이동시 이거 호출해서 게임매니저에 저장
    /// </summary>
    public void ChangeSceneData_To_GameManager()
    {
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

        GameManager.instance.MainHallScene_Save = int.Parse(binarySystem_forDataSave); // 복도로 변경
        GameManager.instance.SaveData();
    }

    void ChangeGameManager_To_SceneData()
    {
        binarySystem_LoadData = GameManager.instance.MainHallScene_Save;

        //받아온 int값 string으로 변환 후 맨 앞 1 제거
        string binarySystem_LoadData_String = binarySystem_LoadData.ToString();
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
    }
}
