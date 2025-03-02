using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Door : MonoBehaviour
{
    public bool run_Gimic; // ��� �۵� ����
    GameObject player; // �÷��̾�
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
            // �÷��̾�� ��ȣ�ۿ��ϴ� ������Ʈ ������ �Ÿ�
            float dist = Vector3.Distance(player.transform.position, transform.position);

            // �Ÿ��� 5���� ���� ��� ����
            if (dist < 5)
            {
                // ��ȣ�ۿ� ������ ��ü�� ���̶���Ʈó�� ���
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

    IEnumerator running() // ��� �۵����� �� 
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

    IEnumerator closing() // ����� �ǵ��� ����
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
