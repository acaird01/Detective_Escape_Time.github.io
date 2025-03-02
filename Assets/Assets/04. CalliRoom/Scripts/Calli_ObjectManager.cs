using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_ObjectManager : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    
    #endregion

    #region �ش������ ����� ���� ����
    private int checkEndingEpisode_Num = 1; // ����ȸ�� Ȯ�ο� ����. ���� �׽�Ʈ�� ���� 1�� �ʱ�ȭ
    public int CheckEndingEpisode_Num   // �������� �ٸ� ������Ʈ���� ���� ��ȸ������ Ȯ���ϱ� ���� ������Ƽ
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
    #endregion

    // ������ �޾ƿͼ� 10�������� ���� ���� ������Ʈ���� �ѷ��ֱ�
    // ���� ������Ʈ�� ���� �޾Ƽ� 2�������� �����Ѱ��ֱ�

    public GameObject[] SceneGimics;  // �� ���� ��ġ�� �� ������ �ְ���� ��͵� ���⿡ ����
    string binarySystem_forDataSave = "1"; // ���� �տ� 1���� ������ �ڸ��� ����

    int binarySystem_LoadData = 0;

    GameObject player;
    [SerializeField]
    Transform playerSpawnPos;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        ChangeGameManager_To_SceneData(); // �� �������� �� ������ �ҷ��ͼ� �ѷ��ֱ�
        player = GameObject.Find("Player");
        player.transform.position = playerSpawnPos.position;
        player.transform.rotation = Quaternion.Euler(0, 90, 0);


        Init();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �ʱ�ȭ �Լ�
    private void Init()
    {
        // �� ���� ��� ���
        // ���� ���嵥���Ͱ� 0�̶�� �������� ���ٴ� ���̹Ƿ� �ȳ����� ���
        if (GameManager.instance.CalliScene_Save == 0)
        {
            if (GameManager.instance.Episode_Round == 1)    // 1ȸ��
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("�ʹ� ��ο..\n������ ���� ���Ϸ��� ������ ���� �� ���� ������?"));
            }
            else if (GameManager.instance.Episode_Round == 2)    // 2ȸ��
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("���� �����Ⱑ.. �ϴ� ���Ϸθ� �̿��� �ֺ��� �����߰ھ�."));
            }
            else
            {
                // �߸��� ���� ������ ���
                Debug.Log("���� ���� ����(Calli_ObjectManager)");
            }
        }
    }

    public void ChangeSceneData_To_GameManager() // ������ ���� ��ư or �� �̵��� �̰� ȣ���ؼ� ���ӸŴ����� ����
    {
        int tempSave = 0;

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

        tempSave = Convert.ToInt32(binarySystem_forDataSave, 2);
        GameManager.instance.CalliScene_Save = tempSave;

        GameManager.instance.SaveData();
    }

    void ChangeGameManager_To_SceneData()
    {
        binarySystem_LoadData = GameManager.instance.CalliScene_Save;

        string tempSave = Convert.ToString(binarySystem_LoadData, 2);
        //�޾ƿ� int�� string���� ��ȯ �� �� �� 1 ����
        string binarySystem_LoadData_String = tempSave;
        binarySystem_LoadData_String = binarySystem_LoadData_String.Substring(1);

        //string�� char[]�� ��ȯ �� ������ char�� int�� ��ȯ�Ͽ� �迭�� ����
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

        GameObject.FindAnyObjectByType<Number_if_Gimic>().FindObjManager();
        // �� �����ϰ� ��͵� �����Ȳ �ݿ�
        GameObject.FindAnyObjectByType<Number_if_Gimic>().FirstTextSet();
    }
}
