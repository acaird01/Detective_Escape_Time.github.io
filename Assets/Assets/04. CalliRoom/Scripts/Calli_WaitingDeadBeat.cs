using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// 테이프 기믹의 2회차 힌트 제공 역할
/// </summary>
public class Calli_WaitingDeadBeat : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private Animator animator;  // 해골을 움직여줄 애니메이터
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private string interactionText;        // 상호작용 시 출력할 대사

    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        player = GameObject.Find("Player");                                             // 플레이어를 찾아와서 할당
        interaction = this.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();               // 플레이어의 자식에서 TextController를 찾아와서 할당
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        //me = this.gameObject;   // 본인을 찾아와서 할당
        //player = GameObject.Find("Player");                                             // 플레이어를 찾아와서 할당
        //interaction = me.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        //calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        //textController = player.GetComponentInChildren<TextController>();               // 플레이어의 자식에서 TextController를 찾아와서 할당

        // Init();     // 초기화 함수 호출
    }

    // 초기화 함수(Calli_StoneTableGimic에서 실행)
    public void Init(bool _isTapeGimicEnd)
    {
        animator = this.GetComponent<Animator>();   // 해골을 움직여줄 애니메이터
        settingGimic = _isTapeGimicEnd;   // 기믹 수행 여부 초기화

        Setting_SceneStart();       // 세이브 데이터에 따라 기믹 세팅
    }

    // 세이브 데이터에 따라 시작전에 기믹 세팅
    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            interactionText = "도와줘서 고마워! 거기 열쇠 잊지 말고 챙겨가!";        // 상호작용 시 출력할 대사 설정
        }
        else
        {
            interactionText = "혹시 테이프 좀 출시 순서대로 정리 좀 해주지 않을래?";        // 상호작용 시 출력할 대사 설정
        }

        StartCoroutine(WaitTouch());    // 상호작용 대기 코루틴 호출
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에

            if (interaction.run_Gimic && settingGimic)
            {
                interactionText = "도와줘서 고마워! 거기 열쇠 잊지 말고 챙겨가!";        // 상호작용 시 출력할 대사 설정
                StartCoroutine(textController.SendText(interactionText));   // 상호작용 대사 출력

                interaction.run_Gimic = false;
            }
            else
            {
                interactionText = "혹시 테이프 좀 출시 순서대로 정리 좀 해주지 않을래?";        // 상호작용 시 출력할 대사 설정
                StartCoroutine(textController.SendText(interactionText));   // 상호작용 대사 출력

                interaction.run_Gimic = false;
            }
        }
    }
}
