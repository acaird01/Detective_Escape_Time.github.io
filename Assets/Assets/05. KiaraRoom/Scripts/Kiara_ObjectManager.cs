using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kiara_ObjectManager : MonoBehaviour
{
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        ChangeGameManager_To_SceneData(); // �� �������� �� ������ �ҷ��ͼ� �ѷ��ֱ�
        player = GameObject.Find("Player");
        player.transform.position = playerSpawnPos.position;
        player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 30f, 0f);  // ������ ���� �ְԲ� ī�޶� ����

        // �� ���� ��� ���
        // ���� ���嵥���Ͱ� 0�̶�� �������� ���ٴ� ���̹Ƿ� �ȳ����� ���
        if (GameManager.instance.KiaraScene_Save == 0)
        {
            if (GameManager.instance.Episode_Round == 1)    // 1ȸ��
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("�н�ƮǪ�������ݾ�?\n�׷����� ����� ������ Ÿ�ڴ� �丮�� ���ڸ� ���� �־���."));
            }
            else if (GameManager.instance.Episode_Round == 2)    // 2ȸ��
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("����ð��� �����ΰ�?\n�����̶�� ������ �� ���� ���� �ѷ��� �� ������?"));
            }
            else
            {
                // �߸��� ���� ������ ���
                Debug.Log("���� ���� ����(Kiara_ObjectManager)");
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void ChangeSceneData_To_GamaManager() 
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
        GameManager.instance.KiaraScene_Save = tempSave;
        GameManager.instance.SaveData();
    }
    void ChangeGameManager_To_SceneData()
    {
        binarySystem_LoadData = GameManager.instance.KiaraScene_Save;
        string tempSave = Convert.ToString(binarySystem_LoadData, 2);
        string binarySystem_LoadData_String = tempSave;
        binarySystem_LoadData_String = binarySystem_LoadData_String.Substring(1);
        char[] binarySystem_LoadData_Char = binarySystem_LoadData_String.ToCharArray();
        
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
        GameObject.FindAnyObjectByType<Number_if_Gimic>().FirstTextSet();
    }
}
