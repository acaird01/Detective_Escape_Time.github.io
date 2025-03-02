using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_FixClock_Check : MonoBehaviour
{
    Interaction_Gimics interaction;
    Calli_DialLockScene2 dialLock;
    TextController textController;
    GameObject player;
    BoxCollider boxCollider;

    string before_Obtaining = "�ڹ��谡 �� ������ ���ڶ�.";
    string after_Obtaining = "�� �´� ������ �� ����.";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        dialLock = gameObject.GetComponentInChildren<Calli_DialLockScene2>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        SceneStartSetting_Calli_FixClock_Check();
        StartCoroutine(WaitTouch());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    IEnumerator WaitTouch() // clock�� ���� ��� ������ �� true���� ���� �� ��ũ��Ʈ�� ����� �� gimicmove�� true�� �ɰ�
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (ItemManager._instance.hotkeyItemIndex == 26)
                {
                    // �ڹ��� ������ �κ��丮�� �ǵ�����
                    ItemManager._instance.ReturnItem(26);
                    ItemManager._instance.DeactivateItem(26);

                    StartCoroutine(textController.SendText(after_Obtaining));
                    dialLock.fixClock_Check = true;
                    boxCollider.enabled = false;
                    break;
                }
                else
                {
                    StartCoroutine(textController.SendText(before_Obtaining));
                }
            }
            else
            {
                interaction.run_Gimic = false;
            }
        }
    }

    public void SceneStartSetting_Calli_FixClock_Check() 
    {
        if (interaction.run_Gimic)
        {
            dialLock.fixClock_Check = true;
            gameObject.GetComponent<Calli_FixClock_Check>().enabled = false; // ��ͼ��������� �ڱ��ڽ� ����
        }
    }
}
