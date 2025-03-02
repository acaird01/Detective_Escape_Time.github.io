using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Kiara_SceneChangeDoor : MonoBehaviour
{
    GameObject player; // 플레이어
    GameObject interaction_F;
    AudioManager audioManager;
    Kiara_ObjectManager objectMaanger;

    private void Awake()
    {
        // interaction_F = ItemManager._instance.interaction_F;
    }

    void Start()
    {
        // interaction_F = GameObject.Find("F");
        player = GameObject.Find("Player");
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        objectMaanger = GameObject.FindAnyObjectByType<Kiara_ObjectManager>();
        interaction_F = ItemManager._instance.interaction_F;
    }



    void OnMouseOver()
    {
        if (player)
        {
            // 플레이어와 상호작용하는 오브젝트 사이의 거리
            float dist = Vector3.Distance(player.transform.position, transform.position);

            // 거리가 5보다 작을 경우 실행
            if (dist < 5)
            {
                // 상호작용 가능한 물체면 하이라이트처리 출력
                interaction_F.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (gameObject.name == "02. MainHallScene")
                    {
                        audioManager.SaveVolume();
                        objectMaanger.ChangeSceneData_To_GamaManager();
                        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;
                        GameManager.instance.SaveData();
                        LoadingSceneManager.LoadScene(gameObject.name);
                    }
                }
            }
            else
            {
                interaction_F.gameObject.SetActive(false);
            }
        }
    }

    private void OnMouseExit()
    {
        interaction_F.gameObject.SetActive(false);
    }

}
