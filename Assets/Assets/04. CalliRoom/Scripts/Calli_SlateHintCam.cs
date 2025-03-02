using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Calli_SlateHintCam : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ
    private Canvas playerUI;                            // �÷��̾��� �κ��丮 �� Ui

    private PlayerCtrl playerctrl;                      // �÷��̾� ��Ʈ�� ��ũ��Ʈ
    private Camera player_MainCamera;                   // �÷��̾��� ���� ī�޶�
    private Camera slateHintCamera;                     // ���� ��Ʈ�� �� ī�޶�
    private BoxCollider boxCollider;                    // ������ boxcollider
    private string FirstInteraction_Text = "���� ���� �ִ� �� ����.";    // ó�� ��ȣ�ۿ� ���� �� ������� �ؽ�Ʈ
    private Button exitButton;                          // Ȯ���ؼ� ���°ſ��� ���ư��� ����
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        // ������Ʈ �Ҵ�
        player = GameObject.Find("Player");
        player_MainCamera = player.GetComponentInChildren<Camera>();
        playerctrl = player.GetComponent<PlayerCtrl>();
        playerUI = player.GetComponentInChildren<Canvas>();                 // player�� �ڽ����� �ִ� canvas�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();
        slateHintCamera = gameObject.GetComponentInChildren<Camera>();
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        exitButton = gameObject.GetComponentInChildren<Button>();
        boxCollider = GetComponent<BoxCollider>();

        // ���� �� ������Ʈ �ʱ�ȭ
        exitButton.gameObject.SetActive(false);
        slateHintCamera.gameObject.SetActive(false);

        StartCoroutine(WaitTouch());    // ��� �ڷ�ƾ ���
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        // ĵ������ Ȱ��ȭ �Ǿ� �ִ� ������ ���
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

    void FirstInteraction() // ó�� �ѹ��� ��ȣ�ۿ�� ����� �ؽ�Ʈ �߰��� �ڷ�ƾ
    {
        StartCoroutine(textController.SendText(FirstInteraction_Text)); // ù ��ȣ�ۿ�ÿ��� �ؽ�Ʈ ���
        boxCollider.enabled = false;
        playerctrl.keystrokes = true;

        playerUI.enabled = false;   // �÷��̾� UI�� ����

        textController.SetActiveFalseText();
        slateHintCamera.gameObject.SetActive(true);
        player_MainCamera.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        interaction.run_Gimic = false;
    }


    IEnumerator WaitTouch() // ���� ��ȣ�ۿ� ��� �ڷ�ƾ
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

    // ĵ���� ���°� ������ �Լ�
    private void openCanavs()
    {
        boxCollider.enabled = false;
        playerctrl.keystrokes = true;

        playerUI.enabled = false;   // �÷��̾� UI�� ����

        textController.SetActiveFalseText();
        slateHintCamera.gameObject.SetActive(true);
        player_MainCamera.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        interaction.run_Gimic = false;
    }

    public void ExitButton() // ���ư��� ��ư
    {
        playerUI.enabled = true;   // �÷��̾� UI�� ����

        slateHintCamera.gameObject.SetActive(false);
        boxCollider.enabled = true;
        player_MainCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        playerctrl.keystrokes = false;
        exitButton.gameObject.SetActive(false);
    }
}
