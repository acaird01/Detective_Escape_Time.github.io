using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public Image TextParent;
    public Text Interaction_Text;

    float closeTextDelay = 3f;

    public bool coroutine_running = false;

    PlayerCtrl playerCtrl;
    AmeSoloTalkManager ameSoloTalkManager;
    // Start is called before the first frame update
    void Start()
    {
        TextParent.gameObject.SetActive(false); // ��ȣ�ۿ� �ؽ�Ʈ
        playerCtrl = GameObject.FindAnyObjectByType<PlayerCtrl>();
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        if (GameObject.FindAnyObjectByType<AmeSoloTalkManager>())
        {
            ameSoloTalkManager = GameObject.FindAnyObjectByType<AmeSoloTalkManager>();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// �ؽ�Ʈ �ʿ��� �� �̰� ȣ���ϱ�
    /// </summary>
    /// <param name="_text"></param>
    /// <returns></returns>

    private void Update()
    {
        if(TextParent.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) 
            {
                TextParent.gameObject.SetActive(false);

                coroutine_running = false;
                playerCtrl.keystrokes = false;
            }
        }
    }

    public IEnumerator SendText(string _text)
    {
        if (!coroutine_running)
        {
            if (TextParent)
            {
                if (ameSoloTalkManager != null)
                {
                    if (ameSoloTalkManager.Coroutine_running == false) // �������� ���� �ؽ�Ʈ�� ������ �� send text ���� X
                    {
                        coroutine_running = true;
                        playerCtrl.keystrokes = true;

                        TextParent.gameObject.SetActive(true);
                        Interaction_Text.text = _text;
                        yield return new WaitForSecondsRealtime(closeTextDelay);
                        TextParent.gameObject.SetActive(false);

                        coroutine_running = false;
                        playerCtrl.keystrokes = false;
                    }
                }
            }
        }
        else
        {
            StopCoroutine("SendText");
        }
    }

    /// <summary>
    /// ��ȣ�ۿ� �� ȣ���ؼ� ��ȭâ ����
    /// </summary>
    public void SetActiveFalseText()
    {
        TextParent.gameObject.SetActive(false); 
        if(coroutine_running)
        {
            StopCoroutine("SendText");
        }
        coroutine_running = false;
        playerCtrl.keystrokes = false;
    }

}
