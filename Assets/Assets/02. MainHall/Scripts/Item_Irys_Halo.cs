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
    /// 헤일로를 껐다키는 함수(아이템매니저에서 이거 호출)
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