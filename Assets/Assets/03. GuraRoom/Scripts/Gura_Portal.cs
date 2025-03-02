    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Gura_Portal : MonoBehaviour
{
    public bool run_Gimic; // ��� �۵� ����
    GameObject player; // �÷��̾�
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
            // �÷��̾�� ��ȣ�ۿ��ϴ� ������Ʈ ������ �Ÿ�
            float dist = Vector3.Distance(player.transform.position, transform.position);

            // �Ÿ��� 5���� ���� ��� ����
            if (dist < 10)
            {
                // ��ȣ�ۿ� ������ ��ü�� ���̶���Ʈó�� ���
                Debug.Log("��ȣ�ۿ� �Ͻðڽ��ϱ�");
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

    IEnumerator running() // ��� �۵����� �� 
    {
        run_Gimic = true;
        yield return new WaitForSeconds(1f);
    }

    IEnumerator closing() // ����� �ǵ��� ����
    {
        run_Gimic = false;
        yield return new WaitForSeconds(1f);
    }
}
