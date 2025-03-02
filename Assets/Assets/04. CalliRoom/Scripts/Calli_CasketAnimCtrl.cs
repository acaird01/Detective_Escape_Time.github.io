using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_CasketAnimCtrl : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;   // 칼리씬 오브젝트 매니저(현재 테스트용 정보 획득을 위함)
    private Interaction_Gimics interaction;            // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                         // 플레이어

    private Calli_DeadBeat deadBeat;                   // 데드비츠 해골 오브젝트
    private Animator CasketAnimator;                   // 사각관을 열고 닫을 Animator 컴포넌트
    private AudioSource casket_AudioSource;            // 사각관 열리고 닫힐때 사용할 Audio Source 컴포넌트
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); // 플레이어를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();        // 상호작용을 위한 Interaction_Gimics 할당
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당

        casket_AudioSource = gameObject.GetComponent<AudioSource>();

        deadBeat = GameObject.FindAnyObjectByType<Calli_DeadBeat>();    // Calli_DeadBeat를 찾아와서 할당
        CasketAnimator = this.gameObject.GetComponent<Animator>();  // 해당 오브젝트가 가진 animator를 찾아와서 할당
        settingGimic = interaction.run_Gimic;

        StartCoroutine(WaitTouch());    // 대기 코루틴 실행

        Setting_SceneStart();           // 세팅 시작 코루틴 실행
    }


    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            // 이미 뚜껑을 열어둔 상태이므로 문열리는 애니메이션 재생
            StartCoroutine(opening());
        }
        else
        {
            CasketAnimator.Play("Idle");
            deadBeat.GetComponent<CapsuleCollider>().enabled = false;    // 상호작용이 불가능하게끔 해골의 콜라이더 비활성화
        }
    }

    IEnumerator WaitTouch()
    {
        while (player) // 한번하고 뽀사지면 이거빼고, 창문 열고 닫는거처럼 반복필요하면 이거 넣고 쓰기
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에

            if (interaction.run_Gimic)
            {
                // 문열리는 애니메이션 재생
                StartCoroutine(opening());
            }
            else
            {
                // 문닫히는 애니메이션 재생
                StartCoroutine(closing());
            }
        }
    }

    IEnumerator opening() // 기믹 작동했을 때 
    {
        deadBeat.GetComponent<CapsuleCollider>().enabled = true;    // 상호작용이 가능하게끔 해골의 콜라이더 활성화

        // CasketAnimator.Play("CasketOpen");
        CasketAnimator.SetBool("isOpenCasket", true);

        if (casket_AudioSource.isPlaying)   // 이미 재생 중이던 소리가 있을 경우
        { 
            casket_AudioSource.Stop();      // 관 뚜경 소리 정지
        }
        casket_AudioSource.Play();      // 관 뚜경 소리 재생

        yield return new WaitForSeconds(.5f);
    }

    IEnumerator closing() // 기믹이 되돌아 갈때
    {
        deadBeat.GetComponent<CapsuleCollider>().enabled = false;    // 상호작용이 불가능하게끔 해골의 콜라이더 비활성화

        // CasketAnimator.Play("CasketClose");
        CasketAnimator.SetBool("isOpenCasket", false);

        if (casket_AudioSource.isPlaying)   // 이미 재생 중이던 소리가 있을 경우
        {
            casket_AudioSource.Stop();      // 관 뚜경 소리 정지
        }
        casket_AudioSource.Play();      // 관 뚜경 소리 재생

        yield return new WaitForSeconds(.5f);
    }
}
