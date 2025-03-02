using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Calli_ChestUnlock : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private Calli_ChestAnimCtrl chestAnimation_Ctrl;    // 상자를 열기위한 스크립트

    [SerializeField]
    private GameObject Hinzi;   // 상자 힌지
    [Header("상자가 아직 안열렸을때 재생할 소리")]
    [SerializeField]
    private AudioClip beforeChestOpen;
    [Header("상자가 열릴때 재생할 소리")]
    [SerializeField]
    private AudioClip afterChestOpen;

    private AudioSource audioSource;        // 이펙트 실행할때 재생할 audio source
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    private const string needItemName = "Item_34_ChestKey";   // 기믹 해제에 필요한 아이템 이름
    private bool isUnlock;          // 자물쇠가 해제됬는지 확인하기 위한 변수
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");                                             // 플레이어를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();               // 플레이어의 자식에서 TextController를 찾아와서 할당

        Init();     // 초기화 함수 호출
    }

    // 초기화 함수
    private void Init()
    {
        interaction = GetComponent<Interaction_Gimics>();   // 자신에게 붙어있는 interaction gimic을 할당
        chestAnimation_Ctrl = GameObject.FindAnyObjectByType<Calli_ChestAnimCtrl>();    // 상자여는데 사용할 스크립트 찾아와서 할당
        audioSource = this.GetComponent<AudioSource>();              // 재생할 audio source를 할당

        settingGimic = interaction.run_Gimic;   // 기믹 수행 여부 초기화

        Setting_SceneStart();       // 세이브 데이터에 따라 기믹 세팅
    }

    // 세이브 데이터에 따라 시작전에 기믹 세팅
    void Setting_SceneStart()
    {
        isUnlock = settingGimic;    // 자물쇠가 해제되었는지 아닌지 설정

        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            // 상자 초기화 함수 호출
            chestAnimation_Ctrl.Init(isUnlock);

            // 자물쇠 사라짐
            this.gameObject.SetActive(false);
        }
        else
        {
            // 기믹을 아직 수행하지 않았으면 setgimic이 false

            // 상자 초기화 함수 호출
            chestAnimation_Ctrl.Init(isUnlock);

            StartCoroutine(WaitTouch());    // 상호작용 대기 코루틴 실행
        }
    }

    // 상호작용되기 전까지 대기할 코루틴 함수
    private IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // 생성되고난뒤 여기서 대기하다가

            // 기믹 실행이 참인 경우
            // 자물쇠가 아직 열리지 않았고 현재 핫키에 열쇠를 선택하고 있는 경우
            if (!isUnlock && string.Equals(ItemManager._instance.hotkeyItemName, needItemName))
            {
                isUnlock = true;                            // 자물쇠를 해제했다고 상태 변경. 추가 반복을 막음
                chestAnimation_Ctrl.IsChestUnlock = true;     // 자물쇠가 해제되었으니 상자를 열수 있도록 하기 위한 bool값 변경

                chestAnimation_Ctrl.gameObject.GetComponent<BoxCollider>().enabled = false; // 상자 전체의 콜라이더를 꺼줌
                chestAnimation_Ctrl.OpenChest();

                GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate(); // 기믹 종료 시점에 호출

                // 자물쇠가 열렸으므로 열쇠를 돌려보냄
                ItemManager._instance.ReturnItem(34);
                ItemManager._instance.DeactivateItem(34);

                this.gameObject.SetActive(false);           // 자물쇠 비활성화
            }
            else
            {
                StartCoroutine(textController.SendText("자물쇠로 잠겨있어.\n설마 카세트 플레이어 안에 들어있진 않겠지?"));   // 상호작용 대사 출력

                interaction.run_Gimic = false;  // false로 변경
            }
        }
    }
}
