using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainHall_ChessBoard : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private MainHall_ObjectManager mainHall_ObjectManager;          // 복도씬 오브젝트 매니저
    private MainHall_EndingExitDoorCtrl mainHall_EndingExitDoor;    // 복도씬 엔딩문 컨트롤 스크립트
    private MainHall_GetAmeClock ameClock;                          // 아메 시계 아이템
    private Interaction_Gimics interaction;                         // 세이브 데이터에 따라 기믹 세팅을 위한 스크립트

    [Header("엔딩전에 문으로 가는거 막는 콜라이더")]
    [SerializeField]
    private GameObject blockBeforeEnding;                    // 엔딩전에 문으로 가는거 막는 콜라이더
    [Header("엔딩전에 문으로 가는거 막는 콜라이더를 세팅할 interaction gimic")]
    [SerializeField]
    private Interaction_Gimics interaction_BlockBeforeEnding;       // 엔딩전에 문으로 가는거 막는 콜라이더를 세팅할 interaction gimic

    // 시계를 소환할 마법진 및 소리 변수
    [Header("시계소환 파티클")]
    [SerializeField]
    private GameObject ameClockGeneration_Particle;
    [Header("시계소환 SFX")]
    [SerializeField]
    private GameObject ameClockGeneration_SFX;
    #endregion

    #region 각 체스칸에서 사용할 Action
    private Action InitChessBoard;      // 각 체스칸을 초기화할 함수를 실행할 Aciton
    public void SetInitChessBoard(Action _InitChessBoard)     // InitChessBoard에 함수를 체스칸에서 할당할 set 함수
    {
        InitChessBoard += _InitChessBoard;
    }

    private Action DestroyWrongPlaceTakos;  // 지정되지 않은 잘못된 위치에 타코들이 생겼을 경우 실행할 Action
    public void SetDestroyWrongPlaceTakos_CallBack(Action _DestroyWrongPlaceTakos) // DestroyWrongPlaceTakos에 함수를 체스칸에서 할당할 set 함수
    {
        DestroyWrongPlaceTakos += _DestroyWrongPlaceTakos;
    }
    #endregion

    #region 체스판에서 사용할 변수 및 프로퍼티 모음
    private bool settingGimic { get; set; }     // 기믹 진행 여부를 확인하기 위한 변수(true : 기믹 수행 완료, false : 기믹 미수행)
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    public bool isTakoReachMaxNum;              // 최대 타코수에 도달했는지 확인하기 위한 변수
    private const int maxTakos_ending1 = 3;     // 1회차 엔딩에서 필요한 최대 타코수
    private const int maxTakos_ending2 = 5;     // 2회차 엔딩에서 필요한 최대 타코수
    private int maxTakos;                       // 엔딩을 위해 체스판에 놓을 최대 타코수
    public int MaxTakos                         // 외부에서 최대 타코수를 확인하기 위한 프로퍼티(MainHall_CheckTakoPos에서 사용)
    {
        get
        {
            return maxTakos;
        }
    }
    private int countTakos;                     // 현재 체스판에 놓인 타코수
    public int CountTakos                       // 현재 체스판에 놓인 타코수 프로퍼티(MainHall_CheckTakoPos에서 사용)
    {
        get
        {
            return countTakos;
        }
        set
        {
            if (countTakos < maxTakos)  // 최대 타코수보다 현재 타코수가 적을때만 할당할 수 있게끔 설정
            {
                countTakos = value;
            }
        }
    }

    // 제출된 타코들의 아이템번호 및 위치를 저장할 구조체
    private struct AnswerTakoInfo
    {
        public int TakoIndex;         // 현재 체스판에 놓인 타코의 아이템 인덱스 번호를 저장할 구조체 멤버
        public string TakoPosition;   // 현재 체스판에 놓인 타코의 체스판 위치를 저장할 구조체 멤버
    };
    // answerTakoNumbers 구조체를 담을 배열 선언
    private AnswerTakoInfo[] answerChessTako;
    // 구조체에 값을 할당하기 위한 set 함수(MainHal_CheckTakoPos에서 사용)
    public void SetAnswerChessTako(int _takoIndex, string _takoPosition)
    {
        if (countTakos < maxTakos)  // 최대 타코수보다 현재 타코수가 적을때만 할당할 수 있게끔 설정
        {
            answerChessTako[countTakos].TakoIndex = _takoIndex;
            answerChessTako[countTakos].TakoPosition = _takoPosition;
            countTakos++;
        }
    }

    // 엔딩을 위한 체스판 정답 저장용 Dictionary
    private Dictionary<int, string> CorrectTako_ending1 = new Dictionary<int, string>();  // 엔딩1에서 사용할 정답 위치(현재 3칸만 사용)
    private Dictionary<int, string> CorrectTako_ending2 = new Dictionary<int, string>();  // 엔딩2에서 사용할 정답 위치(현재 5칸만 사용)

    private bool isChessGimicEnding;    // 체스판에서 정답을 맞춰서 엔딩을 실행하는지 확인하기 위한 변수(true : 엔딩조건 달성 / false : 엔딩조건 미달성)
    public bool IsChessGimicEnding      // 체스판에서 정답을 맞췄는지 설정하기 위한 프로퍼티(MainHall_GetAmeClock에서 사용)
    {
        set
        {
            isChessGimicEnding = value;
        }
    }
    #endregion

    private void Awake()
    {
        // 사용전 Action을 null로 초기화
        DestroyWrongPlaceTakos = null;
        InitChessBoard = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init(); // 초기화 함수 호출
    }

    // 초기화 함수
    private void Init()
    {
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();          // MainHall_ObjectManager를 찾아와서 할당
        ameClock = GameObject.FindAnyObjectByType<MainHall_GetAmeClock>();                          // 아메시계 오브젝트를 찾아와서 할당
        mainHall_EndingExitDoor = GameObject.FindAnyObjectByType<MainHall_EndingExitDoorCtrl>();    // 엔딩문 오브젝트를 찾아와서 할당
        interaction = GetComponent<Interaction_Gimics>();                                           // 자신에게 붙어있는 interaction gimic을 할당

        ameClock.Init();    // 아메시계의 초기화 함수 호출
        ameClock.gameObject.SetActive(false);  // 엔딩을 못봤으므로 비활성화
        settingGimic = interaction.run_Gimic;   // 기믹 수행 여부 초기화

        countTakos = 0; // 체스판에 놓인 타코수 0으로 초기화
        // 체스판에 놓을 수 있는 최대 타코수 초기화
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1) // 1회차인 경우
        {
            maxTakos = maxTakos_ending1;
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2) // 2회차인 경우
        {
            maxTakos = maxTakos_ending2;
        }
        #region 체스판 답안지 설정
        // 사용전 딕셔너리 초기화
        CorrectTako_ending1?.Clear();
        CorrectTako_ending2?.Clear();

        // 1회차 체스판 정답 배열 초기화
        // 정답 타코 인덱스 및 위치
        CorrectTako_ending1.Add(6, "B3");  // 구라
        CorrectTako_ending1.Add(7, "C4");  // 칼리
        CorrectTako_ending1.Add(8, "H8");  // 키아라

        // 2회차 체스판 정답 배열 초기화
        // 정답 타코 인덱스 및 위치
        CorrectTako_ending2.Add(5, "D4");  // 베이
        CorrectTako_ending2.Add(1, "G2");  // 무메이
        CorrectTako_ending2.Add(2, "A1");  // 아이리스
        CorrectTako_ending2.Add(3, "E6");  // 크로니
        CorrectTako_ending2.Add(4, "F5");  // 파우나
        #endregion

        answerChessTako = new AnswerTakoInfo[maxTakos];   // 체스판에 놓인 타코들의 정보를 받아 저장할 구조체 배열 초기화

        InitChessBoard?.Invoke();   // 체스칸들 초기화 함수들 실행

        Setting_SceneStart();       // 세이브 데이터에 따라 기믹 세팅
    }

    // 세이브 데이터에 따라 시작전에 기믹 세팅
    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            if (ameClock.GetComponent<Item18AmeClock>().isGetItem == true)
            {
                // 아메 시계를 이미 획득했을 경우 비활성화 시킴
                ameClock.gameObject.SetActive(false);
                blockBeforeEnding.SetActive(false);
            }
            else
            {
                // 아메 시계를 아직 획득하지 못했을 경우 활성화 시킴
                ameClock.gameObject.SetActive(true);
                blockBeforeEnding.SetActive(true);
            }

            StartCoroutine(EndingGimicStart());             // 엔딩 조건달성 확인 함수 호출
        }
        else
        {
            // 기믹을 아직 수행하지 않았으면 setgimic이 false
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 생성 및 파괴 테스트용 코드
        if ((countTakos == maxTakos) && !isTakoReachMaxNum)
        {
            isTakoReachMaxNum = true;           // 타코가 최대수에 도달했다고 상태 변경

            ChessTakoCheck();                   // 타코 위치가 맞는지 확인하기 위한 함수 호출
        }
    }

    /// <summary>
    /// 해당 인덱스의 타코가 이미 체스판에 있는지 확인해서 bool값을 반환해줄 함수(true : 존재함 / false : 존재하지 않음)
    /// </summary>
    /// <param name="_trySummonTakoIndex"></param>
    /// <returns></returns>
    public bool GetAnswerChessTako(int _trySummonTakoIndex)
    {
        bool result = false;

        if (answerChessTako == null)    // 배열이 비어있을 경우
        {
            // Debug.Log("no takos in array");
            return result;  // 결과 반환
        }

        // 최대 타코수 만큼 반복해서 이미 생성되었는지 체크
        for (int i = 0; i < maxTakos; i++)
        {
            // 만약 해당 인덱스의 타코가 이미 생성되었을 경우 true를 result를 변경하고 반복종료
            if (answerChessTako[i].TakoIndex == _trySummonTakoIndex)
            {
                // Debug.Log("that tako is in array");
                result = true;
                break;
            }
        }
        // Debug.Log("no takos in array");

        return result;  // 결과 반환
    }

    // 타코들이 올바른 위치에 놓였는지 확인하기 위한 함수
    private void ChessTakoCheck()
    {
        int TakoCheckNum = 0;   // 정답과 동일한 타코수를 확인하기 위한 변수

        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1) // 1회차인 경우
        {
            for (int i = 0; i < maxTakos; i++)
            {
                // 엔딩 회차에 따른 정답지와 현재 제출된 타코의 위치를 비교하기 위한 함수 호출해서 결과 저장
                TakoCheckNum += IsTakoOnCorrectPosition(CorrectTako_ending1, i);
            }

            // 정답지와 같은 타코수가 최대 타코수보다 낮은 경우 틀렸으므로 전부 삭제
            if (TakoCheckNum < maxTakos)
            {
                Debug.Log("오답");
                DestroyWrongTakos();    // 틀렸을 경우 타코를 삭제시킬 함수 호출

                return; // 삭제시켰으므로 종료
            }
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2) // 2회차인 경우
        {
            for (int i = 0; i < maxTakos; i++)
            {
                // 엔딩 회차에 따른 정답지와 현재 제출된 타코의 위치를 비교하기 위한 함수 호출해서 결과 저장
                TakoCheckNum += IsTakoOnCorrectPosition(CorrectTako_ending2, i);
            }

            // 정답지와 같은 타코수가 최대 타코수보다 낮은 경우 틀렸으므로 전부 삭제
            if (TakoCheckNum < maxTakos)
            {
                Debug.Log("오답");
                DestroyWrongTakos();    // 틀렸을 경우 타코를 삭제시킬 함수 호출

                return; // 삭제시켰으므로 종료
            }
        }
        else 
        {
            Debug.Log("잘못된 엔딩 설정");
        }

        Debug.Log("정답");
        // 엔딩 문 열리고 엔딩 스토리 출력하는 함수 호출.

        // 시계 생성 시 파티클 및 소리 재생
        ameClockGeneration_Particle.SetActive(true);
        ameClockGeneration_Particle.GetComponent<ParticleSystem>().Play();
        ameClockGeneration_SFX.SetActive(true);
        // 해당 함수에서 엔딩이 진행되는동안 회차정보를 업데이트하고 아이템을 초기화 시키는 함수도 호출
        ameClock.gameObject.SetActive(true);            // 아메 회중시계를 활성화

        // 아메 시계 소환 함수 호출

        StartCoroutine(EndingGimicStart());             // 엔딩 조건달성 확인 함수 호출
    }

    // 현재 체스판에 올라온 타코들이 정답지와 동일한 위치에 있는지 확인하기 위한 함수
    private int IsTakoOnCorrectPosition(Dictionary<int, string> _CorrectTako_ending, int _i)
    {
        int result = 0;            // 맞췄는지 확인하기 위한 결과값 반환을 위한 변수(0: 오답 / 1: 정답)
        string tempTakoPos = null; // 임시로 해당 엔딩의 체스핀 답안지 딕셔너리에서 검색해온 타코의 올바른 위치를 저장할 변수

        // 정답지 딕셔너리에 현재 제출된 타코의 인덱스가 있는지 확인하기위한 조건문
        if (_CorrectTako_ending.ContainsKey(answerChessTako[_i].TakoIndex))
        {
            _CorrectTako_ending.TryGetValue(answerChessTako[_i].TakoIndex, out tempTakoPos); // 정답지 딕셔너리에 해당 타코가 있으므로 정답 위치 값을 찾아옴

            // 해당 타코의 현재 위치와 올바른 위치가 동일한지 확인
            if (String.Equals(tempTakoPos, answerChessTako[_i].TakoPosition))
            {
                result = 1;    // 정답확인 변수 1로 설정
            }
        }

        return result;  // 결과 반환
    }

    // 틀렸을 경우 타코를 삭제시킬 함수
    private void DestroyWrongTakos()
    {
        answerChessTako = new AnswerTakoInfo[maxTakos];   // 체스판에 놓인 타코들의 정보가 저장된 구조체 배열 초기화
        countTakos = 0;                     // 현재 놓인 타코수를 0으로 초기화
        isTakoReachMaxNum = false;          // 타코가 최대수만큼 놓이지 않았다고 상태 변경

        DestroyWrongPlaceTakos?.Invoke();   // 해당 Action이 비어있지 않을 경우 들어있는 모든 함수를 실행
        DestroyWrongPlaceTakos = null;      // 실행했으므로 Action 초기화
    }

    /// <summary>
    /// 체스판에서 정답을 맞췄을 경우 실행할 함수 호출(MainHall_ChessBorard에서 실행)
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndingGimicStart()
    {
        yield return new WaitUntil(() => ameClock.GetComponent<Item18AmeClock>().isGetItem);   // 아메 시계를 획득할때까지 대기

        blockBeforeEnding.SetActive(false); // 비활성화해서 지나갈 수 있게 바꿈

        // 시계 생성 시 파티클 및 소리 정지
        ameClockGeneration_Particle.GetComponent<ParticleSystem>().Stop();
        ameClockGeneration_SFX.SetActive(false);

        mainHall_EndingExitDoor.DoorOpenForEnding();    // 조건을 달성했으므로 엔딩 문을 열어줄 함수 호출
    }
}
