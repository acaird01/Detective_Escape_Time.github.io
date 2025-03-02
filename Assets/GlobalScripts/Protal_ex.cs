using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Protal_ex : MonoBehaviour
{
    Interaction_Gimics interaction;
    GameObject player;

    private bool settingGimic { get; set; }
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private void Start()
    {
        //settingGimic = false;
        player = GameObject.Find("Player");
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        
        StartCoroutine(WaitTouch()); // 이건 문만 다른거할때 지우자!

        Setting_SceneStart();
    }
    public void NextScene()
    {
        // 애니메이션
        LoadingSceneManager.LoadScene(gameObject.name);
    }

    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if(settingGimic)
        {

        }
        else
        {

        }
    }

    IEnumerator WaitTouch()
    {
        while (player) // 한번하고 뽀사지면 이거빼고, 창문 열고 닫는거처럼 반복필요하면 이거 넣고 쓰기
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에
            NextScene();
        }
    }
}
