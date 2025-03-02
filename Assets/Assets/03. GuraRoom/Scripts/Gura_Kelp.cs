using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_Kelp : MonoBehaviour
{
    [SerializeField]
    GameObject[] kelps;

    private void Update()
    {
        //set every kelp's texture offset
        for (int i = 0; i < kelps.Length; i++)
        {
            GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0.2f, 0) * Time.deltaTime;
        }
    }
}
