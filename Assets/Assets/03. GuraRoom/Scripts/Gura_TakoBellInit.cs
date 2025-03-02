using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_TakoBellInit : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Interaction_Items>().Init();
    }
}
