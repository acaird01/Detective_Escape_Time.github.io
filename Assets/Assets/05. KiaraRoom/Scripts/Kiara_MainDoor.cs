using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_MainDoor : MonoBehaviour // 이건 1회차때 문 잠겨있는거 보여줄려고 하는거라 기믹 데이터 저장은 x
{
    GameObject player;

    Interaction_Gimics interaction;
    TextController textController;
    int episode_Round; // 현재 회차 저장할 변수ㅆ

    string Ground1Text = "굳게 잠겨있어.\n완전히 갇혔나보네."; // 1회차에 문 상호작용시
    

    AudioSource audiosSource;
    Kiara_Door[] kiara_door;
    BoxCollider boxCollider; // 2회차에선 텍스트 출력하는 부모 문 콜라이더 꺼줄려고

    public bool DoorOpen = false; // 자식들이 이거 값으로 문열고 닫으려고
    void Start()
    {
        //gameObject.GetComponent<Interaction_Gimics>().run_Gimic = false; // 이거 나중에 반드시 지울 것 테스트용!!!!!!!!!!!!!!!!!!!!!!!!!
        //episode_Round = 2; // 이것도 테스트용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        audiosSource = gameObject.GetComponent<AudioSource>();
        kiara_door = gameObject.GetComponentsInChildren<Kiara_Door>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        episode_Round = GameManager.instance.Episode_Round; // 이거 나중에 반드시 킬것!!!!!!!!!!!!!!!!!테스트용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        SceneStartSetting(); // 데이터에서 불러온 기믹 상태에 따른 초기위치 설정
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
    public void SceneStartSetting() // 받아온 데이터에 따른 초기 위치 설정
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
