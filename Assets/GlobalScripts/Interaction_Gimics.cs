using cakeslice;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interaction_Gimics : MonoBehaviour
{
    public bool run_Gimic; // 기믹 작동 여부
    public int interactionRange; // 상호작용 가능 거리
    private bool isActiveF; // f가 위에 뜨고 있는지 확인하기 위한 bool 변수
    public bool IsActiveF   // f가 위에 뜨고 있는지 확인하기 위한 프로퍼티
    {
        get
        { 
            return isActiveF;
        }
    }

    GameObject player; // 플레이어
    GameObject interaction_F;

    cakeslice.Outline outline;

    //public GameObject[] hintChild;
    public cakeslice.Outline[] hintChild_OutLine;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        Init();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void Setting_Scene_Gimic(bool loadData)
    {
        // 여기서 게임 매니저의 정보를 받아서 정보를 토대로
        run_Gimic = loadData; // true false 세팅
        //gameObject.GetComponent<DoorAnimationCtrl>().SettingForObjectToInteration = run_Gimic;
        // 상호작용할 오브젝트에게 bool값을 던져줘서 상태에 맞는 텍스처 세팅해줌
    }

    void OnMouseOver()
    {
        if (player)
        {
            // 플레이어와 상호작용하는 오브젝트 사이의 거리
            float dist = Vector3.Distance(player.transform.position, transform.position);

            // 거리가 5보다 작을 경우 실행
            if (dist < interactionRange)
            {
                // 상호작용 가능한 물체면 하이라이트처리 출력
                //Debug.Log("상호작용 하시겠습니까");
                interaction_F.gameObject.SetActive(true);
                isActiveF = true;

                if (run_Gimic == false)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        StartCoroutine(running());
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        StartCoroutine(closing());
                    }
                }
            }
            else
            {
                // interaction_F가 현재 꺼져있거나 null이 아닐 경우에만 실행
                if (interaction_F != null)
                {
                    interaction_F.gameObject.SetActive(false);
                    isActiveF = false;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        // interaction_F가 현재 꺼져있거나 null이 아닐 경우에만 실행
        if (interaction_F != null)
        {
            interaction_F.gameObject.SetActive(false);
            isActiveF = false;
        }
    }

    private void Update()
    {
        //Debug.Log(gameObject.name + " : " + interactionRange);
        if (outline != null)
        {
            FindNearPlayer();
        }
    }

    void FindNearPlayer()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        if ((dist < interactionRange * 1.5f) && run_Gimic == false)
        {
            outline.enabled = true;
            
            if (hintChild_OutLine != null)
            {
                for (int i = 0; i < hintChild_OutLine.Length; i++)
                {
                    hintChild_OutLine[i].enabled = true;
                }
            }
        }
        else
        {            
            outline.enabled = false;

            if (hintChild_OutLine != null)
            {
                for (int i = 0; i < hintChild_OutLine.Length; i++)
                {
                    hintChild_OutLine[i].enabled = false;
                }
            }
        }
    }

    IEnumerator running() // 기믹 작동했을 때 
    {
        run_Gimic = true;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator closing() // 기믹이 되돌아 갈때
    {
        run_Gimic = false;
        yield return new WaitForSeconds(.5f);
    }

    public void Init()
    {
        player = GameObject.Find("Player");
        interaction_F = ItemManager._instance.interaction_F;    // 상호작용 가능 표시 이미지를 아이템 매니저에서 가져오기

        if (gameObject.GetComponent<cakeslice.Outline>())
        {
            outline = gameObject.GetComponent<cakeslice.Outline>();
        }
        else
        {
            if (gameObject.GetComponentInChildren<cakeslice.Outline>())
            {
                outline = gameObject.GetComponentInChildren<cakeslice.Outline>();
            }
        }

        if (GameManager.instance.nowSceneName == "04. CalliScene_1" || GameManager.instance.nowSceneName == "07. CalliScene_2")
        {
            interactionRange = 10;
        }
        else
        {
            interactionRange = 5;
        }
    }
}
