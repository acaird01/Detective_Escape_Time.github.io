using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmeSoloTalk : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트 변수 모음
    private PlayerCtrl playerCtrl;        // 플레이어
    private AmeSoloTalkManager ameSoloTalkManager;  // 아메 혼잣말을 관리하는 스크립트
    private Interaction_Gimics interact_AmeSoloTalk_Object; // 혼잣말을 이미 했는지 저장하기 위한 interaction gimic
    #endregion

    #region 스크립트에서 사용할 변수 모음
    [Header("아메 혼잣말을 작성할 변수")]
    [TextArea] // 인스펙터창에 여러줄 입력이 가능한 영역을 만들어줌
    public string ameSoloTalk_Text;
    private bool isAmeSoloTalkStart;    // 아메 혼잣말을 시작했는지 확인하기 위한 변수
    #endregion


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        playerCtrl = GameObject.FindAnyObjectByType<PlayerCtrl>();   // PlayerCtrl을 찾아와서 할당
        ameSoloTalkManager = GameObject.FindAnyObjectByType<AmeSoloTalkManager>();  // 아메 혼잣말을 관리하는 스크립트

        interact_AmeSoloTalk_Object = this.gameObject.GetComponentInParent<Interaction_Gimics>();   // 부모가 가진 Interaction_Gimics를 찾아와서 할당

        isAmeSoloTalkStart = false;     // 아메라 혼잣말을 하고 있지 않다고 변경

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
    private void OnTriggerEnter(Collider other)
    {
        playerCtrl = other.GetComponentInChildren<PlayerCtrl>();  // other의 자식이 가진 TextController 컴포넌트 획득 시도

        // 들어온 대상이 플레이어인 경우, 아메가 혼잣말을 하고 있지 않은 경우에만 실행
        if ((playerCtrl != null) && !isAmeSoloTalkStart)
        {
            isAmeSoloTalkStart = true;  // 추가로 실행되는 현상 방지용

            if (ameSoloTalkManager.Coroutine_running)
            {
                StopCoroutine("waitAndDeactivateAmeSoloTalk");
                ameSoloTalkManager.Coroutine_running = false;
            }

            // 아메의 혼잣말을 출력할 코루틴 실행
            // StartCoroutine(ameSoloTalkManager.activeAmeSoloTalk(ameSoloTalk_Text));
            ameSoloTalkManager.activeAmeSoloTalk(ameSoloTalk_Text);

            interact_AmeSoloTalk_Object.run_Gimic = true;   // 혼잣말을 이미 진행했으므로 true로 변경()

            this.gameObject.SetActive(false);   // 해당 콜라이더를 비활성화 시켜 두번 출력이 안되도록 막음.
        }
    }
}