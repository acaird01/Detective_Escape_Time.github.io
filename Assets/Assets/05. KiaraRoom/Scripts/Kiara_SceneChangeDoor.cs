using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Kiara_SceneChangeDoor : MonoBehaviour
{
    GameObject player; // �÷��̾�
    GameObject interaction_F;
    AudioManager audioManager;
    Kiara_ObjectManager objectMaanger;

    private void Awake()
    {
        // interaction_F = ItemManager._instance.interaction_F;
    }

    void Start()
    {
        // interaction_F = GameObject.Find("F");
        player = GameObject.Find("Player");
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        objectMaanger = GameObject.FindAnyObjectByType<Kiara_ObjectManager>();
        interaction_F = ItemManager._instance.interaction_F;
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
                        objectMaanger.ChangeSceneData_To_GamaManager();
                        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;
                        GameManager.instance.SaveData();
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
