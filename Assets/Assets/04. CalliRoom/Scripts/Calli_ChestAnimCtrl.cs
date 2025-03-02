using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_ChestAnimCtrl : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private GameObject item_match;                      // ��� ������ �Ϸ��ϸ� �������� ������(����, 27)

    [Header("���� �Ѳ� ���� ������Ʈ")]
    [SerializeField]
    private GameObject Chest_Lid;                       // ���� �Ѳ� ���� ������Ʈ
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    private bool isChestUnlock;
    public bool IsChestUnlock
    {
        set
        {
            isChestUnlock = value;
        }
    }
    // private bool isChestOpen;
    private float rotationDuration = 0.5f; // ȸ���� �ɸ��� �ð�
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");                                             // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();               // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�

    }

    // �ʱ�ȭ �Լ� (calli_ChestUnlock���� ȣ��)
    public void Init(bool _isChestUnlock)
    {
        interaction = GetComponent<Interaction_Gimics>();   // �ڽſ��� �پ��ִ� interaction gimic�� �Ҵ�
        item_match = this.GetComponentInChildren<Item27Matches>().gameObject;    // ���� �������� ã�ƿͼ� �Ҵ�

        settingGimic = interaction.run_Gimic;   // ��� ���� ���� �ʱ�ȭ
        isChestUnlock = _isChestUnlock;      // ���ڸ� ���� ���� ���ٰ� ����

        Setting_SceneStart();       // ���̺� �����Ϳ� ���� ��� ����
    }

    // ���̺� �����Ϳ� ���� �������� ��� ����
    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            // ���� ���� �Լ� ȣ��
            StartCoroutine(ChestOpen()); // ���� ���� �Լ� ȣ��
            item_match.SetActive(true); // ������ Ȱ��ȭ ����
        }
        else
        {
            // ����� ���� �������� �ʾ����� setgimic�� false
            item_match.SetActive(false); // ������ ��Ȱ��ȭ ����
        }
    }

    IEnumerator ChestOpen() // ��� �۵����� �� 
    {
        item_match.SetActive(true); // ������ Ȱ��ȭ ����

        Quaternion startRotation = Chest_Lid.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(-110, 0, 0);

        //        audioSource.Play();
        float elapsedTime = 0;
        while (elapsedTime < rotationDuration)
        {
            Chest_Lid.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Chest_Lid.transform.rotation = endRotation;

        yield return new WaitForSeconds(.1f);
    }

    /// <summary>
    /// ���ڸ� �����ֱ� ���� �ڷ�ƾ �Լ� ����� �Լ�
    /// </summary>
    public void OpenChest()
    { 
        StartCoroutine(ChestOpen());
    }
}
