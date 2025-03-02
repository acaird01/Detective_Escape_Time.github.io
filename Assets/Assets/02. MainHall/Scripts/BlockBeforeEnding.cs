using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockBeforeEnding : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트 변수 모음
    private PlayerCtrl playerCtrl;        // 플레이어
    private AmeSoloTalkManager ameSoloTalkManager;  // 아메 혼잣말을 관리하는 스크립트
    public Interaction_Gimics interact_AmeSoloTalk_Object; // 혼잣말을 이미 했는지 저장하기 위한 interaction gimic
    private TextController textController;  // 텍스트 컨트롤러
    #endregion

    #region 스크립트에서 사용할 변수 모음
    [Header("아메 혼잣말을 작성할 변수")]
    [TextArea] // 인스펙터창에 여러줄 입력이 가능한 영역을 만들어줌
    public string ameSoloTalk_Text;
    #endregion


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        playerCtrl = GameObject.FindAnyObjectByType<PlayerCtrl>();   // PlayerCtrl을 찾아와서 할당
        ameSoloTalkManager = GameObject.FindAnyObjectByType<AmeSoloTalkManager>();  // 아메 혼잣말을 관리하는 스크립트
        textController = playerCtrl.gameObject.GetComponentInChildren<TextController>();    // 텍스트 컨트롤러를 찾아와서 할당

        interact_AmeSoloTalk_Object = this.gameObject.GetComponentInParent<Interaction_Gimics>();   // 부모가 가진 Interaction_Gimics를 찾아와서 할당

        Setting();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Setting()
    {
        if (interact_AmeSoloTalk_Object.run_Gimic)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    // 해당 콜라이더를 통과할 경우
    private void OnCollisionEnter(Collision collision)
    {
        playerCtrl = collision.gameObject.GetComponentInChildren<PlayerCtrl>();  // other의 자식이 가진 TextController 컴포넌트 획득 시도

        // 들어온 대상이 플레이어인 경우, 아메가 혼잣말을 하고 있지 않은 경우에만 실행
        if ((playerCtrl != null))
        {
            StartCoroutine(textController.SendText(ameSoloTalk_Text));  // 출력
        }
    }
}