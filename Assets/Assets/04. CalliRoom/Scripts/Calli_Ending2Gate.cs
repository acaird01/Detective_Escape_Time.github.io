using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_Ending2Gate : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    private GameObject player;
    private TextController textController;
    #endregion

    private void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // 플레이어를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();   // 상호작용시 대사 출력할 컴포넌트 할당
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 만약 1회차에서 통과하려는 대상이 플레이어인 경우 아직 지나갈 수 없다는 스크립트 재생
        if (collision.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            // 플레이어가 가지고 있는 text를 이용해 대사 출력
            StartCoroutine(textController.SendText("여긴 아직 지나갈 수 없을 것 같아."));
        }
    }
}
