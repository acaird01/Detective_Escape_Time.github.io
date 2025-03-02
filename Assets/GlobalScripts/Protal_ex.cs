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
        
        StartCoroutine(WaitTouch()); // �̰� ���� �ٸ����Ҷ� ������!

        Setting_SceneStart();
    }
    public void NextScene()
    {
        // �ִϸ��̼�
        LoadingSceneManager.LoadScene(gameObject.name);
    }

    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if(settingGimic)
        {

        }
        else
        {

        }
    }

    IEnumerator WaitTouch()
    {
        while (player) // �ѹ��ϰ� �ǻ����� �̰Ż���, â�� ���� �ݴ°�ó�� �ݺ��ʿ��ϸ� �̰� �ְ� ����
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // ��� �����ض� ���⿡
            NextScene();
        }
    }
}
