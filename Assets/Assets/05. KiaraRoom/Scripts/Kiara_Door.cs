using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Door : MonoBehaviour
{
    public bool run_Gimic; // 기믹 작동 여부
    GameObject player; // 플레이어
    GameObject interaction_F;
    Kiara_MainDoor kiara_MainDoor;
    Animation animation;
    AudioSource audioSource;


    void Start()
    {
        animation = gameObject.GetComponentInParent<Animation>();
        kiara_MainDoor = gameObject.GetComponentInParent<Kiara_MainDoor>();
        audioSource = gameObject.GetComponent<AudioSource>();
        interaction_F = ItemManager._instance.interaction_F;

        player = GameObject.Find("Player");
        run_Gimic = false;

    }

    private void Update()
    {
        if (kiara_MainDoor != null)
        {
            run_Gimic = kiara_MainDoor.DoorOpen;
        }
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
        animation.CrossFade("Open");
        if (kiara_MainDoor != null)
        {
            kiara_MainDoor.DoorOpen = true;
            audioSource.Play();
        }
        else
        {
            run_Gimic = true;
            audioSource.Play();
        }
        yield return new WaitForSeconds(1f);
    }

    IEnumerator closing() // 기믹이 되돌아 갈때
    {
        animation.CrossFade("Close");

        if (kiara_MainDoor != null)
        {
            kiara_MainDoor.DoorOpen = false;
        }
        else
        {
            run_Gimic = false;
        }
        yield return new WaitForSeconds(1f);
    }
}
