using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockBeforeEnding : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ ���� ����
    private PlayerCtrl playerCtrl;        // �÷��̾�
    private AmeSoloTalkManager ameSoloTalkManager;  // �Ƹ� ȥ�㸻�� �����ϴ� ��ũ��Ʈ
    public Interaction_Gimics interact_AmeSoloTalk_Object; // ȥ�㸻�� �̹� �ߴ��� �����ϱ� ���� interaction gimic
    private TextController textController;  // �ؽ�Ʈ ��Ʈ�ѷ�
    #endregion

    #region ��ũ��Ʈ���� ����� ���� ����
    [Header("�Ƹ� ȥ�㸻�� �ۼ��� ����")]
    [TextArea] // �ν�����â�� ������ �Է��� ������ ������ �������
    public string ameSoloTalk_Text;
    #endregion


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        playerCtrl = GameObject.FindAnyObjectByType<PlayerCtrl>();   // PlayerCtrl�� ã�ƿͼ� �Ҵ�
        ameSoloTalkManager = GameObject.FindAnyObjectByType<AmeSoloTalkManager>();  // �Ƹ� ȥ�㸻�� �����ϴ� ��ũ��Ʈ
        textController = playerCtrl.gameObject.GetComponentInChildren<TextController>();    // �ؽ�Ʈ ��Ʈ�ѷ��� ã�ƿͼ� �Ҵ�

        interact_AmeSoloTalk_Object = this.gameObject.GetComponentInParent<Interaction_Gimics>();   // �θ� ���� Interaction_Gimics�� ã�ƿͼ� �Ҵ�

        Setting();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Setting()
    {
        if (interact_AmeSoloTalk_Object.run_Gimic)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    // �ش� �ݶ��̴��� ����� ���
    private void OnCollisionEnter(Collision collision)
    {
        playerCtrl = collision.gameObject.GetComponentInChildren<PlayerCtrl>();  // other�� �ڽ��� ���� TextController ������Ʈ ȹ�� �õ�

        // ���� ����� �÷��̾��� ���, �Ƹް� ȥ�㸻�� �ϰ� ���� ���� ��쿡�� ����
        if ((playerCtrl != null))
        {
            StartCoroutine(textController.SendText(ameSoloTalk_Text));  // ���
        }
    }
}