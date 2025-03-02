using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_AmeOnChair : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private AudioSource chair_SFX;                      // 아메가 의자 위에 올라섰을때 효과음을 출력할 AudioSource
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    // private bool isAmeOnChair;       // 아메가 의자 위에 올라갔는지 확인하기 위한 bool 변수
    private float chairSoundLength;  // 의자에 앉을 때 나올 소리 길이
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");                                             // 플레이어를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();               // 플레이어의 자식에서 TextController를 찾아와서 할당

        Init(); // 초기화 함수 호출
    }

    // 초기화 함수 호출
    private void Init()
    {
        chair_SFX = GetComponent<AudioSource>();    // 가지고 있는 AudioSource를 할당

        // isAmeOnChair = false;     // 현재 안보이는 상태로 설정
        // chairSoundLength = (chair_SFX.clip.length / 3) * 2; // 유령소리를 2/3길이로 조정

        StartCoroutine(WaitTouch());                    // 상호작용 대기 코루틴 함수 실행
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 상호작용되기 전까지 대기할 코루틴 함수
    private IEnumerator WaitTouch()
    {
        while (player) // 한번하고 뽀사지면 이거빼고, 창문 열고 닫는거처럼 반복필요하면 이거 넣고 쓰기
        {
            // this.GetComponentInChildren<ParticleSystem>().Play();

            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // 생성되고난뒤 여기서 대기하다가

            AmeSitChair();

            interaction.run_Gimic = false;
        }
    }

    private void AmeSitChair()
    {
        // 의자에 올라서는 사운드 출력
        player.transform.position = this.gameObject.transform.position + (Vector3.up * 1f);   // 아메의 위치를 의자 위쪽으로 이동
        player.transform.rotation = Quaternion.Euler(0f, -90f, 0f);     // 힌트가 있는 책상을 쳐다보게끔 방향 지정
    }
}
