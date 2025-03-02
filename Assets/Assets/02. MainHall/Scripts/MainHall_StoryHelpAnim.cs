using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHall_StoryHelpAnim : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    private GameObject player;                              // �÷��̾� ������Ʈ
    private MainHall_ObjectManager mainHall_ObjectManager;  // �������� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;                 // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ

    // private GameObject mainHall_StartRoomDoor;      // ���� ���� ���۹��� �� ���� ������Ʈ
    private MainHall_PassDoorAfterTutorial mainHall_PassDoorAfterTutorial;  // Ʃ�丮�� ���� ���� ����Ҷ� ����� ��ũ��Ʈ
    private MainHall_TakoMoveCtrl mainHall_TakoMoveCtrl;                    // ���丮 ���� ������ ���� �ó׸ӽ� ī�޶� �̵� �� Ÿ�ڵ��� �������� ��ũ��Ʈ
    private MainHall_StartRoomDoorWall startRoomDoorWall;   // ���۹��� ���� �޸� ��
    private GameObject mainHall_StartRoomDoor;      // ���� ���� ���۹��� �� ���� ������Ʈ
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ���� ���θ� Ȯ���ϱ� ���� ����(true : ��� ���� �Ϸ� / false : ��� �̼���)
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    public bool mainHallGuideCamMoveEnd;    // ������ ���̵� ī�޶� ��ŷ�� �������� Ȯ���� ����(true : ���� / false : �̿�)
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // �÷��̾ ã�ƿͼ� �Ҵ�
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();    // MainHall_ObjectManager�� ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�

        Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Init()
    {
        // ���� ������ ī�޶�� Ÿ�ڵ��� �������ִ� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�
        mainHall_PassDoorAfterTutorial = GameObject.FindAnyObjectByType<MainHall_PassDoorAfterTutorial>();
        mainHall_TakoMoveCtrl = GameObject.FindAnyObjectByType<MainHall_TakoMoveCtrl>();

        // ���� ���� �ʱ�ȭ
        settingGimic = interaction.run_Gimic;
        mainHallGuideCamMoveEnd = settingGimic;

        // settingGimic�� ���� �� ��ũ��Ʈ�� �ʱ�ȭ �Լ� ����
        mainHall_PassDoorAfterTutorial.Init(settingGimic);
        mainHall_TakoMoveCtrl.Init(settingGimic);

        interaction.enabled = false;    // ������ �������Ƿ� ���� �񺭼� �Ѿ�� ��Ȳ ������ ���� ��Ȱ��ȭ

        Setting_SceneStart();
    }

    // ���� �������� �Լ�
    private void Setting_SceneStart()
    {
        if (!settingGimic)
        {
            // �Ϸ� ���� ���� ��� ����� �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(WaitForMainHallGuide());
        }
        else
        {
            mainHall_PassDoorAfterTutorial.gameObject.SetActive(false); // ��Ȱ��ȭ ��Ŵ
        }
    }

    // �ִϸ��̼� �� ī�޶� ��ŷ�� ������ ����� �ڷ�ƾ �Լ�
    private IEnumerator WaitForMainHallGuide()
    {
        // Ÿ�� �ִϸ��̼��� ������, ������ ī�޶� ��ŷ�� ���� ���
        yield return new WaitUntil(() => (mainHallGuideCamMoveEnd));

        interaction.enabled = true;

        // �� �������Ƿ� �� �����ٰ� ���� ����
        settingGimic = true;
        interaction.run_Gimic = true;
    }
}