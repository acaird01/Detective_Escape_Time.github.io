using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kiara_ObjectManager : MonoBehaviour
{
    // 데이터 받아와서 10진법으로 만들어서 각각 오브젝트에게 뿌려주기
    // 각각 오브젝트들 상태 받아서 2진법으로 만들어서넘겨주기

    public GameObject[] SceneGimics;  // 각 씬에 배치할 때 데이터 주고받을 기믹들 여기에 저장
    string binarySystem_forDataSave = "1"; // 제일 앞에 1으로 고정해 자릿수 보정

    int binarySystem_LoadData = 0;

    GameObject player;
    [SerializeField]
    Transform playerSpawnPos;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        ChangeGameManager_To_SceneData(); // 씬 시작했을 때 데이터 불러와서 뿌려주기
        player = GameObject.Find("Player");
        player.transform.position = playerSpawnPos.position;
        player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 30f, 0f);  // 정면을 보고 있게끔 카메라 조정

        // 씬 진입 대사 출력
        // 만약 저장데이터가 0이라면 들어온적이 없다는 것이므로 안내문구 출력
        if (GameManager.instance.KiaraScene_Save == 0)
        {
            if (GameManager.instance.Episode_Round == 1)    // 1회차
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("패스트푸드점이잖아?\n그러고보니 여기로 도망간 타코는 요리사 모자를 쓰고 있었지."));
            }
            else if (GameManager.instance.Episode_Round == 2)    // 2회차
            {
                StartCoroutine(player.GetComponentInChildren<TextController>().SendText("저녁시간의 가게인가?\n지금이라면 저번에 못 가본 곳도 둘러볼 수 있을까?"));
            }
            else
            {
                // 잘못된 엔딩 설정인 경우
                Debug.Log("엔딩 설정 오류(Kiara_ObjectManager)");
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
