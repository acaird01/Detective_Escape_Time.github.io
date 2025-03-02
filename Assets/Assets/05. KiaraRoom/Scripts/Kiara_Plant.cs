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
        firstPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z); // �ʱ���ġ ����
        movePos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.5f); // ��� ���� �� ��ġ ����
        audioSource = gameObject.GetComponent<AudioSource>();
        SceneStartSetting_KiaraPosBox(); // �����Ϳ��� �ҷ��� ��� ���¿� ���� �ʱ���ġ ����
        StartCoroutine(WaitTouch()); // ��ȣ�ۿ� ���ö����� ���
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

    public void SceneStartSetting_KiaraPosBox() // �޾ƿ� �����Ϳ� ���� �ʱ� ��ġ ����
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
    void MovePlant() // ��� ����
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        transform.localPosition = Vector3.MoveTowards(firstPos, movePos, 1f);
    }

    void MoveBackPlant() // ��� �ǵ��ư�
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        transform.localPosition = Vector3.MoveTowards(movePos, firstPos, 1f);
    }
}
