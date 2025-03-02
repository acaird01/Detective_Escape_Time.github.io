using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Calli_SceneChangeDoor : MonoBehaviour
{
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    GameObject player; // �÷��̾�
    GameObject interaction_F;   // ��ȣ�ۿ��� ������ ����϶� ȭ�鿡 ����� �̹���
    AudioManager audioManager;

    private void Awake()
    {
        // interaction_F = GameObject.Find("F");
    }

    void Start()
    {
        player = GameObject.Find("Player");
        interaction_F = ItemManager._instance.interaction_F;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();   // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
    }

    void OnMouseOver()
    {
        if (player)
        {
            // �÷��̾�� ��ȣ�ۿ��ϴ� ������Ʈ ������ �Ÿ�
            float dist = Vector3.Distance(player.transform.position, transform.position);

            // �Ÿ��� 5���� ���� ��� ����
            if (dist < 5)
            {
                // ��ȣ�ۿ� ������ ��ü�� ���̶���Ʈó�� ���

                interaction_F.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (gameObject.name == "02. MainHallScene")
                    {
                        audioManager.SaveVolume();
                        calli_ObjectManager.ChangeSceneData_To_GameManager();   // �� �ε��ϱ� �� Į���� ������ ����
                        GameManager.instance.SaveData();
                        GameObject halo = GameObject.FindAnyObjectByType<Item_Irys_Halo>().gameObject;
                        if(halo.gameObject.activeSelf)
                        {
                            halo.gameObject.SetActive(false);
                        }

                        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;
                        LoadingSceneManager.LoadScene(gameObject.name);
                    }
                }
            }
            else
            {
                interaction_F.gameObject.SetActive(false);
            }
        }
    }

    private void OnMouseExit()
    {
        interaction_F.gameObject.SetActive(false);
    }
}
