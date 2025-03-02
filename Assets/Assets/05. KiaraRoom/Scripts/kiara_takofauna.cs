using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kiara_takofauna : MonoBehaviour
{
    void Start()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[4].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}
