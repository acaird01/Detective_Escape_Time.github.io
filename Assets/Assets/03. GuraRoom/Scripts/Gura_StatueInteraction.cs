using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_StatueInteraction : MonoBehaviour
{
    public bool run_Gimic; // ��� �۵� ����

    GameObject player; // �÷��̾�

    GameObject interaction_F;

    void Start()
    {
        player = GameObject.Find("Player");


    }

    public void Setting_Scene_Gimic(bool loadData)
    {
        // ���⼭ ���� �Ŵ����� ������ �޾Ƽ� ������ ����
        run_Gimic = loadData; // true false ����
        //gameObject.GetComponent<DoorAnimationCtrl>().SettingForObjectToInteration = run_Gimic;
        // ��ȣ�ۿ��� ������Ʈ���� bool���� �����༭ ���¿� �´� �ؽ�ó ��������
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
                //Debug.Log("��ȣ�ۿ� �Ͻðڽ��ϱ�");
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
                // interaction_F�� ���� �����ְų� null�� �ƴ� ��쿡�� ����
                if (interaction_F != null)
                {
                    interaction_F.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnMouseExit()
    {
        // interaction_F�� ���� �����ְų� null�� �ƴ� ��쿡�� ����
        if (interaction_F != null)
        {
            interaction_F.gameObject.SetActive(false);
        }
    }

    IEnumerator running() // ��� �۵����� �� 
    {
        run_Gimic = true;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator closing() // ����� �ǵ��� ����
    {
        run_Gimic = false;
        yield return new WaitForSeconds(.5f);
    }


}
