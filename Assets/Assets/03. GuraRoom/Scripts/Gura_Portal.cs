    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Gura_Portal : MonoBehaviour
{
    public bool run_Gimic; // 기믹 작동 여부
    GameObject player; // 플레이어
    GameObject interaction_F;

    private void Awake()
    {
        // interaction_F = GameObject.Find("F");
    }

    void Start()
    {
        // interaction_F = GameObject.Find("F");
        player = GameObject.Find("Player");
        run_Gimic = false;
        interaction_F = ItemManager._instance.interaction_F;
    }

    private void Update()
    {
        GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0.2f, 0) * Time.deltaTime;
    }



    void OnMouseOver()
    {
        if (player)
        {
            // 플레이어와 상호작용하는 오브젝트 사이의 거리
            float dist = Vector3.Distance(player.transform.position, transform.position);

            // 거리가 5보다 작을 경우 실행
            if (dist < 10)
            {
                // 상호작용 가능한 물체면 하이라이트처리 출력
                Debug.Log("상호작용 하시겠습니까");
                interaction_F.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (gameObject.name == "02. MainHallScene")
                    {
                        player.transform.localScale = new Vector3(1, 1, 1);

                        //find obj named "Gura_ObjectManager" and call ChangeSceneData_To_GamaManager() method
                        GameObject.Find("Gura_ObjectManager").GetComponent<Gura_ObjectManager>().ChangeSceneData_To_GamaManager();
                        GameObject.Find("AudioManager").GetComponent<AudioManager>().SaveVolume();

                        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;
                        GameManager.instance.SaveData();
                        LoadingSceneManager.LoadScene(gameObject.name);
                    }   
                    if (run_Gimic == false)
                    {
                        StartCoroutine(running());
                    }
                    else
                    {
                        StartCoroutine(closing());
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

    IEnumerator running() // 기믹 작동했을 때 
    {
        run_Gimic = true;
        yield return new WaitForSeconds(1f);
    }

    IEnumerator closing() // 기믹이 되돌아 갈때
    {
        run_Gimic = false;
        yield return new WaitForSeconds(1f);
    }
}
