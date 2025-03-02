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

    string FirstInteraction_Text = "���� �ڹ��踦 ���� �� ���� �� ����.";
    Button exitButton; // Ȯ���ؼ� ���°ſ��� ���ư��� ����
    Calli_DialLockScene2 dialLock;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
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



    IEnumerator FirstInteraction() // ó�� �ѹ��� ��ȣ�ۿ�� ����� �ؽ�Ʈ �߰��� �ڷ�ƾ
    {
        StartCoroutine(textController.SendText(FirstInteraction_Text)); // ù ��ȣ�ۿ�ÿ��� �ؽ�Ʈ ���
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

    public void ExitButton() // ���ư��� ��ư
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
