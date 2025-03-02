using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Kiara_MumeiFeather : MonoBehaviour
{
    Transform[] plants;

    void Start()
    {
        plants = GameObject.Find("Plants").GetComponentsInChildren<Transform>();
        RndStartPos();
        ItemSetting();
    }

    void RndStartPos()
    {
        int rndPosNum = Random.Range(1, plants.Length);
        gameObject.transform.position = plants[rndPosNum].transform.position;
    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[17].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}




