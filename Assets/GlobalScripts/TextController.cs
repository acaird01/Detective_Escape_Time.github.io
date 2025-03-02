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
        TextParent.gameObject.SetActive(false); // 상호작용 텍스트
        playerCtrl = GameObject.FindAnyObjectByType<PlayerCtrl>();
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
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
    /// 텍스트 필요할 때 이거 호출하기
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
                    if (ameSoloTalkManager.Coroutine_running == false) // 지나가다 나온 텍스트가 떠있을 땐 send text 실행 X
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
    /// 상호작용 시 호출해서 대화창 끄기
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
