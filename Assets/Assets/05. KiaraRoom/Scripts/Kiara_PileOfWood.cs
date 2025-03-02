using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kiara_PileOfWood : MonoBehaviour
{
    string interactionText = "�н�ƮǪ������ �� ���۴��̰� ����.\n���� �ҿ� �� Ż�͸� ������."; // ���� ���� ��ȣ�ۿ� ���� �� ���� �ؽ�Ʈ

    Interaction_Gimics interaction;
    //bool GimicMove = false; // true�� �� ���� �Ȱ�
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
        //interaction.run_Gimic = false; // �׽�Ʈ�� ������ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
        //GimicMove = interaction.run_Gimic;
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        audioSource = gameObject.GetComponentsInChildren<AudioSource>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
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

    public void SceneStartSetting_KiaraPosBox() // �޾ƿ� �����Ϳ� ���� �ʱ� ��ġ ����
    {
        if (interaction.run_Gimic)
        {
            boxCollider.enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
