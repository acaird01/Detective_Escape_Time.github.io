using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_StatueInteraction : MonoBehaviour
{
    public bool run_Gimic; // 기믹 작동 여부

    GameObject player; // 플레이어

    GameObject interaction_F;

    void Start()
    {
        player = GameObject.Find("Player");


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
            if (dist < 5)
            {
                // 상호작용 가능한 물체면 하이라이트처리 출력
                //Debug.Log("상호작용 하시겠습니까");
                interaction_F.gameObject.SetActive(true);

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


}
