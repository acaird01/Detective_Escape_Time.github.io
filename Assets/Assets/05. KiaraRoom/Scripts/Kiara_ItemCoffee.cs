using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_ItemCoffee : MonoBehaviour
{
    private void Start()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[31].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}
