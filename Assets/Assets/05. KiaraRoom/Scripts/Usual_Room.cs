using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usual_Room : MonoBehaviour
{
    string Ground1Text = "잠겨서 못 열꺼 같아."; // 1회차에 문 상호작용시
    string Ground2_Close_Text = "아직 잠겨있네.\n옆에 키패드로 열 수 있지 않을까?"; // 2회차 문열기 전
    string Ground2_Open_Text = "잠겨진 문이 열렸어.\n앗! 안에 누군가 있는거 같아!"; // 2회차 문열었을 떄

    GameObject player;
    bool fristOpenText = false;

    TextController textController;

    AudioSource audiosSource;

    int episode_Round; // 현제 회차 저장할 변수
    public bool DoorOpen; // 문여는 한붓그리기 성공 여부 저장할 변수 -> 이 정보는 한붓그리기 에서 오브젝트 매니저로 기믹 수행 여부 전달할 예정

    Kiara_Door kiara_Door;

    Interaction_Gimics interaction;

    bool firstOpenDoor = false; // 처음 문열었을때만 기믹 수행 횟수 올리려고 (ㅈ버그)
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Interaction_Gimics>().run_Gimic = false; // 이거 나중에 반드시 지울 것 테스트용!!!!!!!!!!!!!!!!!!!!!!!!!/*
        //episode_Round = 2; // 이것도 테스트용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //DoorOpen = false; // 이것도 테스트용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/

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
