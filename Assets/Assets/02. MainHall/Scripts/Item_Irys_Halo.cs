using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Irys_Halo : MonoBehaviour
{
    public GameObject irysHalo;
    bool itemUse = false;

    private void Start()
    {
        irysHalo.gameObject.SetActive(false);
        //SetTextOrder();
    }

    /// <summary>
    /// ���Ϸθ� ����Ű�� �Լ�(�����۸Ŵ������� �̰� ȣ��)
    /// </summary>
    public void HaloUse() 
    {
        itemUse = !itemUse;
        if(itemUse)
        {
            irysHalo.gameObject.SetActive(true);
        }
        else
        {
            irysHalo.gameObject.SetActive(false);
        }
    }
}