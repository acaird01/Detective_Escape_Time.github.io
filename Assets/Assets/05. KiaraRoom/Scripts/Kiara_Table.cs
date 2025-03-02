using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Table : MonoBehaviour
{
    string interaction_Text_episode1 = "의자가 바르게 놓여있어.\n지금 굳이 어지를 필요는 없을꺼 같아.";
    string interaction_Text_episode2 = "저녁이라 그런가 의자가 엉망이네..\n대신 정리 좀 해줄까?";
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
