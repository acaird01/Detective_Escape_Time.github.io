using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Calli_DialCameraCtrl : MonoBehaviour
{
    GameObject player;
    PlayerCtrl playerctrl;
    Camera player_MainCamera;
    Camera dialCamera;
    Interaction_Gimics interaction;
    TextController textController;

    BoxCollider boxCollider;

    string FirstInteraction_Text = "이제 자물쇠를 돌릴 수 있을 것 같아.";
    Button exitButton; // 확대해서 보는거에서 돌아가기 위해
    Calli_DialLockScene2 dialLock;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        player = GameObject.Find("Player");
        player_MainCamera = player.GetComponentInChildren<Camera>();
        playerctrl = player.GetComponent<PlayerCtrl>();
        textController = player.GetComponentInChildren<TextController>();
        dialCamera = gameObject.GetComponentInChildren<Camera>();
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        exitButton = gameObject.GetComponentInChildren<Button>();
        boxCollider = GetComponent<BoxCollider>();
        dialLock = gameObject.GetComponentInChildren<Calli_DialLockScene2>();
        exitButton.gameObject.SetActive(false);
        dialCamera.gameObject.SetActive(false);
        StartCoroutine(WaitTouch());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    IEnumerator FirstInteraction() // 처음 한번만 상호작용시 실행될 텍스트 추가된 코루틴
    {
        StartCoroutine(textController.SendText(FirstInteraction_Text)); // 첫 상호작용시에만 텍스트 출력
        boxCollider.enabled = false;
        playerctrl.keystrokes = true;
        yield return new WaitForSecondsRealtime(1f);
        textController.SetActiveFalseText();
        dialCamera.gameObject.SetActive(true);
        player_MainCamera.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        interaction.run_Gimic = false;
    }


    IEnumerator WaitTouch() 
    {
        boxCollider.enabled = false;
        yield return new WaitUntil(() => dialLock.fixClock_Check == true);
        boxCollider.enabled = true;

        if (interaction.run_Gimic == false)
        {
            yield return new WaitUntil(() => interaction.run_Gimic == true);
            playerctrl.keystrokes = true;
            StartCoroutine(FirstInteraction());
        }
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (!dialCamera.gameObject.activeSelf)
                {
                    dialLook();
                }
            }
            else
            {
                interaction.run_Gimic = false;
            }
        }
    }

    private void dialLook()
    {
        boxCollider.enabled = false;
        dialCamera.gameObject.SetActive(true);
        player_MainCamera.gameObject.SetActive(false);
        playerctrl.keystrokes = true;
        textController.SetActiveFalseText();
        exitButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        interaction.run_Gimic = false;
    }

    public void ExitButton() // 돌아가기 버튼
    {
        dialCamera.gameObject.SetActive(false);
        player_MainCamera.gameObject.SetActive(true);
        boxCollider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        playerctrl.keystrokes = false;
        exitButton.gameObject.SetActive(false);
        Time.timeScale = 1;

        interaction.run_Gimic = false;
    }
}
