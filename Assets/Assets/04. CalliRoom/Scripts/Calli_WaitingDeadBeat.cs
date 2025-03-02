using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ������ ����� 2ȸ�� ��Ʈ ���� ����
/// </summary>
public class Calli_WaitingDeadBeat : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private Animator animator;  // �ذ��� �������� �ִϸ�����
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private string interactionText;        // ��ȣ�ۿ� �� ����� ���

    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        player = GameObject.Find("Player");                                             // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = this.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();               // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        //me = this.gameObject;   // ������ ã�ƿͼ� �Ҵ�
        //player = GameObject.Find("Player");                                             // �÷��̾ ã�ƿͼ� �Ҵ�
        //interaction = me.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        //calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        //textController = player.GetComponentInChildren<TextController>();               // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�

        // Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    // �ʱ�ȭ �Լ�(Calli_StoneTableGimic���� ����)
    public void Init(bool _isTapeGimicEnd)
    {
        animator = this.GetComponent<Animator>();   // �ذ��� �������� �ִϸ�����
        settingGimic = _isTapeGimicEnd;   // ��� ���� ���� �ʱ�ȭ

        Setting_SceneStart();       // ���̺� �����Ϳ� ���� ��� ����
    }

    // ���̺� �����Ϳ� ���� �������� ��� ����
    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            interactionText = "�����༭ ����! �ű� ���� ���� ���� ì�ܰ�!";        // ��ȣ�ۿ� �� ����� ��� ����
        }
        else
        {
            interactionText = "Ȥ�� ������ �� ��� ������� ���� �� ������ ������?";        // ��ȣ�ۿ� �� ����� ��� ����
        }

        StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ��� �ڷ�ƾ ȣ��
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // ��� �����ض� ���⿡

            if (interaction.run_Gimic && settingGimic)
            {
                interactionText = "�����༭ ����! �ű� ���� ���� ���� ì�ܰ�!";        // ��ȣ�ۿ� �� ����� ��� ����
                StartCoroutine(textController.SendText(interactionText));   // ��ȣ�ۿ� ��� ���

                interaction.run_Gimic = false;
            }
            else
            {
                interactionText = "Ȥ�� ������ �� ��� ������� ���� �� ������ ������?";        // ��ȣ�ۿ� �� ����� ��� ����
                StartCoroutine(textController.SendText(interactionText));   // ��ȣ�ۿ� ��� ���

                interaction.run_Gimic = false;
            }
        }
    }
}
