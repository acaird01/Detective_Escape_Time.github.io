using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kiara_takokiara : MonoBehaviour
{
    void Start()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[8].GetComponent<IItem>().isGetItem)
        {
            // �������� �ƴ� ��쿡�� ����
            if (GameManager.instance.nowSceneName != "02. MainHallScene")
            {
                gameObject.SetActive(false);
            }
        }
    }
}