using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_CheckTapePos : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    private Interaction_Gimics interaction;         // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private Calli_StoneTableGimic calli_StoneTable; // 돌 책생 본체에서 관리하는 스크립트 할당할 변수
    private GameObject player;                      // 플레이어
    private GameObject tapeOnTableHole;             // 테이프칸에 테이프를 생성할때 사용할 GameObject 변수
    private TextController textController;          // 텍스트를 출력하는 UI의 스크립트
    #endregion

    #region 해당 기믹에서 사용할 변수 모음
    private string TableHolePosition;  // 해당 테이프 칸이 몇번째 칸인지 저장할 변수
    private bool setTapeOnTableHole;   // 해당 테이프 칸에 테이프를 놓았는지 확인할 변수
    private bool settingGimic { get; set; } // 기믹 세팅 여부 결정할 변수
    public bool SettingGimic
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion

    private void Awake()
    {
        TableHolePosition = this.gameObject.name;  // 이름을 이용해 현재 테이프 칸의 위치를 저장
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        player = GameObject.Find("Player");
        calli_StoneTable = GameObject.FindAnyObjectByType<Calli_StoneTableGimic>(); // 돌 책상 본체를 찾아와서 할당

        calli_StoneTable.SetInitTapeHole(Init);    // 체스판에서 초기화 진행하기 위해 Action에 함수 할당
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        //calli_StoneTable = GameObject.FindAnyObjectByType<Calli_StoneTableGimic>(); // 돌 책상 본체를 찾아와서 할당

        //calli_StoneTable.SetInitTapeHole(Init);    // 체스판에서 초기화 진행하기 위해 Action에 함수 할당
    }

    // 초기화 함수(calli_StoneTable에서 실행)
    private void Init()
    {
        player = GameObject.Find("Player"); // 플레이어를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();                // 상호작용을 위한 Interaction_Gimics 할당
        //calli_StoneTable = GameObject.FindAnyObjectByType<Calli_StoneTableGimic>(); // 돌 책상 본체를 찾아와서 할당

        //calli_StoneTable.SetInitTapeHole(Init);    // 체스판에서 초기화 진행하기 위해 Action에 함수 할당

        textController = GameObject.FindAnyObjectByType<TextController>();  // 상호작용할 때 출력해줄 텍스트를 관리하는 textcontroller

        tapeOnTableHole = null;       // 현재 생성된 타코가 없게끔 초기화
        settingGimic = interaction.run_Gimic;   // 기믹 설치여부에 따라 기믹 세팅하기 위한 변수 초기화

        settingGimic = false;    // 아직 놓지 않았다고 초기값 설정(나중에 저장데이터 기반으로 세팅다시 할 것.)
        // settingGimic = interaction.run_Gimic;
        setTapeOnTableHole = false; // 아직 해당칸에 놓지 않았다고 변경

        // 초기화 후 기믹을 실행하기 시작
        StartCoroutine(WaitTouch());            // 상호작용전까지 대기시킬 코루틴 함수 호출

        Setting_SceneStart();
    }

    // 상호작용되기 전까지 대기할 코루틴 함수
    private IEnumerator WaitTouch()
    {
        while (player) // 한번하고 뽀사지면 이거빼고, 창문 열고 닫는거처럼 반복필요하면 이거 넣고 쓰기
        {
            // this.GetComponentInChildren<ParticleSystem>().Play();

            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // 생성되고난뒤 여기서 대기하다가

            // 기믹 실행이 참인 경우
            if (interaction.run_Gimic && !setTapeOnTableHole)
            {
                setTapeOnTableHole = true; // 아이템이 해당 칸 위에 존재한다고 상태 변경. 추가 반복을 막음

                // 이미 최대갯수보다 적은 수의 테이프만 소환되었고, 해당 핫키의 테이프가 테이프칸 위에 없는 경우에만 소환을 진행
                if (!calli_StoneTable.isTapeReachMaxNum && (!calli_StoneTable.GetAnswerStoneTableTape(ItemManager._instance.hotkeyItemIndex)))
                {
                    Debug.Log("테이프 소환 시도1" + this.gameObject.name);    // 상호작용 확인용 로그

                    // 현재 핫키의 아이템이 테이프인 경우에만 소환
                    if (ItemManager._instance.hotkeyItemIndex >= 21 && ItemManager._instance.hotkeyItemIndex <= 24)
                    // if (ItemManager._instance.hotkeyItemIndex == 21 || ItemManager._instance.hotkeyItemIndex == 22 || ItemManager._instance.hotkeyItemIndex == 23 || ItemManager._instance.hotkeyItemIndex == 24)
                    {
                        SummonTapeOnStoneTable();   // 현재 테이프칸 위치에 테이프를 소환하기 위한 함수 호출
                    }
                    else
                    {
                        setTapeOnTableHole = false;    // 소환되지 않았으니 상태 변경
                        interaction.run_Gimic = false; //
                    }
                }
                else
                {
                    setTapeOnTableHole = false;    // 소환되지 않았으니 상태 변경
                    interaction.run_Gimic = false; //
                    // Debug.Log("테잎 소환 실패 : " + calli_StoneTable.CountTapes);

                    //if (calli_StoneTable.CountTapes > 0)
                    //{
                    //    calli_StoneTable.CountTapes -= 1;     // 소환된 타코수 -1
                    //}
                    // calli_StoneTable.CountTapes -= 1;     // 소환된 타코수 -1
                }
            }
            else
            {
            }
        }
    }

    // 테이프 칸 위에 타코 소환하기 위한 함수
    private void SummonTapeOnStoneTable()
    {
        Debug.Log("테이프 소환 : " + ItemManager._instance.hotkeyItemName);  // 로그 출력

        // Resources 풀더에서 현재 핫키에 등록된 아이템 프리팹을 찾아와 생성
        tapeOnTableHole = Instantiate(Resources.Load<GameObject>("Items/Prefabs/" + ItemManager._instance.hotkeyItemName));
        tapeOnTableHole.transform.SetParent(this.transform, false);                             // 해당 테이프 칸의 자식으로 생성
        // tapeOnTableHole.transform.position = this.transform.position + (Vector3.up * 0.1f);     // 해당 테이프 칸 위치에 살짝 떠있게끔 이동
        tapeOnTableHole.transform.position = this.transform.position + (Vector3.up * 0.05f);     // 해당 테이프 칸 위치에 살짝 떠있게끔 이동
        tapeOnTableHole.transform.localScale = new Vector3(35f, 2.25f, 15.3f);                    // 생성되는 테이프의 크기 조절
        tapeOnTableHole.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        calli_StoneTable.SetAnswerCasetteTape(ItemManager._instance.hotkeyItemIndex, TableHolePosition);   // 테이프 칸에 놓았으므로 배열에 저장하기위한 함수 호출
        
        // 추가 상호작용을 통해 다시 인벤토리로 들어가지 않도록 collider 비활성화
        tapeOnTableHole.GetComponent<Collider>().enabled = false;
        this.GetComponent<Collider>().enabled = false;

        calli_StoneTable.SetDestroyWrongPlaceTapes_CallBack(DestroyWrongPlaceTape);    // 잘못된 위치에 놓인 경우 실행할 함수 Action에 넣어줌
    }

    // 돌 책상에서 잘못된 위치에 테이프가 놓였을 경우 실행할 함수
    private void DestroyWrongPlaceTape()
    {
        // Debug.Log("테이프 삭제");
        Destroy(tapeOnTableHole);       // 해당 위치에 생성된 테이프 제거
        interaction.run_Gimic = false;  // 해당 기믹이 아직 실행되지 않은 상태로 변경
        setTapeOnTableHole = false;     // 비워졌으므로 false로 변경
        this.GetComponent<Collider>().enabled = true;   // 다시 상호작용 가능하도록 콜라이더 활성화
    }

    // 씬 시작했을 때 초기 위치를 설정하기 위한 코루틴 함수
    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            
        }
        else
        {

        }
    }
}
