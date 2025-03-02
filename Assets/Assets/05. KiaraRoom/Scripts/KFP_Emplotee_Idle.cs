using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KFP_Emplotee_Idle : MonoBehaviour
{
    string interaction_Text;
    int text_Num;
    GameObject player;
    Interaction_Items interaction_item;

    private void Start()
    {
        player = GameObject.Find("Player");
        interaction_item = gameObject.GetComponent<Interaction_Items>();
        StartCoroutine(RndText());
    }

    IEnumerator RndText()
    {
        text_Num = Random.Range(0, 4);
        while (player)
        {
            switch (text_Num)
            {
                case 0:
                    interaction_Text = "���⼭ ������..";
                    break;
                case 1:
                    interaction_Text = "�׳�� ������ ġŲ�� �Կ�";
                    break;
                case 2:
                    interaction_Text = "�漺�漺! ^^7";
                    break;
                case 3:
                    interaction_Text = "#WOKE #YOLK";
                    break;
                default:
                    break;
            }
            interaction_item._text = interaction_Text;
            yield return new WaitForSeconds(5f);
            text_Num += 1;
            if(text_Num == 3)
            {
                text_Num = 0;
            }
        }

    }
}
