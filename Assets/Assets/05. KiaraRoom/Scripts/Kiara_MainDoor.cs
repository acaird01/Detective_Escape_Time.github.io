using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_MainDoor : MonoBehaviour // �̰� 1ȸ���� �� ����ִ°� �����ٷ��� �ϴ°Ŷ� ��� ������ ������ x
{
    GameObject player;

    Interaction_Gimics interaction;
    TextController textController;
    int episode_Round; // ���� ȸ�� ������ ������

    string Ground1Text = "���� ����־�.\n������ ����������."; // 1ȸ���� �� ��ȣ�ۿ��
    

    AudioSource audiosSource;
    Kiara_Door[] kiara_door;
    BoxCollider boxCollider; // 2ȸ������ �ؽ�Ʈ ����ϴ� �θ� �� �ݶ��̴� ���ٷ���

    public bool DoorOpen = false; // �ڽĵ��� �̰� ������ ������ ��������
    void Start()
    {
        //gameObject.GetComponent<Interaction_Gimics>().run_Gimic = false; // �̰� ���߿� �ݵ�� ���� �� �׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!
        //episode_Round = 2; // �̰͵� �׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        audiosSource = gameObject.GetComponent<AudioSource>();
        kiara_door = gameObject.GetComponentsInChildren<Kiara_Door>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        episode_Round = GameManager.instance.Episode_Round; // �̰� ���߿� �ݵ�� ų��!!!!!!!!!!!!!!!!!�׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        SceneStartSetting(); // �����Ϳ��� �ҷ��� ��� ���¿� ���� �ʱ���ġ ����
        StartCoroutine(WaitTouch());
    }

   
    IEnumerator WaitTouch()
    {
        if (episode_Round == 1)
        {
            while (player)
            {
                if (interaction.run_Gimic == false)
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == true);
                    StartCoroutine(textController.SendText(Ground1Text));
                }
                else
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == false);
                    audiosSource.Play();
                }
            }
        }
    }
    public void SceneStartSetting() // �޾ƿ� �����Ϳ� ���� �ʱ� ��ġ ����
    {
        if (episode_Round == 1)
        {
            boxCollider.enabled = true;
            for (int i = 0; i < kiara_door.Length; i++)
            {
                kiara_door[i].enabled = false;
            }
        }
        else
        {
            boxCollider.enabled = false;
            for (int i = 0; i < kiara_door.Length; i++)
            {
                kiara_door[i].enabled = true;
            }
            gameObject.GetComponent<Kiara_MainDoor>().enabled = false;
        }
    }
}
