using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_TapeSelectEnding2 : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private Calli_WhichSongPlayBook whichSongPlayBook;  // 현재 재생하는 노래가 정답인지 확인하는 스크립트
    private Calli_RadioPlay radioPlay;                  // 재생할 노래 정보를 넘겨줄 스크립트
    [SerializeField]
    private GameObject selectTape;                      // 선택되었을때 위치를 이동시킬 테이프 오브젝트
    public GameObject SelectTape                        // 선택되었을때 위치를 이동시킬 테이프를 외부에서 확인할 프로퍼티
    {
        get
        { 
            return selectTape;
        }
    }
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    // private string playSongName = "";          // 현재 출력중인 노래 제목
    private string interactionText;            // 상호작용 시 출력할 대사
    private Vector3 tapeOrigin_Position;     // 해당 테이프가 원래있던 위치를 저장할 변수
    private bool isTapeSelect;               // 테이프가 선택됬는지 아닌지 확인하기 위한 변수
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        player = GameObject.Find("Player");
        whichSongPlayBook = GameObject.FindAnyObjectByType<Calli_WhichSongPlayBook>(); // 재생할 노래를 고르는 책을 찾아와서 할당

        whichSongPlayBook.SetInitTapeHole(Init);    // 노래고르는 책에서 초기화 진행하기 위해 Action에 함수 할당
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 초기화 함수 (Calli_WhichSongPlayBook에서 실행)
    /// </summary>
    /// <param name="_settingGimic"></param>
    private void Init(bool _settingGimic)
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();               // 플레이어의 자식에서 TextController를 찾아와서 할당
        radioPlay = GameObject.FindAnyObjectByType<Calli_RadioPlay>();                  // 노래를 재생할 라디오를 찾아와서 할당

        settingGimic = _settingGimic;                  // 노래 선택 책의 기믹 수행 완료 여부에 따라 초기화
        isTapeSelect = false;   // 선택되지 않았다고 변경

        Setting_SceneStart();       // 세이브 데이터에 따라 기믹 세팅
    }

    // 세이브 데이터에 따라 시작전에 기믹 세팅
    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            this.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }


    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);

            if (!isTapeSelect)   // 해당 테이프가 아직 선택되지 않았을때만 실행
            {
                // 수정 배치처럼 띄워서 선택여부 확인가능
                // 라디오에 상호작용시 해당 테이프의 노래 출력
                // 테이프는 다시 내려놓음.

                isTapeSelect = true; // 해당 수정이 선택되었다고 상태 변경. 추가 반복을 막음

                radioPlay.SetSelectTape(this.gameObject);    // 현재 선택된 테이프가 있는 칸을 현재 선택한 테이프 칸으로 변경
            }

            interaction.run_Gimic = false;
        }
    }

    #region 기믹을 수행하는 함수
    /// <summary>
    /// 해당 칸에 있는 선택할 테이프를 배치된 위치와 함께 세팅해둘 함수
    /// </summary>
    /// <param name="_selectTape"></param>
    public void SetTapeSelectCollider(GameObject _selectTape)
    {
        selectTape = _selectTape;   // 해당 칸에 상호작용했을때 선택할 테이프를 할당
        tapeOrigin_Position = selectTape.transform.position;    // 테이프의 현재 위치를 기본 위치로 설정

        this.GetComponent<BoxCollider>().enabled = true;        // 해당 칸의 콜라이더를 다시 활성화해서 테이프를 선택할 수 있게 바꿈

        StartCoroutine(WaitTouch());    // 상호작용 대기 코루틴 호출
    }

    #region 선택 또는 미선택시 테이프의 위치를 조정해주는 함수
    /// <summary>
    /// 해당 수정이 선택되었다고 살짝 위로 올려줄 함수
    /// </summary>
    public void TapePosUp()
    {
        // 현재 위치보다 약간 위쪽으로 올려서 선택되었다고 표시해줌
        selectTape.transform.position += Vector3.up * 0.5f;

        this.GetComponent<BoxCollider>().enabled = false;        // 해당 칸의 콜라이더를 비활성화해서 테이프를 선택할 수 없게 바꿈
    }

    /// <summary>
    /// 해당 수정이 더 이상 선택되지 않았으므로 다시 본래 위치로 되돌려줄 함수
    /// </summary>
    public void TapeReplace()
    {
        isTapeSelect = false;

        this.GetComponent<BoxCollider>().enabled = true;        // 해당 칸의 콜라이더를 다시 활성화해서 테이프를 선택할 수 있게 바꿈
        // selectTape.gameObject.SetActive(true);

        // 미리 저장해둔 본래 위치로 이동
        selectTape.transform.position = tapeOrigin_Position;
    }
    #endregion
    #endregion
}
