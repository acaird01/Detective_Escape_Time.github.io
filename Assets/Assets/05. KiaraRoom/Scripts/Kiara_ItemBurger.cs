using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_ItemBurger : MonoBehaviour
{
    private void Start()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[28].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}
