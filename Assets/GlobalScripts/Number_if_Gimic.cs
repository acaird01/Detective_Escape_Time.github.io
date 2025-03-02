using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Number_if_Gimic : MonoBehaviour
{
    [Header("UI�� ���൵ ǥ�� �� ��� ������Ʈ �ֱ�")]
    [SerializeField]
    GameObject[] SceneGimics;  // �� ���� ��ġ�� �� ������ �ְ���� ��͵� ���⿡ ����
    [Header("ã�ƾ� �� Ÿ�� �̸�")]
    [SerializeField]
    string[] TakoName; // ã�ƾ� �� Ÿ�� �̸�
    [Header("ã�ƾ� �� Ÿ���� ������ �ѹ�")]
    [SerializeField]
    int[] TakoIndex; // ã�ƾ� �� Ÿ�� ������ ��ȣ

    int performed_Gimic_Num = 0; // ����� ���
    int remaining_Gimic_Num; // ���� ���
    bool cor_running = false; // �ڷ�ƾ ���������� ����
    bool firstSceneLoad = false; // ó�� �� ������ �� üũ

    [Header("��� ����? Ȯ���� �ؽ�Ʈ")]
    public Text announcement_Text; // ���� ��͵� ǥ��� �ؽ�Ʈ
    string announcement_stringText; // ���� ��� ǥ��� �ؽ�Ʈ�� ���������� ��µǴ� string
    [Header ("Ÿ�� ���Ҵ��� Ȯ���� �ؽ�Ʈ")]
    public Text Tako_Text; // Ÿ�� ã�Ҵ��� ǥ���� �ؽ�Ʈ
    string Tako_stringText; // Ÿ�� ã�Ҵ��� ǥ���� �ؽ�Ʈ�� ���������� ��µǴ� text;
    [Header("���� ��ͼ� (0 / 0)ǥ������ �ؽ�Ʈ")]
    public Text remaining_gimicNum_Text;

    string Completion_Text = " (�ϼ�)";
    //string redColor_Text = < color = "red" > </ color >

    [Header("ǥ��� ��Ϳ� ���� �䱸(����Ʈ)����")]
    [SerializeField]
    string[] gimicName; // ������ ���
    [Header("������Ʈ �Ŵ����� ������ִ� ����� �ѹ���")]
    [SerializeField]
    int[] num_In_ObjectManager; // ������Ʈ �Ŵ������� ������ִ� ����� �迭 �ѹ�

    GameObject objectManager; // �� ���� ������Ʈ �Ŵ��� ã�Ƽ� ������Ʈ�� ����
    Kiara_ObjectManager kiara_obj;
    Calli_ObjectManager calli_obj;
    Gura_ObjectManager gura_obj;


    PlayerCtrl playerCtrl;
    [SerializeField]
    GameObject panel;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }
    private void Start()
    {
        FindObjManager();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        firstSceneLoad = true;
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();

        //TextUpdate();
        remaining_Gimic_Num = SceneGimics.Length;
     
        //FirstTextSet();
        StartCoroutine(MoveFirstPanel());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void PanelOpen()
    {
        //panel.transform.localPosition = firstPos;
        panel.gameObject.SetActive(true);
        panel.GetComponent<Image>().color = new Color(0, 0, 0, 255);
    }

    public void PanelClose()
    {
        //panel.transform.localPosition = movePos;
        panel.gameObject.SetActive(false);
        panel.GetComponent<Image>().color = new Color(0, 0, 0, 128);
    }

    

    public void FindObjManager()
    {
        
        if (GameManager.instance.nowSceneName == "03. GuraScene_1" || GameManager.instance.nowSceneName == "06. GuraScene_2")
        {
            objectManager = GameObject.FindAnyObjectByType<Gura_ObjectManager>().gameObject;
            gura_obj = objectManager.GetComponent<Gura_ObjectManager>();
        }
        if (GameManager.instance.nowSceneName == "04. CalliScene_1" || GameManager.instance.nowSceneName == "07. CalliScene_2")
        {
            objectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>().gameObject;
            calli_obj = objectManager.GetComponent<Calli_ObjectManager>();
        }
        if (GameManager.instance.nowSceneName == "05. KiaraScene_1" || GameManager.instance.nowSceneName == "08. KiaraScene_2")
        {
            objectManager = GameObject.FindAnyObjectByType<Kiara_ObjectManager>().gameObject;
            kiara_obj = objectManager.GetComponent<Kiara_ObjectManager>();
        }
    }
    // < color = "red" > </ color >
    public void FirstTextSet()
    {
        announcement_stringText = null;
        performed_Gimic_Num = 0;

        if (gura_obj != null) // ����� ����
        {
            for (int i = 0; i < gimicName.Length; i++)
            {
                if (gura_obj.SceneGimics[num_In_ObjectManager[i]].GetComponent<Interaction_Gimics>().run_Gimic == true)
                {
                    announcement_stringText += string.Format("<color=#FF0000>" + gimicName[i] + Completion_Text + "</color>");
                    performed_Gimic_Num++;
                }
                else
                {
                    announcement_stringText += gimicName[i];
                }
                announcement_stringText += "\n";
            }
            remaining_gimicNum_Text.text = ("���൵ " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;            
        }
        if (calli_obj != null) // Į���� ����
        {
            for (int i = 0; i < gimicName.Length; i++)
            {
                if (calli_obj.SceneGimics[num_In_ObjectManager[i]].GetComponent<Interaction_Gimics>().run_Gimic == true)
                {
                    announcement_stringText += string.Format("<color=#FF0000>" + gimicName[i] + Completion_Text + "</color>");
                    performed_Gimic_Num++;
                }
                else
                {
                    announcement_stringText += gimicName[i];
                }
                announcement_stringText += "\n";
            }
            remaining_gimicNum_Text.text = ("���൵ " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;
        }
        if (kiara_obj != null) // Ű�ƶ�� ����
        {
            for (int i = 0; i < gimicName.Length; i++)
            {
                if (kiara_obj.SceneGimics[num_In_ObjectManager[i]].GetComponent<Interaction_Gimics>().run_Gimic == true)
                {
                    announcement_stringText += string.Format("<color=#FF0000>" + gimicName[i] + Completion_Text + "</color>");
                    performed_Gimic_Num++;
                }
                else
                {
                    announcement_stringText += gimicName[i];
                }
                announcement_stringText += "\n";
            }
            remaining_gimicNum_Text.text = ("���൵ " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;
        }

        if (TakoName.Length == 1)
        {
            TakoGet_TextUpdate(TakoIndex[0]);
        }
        else
        {
            TakoGet_TextUpdate(TakoIndex[0]);
            TakoGet_TextUpdate(TakoIndex[1]);
        }
    }

    IEnumerator MovePanel()
    {
        panel.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        panel.gameObject.SetActive(false);
    }

    IEnumerator MoveFirstPanel()
    {
        yield return new WaitForSecondsRealtime(5f);
        panel.gameObject.SetActive(false);
    }

    public void TakoGet_TextUpdate(int index) // Ÿ�� ȹ�� ������ ȣ��
    {
        cor_running = true;

        if (TakoName.Length == 1)
        {
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == true)
            {
                Tako_stringText = TakoName[0] + " Ÿ�� �߰�!";
            }
            else
            {
                Tako_stringText = TakoName[0] + " Ÿ�� ã��!";
            }
            Tako_Text.text = Tako_stringText;
        }
        else
        {
            // Ÿ�� 1��°�Ÿ� ã������
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == true && ItemManager._instance.inventorySlots[TakoIndex[1]].GetComponent<IItem>().isGetItem == false)
            {
                Tako_stringText = TakoName[0] + " Ÿ�� �߰�!\n" + TakoName[1] + " Ÿ�� ã��!";
            }
            // Ÿ�� 2��°�Ÿ� ã������
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == false && ItemManager._instance.inventorySlots[TakoIndex[1]].GetComponent<IItem>().isGetItem == true)
            {
                Tako_stringText = TakoName[0] + " Ÿ�� ã��!\n" + TakoName[1] + " Ÿ�� �߰�!";
            }
            // Ÿ�� �Ѵ� ã������
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == true && ItemManager._instance.inventorySlots[TakoIndex[1]].GetComponent<IItem>().isGetItem == true)
            {
                Tako_stringText = TakoName[0] + " Ÿ�� �߰�!\n" + TakoName[1] + " Ÿ�� �߰�!";
            }
            // �Ѵ� ��ã������
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == false && ItemManager._instance.inventorySlots[TakoIndex[1]].GetComponent<IItem>().isGetItem == false)
            {
                Tako_stringText = TakoName[0] + " Ÿ�� ã��!\n" + TakoName[1] + " Ÿ�� ã��!";
            }
            Tako_Text.text = Tako_stringText;
        }


        if (cor_running == true)
        {
            StopCoroutine("MovePanel");
        }
        StartCoroutine(MovePanel());

        cor_running = false;
    }

    void performed_Gimic_Num_SceneStartSet()
    {
        if (gura_obj != null)
        {
            for (int i = 0; i < gimicName.Length; i++)
            {
                if (gura_obj.SceneGimics[num_In_ObjectManager[i]].GetComponent<Interaction_Gimics>().run_Gimic == true)
                {
                    performed_Gimic_Num++;
                }
            }
        }
        if (calli_obj != null)
        {
            for (int i = 0; i < gimicName.Length; i++)
            {
                if (calli_obj.SceneGimics[num_In_ObjectManager[i]].GetComponent<Interaction_Gimics>().run_Gimic == true)
                {
                    performed_Gimic_Num++;
                }
            }
        }
        if (kiara_obj != null)
        {
            for (int i = 0; i < gimicName.Length; i++)
            {
                if (kiara_obj.SceneGimics[num_In_ObjectManager[i]].GetComponent<Interaction_Gimics>().run_Gimic == true)
                {
                    performed_Gimic_Num++;
                }
            }
        }
    }

    public void TextUpdate() // ������ ��� ���� ������ �̰� ȣ���ϱ�
    {
        cor_running = true;
        // if (!firstSceneLoad)
        {
            if (performed_Gimic_Num <= SceneGimics.Length)
            {
                performed_Gimic_Num++;
            }
            if (remaining_Gimic_Num >= 1)
            {
                remaining_Gimic_Num = SceneGimics.Length - performed_Gimic_Num;
            }
        }
        //else
        {
            // performed_Gimic_Num_SceneStartSet();
        }
        announcement_stringText = null;
        if (gura_obj != null) // ����� �ʱ�ȭ
        {
            for (int i = 0; i < gimicName.Length; i++)
            {
                if (gura_obj.SceneGimics[num_In_ObjectManager[i]].GetComponent<Interaction_Gimics>().run_Gimic == true)
                {
                    announcement_stringText += string.Format("<color=#FF0000>" + gimicName[i] + Completion_Text + "</color>");
                }
                else
                {
                    announcement_stringText += gimicName[i];
                }
                announcement_stringText += "\n";
            }
            remaining_gimicNum_Text.text = ("���൵ " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;
        }
        if (calli_obj != null) // Į���� �ʱ�ȭ
        {
            for (int i = 0; i < gimicName.Length; i++)
            {
                if (calli_obj.SceneGimics[num_In_ObjectManager[i]].GetComponent<Interaction_Gimics>().run_Gimic == true)
                {
                    announcement_stringText += string.Format("<color=#FF0000>" + gimicName[i] + Completion_Text + "</color>");
                }
                else
                {
                    announcement_stringText += gimicName[i];
                }
                announcement_stringText += "\n";
            }
            remaining_gimicNum_Text.text = ("���൵ " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;
        }
        if (kiara_obj != null) // Ű�ƶ�� �ʱ�ȭ
        {
            for (int i = 0; i < gimicName.Length; i++)
            {
                if (kiara_obj.SceneGimics[num_In_ObjectManager[i]].GetComponent<Interaction_Gimics>().run_Gimic == true)
                {
                    announcement_stringText += string.Format("<color=#FF0000>" + gimicName[i] + Completion_Text + "</color>");
                }
                else
                {
                    announcement_stringText += gimicName[i];
                }
                announcement_stringText += "\n";
            }
            remaining_gimicNum_Text.text = ("���൵ " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;
        }

        if (cor_running == true)
        {
            StopCoroutine("MovePanel");
        }
        StartCoroutine(MovePanel());
        firstSceneLoad = false;
        cor_running = false;
    }

}
