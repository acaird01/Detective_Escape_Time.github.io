using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction_Items : MonoBehaviour
{
    public bool run_Gimic; // ��� �۵� ����
    GameObject player; // �÷��̾�
    GameObject interaction_F;

    public int interactionRange; // ��ȣ�ۿ� ���� �Ÿ�

    TextController textConteroller;
    [TextArea]
    public string _text = ""; // ������ or text�� ���� ������Ʈ�� ����ִ� string �ؽ�Ʈ �� ������ ����

    public GameObject audioSource_Object = null; // ����� �ҽ��� ������� ��� ���⿡ �ش� ������Ʈ�� �־�����

    [SerializeField]
    GameObject ItemEffect;

    int itemIndex = 0;
    IItem itemData; 

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        Init();

        textConteroller = player.GetComponentInChildren<TextController>();

        Setting_Scene_Gimic();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (itemIndex == 0)
        {
            if (gameObject.GetComponent<IItem>() != null)
            {
                itemIndex = gameObject.GetComponent<IItem>().Index;
                itemData = ItemManager._instance.inventorySlots[itemIndex].GetComponent<IItem>();
            }
        }

        if (ItemEffect != null)
        {
            FindNearPlayer();
        }
    }

    void Setting_Scene_Gimic()
    {
        // ���⼭ ���� �Ŵ����� ������ �޾Ƽ� ������ ����
        run_Gimic = false; // true false ����
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
                interaction_F.gameObject.SetActive(true);

                if (run_Gimic == false)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (gameObject.tag == "ITEM")
                        {
                            StartCoroutine(running());
                        }
                        else
                        {
                            SendText();
                            AudioSourcePlay();
                        }
                    }
                }
            }
            else
            {
                if(gameObject.GetComponent<IItem>() != null)
                {
                    itemIndex = itemData.Index;
                }
                interaction_F.gameObject.SetActive(false);
            }
        }
    }

    void FindNearPlayer()
    {
        if (itemIndex != 0)
        {
            if (itemData.isGetItem == false)
            {
                float dist = Vector3.Distance(player.transform.position, transform.position);

                if (dist < interactionRange)
                {
                    ItemEffect.gameObject.SetActive(true);
                }
                else
                {
                    ItemEffect.gameObject.SetActive(false);
                }
            }
            else
            {                
                ItemEffect.gameObject.SetActive(false);
            }
        }
    }


    private void OnMouseExit()
    {
        interaction_F.gameObject.SetActive(false);
    }

    void SendText() // ������(��ȣ�ۿ� �ϴ� ������Ʈ)���� �ؽ�Ʈ �޾ƿͼ� �ؽ�Ʈ ���
    {
        if (_text != "")
        {
            StartCoroutine(textConteroller.SendText(_text));
        }
    }

    void AudioSourcePlay() // ���� ��� �ϰ������ ����� �ҽ� ����ִ� ������Ʈ�� interaction_item��ũ��Ʈ �ְ�, getcomponent�ؼ� audioSource_Object = gameObject.this << �̰� start�� ���ָ��
    {
        if (audioSource_Object != null)
        {
            if (audioSource_Object.GetComponent<AudioSource>() != null)
            {
                AudioSource audioSource = audioSource_Object.GetComponent<AudioSource>();

                if (audioSource)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
            }
        }
    }
    IEnumerator running() // ��� �۵����� �� 
    {
        ItemManager._instance.CollectItem(this.gameObject);
        // Debug.Log("��ȣ�ۿ�: " + this.gameObject.name);

        run_Gimic = true;
        yield return new WaitForSeconds(.1f);

        if (interaction_F)
        {
            interaction_F.gameObject.SetActive(false); // ���������� ��Ȱ��ȭ
        }

        this.gameObject.SetActive(false);
    }

    public void Init()
    {
        player = GameObject.Find("Player");
        interaction_F = ItemManager._instance.interaction_F;    // ��ȣ�ۿ� ���� ǥ�� �̹����� ������ �Ŵ������� ��������  

        if (gameObject.GetComponent<IItem>() != null)
        {
            //itemIndex = ItemManager._instance.GetComponent<IItem>().Index;
            //itemIndex = gameObject.GetComponent<IItem>().Index;
            itemIndex = gameObject.GetComponent<IItem>().Index;
            itemData = ItemManager._instance.inventorySlots[itemIndex].GetComponent<IItem>();
        }
        if(ItemEffect != null)
        {
            ItemEffect.gameObject.SetActive(false);
        }

        if (GameManager.instance.nowSceneName == "04. CalliScene_1" || GameManager.instance.nowSceneName == "07. CalliScene_2")
        {
            interactionRange = 10;
        }
        else
        {
            interactionRange = 5;
        }

        //StartCoroutine(FindNearPlayer());
    }
}
