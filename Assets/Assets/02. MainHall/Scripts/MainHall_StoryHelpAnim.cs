using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHall_StoryHelpAnim : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    private GameObject player;                              // 플레이어 오브젝트
    private MainHall_ObjectManager mainHall_ObjectManager;  // 복도씬의 오브젝트 매니저
    private Interaction_Gimics interaction;                 // 상호작용하는 기믹인지 확인하기 위한 컴포넌트

    // private GameObject mainHall_StartRoomDoor;      // 복도 씬의 시작방의 문 게임 오브젝트
    private MainHall_PassDoorAfterTutorial mainHall_PassDoorAfterTutorial;  // 튜토리얼 이후 문을 통과할때 재생할 스크립트
    private MainHall_TakoMoveCtrl mainHall_TakoMoveCtrl;                    // 스토리 진행 보조를 위해 시네머신 카메라 이동 중 타코들을 움직여줄 스크립트
    private MainHall_StartRoomDoorWall startRoomDoorWall;   // 시작방의 문이 달린 벽
    private GameObject mainHall_StartRoomDoor;      // 복도 씬의 시작방의 문 게임 오브젝트
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 진행 여부를 확인하기 위한 변수(true : 기믹 수행 완료 / false : 기믹 미수행)
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    public bool mainHallGuideCamMoveEnd;    // 복도씬 가이드 카메라 워킹이 종료됬는지 확인할 변수(true : 종료 / false : 미완)
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // 플레이어를 찾아와서 할당
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();    // MainHall_ObjectManager를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();    // 상호작용을 위한 Interaction_Gimics 할당

        Init();     // 초기화 함수 호출
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Init()
    {
        // 문을 지나고 카메라와 타코들을 움직여주는 스크립트를 찾아와서 할당
        mainHall_PassDoorAfterTutorial = GameObject.FindAnyObjectByType<MainHall_PassDoorAfterTutorial>();
        mainHall_TakoMoveCtrl = GameObject.FindAnyObjectByType<MainHall_TakoMoveCtrl>();

        // 세팅 변수 초기화
        settingGimic = interaction.run_Gimic;
        mainHallGuideCamMoveEnd = settingGimic;

        // settingGimic에 따라 각 스크립트의 초기화 함수 실행
        mainHall_PassDoorAfterTutorial.Init(settingGimic);
        mainHall_TakoMoveCtrl.Init(settingGimic);

        interaction.enabled = false;    // 세팅이 끝났으므로 벽에 비벼서 넘어가는 상황 방지를 위해 비활성화

        Setting_SceneStart();
    }

    // 최초 세팅해줄 함수
    private void Setting_SceneStart()
    {
        if (!settingGimic)
        {
            // 완료 되지 않은 경우 대기할 코루틴 함수 호출
            StartCoroutine(WaitForMainHallGuide());
        }
        else
        {
            mainHall_PassDoorAfterTutorial.gameObject.SetActive(false); // 비활성화 시킴
        }
    }

    // 애니메이션 및 카메라 워킹이 끝나길 대기할 코루틴 함수
    private IEnumerator WaitForMainHallGuide()
    {
        // 타코 애니메이션이 끝났고, 복도씬 카메라 워킹이 끝난 경우
        yield return new WaitUntil(() => (mainHallGuideCamMoveEnd));

        interaction.enabled = true;

        // 다 끝났으므로 다 끝났다고 상태 변경
        settingGimic = true;
        interaction.run_Gimic = true;
    }
}