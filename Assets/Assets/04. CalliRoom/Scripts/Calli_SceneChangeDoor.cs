using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Calli_SceneChangeDoor : MonoBehaviour
{
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    GameObject player; // 플레이어
    GameObject interaction_F;   // 상호작용이 가능한 대상일때 화면에 띄워줄 이미지
    AudioManager audioManager;

    private void Awake()
    {
        // interaction_F = GameObject.Find("F");
    }

    void Start()
    {
        player = GameObject.Find("Player");
        interaction_F = ItemManager._instance.interaction_F;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();   // Calli_ObjectManager를 찾아와서 할당
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
                        calli_ObjectManager.ChangeSceneData_To_GameManager();   // 씬 로딩하기 전 칼리씬 데이터 저장
                        GameManager.instance.SaveData();
                        GameObject halo = GameObject.FindAnyObjectByType<Item_Irys_Halo>().gameObject;
                        if(halo.gameObject.activeSelf)
                        {
                            halo.gameObject.SetActive(false);
                        }

                        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;
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
