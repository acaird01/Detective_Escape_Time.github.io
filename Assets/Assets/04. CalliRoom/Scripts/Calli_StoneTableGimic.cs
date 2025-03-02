using System;
using System.Collections.Generic;
using UnityEngine;

public class Calli_StoneTableGimic : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private GameObject key;                            // 테이프 기믹을 풀었을때 소환해줄 열쇠 GameObject
    private Calli_WaitingDeadBeat waitingDeadBeat;     // 2회차에서 위치힌트를 줄 데드비츠 스크립트
    private Calli_WhichSongPlayBook whichSongPlayBook; // 2회차에서 테이프 배치가 완료된 후 노래재생을 위한 스크립트

    [Header("기믹 수행 전 테이프들")]
    public GameObject[] floor_Tapes;
    [Header("기믹 완료 상태를 보여줄 테이프들")]
    public GameObject[] table_Tapes;
    #endregion

    #region 각 돌 책상에서 사용할 Action
    private Action InitTapeHoles;      // 각 테이프 칸을 초기화할 함수를 실행할 Aciton
    public void SetInitTapeHole(Action _InitTapeHoles)     // InitTapeHoles에 함수를 테이프 칸에서 할당할 set 함수
    {
        InitTapeHoles += _InitTapeHoles;
    }

    private Action DestroyWrongPlaceTapes;  // 지정되지 않은 잘못된 위치에 테이프들이 생겼을 경우 실행할 Action
    public void SetDestroyWrongPlaceTapes_CallBack(Action _DestroyWrongPlaceTapes) // DestroyWrongPlaceTapes에 함수를 테이프 칸에서 할당할 set 함수
    {
        DestroyWrongPlaceTapes += _DestroyWrongPlaceTapes;
    }
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private const string summon_KeyName = "Item_34_ChestKey";   // 소환할 열쇠 이름
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    public bool isTapeReachMaxNum;              // 최대 테이프 수에 도달했는지 확인하기 위한 변수
    private const int maxTapeNum_ending1 = 4;   // 엔딩1의 최대 테이프 수
    private const int maxTapeNum_ending2 = 4;   // 엔딩2의 최대 테이프 수
    private int maxTapes;                       // 엔딩을 위해 체스판에 놓을 최대 타코수
    public int MaxTapes                         // 외부에서 최대 타코수를 확인하기 위한 프로퍼티(Calli_CheckTapePos에서 사용)
    {
        get
        {
            return maxTapes;
        }
    }
    private int countTapes;                     // 현재 테이프 칸에 놓인 테이프 수
    public int CountTapes                       // 현재 테이프 칸에 놓인 테이프 수 프로퍼티(Calli_CheckTapePos에서 사용)
    {
        get
        {
            return countTapes;
        }
        set
        {
            if (countTapes < maxTapes)          // 최대 테이프 수보다 현재 테이프 수가 적을때만 할당할 수 있게끔 설정
            {
                countTapes = value;
            }
        }
    }

    // 제출된 테이프들의 아이템번호 및 위치를 저장할 구조체
    private struct AnswerTapeInfo
    {
        public int TapeIndex;         // 현재 체스판에 놓인 타코의 아이템 인덱스 번호를 저장할 구조체 멤버
        public string TapePosition;   // 현재 체스판에 놓인 타코의 체스판 위치를 저장할 구조체 멤버
    };
    // answerTapeNumbers 구조체를 담을 배열 선언
    private AnswerTapeInfo[] answerTableHoleTape;
    // 구조체에 값을 할당하기 위한 set 함수(Calli_CheckTapePos에서 사용)
    public void SetAnswerCasetteTape(int _tapeIndex, string _tapePosition)
    {
        Debug.Log("countTapes : " + countTapes);//test

        if (countTapes < maxTapes)  // 최대 테이프수보다 현재 테이프수가 적을때만 실행
        {
            answerTableHoleTape[countTapes].TapeIndex = _tapeIndex;
            answerTableHoleTape[countTapes].TapePosition = _tapePosition;
            countTapes++;
            Debug.Log("countTapes+ : " + countTapes);//test
        }
        else
        {
            Debug.Log("AnswerCasetteTape : " + answerTableHoleTape.Length);
        }
    }

    // 카세트 테이프 정답 저장용 Dictionary(답안지)
    private Dictionary<int, string> CorrectTape_ending1 = new Dictionary<int, string>();  // 엔딩1에서 사용할 정답 위치(현재 4칸만 사용)
    private Dictionary<int, string> CorrectTape_ending2 = new Dictionary<int, string>();  // 엔딩2에서 사용할 정답 위치(현재 5칸만 사용)

    private bool isStoneTableGimicEnding;    // 돌 테이블에서 정답을 맞춰서 기믹이 동작 완료되었는지 확인하기 위한 변수(true : 기믹 조건 달성 / false : 기믹 조건 미달성)
    public bool IsStoneTableGimicEnding      // 돌 테이블에서 정답을 맞췄는지 설정하기 위한 프로퍼티(???에서 사용)
    {
        set
        {
            isStoneTableGimicEnding = value;
        }
    }
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
        interaction = GetComponent<Interaction_Gimics>();               // 자신에게 붙어있는 interaction gimic을 할당
        key = GetComponentInChildren<Item34ChestKey>().gameObject;      // 현재 배치된 열쇠를 찾아와서 할당

        settingGimic = interaction.run_Gimic;   // 기믹 수행 여부 초기화
        isStoneTableGimicEnding = key.GetComponent<Item34ChestKey>().isGetItem; // 열쇠까지 소환되었는지 확인하기 위한 변수 초기화

        countTapes = 0; // 돌 책상에 놓인 테이프 수 0으로 초기화
        // 돌 책상에 놓인 테이프 수 초기화
        if (calli_ObjectManager.CheckEndingEpisode_Num == 1) // 1회차인 경우
        {
            maxTapes = maxTapeNum_ending1;
        }
        else if (calli_ObjectManager.CheckEndingEpisode_Num == 2) // 2회차인 경우
        {
            maxTapes = maxTapeNum_ending2;
        }
        #region 돌 책상 답안지 설정
        // 사용전 딕셔너리 초기화
        CorrectTape_ending1?.Clear();
        CorrectTape_ending2?.Clear();

        // 1회차 돌 책상 정답 배열 초기화
        // 정답 테이프 인덱스 및 위치
        CorrectTape_ending1.Add(21, "StoneTable_Hole_M");  // M
        CorrectTape_ending1.Add(22, "StoneTable_Hole_Y");  // Y
        CorrectTape_ending1.Add(23, "StoneTable_Hole_T");  // T
        CorrectTape_ending1.Add(24, "StoneTable_Hole_H");  // H

        // 2회차 돌 책상 정답 배열 초기화
        // 정답 테이프 인덱스 및 위치
        CorrectTape_ending2.Add(23, "StoneTable_Hole_M");  // T 2021. 8. 2.
        CorrectTape_ending2.Add(24, "StoneTable_Hole_Y");  // H 2022. 3. 20.
        CorrectTape_ending2.Add(21, "StoneTable_Hole_T");  // M 2022. 5. 18.
        CorrectTape_ending2.Add(22, "StoneTable_Hole_H");  // Y 2023. 8. 17.
        #endregion

        answerTableHoleTape = new AnswerTapeInfo[maxTapes];   // 돌 책상에 놓인 테이프들의 정보를 받아 저장할 구조체 배열 초기화

        InitTapeHoles?.Invoke();   // 테이프 놓을 칸들 초기화 함수들 실행

        // 2회차인 경우 테이프 정리를 기다리는 데드비츠의 초기화 함수 호출
        if (GameManager.instance.Episode_Round == 2)
        {
            whichSongPlayBook = GameObject.FindAnyObjectByType<Calli_WhichSongPlayBook>();  // 노래 재생을 위한 책 스크립트 할당
            waitingDeadBeat = GameObject.FindAnyObjectByType<Calli_WaitingDeadBeat>();
            waitingDeadBeat.Init(settingGimic);
            // whichSongPlayBook.Init(settingGimic);   // 노래를 고르는 책의 초기화 함수 호출해서 실행
        }

        Setting_SceneStart();       // 세이브 데이터에 따라 기믹 세팅
    }

    // 세이브 데이터에 따라 시작전에 기믹 세팅
    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            // 열쇠를 획득했는지 확인
            if (isStoneTableGimicEnding)
            {
                // 열쇠를 이미 획득했을 경우 비활성화 시킴
                key.gameObject.SetActive(false);

                for (int i = 0; i < maxTapes; i++)
                {
                    floor_Tapes[i].SetActive(false); // 바닥의 테이프 비활성화
                    table_Tapes[i].SetActive(true);  // 테이블의 테이프 활성화
                }
            }
            else
            {
                // 열쇠를 아직 획득하지 못했을 경우 활성화 시킴
                key.gameObject.SetActive(true);

                for (int i = 0; i < maxTapes; i++)
                {
                    floor_Tapes[i].SetActive(true); // 바닥의 테이프 활성화
                    table_Tapes[i].SetActive(false);  // 테이블의 테이프 비활성화
                }
            }
        }
        else
        {
            // 기믹을 아직 수행하지 않았으면 setgimic이 false
            key.gameObject.SetActive(false);    // 아직 기믹을 수행하지 않았으므로 비활성화

            for (int i = 0; i < maxTapes; i++)
            {
                floor_Tapes[i].SetActive(true); // 바닥의 테이프 활성화
                table_Tapes[i].SetActive(false);  // 테이블의 테이프 비활성화
            }
        }

        whichSongPlayBook.Init(settingGimic);   // 노래를 고르는 책의 초기화 함수 호출해서 실행
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 생성 및 파괴 테스트용 코드
        if ((countTapes == maxTapes) && !isTapeReachMaxNum)
        {
            isTapeReachMaxNum = true;           // 테이프가 최대수에 도달했다고 상태 변경

            TapeHoleCheck();                   // 테이프 위치가 맞는지 확인하기 위한 함수 호출
        }
    }

    /// <summary>
    /// 해당 인덱스의 테이프가 이미 테이프 칸에 있는지 확인해서 bool값을 반환해줄 함수(true : 존재함 / false : 존재하지 않음)
    /// </summary>
    /// <param name="_trySummonTapeIndex"></param>
    /// <returns></returns>
    public bool GetAnswerStoneTableTape(int _trySummonTapeIndex)
    {
        bool result = false;

        if (answerTableHoleTape == null)    // 배열이 비어있을 경우
        {
            // Debug.Log("no tapes in array");
            return result;  // 결과 반환
        }

        // 최대 테이프수 만큼 반복해서 이미 생성되었는지 체크
        for (int i = 0; i < maxTapes; i++)
        {
            // 만약 해당 인덱스의 테이프가 이미 생성되었을 경우 true를 result로 변경하고 반복종료
            if (answerTableHoleTape[i].TapeIndex == _trySummonTapeIndex)
            {
                // Debug.Log("that tape is in array");
                result = true;
                break;
            }
        }
        // Debug.Log("no tapes in array");

        return result;  // 결과 반환
    }

    // 테이프들이 올바른 위치에 놓였는지 확인하기 위한 함수
    private void TapeHoleCheck()
    {
        int TapeCheckNum = 0;   // 정답과 동일한 테이프수를 확인하기 위한 변수

        if (calli_ObjectManager.CheckEndingEpisode_Num == 1) // 1회차인 경우
        {
            for (int i = 0; i < maxTapes; i++)
            {
                // 엔딩 회차에 따른 정답지와 현재 제출된 테이프의 위치를 비교하기 위한 함수 호출해서 결과 저장
                if (GameManager.instance.Episode_Round == 1)
                {
                    TapeCheckNum += IsTapeOnCorrectPosition(CorrectTape_ending1, i);
                }
                else if (GameManager.instance.Episode_Round == 2)
                {
                    TapeCheckNum += IsTapeOnCorrectPosition(CorrectTape_ending2, i);
                }
                else
                {
                    Debug.Log("잘못된 엔딩 회차 설정");
                    TapeCheckNum += IsTapeOnCorrectPosition(CorrectTape_ending1, i);    // 임시로 1회차로 설정
                }
            }

            // Debug.Log("TapeCheckNum : " + TapeCheckNum); // 로그 출력

            // 정답지와 같은 테이프수가 최대 테이프수보다 낮은 경우 틀렸으므로 전부 삭제
            if (TapeCheckNum < maxTapes)
            {
                DestroyWrongTapes();    // 틀렸을 경우 타코를 삭제시킬 함수 호출

                return; // 삭제시켰으므로 종료
            }
        }
        else if (calli_ObjectManager.CheckEndingEpisode_Num == 2) // 2회차인 경우
        {
            for (int i = 0; i < maxTapes; i++)
            {
                // 엔딩 회차에 따른 정답지와 현재 제출된 테이프의 위치를 비교하기 위한 함수 호출해서 결과 저장
                TapeCheckNum += IsTapeOnCorrectPosition(CorrectTape_ending2, i);
            }

            // 정답지와 같은 테이프수가 최대 테이프수보다 낮은 경우 틀렸으므로 전부 삭제
            if (TapeCheckNum < maxTapes)
            {
                DestroyWrongTapes();    // 틀렸을 경우 테이프를 삭제시킬 함수 호출

                return; // 삭제시켰으므로 종료
            }
        }
        else
        {
            Debug.Log("잘못된 엔딩 설정");
        }

        interaction.run_Gimic = true;                           // 정답을 맞춰서 기믹 수행이 완료되었다고 변경
        // 2회차인 경우 대기중인 데드비츠의 상호작용 대사도 변경해주기 위해 상태 변경
        if (GameManager.instance.Episode_Round == 2)
        {
            // whichSongPlayBook.IsTapeGimicEnd = true;                // 테이프 배치 기믹이 종료되었다고 변경
            whichSongPlayBook.SetPlaySong(true);                    // 노래 재생 선택 기믹 실행 함수 호출
            waitingDeadBeat.SettingForObjectToInteration = true;    // 열쇠가 소환되어있다고 변경
        }

        SummonKey();    // 열쇠를 소환해줄 함수 호출
    }

    // 현재 테이프들이 정답지와 동일한 위치에 있는지 확인하기 위한 함수
    private int IsTapeOnCorrectPosition(Dictionary<int, string> _CorrectTape_ending, int _i)
    {
        int result = 0;            // 맞췄는지 확인하기 위한 결과값 반환을 위한 변수(0: 오답 / 1: 정답)
        string tempTapePos = null; // 임시로 해당 엔딩의 테이프 칸 답안지 딕셔너리에서 검색해온 테이프의 올바른 위치를 저장할 변수

        // 정답지 딕셔너리에 현재 제출된 테이프의 인덱스가 있는지 확인하기위한 조건문
        if (_CorrectTape_ending.ContainsKey(answerTableHoleTape[_i].TapeIndex))
        {
            _CorrectTape_ending.TryGetValue(answerTableHoleTape[_i].TapeIndex, out tempTapePos); // 정답지 딕셔너리에 해당 테이프가 있으므로 정답 위치 값을 찾아옴

            // 해당 테이프의 현재 위치와 올바른 위치가 동일한지 확인
            if (string.Equals(tempTapePos, answerTableHoleTape[_i].TapePosition))
            {
                result = 1;    // 정답확인 변수 1로 설정
            }
        }

        return result;  // 결과 반환
    }

    // 틀렸을 경우 테이프들을 삭제시킬 함수
    private void DestroyWrongTapes()
    {
        answerTableHoleTape = new AnswerTapeInfo[maxTapes];   // 돌 책상의 테이프칸에 놓인 테이프들의 정보가 저장된 구조체 배열 초기화
        countTapes = 0;                     // 현재 놓인 테이프수를 0으로 초기화
        isTapeReachMaxNum = false;          // 테이프가 최대수만큼 놓이지 않았다고 상태 변경

        DestroyWrongPlaceTapes?.Invoke();   // 해당 Action이 비어있지 않을 경우 들어있는 모든 함수를 실행
        DestroyWrongPlaceTapes = null;      // 실행했으므로 Action 초기화
    }

    // 열쇠를 소환하는 함수
    private void SummonKey()
    {
        if (!key.activeSelf)
        {
            // 진행도 업데이트
            GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        }

        key.gameObject.SetActive(true);                             // 열쇠를 활성화 시켜줌
        key.transform.localScale = new Vector3(1f, 2f, 1.4f);   // 생성되는 열쇠의 크기 조절

        // 열쇠를 생성했으므로 테이프 (21~24)를 전부 인벤토리로 되돌리고 비활성화
        for (int i = 21; i < 25; i++)
        {
            ItemManager._instance.ReturnItem(i);
            ItemManager._instance.DeactivateItem(i);
        }
    }
}
