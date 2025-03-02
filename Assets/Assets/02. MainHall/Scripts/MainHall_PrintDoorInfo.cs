using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainHall_PrintDoorInfo : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    private GameObject player;                                  // �÷��̾� ������Ʈ
    private MainHall_ObjectManager mainHall_ObjectManager;      // �������� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;                     // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private PlayerCtrl playerCtrl;                              // �÷��̾� ��Ʈ�� ��ũ��Ʈ

    private GuraScene_1 guraScene_1;                            // 1ȸ�� ��������� �Ѿ ��
    private GuraScene_2 guraScene_2;                            // 2ȸ�� ��������� �Ѿ ��
    private CalliScene_1 calliScene_1;                          // 1ȸ�� Į�������� �Ѿ ��
    private CalliScene_2 calliScene_2;                          // 2ȸ�� Į�������� �Ѿ ��
    private KiaraScene_1 kiaraScene_1;                          // 1ȸ�� Ű�ƶ������ �Ѿ ��
    private KiaraScene_2 kiaraScene_2;                          // 2ȸ�� Ű�ƶ������ �Ѿ ��

    private Canvas doorInfo_Canvas;                             // ���� ������ ����� Canvas
    private Text doorInfo_Text;                                 // ���� ������ ����� Text
    #endregion

    #region ��ũ��Ʈ���� ����� ���� ����
    private bool isPrintingText;    // �ؽ�Ʈ�� ����ְ� �ִ��� Ȯ���ϱ� ���� bool ����
    private string doorName;        // ���� �̸��� Ȯ���� ����
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        player = GameObject.Find("Player"); // ���̾��Ű���� �÷��̾ ã�ƿͼ� �Ҵ�
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();     // ���� ���� �ִ� ������Ʈ �Ŵ����� �Ҵ�
        interaction = GetComponent<Interaction_Gimics>();   // �ش� ������Ʈ�� �޷��ִ� interaction Gimic�� �����ͼ� �Ҵ�

        playerCtrl = player.GetComponent<PlayerCtrl>();

        // Init();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// �ش� ������ ����� ������ �Ҵ� �� �ʱ�ȭ�ϱ� ���� �Լ�(������Ʈ �Ŵ������� ����)
    /// </summary>
    public void Init()
    {
        doorInfo_Canvas = GameObject.FindAnyObjectByType<MainHall_DoorGuideCanvas>().GetComponentInChildren<Canvas>();    // ���� ������ ����� Canvas�� ã�ƿͼ� �Ҵ�
        doorInfo_Text = doorInfo_Canvas.GetComponentInChildren<Text>();     // ������ �Ѿ ���� ��縦 ����� �ؽ�Ʈ

        // 1ȸ�� ����� ����� ���� ã�ƿͼ� �Ҵ�
        guraScene_1 = GameObject.FindAnyObjectByType<GuraScene_1>();
        calliScene_1 = GameObject.FindAnyObjectByType<CalliScene_1>();
        kiaraScene_1 = GameObject.FindAnyObjectByType<KiaraScene_1>();
        // 2ȸ�� ����� ����� ���� ã�ƿͼ� �Ҵ�
        guraScene_2 = GameObject.FindAnyObjectByType<GuraScene_2>();
        calliScene_2 = GameObject.FindAnyObjectByType<CalliScene_2>();
        kiaraScene_2 = GameObject.FindAnyObjectByType<KiaraScene_2>();

        doorInfo_Canvas.enabled = false; // ĵ���� ��Ȱ��ȭ

        doorName = this.gameObject.name;    // �ش� ���� �̸��� ����
    }

    private void Update()
    {
        // ���� ��������� �ʰ� �� ��ó�� ������ ��쿡�� ����
        if (!isPrintingText && interaction.IsActiveF)
        {
            // �κ��丮�� ����â�� �������� �������� ����
            if (!playerCtrl.invOpen || !playerCtrl.escOpen)
            {
                isPrintingText = true;          // ������� ���·� ����
                doorInfo_Canvas.enabled = true; // ĵ���� Ȱ��ȭ

                if (String.Equals(doorName, guraScene_1.name))
                {
                    // �����1
                    if (ItemManager._instance.inventorySlots[6].GetComponent<IItem>().isGetItem)    // Ÿ�ڸ� ȹ������ ���
                    { 
                        // �̰� �Լ��� ó���ϴ°� ��������
                    }
                    doorInfo_Text.text = "���� �ٴ� ������ ���� �͸� ����.\n����� ������, �Ƹ�?";
                }
                else if (String.Equals(doorName, calliScene_1.name))
                {
                    // Į����1
                    doorInfo_Text.text = "���� �������ѵ�.. �����̶� �����ִ°� �ƴұ�..?\n���� ����� ������, �Ƹ�?";
                }
                else if (String.Equals(doorName, kiaraScene_1.name))
                {
                    // Ű�ƶ��1
                    doorInfo_Text.text = "��~ ���ִ� ������ ��.\n����� ������, �Ƹ�?";
                }
                else if (String.Equals(doorName, guraScene_2.name))
                {
                    // �����2
                    doorInfo_Text.text = "���� ��� �񸰳��� ���� �͸� ����.\n����� ������?";
                }
                else if (String.Equals(doorName, calliScene_2.name))
                {
                    // Į����2
                    doorInfo_Text.text = "�и� ��¥ ������ �־���..\n...��..����� ������?";
                }
                else if (String.Equals(doorName, kiaraScene_2.name))
                {
                    // Ű�ƶ��2
                    doorInfo_Text.text = "����..�۷��ٸ� �ƴϿ��..!\n����..����� ������?";
                }
                else
                {
                    // �߸��� ���� ��ũ��Ʈ�� �޸�
                    Debug.Log("�߸��� ���� ��ũ��Ʈ�� �޸�(����)");
                }
            }
            else
            {
                isPrintingText = false;          // ����� ���·� ����
                doorInfo_Canvas.enabled = false; // ĵ���� ��Ȱ��ȭ
            }
        }
        else if (isPrintingText && !interaction.IsActiveF)   // ������̿��ٰ� ������ �־����� ��� ����
        {
            isPrintingText = false;          // ����� ���·� ����
            doorInfo_Canvas.enabled = false; // ĵ���� ��Ȱ��ȭ
        }
        else
        {
            // isPrintingText && interaction.IsActiveF // �Ѵ� true true�� ��� <- �ϰ͵� ���ص� ��
            // !isPrintingText && !interaction.IsActiveF // �Ѵ� false false�� ��� <- �ϰ͵� ���ص� ��
            // Debug.Log("�� ��ũ��Ʈ ����� ����ġ ���� ���� �߻�(�̸�) : " + isPrintingText + "/" + interaction.IsActiveF + "(" + doorName + ")");
        }

        // ���� �κ��丮�� ����â�� �������� ����
        if (playerCtrl.invOpen || playerCtrl.escOpen)
        {
            isPrintingText = false;          // ����� ���·� ����
            doorInfo_Canvas.enabled = false; // ĵ���� ��Ȱ��ȭ
        }
    }
}
