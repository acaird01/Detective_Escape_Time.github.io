using cakeslice;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interaction_Gimics : MonoBehaviour
{
    public bool run_Gimic; // ��� �۵� ����
    public int interactionRange; // ��ȣ�ۿ� ���� �Ÿ�
    private bool isActiveF; // f�� ���� �߰� �ִ��� Ȯ���ϱ� ���� bool ����
    public bool IsActiveF   // f�� ���� �߰� �ִ��� Ȯ���ϱ� ���� ������Ƽ
    {
        get
        { 
            return isActiveF;
        }
    }

    GameObject player; // �÷��̾�
    GameObject interaction_F;

    cakeslice.Outline outline;

    //public GameObject[] hintChild;
    public cakeslice.Outline[] hintChild_OutLine;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        Init();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
            if (dist < interactionRange)
            {
                // ��ȣ�ۿ� ������ ��ü�� ���̶���Ʈó�� ���
                //Debug.Log("��ȣ�ۿ� �Ͻðڽ��ϱ�");
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
                // interaction_F�� ���� �����ְų� null�� �ƴ� ��쿡�� ����
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
        // interaction_F�� ���� �����ְų� null�� �ƴ� ��쿡�� ����
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

    public void Init()
    {
        player = GameObject.Find("Player");
        interaction_F = ItemManager._instance.interaction_F;    // ��ȣ�ۿ� ���� ǥ�� �̹����� ������ �Ŵ������� ��������

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
