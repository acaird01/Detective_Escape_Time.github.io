using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHall_DoorAnimationCtrl : MonoBehaviour
{
    Interaction_Gimics interaction;
    GameObject player;
    private MainHall_ObjectManager mainHall_ObjectManager;  // �������� ������Ʈ �Ŵ���

    private AudioManager audioManager;
    private bool settingGimic { get; set; }
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    // �ִϸ��̼� ����� ������Ʈ�� Animator ������Ʈ
    private Animator animator;

    private void Start()
    {
        player = GameObject.Find("Player");
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();  // �������� ������Ʈ �Ŵ����� ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        audioManager = GameObject.FindAnyObjectByType<AudioManager>();

        settingGimic = interaction.run_Gimic;
        // �ش� ���� ���� animator������Ʈ �Ҵ�
        animator = GetComponent<Animator>();

        StartCoroutine(WaitTouch());

        Setting_SceneStart();
    }

    // ���� �� �ε� �Լ�
    public void NextScene()
    {
        // �� �̵� �� ���� �ʿ����� ������, ��� ���࿩�ε��� gamemanager�� ����
        audioManager.SaveVolume();
        mainHall_ObjectManager.ChangeSceneData_To_GameManager();
        GameManager.instance.SaveData();
        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;

        // ���� �� �ε�(���� ������ �̵��ϱ� �� �ε��� ����)
        LoadingSceneManager.LoadScene(gameObject.name);
    }

    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            animator.Play("DoorIdle");
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

            if (interaction.run_Gimic)
            {
                // �������� �ִϸ��̼� ���
                StartCoroutine(opening());
            }
            else
            {
                // �������� �ִϸ��̼� ���
                StartCoroutine(closing());
            }
        }
    }

    IEnumerator opening() // ��� �۵����� �� 
    {
        // �ִϸ��̼��� �÷������� �ƴ϶��
        animator.SetBool("DoorOpenClose", true);

        yield return new WaitForSeconds(.1f);

        // ���� �ش� ���� ���� �湮�� �ƴ϶�� ���� ������ �Ѿ.
        if (!String.Equals(gameObject.name, "StartPos_Door"))
        {
            GameManager.instance.PrevSceneName = gameObject.name;   // ���� �� ������ ���� �Ѿ ���� �̸��� ����
            NextScene();
        }
    }

    IEnumerator closing() // ����� �ǵ��� ����
    {
        animator.SetBool("DoorOpenClose", false);

        yield return new WaitForSeconds(.1f);
    }
}