    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_Tako : MonoBehaviour
{
    Item06TakoGura takoGura;


    private void Awake()
    {

            takoGura = gameObject.GetComponent<Item06TakoGura>();
            ItemSetting();

    }

    void ItemSetting()
    {
        if (takoGura.isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}
