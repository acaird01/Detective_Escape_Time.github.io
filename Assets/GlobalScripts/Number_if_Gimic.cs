using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Number_if_Gimic : MonoBehaviour
{
    [Header("UI에 진행도 표기 될 기믹 오브젝트 넣기")]
    [SerializeField]
    GameObject[] SceneGimics;  // 각 씬에 배치할 때 데이터 주고받을 기믹들 여기에 저장
    [Header("찾아야 할 타코 이름")]
    [SerializeField]
    string[] TakoName; // 찾아야 할 타코 이름
    [Header("찾아야 할 타코의 아이템 넘버")]
    [SerializeField]
    int[] TakoIndex; // 찾아야 할 타코 아이템 번호

    int performed_Gimic_Num = 0; // 수행된 기믹
    int remaining_Gimic_Num; // 남은 기믹
    bool cor_running = false; // 코루틴 실행중인지 여부
    bool firstSceneLoad = false; // 처음 씬 시작일 떄 체크

    [Header("기믹 내용? 확인할 텍스트")]
    public Text announcement_Text; // 남은 기믹들 표기될 텍스트
    string announcement_stringText; // 남은 기믹 표기될 텍스트에 최종적으로 출력되는 string
    [Header ("타코 남았는지 확인할 텍스트")]
    public Text Tako_Text; // 타코 찾았는지 표기할 텍스트
    string Tako_stringText; // 타코 찾았는지 표기할 텍스트에 최종적으로 출력되는 text;
    [Header("남은 기믹수 (0 / 0)표기해줄 텍스트")]
    public Text remaining_gimicNum_Text;

    string Completion_Text = " (완성)";
    //string redColor_Text = < color = "red" > </ color >

    [Header("표기될 기믹에 대한 요구(퀘스트)내용")]
    [SerializeField]
    string[] gimicName; // 수행할 기믹
    [Header("오브젝트 매니저에 저장되있는 기믹의 넘버링")]
    [SerializeField]
    int[] num_In_ObjectManager; // 오브젝트 매니저에서 저장되있는 기믹의 배열 넘버

    GameObject objectManager; // 각 씬에 오브젝트 매니저 찾아서 오브젝트에 저장
    Kiara_ObjectManager kiara_obj;
    Calli_ObjectManager calli_obj;
    Gura_ObjectManager gura_obj;


    PlayerCtrl playerCtrl;
    [SerializeField]
    GameObject panel;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }
    private void Start()
    {
        FindObjManager();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
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

        if (gura_obj != null) // 구라씬 세팅
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
            remaining_gimicNum_Text.text = ("진행도 " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;            
        }
        if (calli_obj != null) // 칼리씬 세팅
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
            remaining_gimicNum_Text.text = ("진행도 " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;
        }
        if (kiara_obj != null) // 키아라씬 세팅
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
            remaining_gimicNum_Text.text = ("진행도 " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
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

    public void TakoGet_TextUpdate(int index) // 타코 획득 시점에 호출
    {
        cor_running = true;

        if (TakoName.Length == 1)
        {
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == true)
            {
                Tako_stringText = TakoName[0] + " 타코 발견!";
            }
            else
            {
                Tako_stringText = TakoName[0] + " 타코 찾기!";
            }
            Tako_Text.text = Tako_stringText;
        }
        else
        {
            // 타코 1번째거만 찾았을때
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == true && ItemManager._instance.inventorySlots[TakoIndex[1]].GetComponent<IItem>().isGetItem == false)
            {
                Tako_stringText = TakoName[0] + " 타코 발견!\n" + TakoName[1] + " 타코 찾기!";
            }
            // 타코 2번째거만 찾았을때
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == false && ItemManager._instance.inventorySlots[TakoIndex[1]].GetComponent<IItem>().isGetItem == true)
            {
                Tako_stringText = TakoName[0] + " 타코 찾기!\n" + TakoName[1] + " 타코 발견!";
            }
            // 타코 둘다 찾았을때
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == true && ItemManager._instance.inventorySlots[TakoIndex[1]].GetComponent<IItem>().isGetItem == true)
            {
                Tako_stringText = TakoName[0] + " 타코 발견!\n" + TakoName[1] + " 타코 발견!";
            }
            // 둘다 못찾았을때
            if (ItemManager._instance.inventorySlots[TakoIndex[0]].GetComponent<IItem>().isGetItem == false && ItemManager._instance.inventorySlots[TakoIndex[1]].GetComponent<IItem>().isGetItem == false)
            {
                Tako_stringText = TakoName[0] + " 타코 찾기!\n" + TakoName[1] + " 타코 찾기!";
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

    public void TextUpdate() // 저장할 기믹 종료 시점에 이거 호출하기
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
        if (gura_obj != null) // 구라씬 초기화
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
            remaining_gimicNum_Text.text = ("진행도 " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;
        }
        if (calli_obj != null) // 칼리씬 초기화
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
            remaining_gimicNum_Text.text = ("진행도 " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
            announcement_Text.text = announcement_stringText;
        }
        if (kiara_obj != null) // 키아라씬 초기화
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
            remaining_gimicNum_Text.text = ("진행도 " + performed_Gimic_Num.ToString() + " / " + SceneGimics.Length.ToString());
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
