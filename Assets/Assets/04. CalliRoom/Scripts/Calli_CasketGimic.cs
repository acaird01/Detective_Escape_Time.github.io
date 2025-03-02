using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_CasketGimic : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;   // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;            // 상호작용하는 기믹인지 확인하기 위한 컴포넌트

    private GameObject calliScythe;                    // 칼리 낫 아이템
    private Calli_DeadBeat deadBeat;                   // 관에 누워있는 데드비츠 해골
    private Animator casketAnimator;                   // 관을 통째로 옮길 Animator 컴포넌트

    private AudioSource casket_AudioSource;            // 사각관 열리고 닫힐때 사용할 Audio Source 컴포넌트
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    private bool isCasketMove;
    // private bool isCasketMoveDone;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();    // 상호작용을 위한 Interaction_Gimics 할당

        Init();     // 초기화 함수 호출
    }

    private void Init()
    {
        deadBeat = GameObject.FindAnyObjectByType<Calli_DeadBeat>();    // Calli_DeadBeatSkull를 찾아와서 할당
        calliScythe = GameObject.FindAnyObjectByType<Item12CalliopeScythe>().gameObject;    // 칼리 낫을 찾아와서 할당
        casketAnimator = GetComponent<Animator>();                      // 해당 오브젝트가 가진 Animator 컴포넌트 할당
        casket_AudioSource = gameObject.GetComponent<AudioSource>();
        settingGimic = interaction.run_Gimic;   // 세팅을 위한 상태 변경

        Setting_SceneStart();           // 세팅 시작 코루틴 실
    }

    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            // 이미 뚜껑을 열어둔 상태이므로 문열리는 애니메이션 재생
            moveCasket("CasketMove");

            if (ItemManager._instance.inventorySlots[12].GetComponent<Item12CalliopeScythe>().isGetItem)
            {
                calliScythe.SetActive(false);   // 칼리낫을 비활성화 해둠
            }
            else
            {
                calliScythe.SetActive(true);   // 칼리낫을 활성화 해둠
            }
        }
        else
        {
            moveCasket("Idle"); // 가만히 있는 애니메이션 재생
            calliScythe.SetActive(false);   // 칼리낫을 비활성화 해둠
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 데드비츠가 머리를 받았고 관이 아직 안움직였을 경우 실행
        if (deadBeat.IsDeadBeatAlive && !isCasketMove)
        {
            // if (!isCasketMoveDone)
            {
                // isCasketMoveDone = true;
                isCasketMove = true;

                calliScythe.SetActive(true);   // 칼리낫을 활성화 해둠
                moveCasket("CasketMove");      // 관 움직이는 애니메이션 실행

                StartCoroutine(playMoveCasketSound());// 관 뚜경 소리 재생
            }
        }
    }

    // 관움직이는 애니메이션을 재생할 함수
    private void moveCasket(string _clipName)
    {
        casketAnimator.Play(_clipName); // 매개변수로 받은 이름의 애니메이션 재생
    }

    private IEnumerator playMoveCasketSound()
    {
        casket_AudioSource.Play();      // 관 뚜경 소리 재생

        yield return new WaitForSeconds(3f);  // 3초 뒤 정지

        casket_AudioSource.Stop();      // 관 뚜경 소리 재생
    }
}