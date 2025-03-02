using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjcetManager : MonoBehaviour
{
    // 데이터 받아와서 10진법으로 만들어서 각각 오브젝트에게 뿌려주기
    // 각각 오브젝트들 상태 받아서 2진법으로 만들어서넘겨주기
    
    public GameObject[] SceneGimics;  // 각 씬에 배치할 때 데이터 주고받을 기믹들 여기에 저장
    string binarySystem_forDataSave = "1"; // 제일 앞에 1으로 고정해 자릿수 보정

    int binarySystem_LoadData = 0;

    GameObject player;
    [SerializeField]
    Transform playerSpawnPos;

    void Start()
    {
        // ChangeGameManager_To_SceneData(); // 씬 시작했을 때 데이터 불러와서 뿌려주기
        player = GameObject.Find("Player");
        player.transform.position = playerSpawnPos.position;
    }

    public void ChangeSceneData_To_GamaManager() // 데이터 저장 버튼 or 씬 이동시 이거 호출해서 게임매니저에 저장
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
