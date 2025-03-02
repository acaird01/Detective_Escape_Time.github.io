using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Gura_ObjectManager : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����

    #endregion

    // ������ �޾ƿͼ� 10�������� ���� ���� ������Ʈ���� �ѷ��ֱ�
    // ���� ������Ʈ�� ���� �޾Ƽ� 2�������� �����Ѱ��ֱ�

    public GameObject[] SceneGimics;  // �� ���� ��ġ�� �� ������ �ְ���� ��͵� ���⿡ ����
    string binarySystem_forDataSave = "1"; // ���� �տ� 1���� ������ �ڸ��� ����

    int binarySystem_LoadData = 0;

    GameObject player;
    [SerializeField]
    Transform playerSpawnPos;

    [SerializeField]
    GameObject Trident;
    [SerializeField]
    GameObject TakoGura;

    [SerializeField]
    GameObject KroniiSword;
    [SerializeField]
    GameObject BaelzDice;
    [SerializeField]
    GameObject TakoMumei;
    [SerializeField]
    GameObject TakoBell;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        ChangeGameManager_To_SceneData(); // �� �������� �� ������ �ҷ��ͼ� �ѷ��ֱ�
        player = GameObject.Find("Player");
        player.transform.position = playerSpawnPos.position;

        player.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (GameManager.instance.Episode_Round == 1)
        {
            if (ItemManager._instance.inventorySlots[11].GetComponent<IItem>().isGetItem == true)
            {
                Trident.SetActive(false);
            }

            if (ItemManager._instance.inventorySlots[6].GetComponent<IItem>().isGetItem == true)
            {
                TakoGura.SetActive(false);
            }
        }
        else if (GameManager.instance.Episode_Round == 2)
        {
            if (ItemManager._instance.inventorySlots[1].GetComponent<IItem>().isGetItem == true)
            {
                TakoMumei.SetActive(false);
            }

            if (ItemManager._instance.inventorySlots[5].GetComponent<IItem>().isGetItem == true)
            {
                TakoBell.SetActive(false);
            }

            if (ItemManager._instance.inventorySlots[14].GetComponent<IItem>().isGetItem == true)
            {
                BaelzDice.SetActive(false);
            }

            if (ItemManager._instance.inventorySlots[16].GetComponent<IItem>().isGetItem == true)
            {
                KroniiSword.SetActive(false);
            }
        }

        // �� ���� ��� ���
        // ���� ���嵥���Ͱ� 0�̶�� �������� ���ٴ� ���̹Ƿ� �ȳ����� ���
        if (GameManager.instance.GuraScene_Save == 0)
        {
            if (GameManager.instance.Episode_Round == 1)    // 1ȸ��
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("���� ��¥ �ٴ���ΰ�?\n�ϴ� �� ���� �������� Ȯ���غ��߰ھ�."));
            }
            else if (GameManager.instance.Episode_Round == 2)    // 2ȸ��
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("�и� ���������� �־��� �������ΰ� ������..\n���� �ٸ��� ������ �����ϰ� Ȯ���� ���߰ھ�."));
            }
            else
            {
                // �߸��� ���� ������ ���
                Debug.Log("���� ���� ����(Gura_ObjectManager)");
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void ChangeSceneData_To_GamaManager() // ������ ���� ��ư or �� �̵��� �̰� ȣ���ؼ� ���ӸŴ����� ����
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
        GameManager.instance.GuraScene_Save = tempSave;
        GameManager.instance.SaveData();
    }
    void ChangeGameManager_To_SceneData()
    {
        binarySystem_LoadData = GameManager.instance.GuraScene_Save;
        string tempSave = Convert.ToString(binarySystem_LoadData, 2);
        //�޾ƿ� int�� string���� ��ȯ �� �� �� 1 ����
        string binarySystem_LoadData_String = tempSave;
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

        GameObject.FindAnyObjectByType<Number_if_Gimic>().FindObjManager();
        // �� �����ϰ� ��͵� �����Ȳ �ݿ�
        GameObject.FindAnyObjectByType<Number_if_Gimic>().FirstTextSet();
    }
}
