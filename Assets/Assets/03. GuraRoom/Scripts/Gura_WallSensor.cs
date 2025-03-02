using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_WallSensor : MonoBehaviour
{
    GameObject player;
    TextController textConteroller;

    private void Start()
    {
        player = GameObject.Find("Player");
        textConteroller = player.GetComponentInChildren<TextController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine(textConteroller.SendText("이쪽으로는 갈 필요 없을꺼 같아."));
        }
    }

}
