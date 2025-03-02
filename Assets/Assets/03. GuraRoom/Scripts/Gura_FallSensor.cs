using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_FallSensor : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }


    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject == player)
        {
            player.transform.position = new Vector3(0, 30, 0);
        }
    }
}
