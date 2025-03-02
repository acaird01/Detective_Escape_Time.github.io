using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Calli_WhichSongPlayBook : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private GameObject skullHead;                       //  기믹을 풀었을때 소환해줄 두개골 GameObject
    [Header("테이프를 선택하기 위한 테이블 칸들")]
    [SerializeField]
    private Calli_TapeSelectEnding2[] tapeSelectCheck;
    [Header("테이프가 현재 생성되어 있는 테이블 칸들")]
    [SerializeField]
    private GameObject[] tapeSummonHoles;
    [Header("정답을 보여줄 이미지 메테리얼 배열")]
    [SerializeField]
    private Material[] albumImages;                     // (0 : MERA MERA / 1 : You're Not Special / 2 : The Grim Reaper is a Live-Streamer / 3 : HUGE W)
    [Header("정답을 보여줄 노래제목 메테리얼 배열")]
    [SerializeField]
    private Material[] songNames;                       // (0 : MERA MERA / 1 : You're Not Special / 2 : The Grim Reaper is a Live-Streamer / 3 : HUGE W)
    private MeshRenderer albumImage_Quad;               // 앨범 사진을 보여줄 판넬
    private MeshRenderer songName_Quad;                 // 노래 제목을 보여줄 판넬
    private AudioSource audioSource;                    // 앨범사진이나 노래 제목을 보여줄때 효과음을 재생해줄 audio source
    #endregion

    #region 2회차 중 각 테이프 칸에서 사용할 Action
    private Action<bool> InitTapeHoles;      // 각 테이프 칸을 초기화할 함수를 실행할 Aciton
    public void SetInitTapeHole(Action<bool> _InitTapeHoles)     // InitTapeHoles에 함수를 테이프 칸에서 할당할 set 함수
    {
        InitTapeHoles += _InitTapeHoles;
    }
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private string correctSongName = "";        // 정답 노래 제목
    private string[] allSongNames;              // 모든 노래 제목

    private string interactionText;        // 상호작용 시 출력할 대사

    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private bool isRightSongPlay;               // 정답 노래가 나오고 있는지 확인하기 위한 변수
    private bool isTapeGimicEnd;                // 테이프 기믹이 끝났는지 확인하기 위한 변수
    /// <summary>
    /// 테이프 배치 기믹이 종료되었는지 노래 재생 책에서 변경해주기 위한 변수
    /// </summary>
    public bool IsTapeGimicEnd
    {
        set
        {
            isTapeGimicEnd = value;
        }
    }

    #endregion

    /// <summary>
    /// 성공하면 두개골을 활성화 시켜줌
    /// </summary>

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

    /// <summary>
    /// 초기화 함수(Calli_StoneTableGimic에서 호출)
    /// </summary>
    /// <param name="_settingGimic"></param>
    public void Init(bool _settingGimic)
    {
        skullHead = GameObject.FindAnyObjectByType<Item20Skull>().gameObject;       // 두개골 아이템을 찾아와서 할당
        albumImage_Quad = GetComponentInChildren<Calli_AlbumImage>().GetComponent<MeshRenderer>();   // 앨범 그림을 보여줄 판떼기의 meshrenderer를 찾아와서 할당
        songName_Quad = GetComponentInChildren<Calli_SongName>().GetComponent<MeshRenderer>();       // 노래 제목을 보여줄 판떼기의 meshrenderer를 찾아와서 할당
        audioSource = GetComponent<AudioSource>();          // 효과음을 재생할 audio source를 할당

        settingGimic = _settingGimic;   // 기믹 수행 여부 초기화
        isRightSongPlay = false;    // 현재 정답노래가 나오고 있지 않다고 변경

        allSongNames = new string[4];   // 노래 제목을 저장할 배열 생성

        // 모든 노래 제목을 배열에 넣어줌
        allSongNames[0] = "MERA MERA";
        allSongNames[1] = "You're Not Special";
        allSongNames[2] = "The Grim Reaper is a Live-Streamer";
        allSongNames[3] = "HUGE W";

        // 0~3 사이의 랜덤한 번호의 노래를 정답으로 지정
        int correctNum = UnityEngine.Random.Range(0, 4);

        correctSongName = allSongNames[correctNum];             // 정답 노래 제목을 설정
        albumImage_Quad.material = albumImages[correctNum];     // 정답 노래에 맞는 앨범 이미지로 설정
        songName_Quad.material = songNames[correctNum];         // 정답 노래에 맞는 제목 글자 이미지로 설정

        albumImage_Quad.gameObject.SetActive(false);            // 앨범이미지 비활성화 해서 안보이게 해줌
        songName_Quad.gameObject.SetActive(false);              // 노래제목이미지 비활성화 해서 안보이게 해줌

        Setting_SceneStart();       // 세이브 데이터에 따라 기믹 세팅
    }

    // 세이브 데이터에 따라 시작전에 기믹 세팅
    void Setting_SceneStart()
    {
        InitTapeHoles?.Invoke(settingGimic);   // 테이프 놓을 칸들 초기화 함수들 실행

        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            // 두개골을 획득했는지 확인
            if (skullHead.GetComponent<Item20Skull>().isGetItem)
            {
                // 두개골을 이미 획득했을 경우 비활성화 시킴
                skullHead.gameObject.SetActive(false);
            }
            else
            {
                // 두개골을 아직 획득하지 못했을 경우 활성화 시킴
                skullHead.gameObject.SetActive(true);
            }

            SetPlaySong(settingGimic);             // 출력할 노래를 세팅해줌
        }
        else
        {
            // 기믹을 아직 수행하지 않았으면 setgimic이 false
            skullHead.gameObject.SetActive(false);    // 아직 기믹을 수행하지 않았으므로 비활성화
        }

        isTapeGimicEnd = settingGimic;  // 테이프 기믹이 완료되었는지 확인하기 위한 변수 초기화

        StartCoroutine(WaitTouch());    // 상호작용 대기 코루틴 호출
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에


            // 테이프 배치 기믹이 끝났을 경우 실행
            if (isTapeGimicEnd)
            {
                // 테이프 위치 배치완료됬으므로
                // 다시 칸의 다른 오브젝트 활성화하고 이 스크립트 활성화 시킴
                // 수정 배치처럼 띄워서 선택여부 확인가능
                // 라디오에 상호작용시 해당 테이프의 노래 출력
                // 테이프는 다시 내려놓음
                interactionText = "뭔가 그려져 있다. 이 노래를 한 번 틀어볼까?";        // 상호작용 시 출력할 대사 설정
                StartCoroutine(textController.SendText(interactionText));          // 상호작용 대사 출력

                interaction.run_Gimic = false;
            }
            else
            {
                interactionText = "아직 아무것도 쓰여있지 않은 책이다.";        // 상호작용 시 출력할 대사 설정
                StartCoroutine(textController.SendText(interactionText));   // 상호작용 대사 출력

                interaction.run_Gimic = false;
            }
        }
    }

    /// <summary>
    /// 카세트 테이프에서 재생해야될 노래를 세팅하고 테이프 선택 콜라이더를 활성화 시킬 함수
    /// </summary>
    /// <param name="_isTapeGimicEnd"></param>
    public void SetPlaySong(bool _isTapeGimicEnd)
    {
        // 최종적으로 기믹이 수행 완료되었다고 변경해줌
        isTapeGimicEnd = _isTapeGimicEnd;
        settingGimic = _isTapeGimicEnd;

        // 책에 앨범 사진과 곡 명을 띄워줌
        albumImage_Quad.gameObject.SetActive(true);            // 앨범이미지 비활성화 해서 안보이게 해줌
        songName_Quad.gameObject.SetActive(true);              // 노래제목이미지 비활성화 해서 안보이게 해줌
        // 책에 글자가 쓰이는 소리 재생
        audioSource.Play();

        // 각 테이프 칸의 콜라이더 활성화 함수 실행
        if (tapeSelectCheck[0].gameObject != null)
        {
            tapeSelectCheck[0].SetTapeSelectCollider(tapeSummonHoles[0].GetComponentInChildren<Item23CassetteTapeT>().gameObject);
        }
        if (tapeSelectCheck[1].gameObject != null)
        {
            tapeSelectCheck[1].SetTapeSelectCollider(tapeSummonHoles[1].GetComponentInChildren<Item24CassetteTapeH>().gameObject);
        }
        if (tapeSelectCheck[2].gameObject != null)
        {
            tapeSelectCheck[2].SetTapeSelectCollider(tapeSummonHoles[2].GetComponentInChildren<Item21CassetteTapeM>().gameObject);
        }
        if (tapeSelectCheck[3].gameObject != null)
        {
            tapeSelectCheck[3].SetTapeSelectCollider(tapeSummonHoles[3].GetComponentInChildren<Item22CassetteTapeY>().gameObject);
        }
    }

    /// <summary>
    /// 정답과 동일한 노래를 출력하고 있는지 확인하기 위한 함수
    /// </summary>
    /// <param name="_answerSongName"></param>
    public void IsPlaySongCorrectCheck(string _answerSongName)
    {
        // 아직 정답이 나오지 않았고 노래 제목이 같을 경우 실행
        if (!isRightSongPlay && (string.Equals(correctSongName, _answerSongName)))
        {
            isRightSongPlay = true;
            skullHead.gameObject.SetActive(true);   // 두개골을 활성화 시켜줌
        }
    }
}
