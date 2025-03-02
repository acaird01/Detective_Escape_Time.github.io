using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Calli_CrystalPosGimic : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    [Header("��ġ�� �ڹ��� ����")]
    [SerializeField]
    private GameObject DialLockPiece;                   // ���� ����� Ǯ������ ��ȯ���� �ڹ������� ������ GameObject
    #endregion

    #region �� �� å�󿡼� ����� Action
    private Action InitCrystalFrame;      // �� ���ڸ� �ʱ�ȭ�� �Լ��� ������ Aciton
    public void SetInitTapeHole(Action _InitCrystalFrame)     // InitCrystalFrame�� �Լ��� ������ ĭ���� �Ҵ��� set �Լ�
    {
        InitCrystalFrame += _InitCrystalFrame;
    }

    private Action ReplaceWrongPlaceCrystals;  // �������� ���� �߸��� ��ġ�� �������� ������ ��� ������ Action
    public void SetReplaceWrongPlaceCrystals_CallBack(Action _ReplaceWrongPlaceCrystals) // ReplaceWrongPlaceCrystals�� �Լ��� ������ ĭ���� �Ҵ��� set �Լ�
    {
        ReplaceWrongPlaceCrystals += _ReplaceWrongPlaceCrystals;
    }
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private const string summon_ItemName = "Item_26_DialLockPiece";   // ��ȯ�� ������ �̸�
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    public bool isCrystalReachMaxNum;             // �ִ� ���� ���� �����ߴ��� Ȯ���ϱ� ���� ����
    private const int maxCrystalNum = 5;          // ����2�� �ִ� ���� ��
    private int maxCrystals;                      // ������ ���� ü���ǿ� ���� �ִ� Ÿ�ڼ�
    public int MaxCrystals                        // �ܺο��� �ִ� �������� Ȯ���ϱ� ���� ������Ƽ(Calli_CheckCrystalPos���� ���)
    {
        get
        {
            return maxCrystals;
        }
    }

    private int countCrystal;                     // ���� ���ڿ� ���� ���� ��
    public int CountCrystal                       // ���� ���ڿ� ���� ���� �� ������Ƽ(Calli_CheckCrystalPos���� ���)
    {
        get
        {
            return countCrystal;
        }
        set
        {
            if (countCrystal < maxCrystals)       // �ִ� ������ ������ ���� ������ ���� �������� �Ҵ��� �� �ְԲ� ����
            {
                countCrystal = value;
            }
        }
    }

    // ����� �������� ��ȣ �� ��ġ�� ������ ����ü
    private struct AnswerCrystalInfo
    {
        public string CrystalIndex;      // ���� ���ڿ� ���� ������ �̸��� ������ ����ü ���
        public string CrystalPosition;   // ���� ���ڿ� ���� ������ ��ġ�� ������ ����ü ���
    };
    // answerCrystalNumbers ����ü�� ���� �迭 ����
    private AnswerCrystalInfo[] answerCrystalPosition;
    /// <summary>
    /// ����� ������ ��Ƶ� ����ü�� ���� �Ҵ��ϱ� ���� set �Լ�(Calli_CrystalPosCheck���� ���)
    /// </summary>
    /// <param name="_crystalIndex"></param>
    /// <param name="_crystalPosition"></param>
    public void SetAnswerCrystal(string _crystalIndex, string _crystalPosition)
    {
        if (countCrystal < maxCrystals)  // �ִ� ������������ ���� ���������� �������� ����
        {
            selectCrystal = null;   // �ٽ� ���ð����ϵ��� ���� �������� ������ ���ٰ� �������
            answerCrystalPosition[countCrystal].CrystalIndex = _crystalIndex;
            answerCrystalPosition[countCrystal].CrystalPosition = _crystalPosition;
            countCrystal++;
            Debug.Log("countTapes+ : " + countCrystal);//test
        }
        else
        {
            Debug.Log("AnswerCasetteTape : " + answerCrystalPosition.Length);
        }
    }

    // ���� ���� ����� Dictionary(�����)
    private Dictionary<string, string> CorrectCrystal = new Dictionary<string, string>();  // ����2���� ����� ���� ��ġ(���� 5ĭ�� ���)

    public GameObject selectCrystal;    // ���� ���õ� ������ ������ ������ ����
    //public GameObject selectFrame;      // ���� ���õ� ������ ������ ������ ����
    //public bool[] selectCrytalCheckBool;  // ���� ���� ���É���� ������ ������ �迭

    // �׽�Ʈ��. ���� ������ ������ǰ��� ���ڸ��� �����ϋ� �ȵ��ư�
    [SerializeField]
    private bool isCrystalGimicEnding;    // ���ڿ��� ������ ���缭 ����� ���� �Ϸ�Ǿ����� Ȯ���ϱ� ���� ����(true : ��� ���� �޼� / false : ��� ���� �̴޼�)
    public bool IsCrystalGimicEnding      // ���ڿ��� ������ ������� �����ϱ� ���� ������Ƽ
    {
        set
        {
            isCrystalGimicEnding = value;
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
        
        settingGimic = interaction.run_Gimic;   // ��� ���� ���� �ʱ�ȭ

        isCrystalGimicEnding = ItemManager._instance.inventorySlots[26].GetComponent<IItem>().isGetItem; // �ڹ��������� ȹ��Ǿ����� Ȯ���ϱ� ���� ���� �ʱ�ȭ

        // ���� �������� ������ ���ڸ� null�� �ʱ�ȭ
        selectCrystal = null;
        // selectFrame = null;

        countCrystal = 0; // ���ڵ鿡 ���� ���� �� 0���� �ʱ�ȭ
        // ����ĭ�� ���� ������ �ʱ�ȭ
        maxCrystals = maxCrystalNum;

        #region ���� ��ġ ����� ����
        // ����� ��ųʸ� �ʱ�ȭ
        CorrectCrystal?.Clear();

        // ���� ���� �迭 �ʱ�ȭ
        // ���� ������ �ε��� �� ��ġ
        CorrectCrystal.Add("PurpleCrystal", "Picard_CrytalPos1"); // ���� 1
        CorrectCrystal.Add("YellowCrystal", "Picard_CrytalPos2"); // ��� 2
        CorrectCrystal.Add("BlueCrystal", "Picard_CrytalPos3");   // �Ķ� 3
        CorrectCrystal.Add("OrangeCrystal", "Picard_CrytalPos4"); // ��Ȳ 4
        CorrectCrystal.Add("RedCrystal", "Picard_CrytalPos5");    // ���� 5
        #endregion

        answerCrystalPosition = new AnswerCrystalInfo[maxCrystals];   // ���ڿ� ���� �������� ������ �޾� ������ ����ü �迭 �ʱ�ȭ

        InitCrystalFrame?.Invoke();   // ���ڵ� �ʱ�ȭ �Լ��� ����

        Setting_SceneStart();         // ���̺� �����Ϳ� ���� ��� ����
    }

    // ���̺� �����Ϳ� ���� �������� ��� ����
    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            // �̹� ����� ���� �Ǿ����� ������� ���� �������� ������ ��ġ�� ��ġ
            foreach (var crystal_info in CorrectCrystal)
            {
                GameObject.Find(crystal_info.Key).transform.position = GameObject.Find(crystal_info.Value).transform.position + (Vector3.up * -0.2f);
            }

            // ���踦 ȹ���ߴ��� Ȯ��
            if (isCrystalGimicEnding)
            {
                // �ڹ��������� �̹� ȹ������ ��� ��Ȱ��ȭ ��Ŵ
                DialLockPiece.gameObject.SetActive(false);
            }
            else
            {
                // �ڹ��������� ���� ȹ������ ������ ��� Ȱ��ȭ ��Ŵ
                DialLockPiece.gameObject.SetActive(true);
            }
        }
        else
        {
            // ����� ���� �������� �ʾ����� setgimic�� false
            DialLockPiece.gameObject.SetActive(false);    // ���� ����� �������� �ʾ����Ƿ� ��Ȱ��ȭ
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���� �� �ı� �׽�Ʈ�� �ڵ�
        if ((countCrystal == maxCrystals) && !isCrystalGimicEnding)
        {
            isCrystalGimicEnding = true;           // �������� �ִ���� �����ߴٰ� ���� ����

            CrystalFrameCheck();                   // ���� ��ġ�� �´��� Ȯ���ϱ� ���� �Լ� ȣ��
        }
    }
    /// <summary>
    /// �ش� �̸��� ������ �̹� ���ڿ� �ִ��� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <param name="_trySummonCrystalIndex"></param>
    /// <returns></returns>
    public bool GetAnswerCrystalFramePos(string _trySummonCrystalIndex)
    {
        bool result = false;

        if (answerCrystalPosition == null)    // �迭�� ������� ���
        {
            return result;  // ��� ��ȯ
        }

        // �ִ� �������� ��ŭ �ݺ��ؼ� �̹� �����Ǿ����� üũ
        for (int i = 0; i < maxCrystals; i++)
        {
            // ���� �ش� �ε�����  �̹� �����Ǿ��� ��� true�� result�� �����ϰ� �ݺ�����
            if (answerCrystalPosition[i].CrystalIndex == _trySummonCrystalIndex)
            {
                result = true;
                break;
            }
        }

        return result;  // ��� ��ȯ
    }

    // �������� �ùٸ� ��ġ�� �������� Ȯ���ϱ� ���� �Լ�
    private void CrystalFrameCheck()
    {
        int CrystalCheckNum = 0;   // ����� ������ ���� ���� Ȯ���ϱ� ���� ����

        for (int i = 0; i < maxCrystals; i++)
        {
            // ���� ȸ���� ���� �������� ���� ����� ������ ��ġ�� ���ϱ� ���� �Լ� ȣ���ؼ� ��� ����
            CrystalCheckNum += IsCrystalOnCorrectPosition(CorrectCrystal, i);
        }

        // �������� ���� ���� ���� �ִ� ���������� ���� ��� Ʋ�����Ƿ� ���� ����ġ
        if (CrystalCheckNum < maxCrystals)
        {
            Debug.Log("����");
            StartCoroutine(ReplaceWrongCrystals());    // Ʋ���� ��� �������� ���ڸ��� �ǵ��� �Լ� ȣ��

            return; // �����������Ƿ� ����
        }

        Debug.Log("����");
        SummonItem();    // �������� ��ȯ���� �Լ� ȣ��
    }

    // ���� ���ڵ鿡 �ö�� �������� �������� ������ ��ġ�� �ִ��� Ȯ���ϱ� ���� �Լ�
    private int IsCrystalOnCorrectPosition(Dictionary<string, string> _CorrectCrystal_ending, int _i)
    {
        int result = 0;            // ������� Ȯ���ϱ� ���� ����� ��ȯ�� ���� ����(0: ���� / 1: ����)
        string tempCrystalPos = null; // �ӽ÷� �ش� ������ ������ ĭ ����� ��ųʸ����� �˻��ؿ� �������� �ùٸ� ��ġ�� ������ ����

        // ������ ��ųʸ��� ���� ����� �������� �ε����� �ִ��� Ȯ���ϱ����� ���ǹ�
        if (_CorrectCrystal_ending.ContainsKey(answerCrystalPosition[_i].CrystalIndex))
        {
            _CorrectCrystal_ending.TryGetValue(answerCrystalPosition[_i].CrystalIndex, out tempCrystalPos); // ������ ��ųʸ��� �ش� ������ �����Ƿ� ���� ��ġ ���� ã�ƿ�

            // �ش� �������� ���� ��ġ�� �ùٸ� ��ġ�� �������� Ȯ��
            if (string.Equals(tempCrystalPos, answerCrystalPosition[_i].CrystalPosition))
            {
                result = 1;    // ����Ȯ�� ���� 1�� ����
            }
        }

        return result;  // ��� ��ȯ
    }

    // Ʋ���� ��� �������� ���ڸ��� �������� �Լ�
    private IEnumerator ReplaceWrongCrystals()
    {
        // Ʋ�ȴٰ� ȿ���� ���

        yield return new WaitForSeconds(0.2f);  // 0.2�� ���(ȿ���� ����ϴ� ���� ����ϰԲ� ����)

        answerCrystalPosition = new AnswerCrystalInfo[maxCrystals];   // ���ڿ� ���� �������� ������ �޾� ������ ����ü �迭 �ʱ�ȭ
        countCrystal = 0;                     // ���� ���� ���������� 0���� �ʱ�ȭ
        isCrystalReachMaxNum = false;          // �������� �ִ����ŭ ������ �ʾҴٰ� ���� ����

        ReplaceWrongPlaceCrystals?.Invoke();   // �ش� Action�� ������� ���� ��� ����ִ� ��� �Լ��� ����
        ReplaceWrongPlaceCrystals = null;      // ���������Ƿ� Action �ʱ�ȭ

        isCrystalGimicEnding = false; // ����� ������� �ʾҴٰ� ����
    }

    /// <summary>
    /// ���� �������� ������ �����ϰ� ������ �������̴��� �ִٸ� �ٽ� �������� �Լ�
    /// </summary>
    /// <param name="_currSelectCrystal"></param>
    public void SetSelectCrystal(GameObject _currSelectCrystal)
    {
        if (selectCrystal != null)      // ���� ������ �������̴� ������ ���� ���
        {
            selectCrystal.GetComponent<Calli_CrystalPosChange>().CrystalReplace();  // ������ �ٽ� ���ڸ��� �ǵ�����.
            selectCrystal = _currSelectCrystal; // ���� ������ ������ �������� �������� ����

            selectCrystal.GetComponent<Calli_CrystalPosChange>().CrystalPosUp();    // ���õǾ��ٰ� ǥ������ �Լ� ȣ��
        }
        else    // ���ٸ� ���� ���� ���� ������ �ٷ� �Ҵ�
        {
            selectCrystal = _currSelectCrystal;

            selectCrystal.GetComponent<Calli_CrystalPosChange>().CrystalPosUp();    // ���õǾ��ٰ� ǥ������ �Լ� ȣ��
        }
    }

    // ���踦 ��ȯ�ϴ� �Լ�
    private void SummonItem()
    {
        interaction.run_Gimic = true;   // ��� ������ �Ϸ�Ǿ��ٰ� ����
        DialLockPiece.gameObject.SetActive(true);                         // �ڹ��������� Ȱ��ȭ ������
        //key.transform.localScale = new Vector3(1f, 2f, 1.4f);   // �����Ǵ� ������ ũ�� ����

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate(); // ��� ���� ������ ȣ��
    }
}
