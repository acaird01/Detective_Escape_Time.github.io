using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_TorchGimic : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private AudioSource torchLightOnSound_AudioSource;   // 횃불이 켜질 때 소리를 재생할 AudioSource
    private Light[] torch_Lights;                        // 성냥과 상호작용하면 빛을 밝힐 Light 컴포넌트들
    public GameObject takoPosHint;                       // 횃불 기믹을 풀었을때 보여줄 타코 위치 힌트
    private Calli_TorchLightOn calli_TorchLightOn;       // 횃불을 벽에 걸어주고 기믹해제 여부를 확인할 스크립트

    [Header("재생할 파티클 이펙트")]
    [SerializeField]
    GameObject effectLight;
    private AudioSource audioSource;        // 이펙트 실행할때 재생할 audio source
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수(횃불 막대기가 활성화 되었는지 확인용)
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;               // 플레이어를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();                         // 상호작용시 대사를 띄워줄 스크립트

        torch_Lights = GetComponentsInChildren<Light>();      // 횃불들의 자식들이 가지고 있는 Light 컴포넌트를 찾아와서 할당
        
        takoPosHint = GameObject.FindAnyObjectByType<Calli_TakoPosCeilingHint>().gameObject; // 횃불 기믹을 풀었을때 보여줄 타코 위치 힌트를 찾아와서 할당
        calli_TorchLightOn = GetComponentInChildren<Calli_TorchLightOn>();  // 횃불을 벽에 걸어주고 기믹해제 여부를 확인할 스크립트를 자식에서 찾아와서 할당
        audioSource = takoPosHint.GetComponent<AudioSource>();              // 재생할 audio source를 할당

        settingGimic = interaction.run_Gimic;       // 기믹 실행 여부를 확인해서 저장

        Setting_SceneStart();   // 세팅 함수 호출
    }

    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            // 횃불들을 전부 켜줌
            for (int i = 0; i < torch_Lights.Length; i++)
            {
                torch_Lights[i].gameObject.SetActive(true);
            }

            takoPosHint.gameObject.SetActive(true); // 타코 위치 힌트 활성화
        }
        else
        {
            // 횃불들을 숨겨둠
            for (int i = 0; i < torch_Lights.Length; i++)
            {
                // 만약 기믹해제에 사용되는 횃불일 경우 여기서 비활성화 시키지 않음.
                if (torch_Lights[i].name == "TorchLight (14)")
                {
                    continue;
                }

                torch_Lights[i].gameObject.SetActive(false);
            }

            takoPosHint.gameObject.SetActive(false); // 타코 위치 힌트 비활성화
        }

        calli_TorchLightOn.Init();  // Calli_TorchLightOn의 초기화 함수 실행
    }

    #region 기믹 수행하는 함수 모음
    // 천장의 힌트를 활성화 해줄 함수
    public IEnumerator CeilingTakoPos()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(0.5f);

        // 횃불들을 전부 켜줌
        for (int i = 0; i < torch_Lights.Length; i++)
        {
            // 횃불의 불을 켜줌
            torch_Lights[i].gameObject.SetActive(true);

            if (torch_Lights[i].name != "TorchLight (14)")
            {
                // 소환 소리 재생
                torchLightOnSound_AudioSource = torch_Lights[i].gameObject.GetComponent<AudioSource>();  // 횃불이 켜질 때 소리를 재생할 AudioSource
                torchLightOnSound_AudioSource.Play();
            }

            yield return waitSeconds;  // 0.2초 대기

            // 카메라 워킹 시도
        }

        // 마법진 이펙트 실행
        effectLight.SetActive(true);
        effectLight.GetComponent<ParticleSystem>().Play();
        audioSource.Play();

        yield return waitSeconds;  // 0.2초 대기

        takoPosHint.gameObject.SetActive(true); // 타코 위치 힌트 활성화

        yield return waitSeconds;  // 0.2초 대기

        // 마법진 이펙트 정지
        effectLight.GetComponent<ParticleSystem>().Stop();

        interaction.run_Gimic = true;

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate(); // 기믹 종료 시점에 호출
    }
    #endregion
}
