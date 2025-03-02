using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KiaraPosBox : MonoBehaviour
{
    Vector3 firstPos;
    Vector3 movePos;

    GameObject player;

    public BoxCollider boxCollider;
    Interaction_Gimics interactino;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        interactino = gameObject.GetComponent<Interaction_Gimics>();
        player = GameObject.Find("Player");
        firstPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        movePos = new Vector3(transform.localPosition.x + 0.004f, transform.localPosition.y, transform.localPosition.z);
        SceneStartSetting_KiaraPosBox();
        StartCoroutine(WaitTouch());
    }


    IEnumerator WaitTouch()
    {
        while (player)
        {
            if (interactino.run_Gimic == false) // 움직일 것
            {
                yield return new WaitUntil(() => interactino.run_Gimic == true);
                OpenPosBox();
            }
            else
            {
                yield return new WaitUntil(() => interactino.run_Gimic == false);
                ClosePosBox();
            }
        }
    }

    public void SceneStartSetting_KiaraPosBox()
    {
        if(interactino.run_Gimic)
        {
            transform.localPosition = movePos;
        }
        else
        {
            transform.localPosition = firstPos;
        }
    }
    void OpenPosBox()
    {
        transform.localPosition = Vector3.MoveTowards(firstPos, movePos, 1f);
    }

    void ClosePosBox()
    {
        transform.localPosition = Vector3.MoveTowards(movePos, firstPos, 1f);
    }
}
