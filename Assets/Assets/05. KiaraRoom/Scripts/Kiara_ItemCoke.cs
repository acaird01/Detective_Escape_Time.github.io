using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_ItemCoke : MonoBehaviour
{

    private void Start()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[32].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}
