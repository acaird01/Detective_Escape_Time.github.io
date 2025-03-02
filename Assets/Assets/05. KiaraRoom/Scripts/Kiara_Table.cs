using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Table : MonoBehaviour
{
    string interaction_Text_episode1 = "���ڰ� �ٸ��� �����־�.\n���� ���� ������ �ʿ�� ������ ����.";
    string interaction_Text_episode2 = "�����̶� �׷��� ���ڰ� �����̳�..\n��� ���� �� ���ٱ�?";
    Interaction_Gimics interaction;
    GameObject player;
    TextController textController;

    private void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();

        StartCoroutine(WaitTouch());
    }
    IEnumerator WaitTouch()
    {
        while (player)
        {
            if (GameManager.instance.Episode_Round == 1)
            {
                if (interaction.run_Gimic == false)
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == true);
                    StartCoroutine(textController.SendText(interaction_Text_episode1));
                }
                else
                {
                    interaction.run_Gimic = false;
                }
            }
            else
            {
                if (interaction.run_Gimic == false)
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == true);
                    StartCoroutine(textController.SendText(interaction_Text_episode2));
                }
                else
                {
                    interaction.run_Gimic = false;
                }
            }
        }
    }
}
