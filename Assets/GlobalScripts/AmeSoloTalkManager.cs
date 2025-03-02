using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmeSoloTalkManager : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트 변수 모음
    private Image TextParent;               // 대사를 출력할 이미지
    private Text Interaction_Text;          // 대사를 출력할 텍스트
    private Image interactionTextESCImage;  // 대사를 출력할 이미지를 꺼줄 버튼 이미지
    private Text interactionTextESCText;    // 대사를 출력할 이미지를 꺼줄 버튼 텍스트

    private TextController textCtrl;  // 플레이어 TextController
    #endregion

    #region 스크립트에서 사용할 변수 모음
    private bool coroutine_running;     // 코루틴이 실행중인지 확인하기 위한 변수
    public bool Coroutine_running       // 코루틴이 실행중인지 확인할 변수에 값을 할당하기 위한 프로퍼티
    {
        get
        { 
            return coroutine_running;
        }
        set
        {
            coroutine_running = value;
        }
    }
    // private bool isTalking;
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        textCtrl = GameObject.FindAnyObjectByType<TextController>();   // PlayerCtrl을 찾아와서 할당

        TextParent = textCtrl.TextParent;              // TextController에서 찾아둔 상호작용 Text UI의 부모를 할당
        Interaction_Text = textCtrl.Interaction_Text;   // TextController에서 찾아둔 상호작용 Text UI를 할당

        // TextParent의 자식으로 있는 esc버튼 안내 이미지를 꺼주기 위해 찾아옴
        interactionTextESCImage = TextParent.GetComponentInChildren<InteractionTextESCImage>().GetComponent<Image>();
        interactionTextESCText = TextParent.GetComponentInChildren<InteractionTextESCText>().GetComponent<Text>();

        coroutine_running = false;  // 현재 실행중이지 않다고 설정
        // isTalking = false;          // 현재 말하고 있지 않다고 설정
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 아메 혼잣말을 출력시킬 코루틴
    /// </summary>
    /// <param 아메혼잣말="_ameSoloTalk_Text"></param>
    /// <returns></returns>
    public void activeAmeSoloTalk(string _ameSoloTalk_Text)
    {
        if (TextParent) // 
        {
            if(textCtrl.coroutine_running == true)
            {
                textCtrl.SetActiveFalseText();
            }
            coroutine_running = true;
            // isTalking = true;

            TextParent.gameObject.SetActive(true);
            Interaction_Text.text = _ameSoloTalk_Text;

            // ESC 버튼 안내를 꺼줌
            interactionTextESCImage.enabled = false;
            interactionTextESCText.enabled = false;

            StartCoroutine(waitAndDeactivateAmeSoloTalk()); // 비활성화 대기 코루틴 실행
        }
    }

    private IEnumerator waitAndDeactivateAmeSoloTalk()
    {
        yield return new WaitForSecondsRealtime(2f);    // 2초 대기

        // ESC 버튼 안내를 켜줌
        interactionTextESCImage.enabled = true;
        interactionTextESCText.enabled = true;

        if (TextParent.gameObject.activeSelf)
        {
            TextParent.gameObject.SetActive(false);
        }
        

        // isTalking = false;
        coroutine_running = false;
    }
}
