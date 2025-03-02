using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Calli_ChestUnlock : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private Calli_ChestAnimCtrl chestAnimation_Ctrl;    // ���ڸ� �������� ��ũ��Ʈ

    [SerializeField]
    private GameObject Hinzi;   // ���� ����
    [Header("���ڰ� ���� �ȿ������� ����� �Ҹ�")]
    [SerializeField]
    private AudioClip beforeChestOpen;
    [Header("���ڰ� ������ ����� �Ҹ�")]
    [SerializeField]
    private AudioClip afterChestOpen;

    private AudioSource audioSource;        // ����Ʈ �����Ҷ� ����� audio source
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    private const string needItemName = "Item_34_ChestKey";   // ��� ������ �ʿ��� ������ �̸�
    private bool isUnlock;          // �ڹ��谡 ��������� Ȯ���ϱ� ���� ����
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");                                             // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();               // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�

        Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    // �ʱ�ȭ �Լ�
    private void Init()
    {
        interaction = GetComponent<Interaction_Gimics>();   // �ڽſ��� �پ��ִ� interaction gimic�� �Ҵ�
        chestAnimation_Ctrl = GameObject.FindAnyObjectByType<Calli_ChestAnimCtrl>();    // ���ڿ��µ� ����� ��ũ��Ʈ ã�ƿͼ� �Ҵ�
        audioSource = this.GetComponent<AudioSource>();              // ����� audio source�� �Ҵ�

        settingGimic = interaction.run_Gimic;   // ��� ���� ���� �ʱ�ȭ

        Setting_SceneStart();       // ���̺� �����Ϳ� ���� ��� ����
    }

    // ���̺� �����Ϳ� ���� �������� ��� ����
    void Setting_SceneStart()
    {
        isUnlock = settingGimic;    // �ڹ��谡 �����Ǿ����� �ƴ��� ����

        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            // ���� �ʱ�ȭ �Լ� ȣ��
            chestAnimation_Ctrl.Init(isUnlock);

            // �ڹ��� �����
            this.gameObject.SetActive(false);
        }
        else
        {
            // ����� ���� �������� �ʾ����� setgimic�� false

            // ���� �ʱ�ȭ �Լ� ȣ��
            chestAnimation_Ctrl.Init(isUnlock);

            StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ��� �ڷ�ƾ ����
        }
    }

    // ��ȣ�ۿ�Ǳ� ������ ����� �ڷ�ƾ �Լ�
    private IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // �����ǰ��� ���⼭ ����ϴٰ�

            // ��� ������ ���� ���
            // �ڹ��谡 ���� ������ �ʾҰ� ���� ��Ű�� ���踦 �����ϰ� �ִ� ���
            if (!isUnlock && string.Equals(ItemManager._instance.hotkeyItemName, needItemName))
            {
                isUnlock = true;                            // �ڹ��踦 �����ߴٰ� ���� ����. �߰� �ݺ��� ����
                chestAnimation_Ctrl.IsChestUnlock = true;     // �ڹ��谡 �����Ǿ����� ���ڸ� ���� �ֵ��� �ϱ� ���� bool�� ����

                chestAnimation_Ctrl.gameObject.GetComponent<BoxCollider>().enabled = false; // ���� ��ü�� �ݶ��̴��� ����
                chestAnimation_Ctrl.OpenChest();

                GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate(); // ��� ���� ������ ȣ��

                // �ڹ��谡 �������Ƿ� ���踦 ��������
                ItemManager._instance.ReturnItem(34);
                ItemManager._instance.DeactivateItem(34);

                this.gameObject.SetActive(false);           // �ڹ��� ��Ȱ��ȭ
            }
            else
            {
                StartCoroutine(textController.SendText("�ڹ���� ����־�.\n���� ī��Ʈ �÷��̾� �ȿ� ������� �ʰ���?"));   // ��ȣ�ۿ� ��� ���

                interaction.run_Gimic = false;  // false�� ����
            }
        }
    }
}
