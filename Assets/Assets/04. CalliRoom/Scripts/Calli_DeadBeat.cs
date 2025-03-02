using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_DeadBeat : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    [Header("관 수행 완료 저장용 interaction gimic")]
    [SerializeField]
    private Interaction_Gimics save_Interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트

    private GameObject deadBeat_Skull;                  // 데드비츠 두개골 부분
    private GameObject deadBeat_Cloth;                  // 데드비츠 옷 부분
    private GameObject deadBeat_HeadScarf;              // 데드비츠 헤어스카프 부분
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private bool isDeadBeatAlive;       // 데드비츠에게 머리를 되돌려줬는지 확인하기 위한 변수
    public bool IsDeadBeatAlive         // 데드비츠에게 머리를 되돌려줬는지 Calli_CasketGimic에서 확인하기 위한 프로퍼티
    {
        get 
        { 
            return isDeadBeatAlive;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); // 플레이어를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();     // 플레이어의 자식에서 TextController를 찾아와서 할당

        Init();     // 초기화 함수 호출
    }

    // 초기화 함수
    private void Init()
    {
        settingGimic = save_Interaction.run_Gimic;   // 기믹 진행 현황 저장

        deadBeat_Skull = GetComponentInChildren<Calli_DeadBeatSkull>().gameObject;          // 데드비츠 두개골을 자식에서 찾아와서 할당
        deadBeat_Cloth = GetComponentInChildren<Calli_DeadBeatCloth>().gameObject;          // 데드비츠 옷을 자식에서 찾아와서 할당
        deadBeat_HeadScarf = GetComponentInChildren<Calli_DeadBeatHeadScarf>().gameObject;  // 데드비츠 헤어스카프를 자식에서 찾아와서 할당

        StartCoroutine(WaitTouch());    // 대기 코루틴 실행

        Setting_SceneStart();           // 세팅 시작 코루틴 실행
    }

    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            // 기믹이 이미 수행된 상태이므로 전부 보이게 설정
            deadBeat_Skull.SetActive(true);
            deadBeat_Cloth.SetActive(true);
            deadBeat_HeadScarf.SetActive(true);

            isDeadBeatAlive = true;     // 데드비츠도 머리를 받았다고 설정
        }
        else
        {
            // 기믹이 아직 수행되지 않았으므로 몸통뼈만 보이게끔 설정
            deadBeat_Skull.SetActive(false);
            deadBeat_Cloth.SetActive(false);
            deadBeat_HeadScarf.SetActive(false);

            isDeadBeatAlive = false;     // 데드비츠가 아직 머리를 못 받았다고 설정
        }
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에

            // 현재 핫키에서 선택된 아이템이 두개골이고 기믹수행이 완료되지 않은 경우에 기믹 수행
            if (string.Equals(ItemManager._instance.hotkeyItemName, "Item_20_Skull") && (!save_Interaction.run_Gimic))
            {
                // 수행할 기믹 함수 호출
                SetSkullToDeadBeat();

                interaction.run_Gimic = false;  // 기믹이 수행되지 않았으므로 false로 다시 변경
            }
            else if (save_Interaction.run_Gimic)    // 기믹 수행이 완료된 경우  대사 출력
            {
                // 상호작용 문구 출력함수 호출
                StartCoroutine(textController.SendText("기분 탓일까... 머리가 되돌아와서 기뻐보여."));

                interaction.run_Gimic = false;  // 기믹이 수행되지 않았으므로 false로 다시 변경
            }
            else  // 아니라면 텍스트창에 상호작용 문구 출력
            {
                // 상호작용 문구 출력함수 호출
                StartCoroutine(textController.SendText("어라, 해골의 머리는 어디갔지?"));

                interaction.run_Gimic = false;  // 기믹이 수행되지 않았으므로 false로 다시 변경
            }
        }
    }

    #region 기믹을 수행하는 함수 모음
    // 두개골을 소지하고 데드비츠에게 상호작용했을 경우 두개골을 끼워주고 옷을 입힐 함수
    private void SetSkullToDeadBeat()
    {
        // 두개골을 인벤토리로 되돌려줌
        ItemManager._instance.ReturnItem(20);
        ItemManager._instance.DeactivateItem(20);

        deadBeat_Skull.SetActive(true);     // 해골을 다시 끼워줌

        save_Interaction.run_Gimic = true;  // 기믹이 진행 완료 되었다고 상태 변경
        
        StartCoroutine(MoveCasket());       // 해골 옷입히고 관 움직여줄 코루틴 실행
    }

    private IEnumerator MoveCasket()
    {
        yield return new WaitForSeconds(0.3f);  // 0.3초 대기

        // 옷가지를 되돌려줌
        deadBeat_Cloth.SetActive(true);
        deadBeat_HeadScarf.SetActive(true);

        // 애니메이션도 출력할까..
        // 관을 옆으로 이동시켜 칼리낫을 획득 할 수 있게 Calli_CasketGimic에 전달
        isDeadBeatAlive = true; // 두개골을 돌려받았다고 설정

        save_Interaction.run_Gimic = true;   // 기믹 수행 완료 처리

        // 1회차일때만 실행
        if (GameManager.instance.Episode_Round == 1)
        {
            // 진행도 업데이트
            GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        }
        calli_ObjectManager.ChangeSceneData_To_GameManager();   // 칼리 낫이 나왔으므로 수확되었으므로 저장 1회 진행
    }
    #endregion
}
