using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_FixClock_Check : MonoBehaviour
{
    Interaction_Gimics interaction;
    Calli_DialLockScene2 dialLock;
    TextController textController;
    GameObject player;
    BoxCollider boxCollider;

    string before_Obtaining = "자물쇠가 한 조각이 모자라.";
    string after_Obtaining = "딱 맞는 조각인 것 같네.";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        dialLock = gameObject.GetComponentInChildren<Calli_DialLockScene2>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        SceneStartSetting_Calli_FixClock_Check();
        StartCoroutine(WaitTouch());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    IEnumerator WaitTouch() // clock들 정답 모두 맞으면 의 true값이 들어와 이 스크립트가 실행될 때 gimicmove가 true가 될것
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (ItemManager._instance.hotkeyItemIndex == 26)
                {
                    // 자물쇠 조각을 인벤토리로 되돌려줌
                    ItemManager._instance.ReturnItem(26);
                    ItemManager._instance.DeactivateItem(26);

                    StartCoroutine(textController.SendText(after_Obtaining));
                    dialLock.fixClock_Check = true;
                    boxCollider.enabled = false;
                    break;
                }
                else
                {
                    StartCoroutine(textController.SendText(before_Obtaining));
                }
            }
            else
            {
                interaction.run_Gimic = false;
            }
        }
    }

    public void SceneStartSetting_Calli_FixClock_Check() 
    {
        if (interaction.run_Gimic)
        {
            dialLock.fixClock_Check = true;
            gameObject.GetComponent<Calli_FixClock_Check>().enabled = false; // 기믹수행했으면 자기자신 끄기
        }
    }
}
