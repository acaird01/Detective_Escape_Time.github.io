using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainHall_PrintDoorInfo : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    private GameObject player;                                  // 플레이어 오브젝트
    private MainHall_ObjectManager mainHall_ObjectManager;      // 복도씬의 오브젝트 매니저
    private Interaction_Gimics interaction;                     // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private PlayerCtrl playerCtrl;                              // 플레이어 컨트롤 스크립트

    private GuraScene_1 guraScene_1;                            // 1회차 구라씬으로 넘어갈 문
    private GuraScene_2 guraScene_2;                            // 2회차 구라씬으로 넘어갈 문
    private CalliScene_1 calliScene_1;                          // 1회차 칼리씬으로 넘어갈 문
    private CalliScene_2 calliScene_2;                          // 2회차 칼리씬으로 넘어갈 문
    private KiaraScene_1 kiaraScene_1;                          // 1회차 키아라씬으로 넘어갈 문
    private KiaraScene_2 kiaraScene_2;                          // 2회차 키아라씬으로 넘어갈 문

    private Canvas doorInfo_Canvas;                             // 문의 정보를 띄워줄 Canvas
    private Text doorInfo_Text;                                 // 문의 정보를 출력할 Text
    #endregion

    #region 스크립트에서 사용할 변수 모음
    private bool isPrintingText;    // 텍스트를 띄워주고 있는지 확인하기 위한 bool 변수
    private string doorName;        // 문의 이름을 확인할 변수
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        player = GameObject.Find("Player"); // 하이어라키에서 플레이어를 찾아와서 할당
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();     // 복도 씬에 있는 오브젝트 매니저를 할당
        interaction = GetComponent<Interaction_Gimics>();   // 해당 오브젝트에 달려있는 interaction Gimic을 가져와서 할당

        playerCtrl = player.GetComponent<PlayerCtrl>();

        // Init();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 해당 씬에서 사용할 변수들 할당 및 초기화하기 위한 함수(오브젝트 매니저에서 실행)
    /// </summary>
    public void Init()
    {
        doorInfo_Canvas = GameObject.FindAnyObjectByType<MainHall_DoorGuideCanvas>().GetComponentInChildren<Canvas>();    // 문의 정보를 띄워줄 Canvas를 찾아와서 할당
        doorInfo_Text = doorInfo_Canvas.GetComponentInChildren<Text>();     // 문으로 넘어갈 씬의 대사를 출력할 텍스트

        // 1회차 진행시 사용할 문들 찾아와서 할당
        guraScene_1 = GameObject.FindAnyObjectByType<GuraScene_1>();
        calliScene_1 = GameObject.FindAnyObjectByType<CalliScene_1>();
        kiaraScene_1 = GameObject.FindAnyObjectByType<KiaraScene_1>();
        // 2회차 진행시 사용할 문들 찾아와서 할당
        guraScene_2 = GameObject.FindAnyObjectByType<GuraScene_2>();
        calliScene_2 = GameObject.FindAnyObjectByType<CalliScene_2>();
        kiaraScene_2 = GameObject.FindAnyObjectByType<KiaraScene_2>();

        doorInfo_Canvas.enabled = false; // 캔버스 비활성화

        doorName = this.gameObject.name;    // 해당 문의 이름을 저장
    }

    private void Update()
    {
        // 아직 출력중이지 않고 문 근처에 도달한 경우에만 실행
        if (!isPrintingText && interaction.IsActiveF)
        {
            // 인벤토리나 설정창이 열려있지 않을때만 실행
            if (!playerCtrl.invOpen || !playerCtrl.escOpen)
            {
                isPrintingText = true;          // 출력중인 상태로 변경
                doorInfo_Canvas.enabled = true; // 캔버스 활성화

                if (String.Equals(doorName, guraScene_1.name))
                {
                    // 구라씬1
                    if (ItemManager._instance.inventorySlots[6].GetComponent<IItem>().isGetItem)    // 타코를 획득했을 경우
                    { 
                        // 이거 함수로 처리하는게 나을지도
                    }
                    doorInfo_Text.text = "왠지 바다 냄새가 나는 것만 같아.\n여기로 가볼까, 아메?";
                }
                else if (String.Equals(doorName, calliScene_1.name))
                {
                    // 칼리씬1
                    doorInfo_Text.text = "뭔가 으스스한데.. 유령이라도 숨어있는거 아닐까..?\n정말 여기로 갈꺼야, 아메?";
                }
                else if (String.Equals(doorName, kiaraScene_1.name))
                {
                    // 키아라씬1
                    doorInfo_Text.text = "음~ 맛있는 냄새가 나.\n여기로 가볼까, 아메?";
                }
                else if (String.Equals(doorName, guraScene_2.name))
                {
                    // 구라씬2
                    doorInfo_Text.text = "왠지 상어 비린내가 나는 것만 같아.\n여기로 가볼까?";
                }
                else if (String.Equals(doorName, calliScene_2.name))
                {
                    // 칼리씬2
                    doorInfo_Text.text = "분명 진짜 유령이 있었지..\n...후..여기로 가볼까?";
                }
                else if (String.Equals(doorName, kiaraScene_2.name))
                {
                    // 키아라씬2
                    doorInfo_Text.text = "으으..글루텐만 아니였어도..!\n에휴..여기로 가볼까?";
                }
                else
                {
                    // 잘못된 문에 스크립트가 달림
                    Debug.Log("잘못된 문에 스크립트가 달림(복도)");
                }
            }
            else
            {
                isPrintingText = false;          // 미출력 상태로 변경
                doorInfo_Canvas.enabled = false; // 캔버스 비활성화
            }
        }
        else if (isPrintingText && !interaction.IsActiveF)   // 출력중이였다가 문에서 멀어졌을 경우 실행
        {
            isPrintingText = false;          // 미출력 상태로 변경
            doorInfo_Canvas.enabled = false; // 캔버스 비활성화
        }
        else
        {
            // isPrintingText && interaction.IsActiveF // 둘다 true true인 경우 <- 암것도 안해도 됨
            // !isPrintingText && !interaction.IsActiveF // 둘다 false false인 경우 <- 암것도 안해도 됨
            // Debug.Log("문 스크립트 출력중 예기치 못한 문제 발생(이름) : " + isPrintingText + "/" + interaction.IsActiveF + "(" + doorName + ")");
        }

        // 만약 인벤토리나 설정창이 열렸으면 꺼줌
        if (playerCtrl.invOpen || playerCtrl.escOpen)
        {
            isPrintingText = false;          // 미출력 상태로 변경
            doorInfo_Canvas.enabled = false; // 캔버스 비활성화
        }
    }
}
