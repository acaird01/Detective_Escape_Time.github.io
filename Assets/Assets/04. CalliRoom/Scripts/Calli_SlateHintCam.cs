using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Calli_SlateHintCam : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트
    private Canvas playerUI;                            // 플레이어의 인벤토리 등 Ui

    private PlayerCtrl playerctrl;                      // 플레이어 컨트롤 스크립트
    private Camera player_MainCamera;                   // 플레이어의 메인 카메라
    private Camera slateHintCamera;                     // 석판 힌트를 볼 카메라
    private BoxCollider boxCollider;                    // 석판의 boxcollider
    private string FirstInteraction_Text = "뭔가 쓰여 있는 것 같다.";    // 처음 상호작용 했을 때 출력해줄 텍스트
    private Button exitButton;                          // 확대해서 보는거에서 돌아가기 위해
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        // 컴포넌트 할당
        player = GameObject.Find("Player");
        player_MainCamera = player.GetComponentInChildren<Camera>();
        playerctrl = player.GetComponent<PlayerCtrl>();
        playerUI = player.GetComponentInChildren<Canvas>();                 // player의 자식으로 있는 canvas를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();
        slateHintCamera = gameObject.GetComponentInChildren<Camera>();
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        exitButton = gameObject.GetComponentInChildren<Button>();
        boxCollider = GetComponent<BoxCollider>();

        // 변수 및 컴포넌트 초기화
        exitButton.gameObject.SetActive(false);
        slateHintCamera.gameObject.SetActive(false);

        StartCoroutine(WaitTouch());    // 대기 코루틴 출력
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        // 캔버스가 활성화 되어 있는 상태인 경우
        if (slateHintCamera.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitButton();
                return;
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                openCanavs();
                return;
            }
        }
    }

    void FirstInteraction() // 처음 한번만 상호작용시 실행될 텍스트 추가된 코루틴
    {
        StartCoroutine(textController.SendText(FirstInteraction_Text)); // 첫 상호작용시에만 텍스트 출력
        boxCollider.enabled = false;
        playerctrl.keystrokes = true;

        playerUI.enabled = false;   // 플레이어 UI를 꺼줌

        textController.SetActiveFalseText();
        slateHintCamera.gameObject.SetActive(true);
        player_MainCamera.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        interaction.run_Gimic = false;
    }


    IEnumerator WaitTouch() // 석판 상호작용 대기 코루틴
    {
        if (interaction.run_Gimic == false)
        {
            yield return new WaitUntil(() => interaction.run_Gimic == true);
            playerctrl.keystrokes = true;

            FirstInteraction();

            interaction.run_Gimic = false;
        }
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (!slateHintCamera.gameObject.activeSelf)
                {
                    openCanavs();
                }

                interaction.run_Gimic = false;
            }
            else
            {
                interaction.run_Gimic = false;
            }
        }
    }

    // 캔버스 여는걸 막아줄 함수
    private void openCanavs()
    {
        boxCollider.enabled = false;
        playerctrl.keystrokes = true;

        playerUI.enabled = false;   // 플레이어 UI를 꺼줌

        textController.SetActiveFalseText();
        slateHintCamera.gameObject.SetActive(true);
        player_MainCamera.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        interaction.run_Gimic = false;
    }

    public void ExitButton() // 돌아가기 버튼
    {
        playerUI.enabled = true;   // 플레이어 UI를 켜줌

        slateHintCamera.gameObject.SetActive(false);
        boxCollider.enabled = true;
        player_MainCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        playerctrl.keystrokes = false;
        exitButton.gameObject.SetActive(false);
    }
}
