using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Calli_CrystalPosGimic : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    [Header("배치된 자물쇠 조각")]
    [SerializeField]
    private GameObject DialLockPiece;                   // 수정 기믹을 풀었을때 소환해줄 자물쇠조각 아이템 GameObject
    #endregion

    #region 각 돌 책상에서 사용할 Action
    private Action InitCrystalFrame;      // 각 액자를 초기화할 함수를 실행할 Aciton
    public void SetInitTapeHole(Action _InitCrystalFrame)     // InitCrystalFrame에 함수를 테이프 칸에서 할당할 set 함수
    {
        InitCrystalFrame += _InitCrystalFrame;
    }

    private Action ReplaceWrongPlaceCrystals;  // 지정되지 않은 잘못된 위치에 수정들이 생겼을 경우 실행할 Action
    public void SetReplaceWrongPlaceCrystals_CallBack(Action _ReplaceWrongPlaceCrystals) // ReplaceWrongPlaceCrystals에 함수를 테이프 칸에서 할당할 set 함수
    {
        ReplaceWrongPlaceCrystals += _ReplaceWrongPlaceCrystals;
    }
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private const string summon_ItemName = "Item_26_DialLockPiece";   // 소환할 아이템 이름
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    public bool isCrystalReachMaxNum;             // 최대 수정 수에 도달했는지 확인하기 위한 변수
    private const int maxCrystalNum = 5;          // 엔딩2의 최대 수정 수
    private int maxCrystals;                      // 엔딩을 위해 체스판에 놓을 최대 타코수
    public int MaxCrystals                        // 외부에서 최대 수정수를 확인하기 위한 프로퍼티(Calli_CheckCrystalPos에서 사용)
    {
        get
        {
            return maxCrystals;
        }
    }

    private int countCrystal;                     // 현재 액자에 놓인 수정 수
    public int CountCrystal                       // 현재 액자에 놓인 수정 수 프로퍼티(Calli_CheckCrystalPos에서 사용)
    {
        get
        {
            return countCrystal;
        }
        set
        {
            if (countCrystal < maxCrystals)       // 최대 테이프 수보다 현재 테이프 수가 적을때만 할당할 수 있게끔 설정
            {
                countCrystal = value;
            }
        }
    }

    // 제출된 수정들의 번호 및 위치를 저장할 구조체
    private struct AnswerCrystalInfo
    {
        public string CrystalIndex;      // 현재 액자에 놓인 수정의 이름를 저장할 구조체 멤버
        public string CrystalPosition;   // 현재 액자에 놓인 수정의 위치를 저장할 구조체 멤버
    };
    // answerCrystalNumbers 구조체를 담을 배열 선언
    private AnswerCrystalInfo[] answerCrystalPosition;
    /// <summary>
    /// 제출된 수정을 모아둘 구조체에 값을 할당하기 위한 set 함수(Calli_CrystalPosCheck에서 사용)
    /// </summary>
    /// <param name="_crystalIndex"></param>
    /// <param name="_crystalPosition"></param>
    public void SetAnswerCrystal(string _crystalIndex, string _crystalPosition)
    {
        if (countCrystal < maxCrystals)  // 최대 테이프수보다 현재 테이프수가 적을때만 실행
        {
            selectCrystal = null;   // 다시 선택가능하도록 현재 선택중인 수정이 없다고 만들어줌
            answerCrystalPosition[countCrystal].CrystalIndex = _crystalIndex;
            answerCrystalPosition[countCrystal].CrystalPosition = _crystalPosition;
            countCrystal++;
            Debug.Log("countTapes+ : " + countCrystal);//test
        }
        else
        {
            Debug.Log("AnswerCasetteTape : " + answerCrystalPosition.Length);
        }
    }

    // 수정 정답 저장용 Dictionary(답안지)
    private Dictionary<string, string> CorrectCrystal = new Dictionary<string, string>();  // 엔딩2에서 사용할 정답 위치(현재 5칸만 사용)

    public GameObject selectCrystal;    // 현재 선택된 수정의 정보를 저장할 변수
    //public GameObject selectFrame;      // 현재 선택된 액자의 정보를 저장할 변수
    //public bool[] selectCrytalCheckBool;  // 현재 누가 선택됬는지 정보를 저장할 배열

    // 테스트용. 현재 수정이 재생성되고나선 제자리에 오답일떄 안돌아감
    [SerializeField]
    private bool isCrystalGimicEnding;    // 액자에서 정답을 맞춰서 기믹이 동작 완료되었는지 확인하기 위한 변수(true : 기믹 조건 달성 / false : 기믹 조건 미달성)
    public bool IsCrystalGimicEnding      // 액자에서 정답을 맞췄는지 설정하기 위한 프로퍼티
    {
        set
        {
            isCrystalGimicEnding = value;
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
        
        settingGimic = interaction.run_Gimic;   // 기믹 수행 여부 초기화

        isCrystalGimicEnding = ItemManager._instance.inventorySlots[26].GetComponent<IItem>().isGetItem; // 자물쇠조각이 획득되었는지 확인하기 위한 변수 초기화

        // 현재 선택중인 수정과 액자를 null로 초기화
        selectCrystal = null;
        // selectFrame = null;

        countCrystal = 0; // 액자들에 놓인 수정 수 0으로 초기화
        // 액자칸에 놓을 수정수 초기화
        maxCrystals = maxCrystalNum;

        #region 수정 배치 답안지 설정
        // 사용전 딕셔너리 초기화
        CorrectCrystal?.Clear();

        // 액자 정답 배열 초기화
        // 정답 테이프 인덱스 및 위치
        CorrectCrystal.Add("PurpleCrystal", "Picard_CrytalPos1"); // 보라 1
        CorrectCrystal.Add("YellowCrystal", "Picard_CrytalPos2"); // 노랑 2
        CorrectCrystal.Add("BlueCrystal", "Picard_CrytalPos3");   // 파랑 3
        CorrectCrystal.Add("OrangeCrystal", "Picard_CrytalPos4"); // 주황 4
        CorrectCrystal.Add("RedCrystal", "Picard_CrytalPos5");    // 빨강 5
        #endregion

        answerCrystalPosition = new AnswerCrystalInfo[maxCrystals];   // 액자에 놓인 수정들의 정보를 받아 저장할 구조체 배열 초기화

        InitCrystalFrame?.Invoke();   // 액자들 초기화 함수들 실행

        Setting_SceneStart();         // 세이브 데이터에 따라 기믹 세팅
    }

    // 세이브 데이터에 따라 시작전에 기믹 세팅
    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            // 이미 기믹은 해제 되었으니 답안지에 따라 수정들을 지정된 위치에 배치
            foreach (var crystal_info in CorrectCrystal)
            {
                GameObject.Find(crystal_info.Key).transform.position = GameObject.Find(crystal_info.Value).transform.position + (Vector3.up * -0.2f);
            }

            // 열쇠를 획득했는지 확인
            if (isCrystalGimicEnding)
            {
                // 자물쇠조각을 이미 획득했을 경우 비활성화 시킴
                DialLockPiece.gameObject.SetActive(false);
            }
            else
            {
                // 자물쇠조각을 아직 획득하지 못했을 경우 활성화 시킴
                DialLockPiece.gameObject.SetActive(true);
            }
        }
        else
        {
            // 기믹을 아직 수행하지 않았으면 setgimic이 false
            DialLockPiece.gameObject.SetActive(false);    // 아직 기믹을 수행하지 않았으므로 비활성화
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 생성 및 파괴 테스트용 코드
        if ((countCrystal == maxCrystals) && !isCrystalGimicEnding)
        {
            isCrystalGimicEnding = true;           // 수정들이 최대수에 도달했다고 상태 변경

            CrystalFrameCheck();                   // 수정 위치가 맞는지 확인하기 위한 함수 호출
        }
    }
    /// <summary>
    /// 해당 이름의 수정이 이미 액자에 있는지 확인하기 위한 함수
    /// </summary>
    /// <param name="_trySummonCrystalIndex"></param>
    /// <returns></returns>
    public bool GetAnswerCrystalFramePos(string _trySummonCrystalIndex)
    {
        bool result = false;

        if (answerCrystalPosition == null)    // 배열이 비어있을 경우
        {
            return result;  // 결과 반환
        }

        // 최대 테이프수 만큼 반복해서 이미 생성되었는지 체크
        for (int i = 0; i < maxCrystals; i++)
        {
            // 만약 해당 인덱스의  이미 생성되었을 경우 true를 result로 변경하고 반복종료
            if (answerCrystalPosition[i].CrystalIndex == _trySummonCrystalIndex)
            {
                result = true;
                break;
            }
        }

        return result;  // 결과 반환
    }

    // 수정들이 올바른 위치에 놓였는지 확인하기 위한 함수
    private void CrystalFrameCheck()
    {
        int CrystalCheckNum = 0;   // 정답과 동일한 수정 수를 확인하기 위한 변수

        for (int i = 0; i < maxCrystals; i++)
        {
            // 엔딩 회차에 따른 정답지와 현재 제출된 수정의 위치를 비교하기 위한 함수 호출해서 결과 저장
            CrystalCheckNum += IsCrystalOnCorrectPosition(CorrectCrystal, i);
        }

        // 정답지와 같은 수정 수가 최대 수정수보다 낮은 경우 틀렸으므로 전부 원위치
        if (CrystalCheckNum < maxCrystals)
        {
            Debug.Log("오답");
            StartCoroutine(ReplaceWrongCrystals());    // 틀렸을 경우 수정들을 제자리로 되돌릴 함수 호출

            return; // 삭제시켰으므로 종료
        }

        Debug.Log("정답");
        SummonItem();    // 아이템을 소환해줄 함수 호출
    }

    // 현재 액자들에 올라온 수정들이 정답지와 동일한 위치에 있는지 확인하기 위한 함수
    private int IsCrystalOnCorrectPosition(Dictionary<string, string> _CorrectCrystal_ending, int _i)
    {
        int result = 0;            // 맞췄는지 확인하기 위한 결과값 반환을 위한 변수(0: 오답 / 1: 정답)
        string tempCrystalPos = null; // 임시로 해당 엔딩의 테이프 칸 답안지 딕셔너리에서 검색해온 테이프의 올바른 위치를 저장할 변수

        // 정답지 딕셔너리에 현재 제출된 테이프의 인덱스가 있는지 확인하기위한 조건문
        if (_CorrectCrystal_ending.ContainsKey(answerCrystalPosition[_i].CrystalIndex))
        {
            _CorrectCrystal_ending.TryGetValue(answerCrystalPosition[_i].CrystalIndex, out tempCrystalPos); // 정답지 딕셔너리에 해당 수정이 있으므로 정답 위치 값을 찾아옴

            // 해당 테이프의 현재 위치와 올바른 위치가 동일한지 확인
            if (string.Equals(tempCrystalPos, answerCrystalPosition[_i].CrystalPosition))
            {
                result = 1;    // 정답확인 변수 1로 설정
            }
        }

        return result;  // 결과 반환
    }

    // 틀렸을 경우 수정들을 제자리로 돌려보낼 함수
    private IEnumerator ReplaceWrongCrystals()
    {
        // 틀렸다고 효과음 재생

        yield return new WaitForSeconds(0.2f);  // 0.2초 대기(효과음 재생하는 동안 대기하게끔 변경)

        answerCrystalPosition = new AnswerCrystalInfo[maxCrystals];   // 액자에 놓인 수정들의 정보를 받아 저장할 구조체 배열 초기화
        countCrystal = 0;                     // 현재 놓인 테이프수를 0으로 초기화
        isCrystalReachMaxNum = false;          // 테이프가 최대수만큼 놓이지 않았다고 상태 변경

        ReplaceWrongPlaceCrystals?.Invoke();   // 해당 Action이 비어있지 않을 경우 들어있는 모든 함수를 실행
        ReplaceWrongPlaceCrystals = null;      // 실행했으므로 Action 초기화

        isCrystalGimicEnding = false; // 기믹이 종료되지 않았다고 변경
    }

    /// <summary>
    /// 현재 선택중인 수정을 저장하고 이전에 선택중이던게 있다면 다시 내려놓을 함수
    /// </summary>
    /// <param name="_currSelectCrystal"></param>
    public void SetSelectCrystal(GameObject _currSelectCrystal)
    {
        if (selectCrystal != null)      // 만약 기존에 선택중이던 수정이 있을 경우
        {
            selectCrystal.GetComponent<Calli_CrystalPosChange>().CrystalReplace();  // 수정을 다시 제자리로 되돌려줌.
            selectCrystal = _currSelectCrystal; // 현재 선택한 수정을 선택중인 수정으로 변경

            selectCrystal.GetComponent<Calli_CrystalPosChange>().CrystalPosUp();    // 선택되었다고 표현해줄 함수 호출
        }
        else    // 없다면 현재 선택 중인 수정을 바로 할당
        {
            selectCrystal = _currSelectCrystal;

            selectCrystal.GetComponent<Calli_CrystalPosChange>().CrystalPosUp();    // 선택되었다고 표현해줄 함수 호출
        }
    }

    // 열쇠를 소환하는 함수
    private void SummonItem()
    {
        interaction.run_Gimic = true;   // 기믹 수행이 완료되었다고 변경
        DialLockPiece.gameObject.SetActive(true);                         // 자물쇠조각을 활성화 시켜줌
        //key.transform.localScale = new Vector3(1f, 2f, 1.4f);   // 생성되는 열쇠의 크기 조절

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate(); // 기믹 종료 시점에 호출
    }
}
