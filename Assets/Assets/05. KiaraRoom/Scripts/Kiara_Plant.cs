using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Plant : MonoBehaviour
{
    Vector3 firstPos;
    Vector3 movePos;

    GameObject player;
    AudioSource audioSource;
    Interaction_Gimics interaction;
    private void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        player = GameObject.Find("Player");
        firstPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z); // 초기위치 지정
        movePos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.5f); // 기믹 수행 후 위치 지정
        audioSource = gameObject.GetComponent<AudioSource>();
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
                MovePlant();
            }
            else
            {
                yield return new WaitUntil(() => interaction.run_Gimic == false);
                MoveBackPlant();
            }
        }
    }

    public void SceneStartSetting_KiaraPosBox() // 받아온 데이터에 따른 초기 위치 설정
    {
        if (interaction.run_Gimic)
        {
            transform.localPosition = movePos;
        }
        else
        {
            transform.localPosition = firstPos;
        }
    }
    void MovePlant() // 기믹 실행
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        transform.localPosition = Vector3.MoveTowards(firstPos, movePos, 1f);
    }

    void MoveBackPlant() // 기믹 되돌아감
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        transform.localPosition = Vector3.MoveTowards(movePos, firstPos, 1f);
    }
}
