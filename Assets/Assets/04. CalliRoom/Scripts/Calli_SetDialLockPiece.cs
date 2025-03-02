using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_SetDialLockPiece : MonoBehaviour
{
    Item26DialLockPiece dialLockPiece;

    private void Start()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[26].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
        else
        {
        }
    }
}
