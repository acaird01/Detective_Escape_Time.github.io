using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_GhostLantern : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private AudioSource ghostLanternSound_AudioSource;  // 영혼이 수확되었을때 소리를 재생할 AudioSource
    private Light ghostLight;                           // 수확되면 빛을 밝힐 Light 컴포넌트
    private GameObject tako;                            // 랜턴 기믹을 풀었을때 소환해줄 타코 GameObject
    private Calli_SummonTakoEffect calli_SummonTakoEffect;  // 타코 소환 이펙트 재생 및 애니메이션 재생을 위한 스크립트

    [Header("영혼 수확 여부를 저장할 interaction gimic")]
    [SerializeField]
    private Interaction_Gimics save_Interaction;        // 결과를 저장하기 위한 Interaction gimic(부모에 있음)
    [Header("영혼이 수확될때 재생할 audioclip(소리 길이 확인용)")]
    [SerializeField]
    private AudioClip ghostCatch_SFX;                   // 영혼이 수확될때 재생할 audioclip(소리 길이 확인용)
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private const int MaxGhostNum = 5;  // 잡아야할 최대 영혼의 수를 상수값으로 지정(현재 5)
    private bool isAllGhostCatch;       // 영혼이 최대치까지 수확되었는지 확인하기 위한 bool 변수(true : 모두 잡힘 / false : 다 안잡힘)
    private int currGhostCount;         // 현재 영혼이 얼마나 수확되었는지 확인하기 위한 변수
    public int CurrentGhostCount        // 영혼에서 본인이 수확되었다고 체크해주기 위한 프로퍼티
    {
        set 
        {
            // 현재 영혼의 수가 최대치가 아닐 경우에만 set
            if (currGhostCount < MaxGhostNum)
            { 
                currGhostCount = value;
            }
        }
    }
    private const string ending1_TakoName = "Item_06_TakoCalli";    // 현재 엔딩회차가 1일 경우 소환할 타코이름
    private const string ending2_TakoName = "Item_02_TakoIRyS";    // 현재 엔딩회차가 2일 경우 소환할 타코이름
    private float ghostSoundLength;    // 영혼 수확될때 나올 소리 길이
    #endregion


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();    // 상호작용을 위한 Interaction_Gimics 할당

        Init();     // 초기화 함수 호출
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Init()
    {
        ghostLight = GetComponentInChildren<Light>();    // Calli_GhostLight의 Light 컴포넌트를 찾아와서 할당
        ghostLanternSound_AudioSource = GetComponentInChildren<AudioSource>();  // 영혼이 수확완료되면 소리를 재생할 AudioSource를 할당
        calli_SummonTakoEffect = GetComponentInChildren<Calli_SummonTakoEffect>();  // 타코 소환 애니메이션 재생용 스크립트를 찾아와서 할당
        settingGimic = save_Interaction.run_Gimic;       // 기믹 실행 여부를 확인해서 저장
        textController = player.GetComponentInChildren<TextController>();

        currGhostCount = 0;             // 아직 영혼이 하나도 수확되지 않았다고 초기화
        isAllGhostCatch = false;        // 아직 모든 영혼이 수확되지 않았다고 초기화

        ghostSoundLength = (ghostCatch_SFX.length / 3) * 2; // 영혼 소리를 2/3길이로 조정

        if (GameManager.instance.Episode_Round == 1)        // 현재 회차가 1일 경우에 소환할 타코이름으로 초기화
        {
            tako = GetComponentInChildren<Item07TakoCalli>().gameObject;    // 소환해줄 타코를 찾아와서 할당(칼리)
        }
        else if (GameManager.instance.Episode_Round == 2)   // 현재 회차가 2일 경우에 소환할 타코이름으로 초기화
        {
            tako = GetComponentInChildren<Item02TakoIRyS>().gameObject;     // 소환해줄 타코를 찾아와서 할당(아이리스)
        }
        else 
        {
            Debug.Log("엔딩 설정이 잘못됨");
        }

        tako.SetActive(false);                   // 타코를 비활성화

        StartCoroutine(WaitTouch());    // 상호작용 대기할 코루틴 함수 호출

        Setting_SceneStart();           // 세팅 시작 코루틴 실
    }

    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            isAllGhostCatch = true;
        }
        else
        {
            isAllGhostCatch = false;
        }
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에

            // 칼리 낫을 획득하지 못한 경우
            if (!ItemManager._instance.inventorySlots[12].GetComponent<IItem>().isGetItem)
            {
                StartCoroutine(textController.SendText("으스스한 느낌이 드는 랜턴이네.\n마치 영혼이라도 가둬두기 위한 감옥인 것만 같아.."));

                // StartCoroutine(textController.SendText("영혼들을 수확하면 빛이 들어오지 않을까?"));

                interaction.run_Gimic = false;
            }
            else // 칼리 낫을 획득한 경우
            {
                if (isAllGhostCatch) // 모든 영혼을 잡은 경우
                {
                    StartCoroutine(textController.SendText("좋아, 영혼들을 모두 모았어!\n옆의 타코를 데려가자"));
                }
                else
                {
                    StartCoroutine(textController.SendText("영혼들을 모두 수확하면 빛이 들어오지 않을까?"));
                }

                interaction.run_Gimic = false;
            }
        }
    }

    #region 기믹을 수행하는 함수 모음
    // 영혼이 수확될 경우 빛을 밝혀줄 함수(Calli_CatchGhost에서 호출해서 사용)
    public void SetGhostLight()
    {
        // 최대치까지 잡히지 않았다면 영혼이 수확되었으므로 빛을 1단계 올려줌
        if (!isAllGhostCatch)
        {
            if (currGhostCount < MaxGhostNum)
            { 
                currGhostCount++;       // 잡힌 영혼수를 1증가
            }
            
            ghostLight.range += 20; // 밝기를 20 증가
        }

        if (currGhostCount == MaxGhostNum)
        {
            isAllGhostCatch = true; // 수가 같아졌다면 모든 고스트가 잡혔다고 저장
            save_Interaction.run_Gimic = isAllGhostCatch;

            StartCoroutine(SummonTako());   // 다잡혔으므로 타코 제공함수 호출    
        }
    }

    // 타코 소환하는 코루틴 함수
    private IEnumerator SummonTako()
    {
        yield return new WaitForSeconds(ghostSoundLength);  //영혼이 수확될때 재생할 audioclip의 2/3길이만큼 대기 후 소환

        ghostLanternSound_AudioSource.Play();   // 타코 소환 효과음 출력

        tako.SetActive(true);                   // 타코를 소환(활성화)

        // 여기서 애니메이션 실행
        calli_SummonTakoEffect.animationStart();

        // 진행도 업데이트
        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
    }
    #endregion
}
