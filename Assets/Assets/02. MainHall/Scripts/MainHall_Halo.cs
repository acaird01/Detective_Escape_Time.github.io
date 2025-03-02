using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHall_Halo : MonoBehaviour
{
    public GameObject particle;
    private Interaction_Items interaction;  // ��ȣ�ۿ� ���� ��ũ��Ʈ
    private GameObject player;
    private TextController textController;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        interaction = GetComponent<Interaction_Items>();   // �ش� ������Ʈ�� �޷��ִ� interaction Gimic�� �����ͼ� �Ҵ�

//         StartCoroutine(WaitTouch());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        StartCoroutine(WaitTouch());
    }

    private void Update()
    {
        // Debug.Log(ItemManager._instance.hotkeyItemIndex);

        if (GameManager.instance.Episode_Round == 1)
        {
            if (ItemManager._instance.hotkeyItemIndex == 9)
            {
                gameObject.tag = "ITEM";
                particle.SetActive(true);
            }
            else
            {
                gameObject.tag = "Untagged";
                particle.SetActive(false);
            }
        }
        else
        {
            gameObject.tag = "ITEM";
            particle.SetActive(true);
        }
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => interaction.run_Gimic == true);

            if (ItemManager._instance.hotkeyItemIndex != 9)
            {
                StartCoroutine(textController.SendText("�̳��� å�� �̿��� ������ �� ���� �� ����."));// ��ȣ�ۿ� ��� ���
            }

            interaction.run_Gimic = false;
        }
    }
}
