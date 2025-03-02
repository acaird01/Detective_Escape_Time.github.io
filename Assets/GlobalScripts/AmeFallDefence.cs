using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmeFallDefence : MonoBehaviour
{
    [Header("�Ƹ޸� ���������� ���� ��������")]
    public Transform sponPos;       // �Ƹ޸� ���������� ��������
    private GameObject player;      // �÷��̾� ���ӿ�����Ʈ
    private TextController textController;  // �÷��̾��� �ؽ�Ʈ ��Ʈ�ѷ�

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        player = GameObject.Find("Player"); // ���̾��Ű���� �÷��̾ ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region �Ƹ޸� ������������ ���������� �Լ�
    /// <summary>
    /// collision���� �Ƹ޸� ������������ ���������� �Լ�
    /// </summary>
    /// <param name="_collision"></param>
    private void MoveAmeToSponPoint(Collision _collision)
    {
        // �ε��� ����� �÷��̾��� ���
        if (_collision.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            _collision.transform.position = sponPos.position;    // ���� �̵�

            StartCoroutine(textController.SendText("��..�ð����� �帧�� �ָ����� �ʰ� �����ؾ߰ھ�!"));
        }
    }
    /// <summary>
    /// trigger���� �Ƹ޸� ������������ ���������� �Լ�
    /// </summary>
    /// <param name="_collider"></param>
    private void MoveAmeToSponPoint(Collider _collider)
    {
        // �ε��� ����� �÷��̾��� ���
        if (_collider.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            _collider.transform.position = sponPos.position;    // ���� �̵�
        }
    }
    #endregion

    #region Collision���� �Ƹ޸� �������� �Լ� ����
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

    #region Trigger���� �Ƹ޸� �������� �Լ� ����
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
