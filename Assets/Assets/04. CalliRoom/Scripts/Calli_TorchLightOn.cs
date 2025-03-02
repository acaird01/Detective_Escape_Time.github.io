using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_TorchLightOn : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private Light torch_Light;                          // 성냥과 상호작용하면 빛을 밝힐 Light 컴포넌트
    private GameObject takoPosHint;                     // 횃불 기믹을 풀었을때 보여줄 타코 위치 힌트
    private Calli_TorchGimic calli_TorchGimic;          // 횃불 기믹을 풀었을때 힌트를 보여줄 스크립트
    private GameObject torchWall_Prefab;                // 막대가 걸려있는 횃대 프리펩
    private Calli_NoTorchWall calli_NoTorchWall;        // 횃대만 있는 프리펩을 찾아올 스크립트
    #endregion

    #region 오디오 관련
    private AudioSource matchLightOnSound_AudioSource;   // 성냥을 이용해 횃불이 켜질 때 소리를 재생할 AudioSource
    [Header("성냥불 오디오 클립")]
    [SerializeField]
    private AudioClip matchLight_AudioClip;
    [Header("횃불 거치음 오디오 클립")]
    [SerializeField]
    private AudioClip torchSet_AudioClip;
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수(횃불 막대기가 활성화 되었는지 확인용)
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private bool isTakoPosActive;               // 타코 위치 힌트가 활성화 되었는지 확인하기 위한 bool 변수
    #endregion

    /// <summary>
    /// Calli_TorchLightOn 초기화 함수(Calli_TorchGimic에서 실행)
    /// </summary>
    public void Init()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;               // 플레이어를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();                         // 상호작용시 대사를 띄워줄 스크립트

        torchWall_Prefab = GetComponentInChildren<Calli_TorchWall>().gameObject;  // 횃불 막대만 걸린 횃대 프리펩 할당
        torch_Light = GetComponentInChildren<Light>();      // 횃불의 자식이 가지고 있는 Light 컴포넌트를 찾아와서 할당
        matchLightOnSound_AudioSource = GetComponent<AudioSource>();  // 횃불이 켜질 때 소리를 재생할 AudioSource
        calli_TorchGimic = GameObject.FindAnyObjectByType<Calli_TorchGimic>();  // 횃불 기믹을 풀었을때 힌트를 보여줄 스크립트
        takoPosHint = calli_TorchGimic.takoPosHint; // 횃불 기믹을 풀었을때 보여줄 타코 위치 힌트를 찾아와서 할당
        calli_NoTorchWall = GameObject.FindAnyObjectByType<Calli_NoTorchWall>();    // 횃대만 있는 프리펩

        settingGimic = calli_NoTorchWall.GetComponent<Interaction_Gimics>().run_Gimic;       // 기믹 실행 여부를 확인해서 저장
        isTakoPosActive = false;        // 아직 보여지고 있지 않다고 초기화

        Setting_SceneStart();   // 세팅 함수 호출
    }

    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            SummonTorch();      // 횃불을 소환해둠.

            if (takoPosHint.activeSelf)
            {
                torch_Light.gameObject.SetActive(true);     // 힌트가 이미 보여지고 있으므로 불을 켜줌
                isTakoPosActive = true;         // 이미 보여지고 있다고 상태 변경
            }
            else
            {
                torch_Light.gameObject.SetActive(false);    // 힌트가 보여지고 있지 않으므로 불을 꺼줌
                isTakoPosActive = false;        // 아직 보여지고 있지 않다고 상태 변경

                StartCoroutine(WaitTouch());    // 대기 코루틴 함수 호출
            }
        }
        else
        {
            // 막대가 걸려있는 횃대를 비활성화 해줌
            torch_Light.gameObject.SetActive(false);
            torchWall_Prefab.gameObject.SetActive(false);

            StartCoroutine(WaitTouch());    // 대기 코루틴 함수 호출
        }
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에

            if (ItemManager._instance.hotkeyItemIndex == 25 && !settingGimic)    // 현재 핫키의 아이템이 횃불(25번)이고 아직 활성화 되지 않았다면
            {
                // 수행할 기믹 함수 호출
                settingGimic = true;    // 이미 횃불 막대를 소환했다고 변경(추가 반복을 막음)
                calli_NoTorchWall.GetComponent<Interaction_Gimics>().run_Gimic = true;  // 기믹이 실행 완료 되었다고 변경

                // 횃불 막대기가 걸리는 소리 출력
                matchLightOnSound_AudioSource.clip = torchSet_AudioClip;    // 횃불 거치 효과음 설정
                matchLightOnSound_AudioSource.Play();

                SummonTorch();  // 횃불 소환할 함수 호출
            }
            else
            {
                if (settingGimic)   // 횃불이 걸려는 있는 경우
                {
                    if (ItemManager._instance.hotkeyItemIndex == 27 && !isTakoPosActive)    // 현재 핫키의 아이템이 성냥(27번)이고 아직 힌트가 나오고 있지 않은 경우
                    {
                        isTakoPosActive = true;     // 타코 위치 힌트가 나오고 있다고 상태 변경 (추가 반복을 막음)

                        // 불켜는 소리 재생
                        matchLightOnSound_AudioSource.clip = matchLight_AudioClip;    // 성냥불 효과음 설정
                        matchLightOnSound_AudioSource.Play();

                        torch_Light.gameObject.SetActive(true); // torch_Light를 활성화해서 불을 켜줌

                        calli_TorchGimic.SettingForObjectToInteration = true;   // Calli_TorchGimic의 bool 값변경(기믹이 해제 되었다고 상태 변경)

                        ItemManager._instance.ReturnItem(27);
                        ItemManager._instance.DeactivateItem(27);

                        StartCoroutine(calli_TorchGimic.CeilingTakoPos());      // Calli_TorchGimic에서 나머지 불을 전부 켜주고 힌트를 활성화 시킬 코루틴 함수 호출
                    }
                    else
                    {
                        // 만약 힌트가 활성화 되어 있을 경우 이미 기믹 해제가 완전히 완료되었으므로 해당 대사 출력
                        if (takoPosHint.activeSelf)
                        {
                            // StartCoroutine(textController.SendText("동굴을 밝혀주는 횃불"));      // 상호작용 대사 출력
                            StartCoroutine(textController.SendText("Fire Fire Light On Fire"));      // 상호작용 대사 출력
                        }
                        else  // 비활성화라면 아직 성냥기믹은 해제되지 않았으므로 해당 대사 출력
                        {
                            StartCoroutine(textController.SendText("횃불 막대기이다. 어디 불을 붙일만한게 없을까?"));      // 상호작용 대사 출력
                        }
                    }
                }
                else // 횃불 막대가 아직 걸려 있지 않은 경우
                {
                    StartCoroutine(textController.SendText("막대기 같은 걸 걸어둘 수 있을 것 같다."));      // 상호작용 대사 출력
                }
            }

            interaction.run_Gimic = false;  // 다시 상호작용 가능하도록 변경
        }
    }

    #region 기믹 수행하는 함수 모음
    /// <summary>
    /// 횃불 막대기를 소환할 함수
    /// </summary>
    private void SummonTorch()
    {
        // 1회차에서만 실행
        if (GameManager.instance.Episode_Round == 1)
        {
            // 진행도 업데이트
            GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        }

        // 횃불을 인벤토리로 되돌려줌
        ItemManager._instance.ReturnItem(25);
        ItemManager._instance.DeactivateItem(25);

        // 횃불 막대가 있는 프리펩을 활성화 시켜줌
        torchWall_Prefab.gameObject.SetActive(true);
    }
    #endregion
}
