using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_candle : MonoBehaviour
{
    [SerializeField]
    GameObject flames_Gameobj;

    GameObject player;
    TextController textController;
    PlayerCtrl playerCtrl;
    Interaction_Gimics interaction;

    string FirstInteraction_Text = "Ư���� �� ���� �к��̱�";

    // Start is called before the first frame update
    void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        //interaction.run_Gimic = false; // ������ ���� ���ϰ� �׻� �����ִٰ� ���� �״ٸ� �Ϸ��� �̰� �ּ� Ǯ��, object maanger�� ��� ����
        player = GameObject.Find("Player");
        playerCtrl = player.GetComponent<PlayerCtrl>();
        textController = player.GetComponentInChildren<TextController>();

        StartCoroutine(WaitTouch());
    }


    IEnumerator WaitTouch()
    {
        if (interaction.run_Gimic == false)
        {
            yield return new WaitUntil(() => interaction.run_Gimic == true);
            StartCoroutine(textController.SendText(FirstInteraction_Text)); // ù ��ȣ�ۿ�ÿ��� �ؽ�Ʈ ���
        }
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (flames_Gameobj.gameObject.activeSelf)
                {
                    flames_Gameobj.gameObject.SetActive(false);
                }
                else
                {
                    flames_Gameobj.gameObject.SetActive(true);
                }
            }
            else
            {
                interaction.run_Gimic = false;
            }
        }
    }
}
