using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kiara_ItemPotatoes : MonoBehaviour
{

    private void Start()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[29].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}
