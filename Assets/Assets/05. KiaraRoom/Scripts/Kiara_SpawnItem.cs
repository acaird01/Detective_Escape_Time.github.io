using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_SpawnItem : MonoBehaviour
{
    public bool dontSetactivefalse = false;
    private void Start()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        if (!dontSetactivefalse)
        {
            if (ItemManager._instance.inventorySlots[30].GetComponent<IItem>().isGetItem)
            {
                ItemManager._instance.CollectItem(gameObject);
                
                gameObject.SetActive(false);
            }
        }
    }
}
