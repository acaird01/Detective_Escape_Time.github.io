using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHall_ObjectManager : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    // private MainHall_StartStoryAndTutorial StartStoryAndTutorial;   // ���� ������ ���丮�� Ʃ�丮���� ������ ��ũ��Ʈ�� �Ҵ��� ����
    private GameObject player;                                      // �÷��̾�
    private GuraScene_1 guraScene_1;                                // 1ȸ�� ��������� �Ѿ ��
    private GuraScene_2 guraScene_2;                                // 2ȸ�� ��������� �Ѿ ��
    private CalliScene_1 calliScene_1;                              // 1ȸ�� Į�������� �Ѿ ��
    private CalliScene_2 calliScene_2;                              // 2ȸ�� Į�������� �Ѿ ��
    private KiaraScene_1 kiaraScene_1;                              // 1ȸ�� Ű�ƶ������ �Ѿ ��
    private KiaraScene_2 kiaraScene_2;                              // 2ȸ�� Ű�ƶ������ �Ѿ ��

    [Header("2ȸ�� ȹ��� Ÿ�ڵ�")]
    [SerializeField]
    private GameObject[] takos; 

    [Header("�÷��̾��� ���������� ������ �迭")]
    public GameObject[] SponPoints;        // �÷��̾��� ���������� ������ �迭(0: ���۹� ��ġ, 1: ����� ��ġ, 2: Į���� ��ġ, 3: Ű�ƶ�� ��ġ, 4: ù ���丮 ������ ���� ��ġ)
    #endregion

    #region �ش������ ����� ���� ����
    private int checkEndingEpisode_Num = 1; // ����ȸ�� Ȯ�ο� ����. ���� �׽�Ʈ�� ���� 1�� �ʱ�ȭ
    public int CheckEndingEpisode_Num       // �������� �ٸ� ������Ʈ���� ���� ��ȸ������ Ȯ���ϱ� ���� ������Ƽ
    {
        get
        {
            return checkEndingEpisode_Num;
        }
        set
        {
            if (checkEndingEpisode_Num < 2)    // �ִ� ȸ������ �������� �Ҵ� ����
            {
                checkEndingEpisode_Num = value;
            }
        }
    }

    // private bool isGameStart;
    #endregion

    #region ���ӽ����� �� �Լ� ������ Action(���� �̻��)
    private Action StartScene = null;
    public void SetStartSceneCallback(Action _StartScene)   // �Ű������� ������ �Լ��� �޾Ƽ� action�� �߰�
    {
        StartScene += _StartScene;
    }
    #endregion
    // ������ �޾ƿͼ� 10�������� ���� ���� ������Ʈ���� �ѷ��ֱ�
    // ���� ������Ʈ�� ���� �޾Ƽ� 2�������� �����Ѱ��ֱ�

    public GameObject[] SceneGimics;  // �� ���� ��ġ�� �� ������ �ְ���� ��͵� ���⿡ ����
    string binarySystem_forDataSave = "1"; // ���� �տ� 1���� ������ �ڸ��� ����

    int binarySystem_LoadData = 0;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        Init(); // �ʱ�ȭ �Լ� ȣ��

        if (GameManager.instance.PrevSceneName != null)
        {
            if (String.Equals(GameManager.instance.PrevSceneName, guraScene_1.name) || String.Equals(GameManager.instance.PrevSceneName, guraScene_2.name))
            {
                // ��������� ������ �ǵ��ƿ� ���
                player.GetComponent<Transform>().position = SponPoints[1].GetComponent<Transform>().position;
                player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 270f, 0f);  // ������ ���� �ְԲ� ī�޶� ����
            }
            else if (String.Equals(GameManager.instance.PrevSceneName, calliScene_1.name) || String.Equals(GameManager.instance.PrevSceneName, calliScene_2.name))
            {
                // Į�������� ������ �ǵ��ƿ� ���
                player.GetComponent<Transform>().position = SponPoints[2].GetComponent<Transform>().position;
                player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 180f, 0f);  // ������ ���� �ְԲ� ī�޶� ����
            }
            else if (String.Equals(GameManager.instance.PrevSceneName, kiaraScene_1.name) || String.Equals(GameManager.instance.PrevSceneName, kiaraScene_2.name))
            {
                // Ű�ƶ������ ������ �ǵ��ƿ� ���
                player.GetComponent<Transform>().position = SponPoints[3].GetComponent<Transform>().position;
                player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 90f, 0f);  // ������ ���� �ְԲ� ī�޶� ����
            }
        }

        ChangeGameManager_To_SceneData(); // �� �������� �� ������ �ҷ��ͼ� �ѷ��ֱ�
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �ش� ������ ����� ������ �Ҵ� �� �ʱ�ȭ�ϱ� ���� �Լ�
    private void Init()
    {
        // StartStoryAndTutorial = GameObject.FindAnyObjectByType<MainHall_StartStoryAndTutorial>();   // ���丮 �� Ʃ�丮�� �Ѱ� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�
        player = GameObject.Find("Player"); // �÷��̾ ã�ƿͼ� �Ҵ�
        checkEndingEpisode_Num = GameManager.instance.Episode_Round;    // ���� ȸ�� ������ ���ӸŴ����� ����� ȸ�� ������ ����

        // StartStoryAndTutorial.Init();   // ���丮 �� Ʃ�丮�� �Ѱ� ��ũ��Ʈ �ʱ�ȭ

        // Debug.Log("���� ȸ��(����) : " + checkEndingEpisode_Num); // �׽�Ʈ �� 

        // 1ȸ�� ����� ����� ���� ã�ƿͼ� �Ҵ�
        guraScene_1 = GameObject.FindAnyObjectByType<GuraScene_1>();
        calliScene_1 = GameObject.FindAnyObjectByType<CalliScene_1>();
        kiaraScene_1 = GameObject.FindAnyObjectByType<KiaraScene_1>();
        // 2ȸ�� ����� ����� ���� ã�ƿͼ� �Ҵ�
        guraScene_2 = GameObject.FindAnyObjectByType<GuraScene_2>();
        calliScene_2 = GameObject.FindAnyObjectByType<CalliScene_2>();
        kiaraScene_2 = GameObject.FindAnyObjectByType<KiaraScene_2>();

        // �� ���� ��� ����� ���� �ʱ�ȭ �Լ� ȣ��
        guraScene_1.GetComponent<MainHall_PrintDoorInfo>().Init();
        calliScene_1.GetComponent<MainHall_PrintDoorInfo>().Init();
        kiaraScene_1.GetComponent<MainHall_PrintDoorInfo>().Init();
        guraScene_2.GetComponent<MainHall_PrintDoorInfo>().Init();
        calliScene_2.GetComponent<MainHall_PrintDoorInfo>().Init();
        kiaraScene_2.GetComponent<MainHall_PrintDoorInfo>().Init();

        // 1ȸ���� ����� �� Ȱ��ȭ, 2ȸ�� ���� ��Ȱ��ȭ
        // 2ȸ���� ��� �ݴ�
        if (checkEndingEpisode_Num == 1)
        {
            guraScene_1.gameObject.SetActive(true);
            calliScene_1.gameObject.SetActive(true);
            kiaraScene_1.gameObject.SetActive(true);

            guraScene_2.gameObject.SetActive(false);
            calliScene_2.gameObject.SetActive(false);
            kiaraScene_2.gameObject.SetActive(false);

            for (int i = 0; i < takos.Length; i++)
            {
                Destroy(takos[i]);  // 1ȸ���� �ʿ������ ����
            }
        }
        else if (checkEndingEpisode_Num == 2)
        {
            guraScene_1.gameObject.SetActive(false);
            calliScene_1.gameObject.SetActive(false);
            kiaraScene_1.gameObject.SetActive(false);

            guraScene_2.gameObject.SetActive(true);
            calliScene_2.gameObject.SetActive(true);
            kiaraScene_2.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// ������ ���� ��ư or �� �̵��� �̰� ȣ���ؼ� ���ӸŴ����� ����
    /// </summary>
    public void ChangeSceneData_To_GameManager()
    {
        binarySystem_forDataSave = "1";
        for (int i = 0; i < SceneGimics.Length; i++)
        {
            if (SceneGimics[i].GetComponent<Interaction_Gimics>().run_Gimic == true)
            {
                binarySystem_forDataSave += "1";
            }
            else
            {
                binarySystem_forDataSave += "0";
            }
        }

        GameManager.instance.MainHallScene_Save = int.Parse(binarySystem_forDataSave); // ������ ����
        GameManager.instance.SaveData();
    }

    void ChangeGameManager_To_SceneData()
    {
        binarySystem_LoadData = GameManager.instance.MainHallScene_Save;

        //�޾ƿ� int�� string���� ��ȯ �� �� �� 1 ����
        string binarySystem_LoadData_String = binarySystem_LoadData.ToString();
        binarySystem_LoadData_String = binarySystem_LoadData_String.Substring(1);

        char[] binarySystem_LoadData_Char = binarySystem_LoadData_String.ToCharArray();

        //char[]�� ��ȯ�� �����͸� ������ ������Ʈ���� �ѷ��ֱ�
        for (int i = 0; i < binarySystem_LoadData_Char.Length; i++)
        {
            if (binarySystem_LoadData_Char[i] == '1')
            {
                SceneGimics[i].GetComponent<Interaction_Gimics>().Setting_Scene_Gimic(true);
            }
            else
            {
                SceneGimics[i].GetComponent<Interaction_Gimics>().Setting_Scene_Gimic(false);
            }
        }
    }
}
