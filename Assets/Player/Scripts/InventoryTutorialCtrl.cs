using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTutorialCtrl : MonoBehaviour
{
    #region ������Ʈ �� ��Ʈ�ѷ��� �Ҵ��� ���� ����
    private PlayerCtrl player_Ctrl;     // playerCtrl ��ũ��Ʈ
    [Header("�÷��̾��� canvas")]
    [SerializeField]
    private Canvas playerUI;            // �÷��̾ ���� canvas

    [Header("�κ��丮 ������Ʈ")]
    public GameObject Inventory;     // Inventory GameObject
    [Header("Ʃ�丮�� �̹����� ���ִ� ������Ʈ")]
    public GameObject tutorial_Inventory;     // Tutorial_Inventory GameObject
    [Header("Ʃ�丮�� �ȳ� ȭ��ǥ �̹���")]
    public Image arrowImage;    // �κ��丮 ���̵� �̹���
    [Header("Ʃ�丮�� �κ��丮 �̹���")]
    public Image tutorialInventory_Image;    // �κ��丮 ���̵� �̹���
    [Header("Ʃ�丮�� ��� ������ �̹���")]
    public Image turorialGimicItem_Image;    // �κ��丮 ���̵� �̹���
    [Header("Ʃ�丮�� ���� ��ư")]
    public Button nextButton;    // �κ��丮 ���̵� �̹���
    [Header("Ʃ�丮�� ���� ��ư")]
    public Button exitButton;    // �κ��丮 ���̵� �̹���
    #endregion

    private void Start()
    {
        Init(); // �ʱ�ȭ �Լ� ����
    }

    // �ʱ�ȭ �Լ�
    private void Init()
    {
        player_Ctrl = GameObject.FindAnyObjectByType<PlayerCtrl>();     // playerCtrl�� ã�ƿͼ� �Ҵ�

        tutorial_Inventory.SetActive(false);
        arrowImage.color = new Color(255f, 255f, 255f, 0f); // ȭ��ǥ �̹����� ��Ȱ��ȭ����
    }

    private void Update()
    {
        // i �Ǵ� esc Ű�� ������ ���
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {
            ExitTutorialImage();
        }
    }

    /// <summary>
    /// Ʃ�丮�� �̹����� Ʒ���� ��ư�� �Ҵ��� �Լ�
    /// </summary>
    public void ActiveTutorialImage()
    {
        // �κ��丮�� null�� �ƴ� ��쿡 ����
        if (Inventory != null)
        {
            // �κ��丮�� ��Ȱ��ȭ ������ ��쿡 ����
            if (!Inventory.activeSelf)
            {
                Inventory.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Inventory�� null��"); // �κ��丮�� null�̶�� ���� �α� ���
        }
        tutorial_Inventory.SetActive(true);  // i��ư�� ���������Ƿ� Ʃ�丮�� �̹��� Ȱ��ȭ

        // ������ �̹��� �� ��ư ����
        tutorialInventory_Image.gameObject.SetActive(true);
        turorialGimicItem_Image.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(false);

        player_Ctrl.keystrokes = true;    // Ű���� �Է¸���
    }

    /// <summary>
    /// Ʃ�丮�� �̹����� �������� �Ѱ��� ��ư�� �Ҵ��� �Լ�
    /// </summary>
    public void NextTutorialImage()
    {
        // ������ �̹��� �� ��ư ����
        tutorialInventory_Image.gameObject.SetActive(false);
        turorialGimicItem_Image.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);

        player_Ctrl.keystrokes = false;    // Ű���� �Է��� Ǯ����
    }

    /// <summary>
    /// Ʃ�丮�� �̹����� ���� ��ư�� �Ҵ��� �Լ�
    /// </summary>
    public void ExitTutorialImage()
    {
        if (!player_Ctrl.keystrokes)
        {
            // ������ �̹��� �� ��ư ����
            tutorialInventory_Image.gameObject.SetActive(true);
            turorialGimicItem_Image.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);

            tutorial_Inventory.SetActive(false);  // Ʃ�丮�� UI�� ����

            // player_Ctrl.keystrokes = false;    // Ű���� �Է��� Ǯ���� 
        }

        //// ���� ���� ��쿡�� ����
        //if (GameManager.instance.nowSceneName == "02. MainHallScene")
        //{
        //    // �÷��̾��� �˹����� Ȱ��ȭ�Ǿ� �������� ����
        //    if (playerUI.isActiveAndEnabled)
        //    {
        //        // ������ �̹��� �� ��ư ����
        //        tutorialInventory_Image.gameObject.SetActive(true);
        //        turorialGimicItem_Image.gameObject.SetActive(true);
        //        nextButton.gameObject.SetActive(true);
        //        exitButton.gameObject.SetActive(true);

        //        tutorial_Inventory.SetActive(false);  // Ʃ�丮�� UI�� ����

        //        player_Ctrl.keystrokes = false;    // Ű���� �Է��� Ǯ���� 
        //    }
        //}
        //else
        //{
        //    // ������ �̹��� �� ��ư ����
        //    tutorialInventory_Image.gameObject.SetActive(true);
        //    turorialGimicItem_Image.gameObject.SetActive(true);
        //    nextButton.gameObject.SetActive(true);
        //    exitButton.gameObject.SetActive(true);

        //    tutorial_Inventory.SetActive(false);  // Ʃ�丮�� UI�� ����

        //    player_Ctrl.keystrokes = false;    // Ű���� �Է��� Ǯ���� 
        //}
    }
}
