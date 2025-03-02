using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_NapkinHolder : MonoBehaviour
{
    string interaction_Text = "\"A = 2, B = 0, C = 2\"\n음.. 각 알파벳에서 이어지지 않은 부분의 수와 같은거 같은데?";
    /*Interaction_Gimics interaction;
    TextController textController;
    GameObject player;*/

    Interaction_Items interaction;
    private void Start()
    {
        /*interaction = gameObject.GetComponent<Interaction_Gimics>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        if (GameManager.instance.Episode_Round == 1)
        {
            StartCoroutine(WaitTouch());
        }
        else
        {
            gameObject.GetComponent<Kiara_NapkinHolder>().enabled = false;
        }*/
        interaction = gameObject.GetComponent<Interaction_Items>();
        interaction._text = interaction_Text;
    }

    /*IEnumerator WaitTouch()
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                StartCoroutine(textController.SendText(interaction_Text));

            }
            else
            {
                interaction.run_Gimic = false;
            }
        }
    }*/
}
