using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_Ending2Gate : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    private GameObject player;
    private TextController textController;
    #endregion

    private void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // �÷��̾ ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();   // ��ȣ�ۿ�� ��� ����� ������Ʈ �Ҵ�
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� 1ȸ������ ����Ϸ��� ����� �÷��̾��� ��� ���� ������ �� ���ٴ� ��ũ��Ʈ ���
        if (collision.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            // �÷��̾ ������ �ִ� text�� �̿��� ��� ���
            StartCoroutine(textController.SendText("���� ���� ������ �� ���� �� ����."));
        }
    }
}
