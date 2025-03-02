using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmeFallDefence : MonoBehaviour
{
    [Header("아메를 돌려보내줄 씬의 스폰지점")]
    public Transform sponPos;       // 아메를 돌려보내줄 스폰지점
    private GameObject player;      // 플레이어 게임오브젝트
    private TextController textController;  // 플레이어의 텍스트 컨트롤러

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        player = GameObject.Find("Player"); // 하이어라키에서 플레이어를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region 아메를 스폰지점으로 돌려보내줄 함수
    /// <summary>
    /// collision에서 아메를 스폰지점으로 돌려보내줄 함수
    /// </summary>
    /// <param name="_collision"></param>
    private void MoveAmeToSponPoint(Collision _collision)
    {
        // 부딪힌 대상이 플레이어인 경우
        if (_collision.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            _collision.transform.position = sponPos.position;    // 강제 이동

            StartCoroutine(textController.SendText("윽..시공간의 흐름에 휘말리지 않게 조심해야겠어!"));
        }
    }
    /// <summary>
    /// trigger에서 아메를 스폰지점으로 돌려보내줄 함수
    /// </summary>
    /// <param name="_collider"></param>
    private void MoveAmeToSponPoint(Collider _collider)
    {
        // 부딪힌 대상이 플레이어인 경우
        if (_collider.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            _collider.transform.position = sponPos.position;    // 강제 이동
        }
    }
    #endregion

    #region Collision에서 아메를 돌려보낼 함수 모음
    private void OnCollisionEnter(Collision collision)
    {
        MoveAmeToSponPoint(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        MoveAmeToSponPoint(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        MoveAmeToSponPoint(collision);
    }
    #endregion

    #region Trigger에서 아메를 돌려보낼 함수 모음
    private void OnTriggerEnter(Collider other)
    {
        MoveAmeToSponPoint(other);
    }

    private void OnTriggerStay(Collider other)
    {
        MoveAmeToSponPoint(other);
    }

    private void OnTriggerExit(Collider other)
    {
        MoveAmeToSponPoint(other);
    }
    #endregion
}
