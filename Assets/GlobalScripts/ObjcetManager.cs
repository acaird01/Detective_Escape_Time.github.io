using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjcetManager : MonoBehaviour
{
    // ������ �޾ƿͼ� 10�������� ���� ���� ������Ʈ���� �ѷ��ֱ�
    // ���� ������Ʈ�� ���� �޾Ƽ� 2�������� �����Ѱ��ֱ�
    
    public GameObject[] SceneGimics;  // �� ���� ��ġ�� �� ������ �ְ���� ��͵� ���⿡ ����
    string binarySystem_forDataSave = "1"; // ���� �տ� 1���� ������ �ڸ��� ����

    int binarySystem_LoadData = 0;

    GameObject player;
    [SerializeField]
    Transform playerSpawnPos;

    void Start()
    {
        // ChangeGameManager_To_SceneData(); // �� �������� �� ������ �ҷ��ͼ� �ѷ��ֱ�
        player = GameObject.Find("Player");
        player.transform.position = playerSpawnPos.position;
    }

    public void ChangeSceneData_To_GamaManager() // ������ ���� ��ư or �� �̵��� �̰� ȣ���ؼ� ���ӸŴ����� ����
    {
        for(int i = 0; i < SceneGimics.Length; i++)
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
        GameManager.instance.KiaraScene_Save = int.Parse(binarySystem_forDataSave);
    }

    void ChangeGameManager_To_SceneData()
    {
        binarySystem_LoadData = GameManager.instance.KiaraScene_Save;
        for (int i = SceneGimics.Length; i >0 ; i--)
        {
            if(binarySystem_LoadData % 2 == 0)
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
