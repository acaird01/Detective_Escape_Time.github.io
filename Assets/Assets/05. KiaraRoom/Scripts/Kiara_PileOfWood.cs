using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kiara_PileOfWood : MonoBehaviour
{
    string interactionText = "패스트푸드점에 왠 장작더미가 있지.\n뭔가 불에 잘 탈것만 같은걸."; // 깃털 없이 상호작용 했을 때 나올 텍스트

    Interaction_Gimics interaction;
    //bool GimicMove = false; // true일 때 실행 된거
    AudioSource[] audioSource;

    public GameObject fire1;
    public GameObject fire2;

    BoxCollider boxCollider;
    public Transform[] firePos;
    TextController textController;
    GameObject player;

    private void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        //interaction.run_Gimic = false; // 테스트용 지우자 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
        //GimicMove = interaction.run_Gimic;
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        audioSource = gameObject.GetComponentsInChildren<AudioSource>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        SceneStartSetting_KiaraPosBox(); // 데이터에서 불러온 기믹 상태에 따른 초기위치 설정
        StartCoroutine(WaitTouch()); // 상호작용 들어올때까지 대기
    }


    IEnumerator WaitTouch()
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (ItemManager._instance.hotkeyItemIndex == 13)
                {
                    StartCoroutine(Fire_PileOfWood());
                    break;

                }
                else
                {
                    StartCoroutine(textController.SendText(interactionText));
                }
            }
            else
            {
                yield return new WaitUntil(() => interaction.run_Gimic == false);
                if (ItemManager._instance.hotkeyItemIndex == 13)
                {
                    StartCoroutine(Fire_PileOfWood());
                    break;

                }
                else
                {
                    StartCoroutine(textController.SendText(interactionText));
                }
            }
        }
    }

    IEnumerator Fire_PileOfWood()
    {
        interaction.run_Gimic = true;
        for (int i = 0; i < firePos.Length; i++)
        {
            int a = Random.Range(0, 2);
            switch (a)
            {
                case 0:
                    GameObject fire_Effect = Instantiate(fire1, firePos[i]);                    
                    fire_Effect.transform.parent = firePos[i];
                    audioSource[i].Play();
                    Destroy(fire_Effect, 2f);
                    break;
                case 1:
                    GameObject fire_Effect1 = Instantiate(fire2, firePos[i]);
                    fire_Effect1.transform.parent = firePos[i];
                    audioSource[i].Play();
                    Destroy(fire_Effect1, 2f);
                    break;
            }
        }
        interaction.run_Gimic = true;
        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        yield return new WaitForSeconds(2f);
        boxCollider.enabled = false;
        interaction.run_Gimic = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void SceneStartSetting_KiaraPosBox() // 받아온 데이터에 따른 초기 위치 설정
    {
        if (interaction.run_Gimic)
        {
            boxCollider.enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
