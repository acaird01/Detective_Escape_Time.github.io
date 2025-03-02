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

    string FirstInteraction_Text = "특별할 것 없는 촛불이군";

    // Start is called before the first frame update
    void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        //interaction.run_Gimic = false; // 데이터 저장 안하고 항상 켜져있다가 껐다 켰다만 하려면 이거 주석 풀고, object maanger에 등록 ㄴㄴ
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
            StartCoroutine(textController.SendText(FirstInteraction_Text)); // 첫 상호작용시에만 텍스트 출력
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
