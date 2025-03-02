using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usual_Room : MonoBehaviour
{
    string Ground1Text = "��ܼ� �� ���� ����."; // 1ȸ���� �� ��ȣ�ۿ��
    string Ground2_Close_Text = "���� ����ֳ�.\n���� Ű�е�� �� �� ���� ������?"; // 2ȸ�� ������ ��
    string Ground2_Open_Text = "����� ���� ���Ⱦ�.\n��! �ȿ� ������ �ִ°� ����!"; // 2ȸ�� �������� ��

    GameObject player;
    bool fristOpenText = false;

    TextController textController;

    AudioSource audiosSource;

    int episode_Round; // ���� ȸ�� ������ ����
    public bool DoorOpen; // ������ �Ѻױ׸��� ���� ���� ������ ���� -> �� ������ �Ѻױ׸��� ���� ������Ʈ �Ŵ����� ��� ���� ���� ������ ����

    Kiara_Door kiara_Door;

    Interaction_Gimics interaction;

    bool firstOpenDoor = false; // ó�� ������������ ��� ���� Ƚ�� �ø����� (������)
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Interaction_Gimics>().run_Gimic = false; // �̰� ���߿� �ݵ�� ���� �� �׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!/*
        //episode_Round = 2; // �̰͵� �׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //DoorOpen = false; // �̰͵� �׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/

        episode_Round = GameManager.instance.Episode_Round;
        interaction = GetComponent<Interaction_Gimics>();
        DoorOpen = interaction.run_Gimic;
        audiosSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        fristOpenText = DoorOpen;
        textController = player.GetComponentInChildren<TextController>();
        kiara_Door = gameObject.GetComponentInChildren<Kiara_Door>();
        SceneStartSetting_Usual_RoomOpen();
        StartCoroutine(WaitTouch());
        StartCoroutine(ForDataSaveTime());
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            if(episode_Round == 1)
            {
                if (interaction.run_Gimic == false)
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == true);
                    StartCoroutine(textController.SendText(Ground1Text));
                    audiosSource.Play();
                    //teraction.interactingObject();
                }
                else
                {
                    //yield return new WaitUntil(() => interaction.run_Gimic == false);
                    interaction.run_Gimic = false;
                }
            }
            else
            {
                if (interaction.run_Gimic == false && DoorOpen == false)
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == true);
                    //interaction.interactingObject();
                    if (DoorOpen == false)
                    {
                        StartCoroutine(textController.SendText(Ground2_Close_Text));
                        audiosSource.Play();
                    }                    
                }
                else if(interaction.run_Gimic == true && DoorOpen == false)
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == false);
                    //interaction.interactingObject();
                    if (DoorOpen == false)
                    {
                        StartCoroutine(textController.SendText(Ground2_Close_Text));
                        audiosSource.Play();
                    }
                }
                if (DoorOpen == true && (interaction.run_Gimic == false || interaction.run_Gimic == true))
                {
                    if (firstOpenDoor == false)
                    {
                        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
                        firstOpenDoor = true;
                    }
                    StartCoroutine(textController.SendText(Ground2_Open_Text));

                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    kiara_Door.gameObject.GetComponent<MeshCollider>().enabled = true;
                    break;
                }
            }
            
        }
    }

    IEnumerator ForDataSaveTime()
    {
        yield return new WaitUntil(() => DoorOpen == true);
        interaction.run_Gimic = DoorOpen;
    }
    public void SceneStartSetting_Usual_RoomOpen()
    {
        if (episode_Round == 1)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
            kiara_Door.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
        else
        {
            if(DoorOpen)
            {
                firstOpenDoor = true;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                kiara_Door.gameObject.GetComponent<MeshCollider>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<BoxCollider>().enabled = true;
                kiara_Door.gameObject.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }
}
