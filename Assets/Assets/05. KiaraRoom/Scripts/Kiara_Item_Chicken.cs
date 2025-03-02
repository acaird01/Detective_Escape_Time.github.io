using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Item_Chicken : MonoBehaviour
{
    private void Start()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[30].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}
