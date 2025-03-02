using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmeSoloTalk : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ ���� ����
    private PlayerCtrl playerCtrl;        // �÷��̾�
    private AmeSoloTalkManager ameSoloTalkManager;  // �Ƹ� ȥ�㸻�� �����ϴ� ��ũ��Ʈ
    private Interaction_Gimics interact_AmeSoloTalk_Object; // ȥ�㸻�� �̹� �ߴ��� �����ϱ� ���� interaction gimic
    #endregion

    #region ��ũ��Ʈ���� ����� ���� ����
    [Header("�Ƹ� ȥ�㸻�� �ۼ��� ����")]
    [TextArea] // �ν�����â�� ������ �Է��� ������ ������ �������
    public string ameSoloTalk_Text;
    private bool isAmeSoloTalkStart;    // �Ƹ� ȥ�㸻�� �����ߴ��� Ȯ���ϱ� ���� ����
    #endregion


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        playerCtrl = GameObject.FindAnyObjectByType<PlayerCtrl>();   // PlayerCtrl�� ã�ƿͼ� �Ҵ�
        ameSoloTalkManager = GameObject.FindAnyObjectByType<AmeSoloTalkManager>();  // �Ƹ� ȥ�㸻�� �����ϴ� ��ũ��Ʈ

        interact_AmeSoloTalk_Object = this.gameObject.GetComponentInParent<Interaction_Gimics>();   // �θ� ���� Interaction_Gimics�� ã�ƿͼ� �Ҵ�

        isAmeSoloTalkStart = false;     // �Ƹ޶� ȥ�㸻�� �ϰ� ���� �ʴٰ� ����

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
    private void OnTriggerEnter(Collider other)
    {
        playerCtrl = other.GetComponentInChildren<PlayerCtrl>();  // other�� �ڽ��� ���� TextController ������Ʈ ȹ�� �õ�

        // ���� ����� �÷��̾��� ���, �Ƹް� ȥ�㸻�� �ϰ� ���� ���� ��쿡�� ����
        if ((playerCtrl != null) && !isAmeSoloTalkStart)
        {
            isAmeSoloTalkStart = true;  // �߰��� ����Ǵ� ���� ������

            if (ameSoloTalkManager.Coroutine_running)
            {
                StopCoroutine("waitAndDeactivateAmeSoloTalk");
                ameSoloTalkManager.Coroutine_running = false;
            }

            // �Ƹ��� ȥ�㸻�� ����� �ڷ�ƾ ����
            // StartCoroutine(ameSoloTalkManager.activeAmeSoloTalk(ameSoloTalk_Text));
            ameSoloTalkManager.activeAmeSoloTalk(ameSoloTalk_Text);

            interact_AmeSoloTalk_Object.run_Gimic = true;   // ȥ�㸻�� �̹� ���������Ƿ� true�� ����()

            this.gameObject.SetActive(false);   // �ش� �ݶ��̴��� ��Ȱ��ȭ ���� �ι� ����� �ȵǵ��� ����.
        }
    }
}