using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Calli_RadioPlay : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private Animator animator;                          // 노래가 나올때 라디오를 움직여줄 애니메이터
    private AudioSource audioSource;                    // 노래를 재생할 audioSource
    [Header("BGM으로 사용하는 AudioClip 모음")]
    [SerializeField]
    private AudioClip[] bgmAudioClips;
    private Calli_WhichSongPlayBook whichSongPlayBook;  // 현재 재생하는 노래가 정답인지 확인하는 스크립트
    [SerializeField]
    private GameObject selectTape;                      // 선택되었을때 위치를 이동시킬 테이프 오브젝트
    [SerializeField]
    private GameObject prevTape;                        // 직전에 선택한 테이프 오브젝트
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private string playSongName = "";          // 현재 출력중인 노래 제목
    private string interactionText;            // 상호작용 시 출력할 대사
    private bool isBGMChange;                  // BGM을 바꿔서 재생하는 코루틴이 돌아가고 있는지 확인하기 위한 변수
    private int playSongNum;                   // 현재 출력중인 노래 번호(0 : 기본 / 21 : MERA MERA / 22 : You're Not Special / 23 : The Grim Reaper is a Live-Streamer / 24 : HUGE W)
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
        animator = GetComponent<Animator>();                // 자신의 애니메이터 할당
        audioSource = calli_ObjectManager.GetComponentInChildren<AudioSource>();          // 노래를 재생시킬 자신의 audio source 할당
        whichSongPlayBook = GameObject.FindAnyObjectByType<Calli_WhichSongPlayBook>();  // 현재 재생하는 노래가 정답인지 확인하는 스크립트를 찾아와서 할당

        playSongName = "";      // 현재 실행중인 노래 제목을 빈칸으로 초기화
        isBGMChange = false;    // 현재 BGM을 바꾸는 코루틴이 재생중이 아니라고 초기화
        playSongNum = 0;        // 기본 BGM이 재생중이라고 설정

        StartCoroutine(WaitTouch());    // 상호작용 대기 코루틴 호출
    }

    // 상호작용 대기 코루틴 함수
    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에

            // 선택된 테이프가 null이 아니고, 테이프 배치 기믹이 끝났을 경우 실행
            if ((selectTape != null) && (whichSongPlayBook.SettingForObjectToInteration == true))
            {

                if ((selectTape != null))
                {
                    // 선택한 오브젝트가 아이템을 포함한 테이프인 경우에만 실행
                    if ((selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.GetComponent<IItem>() != null))
                    {
                        SetPlaySongName(); // 선택된 테이프의 정보에 따라 노래를 재생할 수 있도록 해줌
                    }
                    else
                    {
                        Debug.Log("선택된 오브젝트가 아이템(테이프)가 아님");
                    }
                }
                else
                {
                    interactionText = "여기서 책에 나오는 노래를 틀 수 있을꺼 같아.";      // 상호작용 시 출력할 대사 설정
                    StartCoroutine(textController.SendText(interactionText));   // 상호작용 대사 출력   
                }

                interaction.run_Gimic = false;
            }
            else // 테이프 배치 기믹이 끝나지 않았을 경우 실행
            {
                interactionText = "여기서 책에 나오는 노래를 틀 수 있을꺼 같아.";      // 상호작용 시 출력할 대사 설정
                StartCoroutine(textController.SendText(interactionText));   // 상호작용 대사 출력

                interaction.run_Gimic = false;
            }
        }
    }

    /// <summary>
    /// 현재 선택중인 테이프를 저장하고 이전에 선택중이던게 있다면 다시 내려놓을 함수
    /// </summary>
    /// <param name="_currSelectTape"></param>
    public void SetSelectTape(GameObject _currSelectTape)
    {
        if (selectTape != null)      // 만약 기존에 선택중이던 테이프가 있을 경우
        {
            if (selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.activeSelf)  // 만약 선택한 테이프가 활성화 상태라면
            {
                selectTape.GetComponent<Calli_TapeSelectEnding2>().TapeReplace();  // 테이프를 다시 제자리로 되돌려줌.
            }

            prevTape = selectTape;        // 선택하고 있던 테이프를 이전에 선택했던 테이프로 저장
            selectTape = _currSelectTape; // 현재 선택한 테이프를 선택중인 테이프로 변경

            selectTape.GetComponent<Calli_TapeSelectEnding2>().TapePosUp();    // 선택되었다고 표현해줄 함수 호출
        }
        else    // 없다면 현재 선택 중인 테이프를 바로 할당
        {
            prevTape = _currSelectTape;
            selectTape = _currSelectTape;

            selectTape.GetComponent<Calli_TapeSelectEnding2>().TapePosUp();    // 선택되었다고 표현해줄 함수 호출
        }
    }

    /// <summary>
    /// 선택한 테이프의 인덱스를 확인해서 해당 노래를 세팅하고 틀어줌
    /// </summary>
    private void SetPlaySongName()
    {
        // 이미 BGM을 바꿔주는 코루틴이 돌아가고 있을 경우
        if (isBGMChange)
        {
            StopCoroutine("PlayBGMByRadio");    // 해당 이름을 가진 코루틴 정지
            isBGMChange = false;
        }

        // 선택한 테이프의 인덱스를 확인해서 해당 노래를 세팅하고 틀어줌
        switch (selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.GetComponent<IItem>().Index)
        {
            case 21:
                {
                    // 현재 재생중인 노래와 다른 경우에만 실행
                    if (playSongNum != 21)
                    {
                        playSongNum = 21;
                        playSongName = "MERA MERA";
                        StartCoroutine(PlayBGMByRadio(bgmAudioClips[1]));   // 해당 AudioClip으로 BGM을 바꿔서 재생하기 위한 함수 호출
                    }

                    break;
                }
            case 22:
                {
                    // 현재 재생중인 노래와 다른 경우에만 실행
                    if (playSongNum != 22)
                    {
                        playSongNum = 22;
                        playSongName = "You're Not Special";
                        StartCoroutine(PlayBGMByRadio(bgmAudioClips[2]));   // 해당 AudioClip으로 BGM을 바꿔서 재생하기 위한 함수 호출
                    }

                    break;
                }
            case 23:
                {
                    // 현재 재생중인 노래와 다른 경우에만 실행
                    if (playSongNum != 23)
                    {
                        playSongNum = 23;
                        playSongName = "The Grim Reaper is a Live-Streamer";
                        StartCoroutine(PlayBGMByRadio(bgmAudioClips[3]));   // 해당 AudioClip으로 BGM을 바꿔서 재생하기 위한 함수 호출
                    }

                    break;
                }
            case 24:
                {
                    // 현재 재생중인 노래와 다른 경우에만 실행
                    if (playSongNum != 24)
                    {
                        playSongNum = 24;
                        playSongName = "HUGE W";
                        StartCoroutine(PlayBGMByRadio(bgmAudioClips[4]));   // 해당 AudioClip으로 BGM을 바꿔서 재생하기 위한 함수 호출
                    }

                    break;
                }
            default:
                {
                    Debug.Log("잘못된 테이프 입력");
                    break;
                }
        }


        if (prevTape != selectTape)
        {
            prevTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.gameObject.SetActive(true); // 이전에 선택했던 테이프를 활성화 시켜 보이게 만들어줌
            prevTape.GetComponent<Calli_TapeSelectEnding2>().TapeReplace();
        }
        selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.gameObject.SetActive(false); // 테이프를 비활성화 시켜 안보이게 만들어줌

        whichSongPlayBook.IsPlaySongCorrectCheck(playSongName);   // 정답 노래를 재생하고 있는지 확인하기 위한 함수 호출
    }

    /// <summary>
    /// 선택된 BGM Clip으로 BGM을 바꿔서 재생해줄 코루틴 함수
    /// </summary>
    /// <param name="_bgmClip"></param>
    private IEnumerator PlayBGMByRadio(AudioClip _bgmClip)
    {
        isBGMChange = true; // 해당 코루틴이 실행중이라고 상태 변경

        audioSource.Stop(); // 현재 재생 중이던 BGM을 정지

        audioSource.clip = _bgmClip;    // BGM의 Audio clip을 선택한 노래로 변경
        audioSource.loop = false;   // loop가 돌지 않도록 변경

        audioSource.Play(); // 해당 클립으로 BGM을 다시 재생

        yield return new WaitUntil(() => !audioSource.isPlaying);   // 해당 노래가 끝나기 전까지 대기

        selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.gameObject.SetActive(true); // 노래가 끝났으므로 테이프를 활성화 시켜 보이게 만들어줌

        // 노래가 끝났으니 둘다 다시 비워줌
        selectTape = null;
        prevTape = null;
        playSongNum = 0;    // 현재 재생 중인 노래가 기본BGM이라고 번호 변경

        audioSource.clip = bgmAudioClips[0];    // BGM의 Audio clip을 원래 사용하고 있던 노래로 변경
        audioSource.loop = true;   // loop가 돌도록 변경

        audioSource.Play(); // 해당 클립으로 BGM을 다시 재생
    }
}
