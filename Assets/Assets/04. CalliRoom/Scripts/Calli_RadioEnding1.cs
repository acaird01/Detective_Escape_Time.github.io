using System.Collections;
using UnityEngine;

public class Calli_RadioEnding1 : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    [Header("기믹이 끝나면 획득할 아이템(열쇠)")]
    [SerializeField]
    private Item34ChestKey item34ChestKey;
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private string interactionText;            // 상호작용 시 출력할 대사
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
        interaction = GetComponent<Interaction_Gimics>();   // 자신에게 붙어있는 interaction gimic을 할당

        StartCoroutine(WaitTouch());    // 상호작용 대기 코루틴 호출
    }

    // 상호작용 대기 코루틴 함수
    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에

            // 열쇠가 활성화되어 있다면 기믹 수행이 완료되었으므로 해당 대사 출력
            if (item34ChestKey.gameObject.activeSelf)
            {
                interactionText = "다행히 라디오를 부술 필요는 없었네.\n이 열쇠로 잠겨있던 상자를 열 수 있지 않을까?";      // 상호작용 시 출력할 대사 설정
                StartCoroutine(textController.SendText(interactionText));   // 상호작용 대사 출력
            }
            else
            {
                interactionText = "위에 있는 글자가 테이프 자리 힌트 것 같은걸.\n한 번 노래 제목 앞 글자에랑 맞춰서 둬볼까?";      // 상호작용 시 출력할 대사 설정
                StartCoroutine(textController.SendText(interactionText));   // 상호작용 대사 출력
            }
            
            interaction.run_Gimic = false;
        }
    }
}
