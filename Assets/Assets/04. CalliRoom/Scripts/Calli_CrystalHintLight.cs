using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_CrystalHintLight : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    [Header("수정의 배치 순서를 보여줄 빛 오브젝트(힌트 순서대로 추가)")]
    [SerializeField]
    private GameObject[] crystalHintLights;                  // 수정의 배치 순서를 보여줄 빛을 저장할 배열
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool isHintPlay;    // 현재 힌트가 제공 중인지 확인하기 위한 변수
    WaitForSeconds waitOneSeconds = new WaitForSeconds(1f); // 1초간 대기 시킬때 사용할 변수
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

    private void Init()
    {
        isHintPlay = false;   // 현재 힌트가 제공 중인지 확인하기 위한 변수

        // 힌트 제공 전이니 빛을 꺼줌
        for (int i = 0; i < crystalHintLights.Length; i++)
        {
            crystalHintLights[i].SetActive(false);
        }

        StartCoroutine(WaitTouch());    // 상호작용 대기함수 호출
    }

    // 상호작용되기 전까지 대기할 코루틴 함수
    private IEnumerator WaitTouch()
    {
        while (player) // 한번하고 뽀사지면 이거빼고, 창문 열고 닫는거처럼 반복필요하면 이거 넣고 쓰기
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // 생성되고난뒤 여기서 대기하다가

            // 기믹 실행이 참인 경우
            if (interaction.run_Gimic && !isHintPlay)
            {
                isHintPlay = true;    // 힌트 재생을 시작했다고 변경(추가 반복을 막음)

                // 수정 배치 힌트 빛을 켜주고 효과음 재생
                for (int i = 0; i < crystalHintLights.Length; i++)
                {
                    crystalHintLights[i].SetActive(true);    // 빛 활성화
                    crystalHintLights[i].GetComponent<AudioSource>().Play();    // 효과음 재생

                    yield return waitOneSeconds;    // 1초간 대기

                    crystalHintLights[i].SetActive(false);    // 빛 비활성화
                }

                isHintPlay = false;    // 힌트 재생이 완료되었으므로 변경해줌
                interaction.run_Gimic = false;
            }
            else
            {
                interaction.run_Gimic = false; // 다시 상호작용할 수 있도록 상태 변경
            }
        }
    }
}
