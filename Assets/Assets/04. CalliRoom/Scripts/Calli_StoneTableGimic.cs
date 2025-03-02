using System;
using System.Collections.Generic;
using UnityEngine;

public class Calli_StoneTableGimic : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private GameObject key;                            // ������ ����� Ǯ������ ��ȯ���� ���� GameObject
    private Calli_WaitingDeadBeat waitingDeadBeat;     // 2ȸ������ ��ġ��Ʈ�� �� ������� ��ũ��Ʈ
    private Calli_WhichSongPlayBook whichSongPlayBook; // 2ȸ������ ������ ��ġ�� �Ϸ�� �� �뷡����� ���� ��ũ��Ʈ

    [Header("��� ���� �� ��������")]
    public GameObject[] floor_Tapes;
    [Header("��� �Ϸ� ���¸� ������ ��������")]
    public GameObject[] table_Tapes;
    #endregion

    #region �� �� å�󿡼� ����� Action
    private Action InitTapeHoles;      // �� ������ ĭ�� �ʱ�ȭ�� �Լ��� ������ Aciton
    public void SetInitTapeHole(Action _InitTapeHoles)     // InitTapeHoles�� �Լ��� ������ ĭ���� �Ҵ��� set �Լ�
    {
        InitTapeHoles += _InitTapeHoles;
    }

    private Action DestroyWrongPlaceTapes;  // �������� ���� �߸��� ��ġ�� ���������� ������ ��� ������ Action
    public void SetDestroyWrongPlaceTapes_CallBack(Action _DestroyWrongPlaceTapes) // DestroyWrongPlaceTapes�� �Լ��� ������ ĭ���� �Ҵ��� set �Լ�
    {
        DestroyWrongPlaceTapes += _DestroyWrongPlaceTapes;
    }
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private const string summon_KeyName = "Item_34_ChestKey";   // ��ȯ�� ���� �̸�
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    public bool isTapeReachMaxNum;              // �ִ� ������ ���� �����ߴ��� Ȯ���ϱ� ���� ����
    private const int maxTapeNum_ending1 = 4;   // ����1�� �ִ� ������ ��
    private const int maxTapeNum_ending2 = 4;   // ����2�� �ִ� ������ ��
    private int maxTapes;                       // ������ ���� ü���ǿ� ���� �ִ� Ÿ�ڼ�
    public int MaxTapes                         // �ܺο��� �ִ� Ÿ�ڼ��� Ȯ���ϱ� ���� ������Ƽ(Calli_CheckTapePos���� ���)
    {
        get
        {
            return maxTapes;
        }
    }
    private int countTapes;                     // ���� ������ ĭ�� ���� ������ ��
    public int CountTapes                       // ���� ������ ĭ�� ���� ������ �� ������Ƽ(Calli_CheckTapePos���� ���)
    {
        get
        {
            return countTapes;
        }
        set
        {
            if (countTapes < maxTapes)          // �ִ� ������ ������ ���� ������ ���� �������� �Ҵ��� �� �ְԲ� ����
            {
                countTapes = value;
            }
        }
    }

    // ����� ���������� �����۹�ȣ �� ��ġ�� ������ ����ü
    private struct AnswerTapeInfo
    {
        public int TapeIndex;         // ���� ü���ǿ� ���� Ÿ���� ������ �ε��� ��ȣ�� ������ ����ü ���
        public string TapePosition;   // ���� ü���ǿ� ���� Ÿ���� ü���� ��ġ�� ������ ����ü ���
    };
    // answerTapeNumbers ����ü�� ���� �迭 ����
    private AnswerTapeInfo[] answerTableHoleTape;
    // ����ü�� ���� �Ҵ��ϱ� ���� set �Լ�(Calli_CheckTapePos���� ���)
    public void SetAnswerCasetteTape(int _tapeIndex, string _tapePosition)
    {
        Debug.Log("countTapes : " + countTapes);//test

        if (countTapes < maxTapes)  // �ִ� ������������ ���� ���������� �������� ����
        {
            answerTableHoleTape[countTapes].TapeIndex = _tapeIndex;
            answerTableHoleTape[countTapes].TapePosition = _tapePosition;
            countTapes++;
            Debug.Log("countTapes+ : " + countTapes);//test
        }
        else
        {
            Debug.Log("AnswerCasetteTape : " + answerTableHoleTape.Length);
        }
    }

    // ī��Ʈ ������ ���� ����� Dictionary(�����)
    private Dictionary<int, string> CorrectTape_ending1 = new Dictionary<int, string>();  // ����1���� ����� ���� ��ġ(���� 4ĭ�� ���)
    private Dictionary<int, string> CorrectTape_ending2 = new Dictionary<int, string>();  // ����2���� ����� ���� ��ġ(���� 5ĭ�� ���)

    private bool isStoneTableGimicEnding;    // �� ���̺��� ������ ���缭 ����� ���� �Ϸ�Ǿ����� Ȯ���ϱ� ���� ����(true : ��� ���� �޼� / false : ��� ���� �̴޼�)
    public bool IsStoneTableGimicEnding      // �� ���̺��� ������ ������� �����ϱ� ���� ������Ƽ(???���� ���)
    {
        set
        {
            isStoneTableGimicEnding = value;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");                                             // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();               // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�

        Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    // �ʱ�ȭ �Լ�
    private void Init()
    {
        interaction = GetComponent<Interaction_Gimics>();               // �ڽſ��� �پ��ִ� interaction gimic�� �Ҵ�
        key = GetComponentInChildren<Item34ChestKey>().gameObject;      // ���� ��ġ�� ���踦 ã�ƿͼ� �Ҵ�

        settingGimic = interaction.run_Gimic;   // ��� ���� ���� �ʱ�ȭ
        isStoneTableGimicEnding = key.GetComponent<Item34ChestKey>().isGetItem; // ������� ��ȯ�Ǿ����� Ȯ���ϱ� ���� ���� �ʱ�ȭ

        countTapes = 0; // �� å�� ���� ������ �� 0���� �ʱ�ȭ
        // �� å�� ���� ������ �� �ʱ�ȭ
        if (calli_ObjectManager.CheckEndingEpisode_Num == 1) // 1ȸ���� ���
        {
            maxTapes = maxTapeNum_ending1;
        }
        else if (calli_ObjectManager.CheckEndingEpisode_Num == 2) // 2ȸ���� ���
        {
            maxTapes = maxTapeNum_ending2;
        }
        #region �� å�� ����� ����
        // ����� ��ųʸ� �ʱ�ȭ
        CorrectTape_ending1?.Clear();
        CorrectTape_ending2?.Clear();

        // 1ȸ�� �� å�� ���� �迭 �ʱ�ȭ
        // ���� ������ �ε��� �� ��ġ
        CorrectTape_ending1.Add(21, "StoneTable_Hole_M");  // M
        CorrectTape_ending1.Add(22, "StoneTable_Hole_Y");  // Y
        CorrectTape_ending1.Add(23, "StoneTable_Hole_T");  // T
        CorrectTape_ending1.Add(24, "StoneTable_Hole_H");  // H

        // 2ȸ�� �� å�� ���� �迭 �ʱ�ȭ
        // ���� ������ �ε��� �� ��ġ
        CorrectTape_ending2.Add(23, "StoneTable_Hole_M");  // T 2021. 8. 2.
        CorrectTape_ending2.Add(24, "StoneTable_Hole_Y");  // H 2022. 3. 20.
        CorrectTape_ending2.Add(21, "StoneTable_Hole_T");  // M 2022. 5. 18.
        CorrectTape_ending2.Add(22, "StoneTable_Hole_H");  // Y 2023. 8. 17.
        #endregion

        answerTableHoleTape = new AnswerTapeInfo[maxTapes];   // �� å�� ���� ���������� ������ �޾� ������ ����ü �迭 �ʱ�ȭ

        InitTapeHoles?.Invoke();   // ������ ���� ĭ�� �ʱ�ȭ �Լ��� ����

        // 2ȸ���� ��� ������ ������ ��ٸ��� ��������� �ʱ�ȭ �Լ� ȣ��
        if (GameManager.instance.Episode_Round == 2)
        {
            whichSongPlayBook = GameObject.FindAnyObjectByType<Calli_WhichSongPlayBook>();  // �뷡 ����� ���� å ��ũ��Ʈ �Ҵ�
            waitingDeadBeat = GameObject.FindAnyObjectByType<Calli_WaitingDeadBeat>();
            waitingDeadBeat.Init(settingGimic);
            // whichSongPlayBook.Init(settingGimic);   // �뷡�� ���� å�� �ʱ�ȭ �Լ� ȣ���ؼ� ����
        }

        Setting_SceneStart();       // ���̺� �����Ϳ� ���� ��� ����
    }

    // ���̺� �����Ϳ� ���� �������� ��� ����
    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            // ���踦 ȹ���ߴ��� Ȯ��
            if (isStoneTableGimicEnding)
            {
                // ���踦 �̹� ȹ������ ��� ��Ȱ��ȭ ��Ŵ
                key.gameObject.SetActive(false);

                for (int i = 0; i < maxTapes; i++)
                {
                    floor_Tapes[i].SetActive(false); // �ٴ��� ������ ��Ȱ��ȭ
                    table_Tapes[i].SetActive(true);  // ���̺��� ������ Ȱ��ȭ
                }
            }
            else
            {
                // ���踦 ���� ȹ������ ������ ��� Ȱ��ȭ ��Ŵ
                key.gameObject.SetActive(true);

                for (int i = 0; i < maxTapes; i++)
                {
                    floor_Tapes[i].SetActive(true); // �ٴ��� ������ Ȱ��ȭ
                    table_Tapes[i].SetActive(false);  // ���̺��� ������ ��Ȱ��ȭ
                }
            }
        }
        else
        {
            // ����� ���� �������� �ʾ����� setgimic�� false
            key.gameObject.SetActive(false);    // ���� ����� �������� �ʾ����Ƿ� ��Ȱ��ȭ

            for (int i = 0; i < maxTapes; i++)
            {
                floor_Tapes[i].SetActive(true); // �ٴ��� ������ Ȱ��ȭ
                table_Tapes[i].SetActive(false);  // ���̺��� ������ ��Ȱ��ȭ
            }
        }

        whichSongPlayBook.Init(settingGimic);   // �뷡�� ���� å�� �ʱ�ȭ �Լ� ȣ���ؼ� ����
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���� �� �ı� �׽�Ʈ�� �ڵ�
        if ((countTapes == maxTapes) && !isTapeReachMaxNum)
        {
            isTapeReachMaxNum = true;           // �������� �ִ���� �����ߴٰ� ���� ����

            TapeHoleCheck();                   // ������ ��ġ�� �´��� Ȯ���ϱ� ���� �Լ� ȣ��
        }
    }

    /// <summary>
    /// �ش� �ε����� �������� �̹� ������ ĭ�� �ִ��� Ȯ���ؼ� bool���� ��ȯ���� �Լ�(true : ������ / false : �������� ����)
    /// </summary>
    /// <param name="_trySummonTapeIndex"></param>
    /// <returns></returns>
    public bool GetAnswerStoneTableTape(int _trySummonTapeIndex)
    {
        bool result = false;

        if (answerTableHoleTape == null)    // �迭�� ������� ���
        {
            // Debug.Log("no tapes in array");
            return result;  // ��� ��ȯ
        }

        // �ִ� �������� ��ŭ �ݺ��ؼ� �̹� �����Ǿ����� üũ
        for (int i = 0; i < maxTapes; i++)
        {
            // ���� �ش� �ε����� �������� �̹� �����Ǿ��� ��� true�� result�� �����ϰ� �ݺ�����
            if (answerTableHoleTape[i].TapeIndex == _trySummonTapeIndex)
            {
                // Debug.Log("that tape is in array");
                result = true;
                break;
            }
        }
        // Debug.Log("no tapes in array");

        return result;  // ��� ��ȯ
    }

    // ���������� �ùٸ� ��ġ�� �������� Ȯ���ϱ� ���� �Լ�
    private void TapeHoleCheck()
    {
        int TapeCheckNum = 0;   // ����� ������ ���������� Ȯ���ϱ� ���� ����

        if (calli_ObjectManager.CheckEndingEpisode_Num == 1) // 1ȸ���� ���
        {
            for (int i = 0; i < maxTapes; i++)
            {
                // ���� ȸ���� ���� �������� ���� ����� �������� ��ġ�� ���ϱ� ���� �Լ� ȣ���ؼ� ��� ����
                if (GameManager.instance.Episode_Round == 1)
                {
                    TapeCheckNum += IsTapeOnCorrectPosition(CorrectTape_ending1, i);
                }
                else if (GameManager.instance.Episode_Round == 2)
                {
                    TapeCheckNum += IsTapeOnCorrectPosition(CorrectTape_ending2, i);
                }
                else
                {
                    Debug.Log("�߸��� ���� ȸ�� ����");
                    TapeCheckNum += IsTapeOnCorrectPosition(CorrectTape_ending1, i);    // �ӽ÷� 1ȸ���� ����
                }
            }

            // Debug.Log("TapeCheckNum : " + TapeCheckNum); // �α� ���

            // �������� ���� ���������� �ִ� ������������ ���� ��� Ʋ�����Ƿ� ���� ����
            if (TapeCheckNum < maxTapes)
            {
                DestroyWrongTapes();    // Ʋ���� ��� Ÿ�ڸ� ������ų �Լ� ȣ��

                return; // �����������Ƿ� ����
            }
        }
        else if (calli_ObjectManager.CheckEndingEpisode_Num == 2) // 2ȸ���� ���
        {
            for (int i = 0; i < maxTapes; i++)
            {
                // ���� ȸ���� ���� �������� ���� ����� �������� ��ġ�� ���ϱ� ���� �Լ� ȣ���ؼ� ��� ����
                TapeCheckNum += IsTapeOnCorrectPosition(CorrectTape_ending2, i);
            }

            // �������� ���� ���������� �ִ� ������������ ���� ��� Ʋ�����Ƿ� ���� ����
            if (TapeCheckNum < maxTapes)
            {
                DestroyWrongTapes();    // Ʋ���� ��� �������� ������ų �Լ� ȣ��

                return; // �����������Ƿ� ����
            }
        }
        else
        {
            Debug.Log("�߸��� ���� ����");
        }

        interaction.run_Gimic = true;                           // ������ ���缭 ��� ������ �Ϸ�Ǿ��ٰ� ����
        // 2ȸ���� ��� ������� ��������� ��ȣ�ۿ� ��絵 �������ֱ� ���� ���� ����
        if (GameManager.instance.Episode_Round == 2)
        {
            // whichSongPlayBook.IsTapeGimicEnd = true;                // ������ ��ġ ����� ����Ǿ��ٰ� ����
            whichSongPlayBook.SetPlaySong(true);                    // �뷡 ��� ���� ��� ���� �Լ� ȣ��
            waitingDeadBeat.SettingForObjectToInteration = true;    // ���谡 ��ȯ�Ǿ��ִٰ� ����
        }

        SummonKey();    // ���踦 ��ȯ���� �Լ� ȣ��
    }

    // ���� ���������� �������� ������ ��ġ�� �ִ��� Ȯ���ϱ� ���� �Լ�
    private int IsTapeOnCorrectPosition(Dictionary<int, string> _CorrectTape_ending, int _i)
    {
        int result = 0;            // ������� Ȯ���ϱ� ���� ����� ��ȯ�� ���� ����(0: ���� / 1: ����)
        string tempTapePos = null; // �ӽ÷� �ش� ������ ������ ĭ ����� ��ųʸ����� �˻��ؿ� �������� �ùٸ� ��ġ�� ������ ����

        // ������ ��ųʸ��� ���� ����� �������� �ε����� �ִ��� Ȯ���ϱ����� ���ǹ�
        if (_CorrectTape_ending.ContainsKey(answerTableHoleTape[_i].TapeIndex))
        {
            _CorrectTape_ending.TryGetValue(answerTableHoleTape[_i].TapeIndex, out tempTapePos); // ������ ��ųʸ��� �ش� �������� �����Ƿ� ���� ��ġ ���� ã�ƿ�

            // �ش� �������� ���� ��ġ�� �ùٸ� ��ġ�� �������� Ȯ��
            if (string.Equals(tempTapePos, answerTableHoleTape[_i].TapePosition))
            {
                result = 1;    // ����Ȯ�� ���� 1�� ����
            }
        }

        return result;  // ��� ��ȯ
    }

    // Ʋ���� ��� ���������� ������ų �Լ�
    private void DestroyWrongTapes()
    {
        answerTableHoleTape = new AnswerTapeInfo[maxTapes];   // �� å���� ������ĭ�� ���� ���������� ������ ����� ����ü �迭 �ʱ�ȭ
        countTapes = 0;                     // ���� ���� ���������� 0���� �ʱ�ȭ
        isTapeReachMaxNum = false;          // �������� �ִ����ŭ ������ �ʾҴٰ� ���� ����

        DestroyWrongPlaceTapes?.Invoke();   // �ش� Action�� ������� ���� ��� ����ִ� ��� �Լ��� ����
        DestroyWrongPlaceTapes = null;      // ���������Ƿ� Action �ʱ�ȭ
    }

    // ���踦 ��ȯ�ϴ� �Լ�
    private void SummonKey()
    {
        if (!key.activeSelf)
        {
            // ���൵ ������Ʈ
            GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        }

        key.gameObject.SetActive(true);                             // ���踦 Ȱ��ȭ ������
        key.transform.localScale = new Vector3(1f, 2f, 1.4f);   // �����Ǵ� ������ ũ�� ����

        // ���踦 ���������Ƿ� ������ (21~24)�� ���� �κ��丮�� �ǵ����� ��Ȱ��ȭ
        for (int i = 21; i < 25; i++)
        {
            ItemManager._instance.ReturnItem(i);
            ItemManager._instance.DeactivateItem(i);
        }
    }
}
