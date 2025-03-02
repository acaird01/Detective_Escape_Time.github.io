using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmeSoloTalkManager : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ ���� ����
    private Image TextParent;               // ��縦 ����� �̹���
    private Text Interaction_Text;          // ��縦 ����� �ؽ�Ʈ
    private Image interactionTextESCImage;  // ��縦 ����� �̹����� ���� ��ư �̹���
    private Text interactionTextESCText;    // ��縦 ����� �̹����� ���� ��ư �ؽ�Ʈ

    private TextController textCtrl;  // �÷��̾� TextController
    #endregion

    #region ��ũ��Ʈ���� ����� ���� ����
    private bool coroutine_running;     // �ڷ�ƾ�� ���������� Ȯ���ϱ� ���� ����
    public bool Coroutine_running       // �ڷ�ƾ�� ���������� Ȯ���� ������ ���� �Ҵ��ϱ� ���� ������Ƽ
    {
        get
        { 
            return coroutine_running;
        }
        set
        {
            coroutine_running = value;
        }
    }
    // private bool isTalking;
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        textCtrl = GameObject.FindAnyObjectByType<TextController>();   // PlayerCtrl�� ã�ƿͼ� �Ҵ�

        TextParent = textCtrl.TextParent;              // TextController���� ã�Ƶ� ��ȣ�ۿ� Text UI�� �θ� �Ҵ�
        Interaction_Text = textCtrl.Interaction_Text;   // TextController���� ã�Ƶ� ��ȣ�ۿ� Text UI�� �Ҵ�

        // TextParent�� �ڽ����� �ִ� esc��ư �ȳ� �̹����� ���ֱ� ���� ã�ƿ�
        interactionTextESCImage = TextParent.GetComponentInChildren<InteractionTextESCImage>().GetComponent<Image>();
        interactionTextESCText = TextParent.GetComponentInChildren<InteractionTextESCText>().GetComponent<Text>();

        coroutine_running = false;  // ���� ���������� �ʴٰ� ����
        // isTalking = false;          // ���� ���ϰ� ���� �ʴٰ� ����
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// �Ƹ� ȥ�㸻�� ��½�ų �ڷ�ƾ
    /// </summary>
    /// <param �Ƹ�ȥ�㸻="_ameSoloTalk_Text"></param>
    /// <returns></returns>
    public void activeAmeSoloTalk(string _ameSoloTalk_Text)
    {
        if (TextParent) // 
        {
            if(textCtrl.coroutine_running == true)
            {
                textCtrl.SetActiveFalseText();
            }
            coroutine_running = true;
            // isTalking = true;

            TextParent.gameObject.SetActive(true);
            Interaction_Text.text = _ameSoloTalk_Text;

            // ESC ��ư �ȳ��� ����
            interactionTextESCImage.enabled = false;
            interactionTextESCText.enabled = false;

            StartCoroutine(waitAndDeactivateAmeSoloTalk()); // ��Ȱ��ȭ ��� �ڷ�ƾ ����
        }
    }

    private IEnumerator waitAndDeactivateAmeSoloTalk()
    {
        yield return new WaitForSecondsRealtime(2f);    // 2�� ���

        // ESC ��ư �ȳ��� ����
        interactionTextESCImage.enabled = true;
        interactionTextESCText.enabled = true;

        if (TextParent.gameObject.activeSelf)
        {
            TextParent.gameObject.SetActive(false);
        }
        

        // isTalking = false;
        coroutine_running = false;
    }
}
