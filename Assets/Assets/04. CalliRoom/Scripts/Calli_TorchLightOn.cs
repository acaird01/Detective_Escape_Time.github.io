using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_TorchLightOn : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private Light torch_Light;                          // ���ɰ� ��ȣ�ۿ��ϸ� ���� ���� Light ������Ʈ
    private GameObject takoPosHint;                     // ȶ�� ����� Ǯ������ ������ Ÿ�� ��ġ ��Ʈ
    private Calli_TorchGimic calli_TorchGimic;          // ȶ�� ����� Ǯ������ ��Ʈ�� ������ ��ũ��Ʈ
    private GameObject torchWall_Prefab;                // ���밡 �ɷ��ִ� ȶ�� ������
    private Calli_NoTorchWall calli_NoTorchWall;        // ȶ�븸 �ִ� �������� ã�ƿ� ��ũ��Ʈ
    #endregion

    #region ����� ����
    private AudioSource matchLightOnSound_AudioSource;   // ������ �̿��� ȶ���� ���� �� �Ҹ��� ����� AudioSource
    [Header("���ɺ� ����� Ŭ��")]
    [SerializeField]
    private AudioClip matchLight_AudioClip;
    [Header("ȶ�� ��ġ�� ����� Ŭ��")]
    [SerializeField]
    private AudioClip torchSet_AudioClip;
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����(ȶ�� ����Ⱑ Ȱ��ȭ �Ǿ����� Ȯ�ο�)
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private bool isTakoPosActive;               // Ÿ�� ��ġ ��Ʈ�� Ȱ��ȭ �Ǿ����� Ȯ���ϱ� ���� bool ����
    #endregion

    /// <summary>
    /// Calli_TorchLightOn �ʱ�ȭ �Լ�(Calli_TorchGimic���� ����)
    /// </summary>
    public void Init()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;               // �÷��̾ ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();                         // ��ȣ�ۿ�� ��縦 ����� ��ũ��Ʈ

        torchWall_Prefab = GetComponentInChildren<Calli_TorchWall>().gameObject;  // ȶ�� ���븸 �ɸ� ȶ�� ������ �Ҵ�
        torch_Light = GetComponentInChildren<Light>();      // ȶ���� �ڽ��� ������ �ִ� Light ������Ʈ�� ã�ƿͼ� �Ҵ�
        matchLightOnSound_AudioSource = GetComponent<AudioSource>();  // ȶ���� ���� �� �Ҹ��� ����� AudioSource
        calli_TorchGimic = GameObject.FindAnyObjectByType<Calli_TorchGimic>();  // ȶ�� ����� Ǯ������ ��Ʈ�� ������ ��ũ��Ʈ
        takoPosHint = calli_TorchGimic.takoPosHint; // ȶ�� ����� Ǯ������ ������ Ÿ�� ��ġ ��Ʈ�� ã�ƿͼ� �Ҵ�
        calli_NoTorchWall = GameObject.FindAnyObjectByType<Calli_NoTorchWall>();    // ȶ�븸 �ִ� ������

        settingGimic = calli_NoTorchWall.GetComponent<Interaction_Gimics>().run_Gimic;       // ��� ���� ���θ� Ȯ���ؼ� ����
        isTakoPosActive = false;        // ���� �������� ���� �ʴٰ� �ʱ�ȭ

        Setting_SceneStart();   // ���� �Լ� ȣ��
    }

    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            SummonTorch();      // ȶ���� ��ȯ�ص�.

            if (takoPosHint.activeSelf)
            {
                torch_Light.gameObject.SetActive(true);     // ��Ʈ�� �̹� �������� �����Ƿ� ���� ����
                isTakoPosActive = true;         // �̹� �������� �ִٰ� ���� ����
            }
            else
            {
                torch_Light.gameObject.SetActive(false);    // ��Ʈ�� �������� ���� �����Ƿ� ���� ����
                isTakoPosActive = false;        // ���� �������� ���� �ʴٰ� ���� ����

                StartCoroutine(WaitTouch());    // ��� �ڷ�ƾ �Լ� ȣ��
            }
        }
        else
        {
            // ���밡 �ɷ��ִ� ȶ�븦 ��Ȱ��ȭ ����
            torch_Light.gameObject.SetActive(false);
            torchWall_Prefab.gameObject.SetActive(false);

            StartCoroutine(WaitTouch());    // ��� �ڷ�ƾ �Լ� ȣ��
        }
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // ��� �����ض� ���⿡

            if (ItemManager._instance.hotkeyItemIndex == 25 && !settingGimic)    // ���� ��Ű�� �������� ȶ��(25��)�̰� ���� Ȱ��ȭ ���� �ʾҴٸ�
            {
                // ������ ��� �Լ� ȣ��
                settingGimic = true;    // �̹� ȶ�� ���븦 ��ȯ�ߴٰ� ����(�߰� �ݺ��� ����)
                calli_NoTorchWall.GetComponent<Interaction_Gimics>().run_Gimic = true;  // ����� ���� �Ϸ� �Ǿ��ٰ� ����

                // ȶ�� ����Ⱑ �ɸ��� �Ҹ� ���
                matchLightOnSound_AudioSource.clip = torchSet_AudioClip;    // ȶ�� ��ġ ȿ���� ����
                matchLightOnSound_AudioSource.Play();

                SummonTorch();  // ȶ�� ��ȯ�� �Լ� ȣ��
            }
            else
            {
                if (settingGimic)   // ȶ���� �ɷ��� �ִ� ���
                {
                    if (ItemManager._instance.hotkeyItemIndex == 27 && !isTakoPosActive)    // ���� ��Ű�� �������� ����(27��)�̰� ���� ��Ʈ�� ������ ���� ���� ���
                    {
                        isTakoPosActive = true;     // Ÿ�� ��ġ ��Ʈ�� ������ �ִٰ� ���� ���� (�߰� �ݺ��� ����)

                        // ���Ѵ� �Ҹ� ���
                        matchLightOnSound_AudioSource.clip = matchLight_AudioClip;    // ���ɺ� ȿ���� ����
                        matchLightOnSound_AudioSource.Play();

                        torch_Light.gameObject.SetActive(true); // torch_Light�� Ȱ��ȭ�ؼ� ���� ����

                        calli_TorchGimic.SettingForObjectToInteration = true;   // Calli_TorchGimic�� bool ������(����� ���� �Ǿ��ٰ� ���� ����)

                        ItemManager._instance.ReturnItem(27);
                        ItemManager._instance.DeactivateItem(27);

                        StartCoroutine(calli_TorchGimic.CeilingTakoPos());      // Calli_TorchGimic���� ������ ���� ���� ���ְ� ��Ʈ�� Ȱ��ȭ ��ų �ڷ�ƾ �Լ� ȣ��
                    }
                    else
                    {
                        // ���� ��Ʈ�� Ȱ��ȭ �Ǿ� ���� ��� �̹� ��� ������ ������ �Ϸ�Ǿ����Ƿ� �ش� ��� ���
                        if (takoPosHint.activeSelf)
                        {
                            // StartCoroutine(textController.SendText("������ �����ִ� ȶ��"));      // ��ȣ�ۿ� ��� ���
                            StartCoroutine(textController.SendText("Fire Fire Light On Fire"));      // ��ȣ�ۿ� ��� ���
                        }
                        else  // ��Ȱ��ȭ��� ���� ���ɱ���� �������� �ʾ����Ƿ� �ش� ��� ���
                        {
                            StartCoroutine(textController.SendText("ȶ�� ������̴�. ��� ���� ���ϸ��Ѱ� ������?"));      // ��ȣ�ۿ� ��� ���
                        }
                    }
                }
                else // ȶ�� ���밡 ���� �ɷ� ���� ���� ���
                {
                    StartCoroutine(textController.SendText("����� ���� �� �ɾ�� �� ���� �� ����."));      // ��ȣ�ۿ� ��� ���
                }
            }

            interaction.run_Gimic = false;  // �ٽ� ��ȣ�ۿ� �����ϵ��� ����
        }
    }

    #region ��� �����ϴ� �Լ� ����
    /// <summary>
    /// ȶ�� ����⸦ ��ȯ�� �Լ�
    /// </summary>
    private void SummonTorch()
    {
        // 1ȸ�������� ����
        if (GameManager.instance.Episode_Round == 1)
        {
            // ���൵ ������Ʈ
            GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        }

        // ȶ���� �κ��丮�� �ǵ�����
        ItemManager._instance.ReturnItem(25);
        ItemManager._instance.DeactivateItem(25);

        // ȶ�� ���밡 �ִ� �������� Ȱ��ȭ ������
        torchWall_Prefab.gameObject.SetActive(true);
    }
    #endregion
}
