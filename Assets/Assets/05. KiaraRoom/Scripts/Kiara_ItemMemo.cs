using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiara_ItemMemo : MonoBehaviour
{
    private void Start()
    {
        ItemSetting();

    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[33].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}
