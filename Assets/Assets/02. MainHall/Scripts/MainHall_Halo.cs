using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHall_Halo : MonoBehaviour
{
    public GameObject particle;
    private Interaction_Items interaction;  // 상호작용 관리 스크립트
    private GameObject player;
    private TextController textController;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        interaction = GetComponent<Interaction_Items>();   // 해당 오브젝트에 달려있는 interaction Gimic을 가져와서 할당

//         StartCoroutine(WaitTouch());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        StartCoroutine(WaitTouch());
    }

    private void Update()
    {
        // Debug.Log(ItemManager._instance.hotkeyItemIndex);

        if (GameManager.instance.Episode_Round == 1)
        {
            if (ItemManager._instance.hotkeyItemIndex == 9)
            {
                gameObject.tag = "ITEM";
                particle.SetActive(true);
            }
            else
            {
                gameObject.tag = "Untagged";
                particle.SetActive(false);
            }
        }
        else
        {
            gameObject.tag = "ITEM";
            particle.SetActive(true);
        }
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => interaction.run_Gimic == true);

            if (ItemManager._instance.hotkeyItemIndex != 9)
            {
                StartCoroutine(textController.SendText("이나의 책을 이용해 가져올 수 있을 것 같아."));// 상호작용 대사 출력
            }

            interaction.run_Gimic = false;
        }
    }
}
