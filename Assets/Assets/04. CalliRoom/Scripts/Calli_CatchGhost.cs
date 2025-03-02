using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Calli_CatchGhost : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private Calli_GhostLantern ghostLantern;            // 영혼을 수확하면 불을 밝힐 랜턴
    private Calli_GhostGimics ghostGimics;              // 영혼 기믹 관리 스크립트
    private AudioSource ghost_SFX;                      // 영혼이 수확될때 효과음을 출력할 AudioSource

    [Header("재생할 파티클 이펙트")]
    [SerializeField]
    GameObject effectLight;
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    private bool isGhostVisible;    // 이미 유령이 보이고 있는지 확인하기 위한 조건문
    [SerializeField]
    private bool isGhostCatch;      // 해당 영혼이 수확되었는지 확인하기 위한 변수
    public bool IsGhostCatch      // 해당 영혼이 수확되었는지 Calli_GhostGimics에서 확인하기 위한 프로퍼티
    {
        get
        { 
            return isGhostCatch;
        }
    }
    private bool isGhostWait;        // 해당 영혼이 아직 안잡히고 대기중인지 확인하기 위한 변수(true : 대기중 / false : 대기종료)
    private float ghostSoundLength;  // 영혼 수확될때 나올 소리 길이
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        //interaction = gameObject.GetComponent<Interaction_Gimics>();    // 상호작용을 위한 Interaction_Gimics 할당

        // Init();     // 초기화 함수 호출
    }

    // 초기화 함수 호출
    public void Init()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();    // 상호작용을 위한 Interaction_Gimics 할당
        ghost_SFX = GetComponent<AudioSource>();    // 가지고 있는 AudioSource를 할당
        ghostLantern = GameObject.FindAnyObjectByType<Calli_GhostLantern>();    // Calli_DeadBeatSkull를 찾아와서 할당
        ghostGimics = GameObject.FindAnyObjectByType<Calli_GhostGimics>();      // 영혼들 관리하는 오브젝트를 찾아와서 할당

        settingGimic = interaction.run_Gimic;       // 기믹 실행 여부를 확인해서 저장
        isGhostVisible = false;     // 현재 안보이는 상태로 설정
        isGhostCatch = false;       // 아직 안잡혔다고 초기화
        isGhostWait = true;         // 아직 대기 중이라고 표시

        isGhostVisible = ghostGimics.IsGhostVisible;    // 활성화 됬을때 현재 보이는지 여부 최신화
        ghostSoundLength = (ghost_SFX.clip.length / 3) * 2; // 유령소리를 2/3길이로 조정

        effectLight.gameObject.SetActive(false);    // 이펙트를 비활성화 해둠
        // StartCoroutine(WaitTouch());                    // 상호작용 대기 코루틴 함수 실행

        Setting_SceneStart();           // 세팅 시작 코루틴 실행
    }

    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            isGhostCatch = true;
            ghostLantern.SetGhostLight();   // 이미 잡혔으므로 빛을 세팅해줄 함수 호출
            this.gameObject.SetActive(false);   // 시작 전에 비활성화 시킴
        }
        else
        {
            isGhostCatch = false;
            this.gameObject.SetActive(false);   // 시작 전에 비활성화 시킴
        }
    }

    private void OnEnable()
    {
        isGhostVisible = true;          // 활성화 됬을때 현재 보이는 중이라고 설정
        isGhostWait = true;             // 다시 영혼을 대기 시키기 위해 true로 설정
    }
    private void OnDisable()
    {
        isGhostVisible = false;         // 비활성화 됬을때 현재 보이지 않는다고 설정
    }

    private void Update()
    {
        if (isGhostVisible && isGhostCatch)   // 활성화됬는데 이미 해당 영혼이 잡혔을 경우
        {
            isGhostVisible = false;        // 영혼 비활성화를 위해 안보인다고 변경
            isGhostWait = true;            // 이미 잡혔으므로 값 변경(일단 없어도 되긴함)
            SetGhostVisible(isGhostCatch); // 활성화상태 전환 함수 실행(비활성화 시킴)
        }

        if (isGhostWait && !isGhostCatch)
        {
            isGhostWait = false;            // 이미 대기 중이므로 false로 변경
            StartCoroutine(WaitTouch());    // 기믹 수행을 위한 대기 코루틴 함수 호출
        }
    }

    IEnumerator WaitTouch()
    {
        yield return new WaitUntil(() => (interaction.run_Gimic) == true);

        if (interaction.run_Gimic)
        {
            isGhostCatch = true;            // 영혼이 수확되었다고 상태 변경
            this.gameObject.GetComponent<SphereCollider>().enabled = false; // 이미 잡혔으므로 소리 재생하는동안 못잡게끔 콜라이더 비활성화

            ghost_SFX.Play();               // 영혼 수확 효과음 출력
            effectLight.SetActive(true);                          // 이펙트 활성화
            effectLight.GetComponent<ParticleSystem>().Play();    // 이펙트 출력
            ghostLantern.SetGhostLight();   // 잡혔으므로 랜턴의 밝기를 올릴 함수 호출

            // yield return new WaitUntil(() => !ghost_SFX.isPlaying); // 소리 재생이 끝날때까지 대기
            yield return new WaitForSeconds(ghostSoundLength); // 소리 재생 시간의 2/3까지 대기

            effectLight.GetComponent<ParticleSystem>().Stop();  // 이펙트 정지

            calli_ObjectManager.ChangeSceneData_To_GameManager();   // 유령이 수확되었으므로 저장 1회 진행

            this.gameObject.SetActive(false); ;  // 영혼이 수확되었으므로 비활성화 시킴
        }
    }

    #region 기믹을 수행하는 함수 모음
    // 칼리 낫을 핫키에서 선택해서 들고 있을 경우 영혼을 활성화시켜 보여줄 함수
    private void SetGhostVisible(bool _isGhostVisible)
    {
        // 영혼을 bool 값에 따라 활성, 비활성화시킴(아직 잡히지 않았다면 활성화)
        if (!isGhostCatch)
        {
            this.gameObject.SetActive(!_isGhostVisible);
        }
    }
    #endregion
}
