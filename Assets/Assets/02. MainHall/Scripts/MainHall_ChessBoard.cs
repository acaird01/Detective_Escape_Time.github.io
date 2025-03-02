using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainHall_ChessBoard : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private MainHall_ObjectManager mainHall_ObjectManager;          // ������ ������Ʈ �Ŵ���
    private MainHall_EndingExitDoorCtrl mainHall_EndingExitDoor;    // ������ ������ ��Ʈ�� ��ũ��Ʈ
    private MainHall_GetAmeClock ameClock;                          // �Ƹ� �ð� ������
    private Interaction_Gimics interaction;                         // ���̺� �����Ϳ� ���� ��� ������ ���� ��ũ��Ʈ

    [Header("�������� ������ ���°� ���� �ݶ��̴�")]
    [SerializeField]
    private GameObject blockBeforeEnding;                    // �������� ������ ���°� ���� �ݶ��̴�
    [Header("�������� ������ ���°� ���� �ݶ��̴��� ������ interaction gimic")]
    [SerializeField]
    private Interaction_Gimics interaction_BlockBeforeEnding;       // �������� ������ ���°� ���� �ݶ��̴��� ������ interaction gimic

    // �ð踦 ��ȯ�� ������ �� �Ҹ� ����
    [Header("�ð��ȯ ��ƼŬ")]
    [SerializeField]
    private GameObject ameClockGeneration_Particle;
    [Header("�ð��ȯ SFX")]
    [SerializeField]
    private GameObject ameClockGeneration_SFX;
    #endregion

    #region �� ü��ĭ���� ����� Action
    private Action InitChessBoard;      // �� ü��ĭ�� �ʱ�ȭ�� �Լ��� ������ Aciton
    public void SetInitChessBoard(Action _InitChessBoard)     // InitChessBoard�� �Լ��� ü��ĭ���� �Ҵ��� set �Լ�
    {
        InitChessBoard += _InitChessBoard;
    }

    private Action DestroyWrongPlaceTakos;  // �������� ���� �߸��� ��ġ�� Ÿ�ڵ��� ������ ��� ������ Action
    public void SetDestroyWrongPlaceTakos_CallBack(Action _DestroyWrongPlaceTakos) // DestroyWrongPlaceTakos�� �Լ��� ü��ĭ���� �Ҵ��� set �Լ�
    {
        DestroyWrongPlaceTakos += _DestroyWrongPlaceTakos;
    }
    #endregion

    #region ü���ǿ��� ����� ���� �� ������Ƽ ����
    private bool settingGimic { get; set; }     // ��� ���� ���θ� Ȯ���ϱ� ���� ����(true : ��� ���� �Ϸ�, false : ��� �̼���)
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    public bool isTakoReachMaxNum;              // �ִ� Ÿ�ڼ��� �����ߴ��� Ȯ���ϱ� ���� ����
    private const int maxTakos_ending1 = 3;     // 1ȸ�� �������� �ʿ��� �ִ� Ÿ�ڼ�
    private const int maxTakos_ending2 = 5;     // 2ȸ�� �������� �ʿ��� �ִ� Ÿ�ڼ�
    private int maxTakos;                       // ������ ���� ü���ǿ� ���� �ִ� Ÿ�ڼ�
    public int MaxTakos                         // �ܺο��� �ִ� Ÿ�ڼ��� Ȯ���ϱ� ���� ������Ƽ(MainHall_CheckTakoPos���� ���)
    {
        get
        {
            return maxTakos;
        }
    }
    private int countTakos;                     // ���� ü���ǿ� ���� Ÿ�ڼ�
    public int CountTakos                       // ���� ü���ǿ� ���� Ÿ�ڼ� ������Ƽ(MainHall_CheckTakoPos���� ���)
    {
        get
        {
            return countTakos;
        }
        set
        {
            if (countTakos < maxTakos)  // �ִ� Ÿ�ڼ����� ���� Ÿ�ڼ��� �������� �Ҵ��� �� �ְԲ� ����
            {
                countTakos = value;
            }
        }
    }

    // ����� Ÿ�ڵ��� �����۹�ȣ �� ��ġ�� ������ ����ü
    private struct AnswerTakoInfo
    {
        public int TakoIndex;         // ���� ü���ǿ� ���� Ÿ���� ������ �ε��� ��ȣ�� ������ ����ü ���
        public string TakoPosition;   // ���� ü���ǿ� ���� Ÿ���� ü���� ��ġ�� ������ ����ü ���
    };
    // answerTakoNumbers ����ü�� ���� �迭 ����
    private AnswerTakoInfo[] answerChessTako;
    // ����ü�� ���� �Ҵ��ϱ� ���� set �Լ�(MainHal_CheckTakoPos���� ���)
    public void SetAnswerChessTako(int _takoIndex, string _takoPosition)
    {
        if (countTakos < maxTakos)  // �ִ� Ÿ�ڼ����� ���� Ÿ�ڼ��� �������� �Ҵ��� �� �ְԲ� ����
        {
            answerChessTako[countTakos].TakoIndex = _takoIndex;
            answerChessTako[countTakos].TakoPosition = _takoPosition;
            countTakos++;
        }
    }

    // ������ ���� ü���� ���� ����� Dictionary
    private Dictionary<int, string> CorrectTako_ending1 = new Dictionary<int, string>();  // ����1���� ����� ���� ��ġ(���� 3ĭ�� ���)
    private Dictionary<int, string> CorrectTako_ending2 = new Dictionary<int, string>();  // ����2���� ����� ���� ��ġ(���� 5ĭ�� ���)

    private bool isChessGimicEnding;    // ü���ǿ��� ������ ���缭 ������ �����ϴ��� Ȯ���ϱ� ���� ����(true : �������� �޼� / false : �������� �̴޼�)
    public bool IsChessGimicEnding      // ü���ǿ��� ������ ������� �����ϱ� ���� ������Ƽ(MainHall_GetAmeClock���� ���)
    {
        set
        {
            isChessGimicEnding = value;
        }
    }
    #endregion

    private void Awake()
    {
        // ����� Action�� null�� �ʱ�ȭ
        DestroyWrongPlaceTakos = null;
        InitChessBoard = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init(); // �ʱ�ȭ �Լ� ȣ��
    }

    // �ʱ�ȭ �Լ�
    private void Init()
    {
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();          // MainHall_ObjectManager�� ã�ƿͼ� �Ҵ�
        ameClock = GameObject.FindAnyObjectByType<MainHall_GetAmeClock>();                          // �Ƹ޽ð� ������Ʈ�� ã�ƿͼ� �Ҵ�
        mainHall_EndingExitDoor = GameObject.FindAnyObjectByType<MainHall_EndingExitDoorCtrl>();    // ������ ������Ʈ�� ã�ƿͼ� �Ҵ�
        interaction = GetComponent<Interaction_Gimics>();                                           // �ڽſ��� �پ��ִ� interaction gimic�� �Ҵ�

        ameClock.Init();    // �Ƹ޽ð��� �ʱ�ȭ �Լ� ȣ��
        ameClock.gameObject.SetActive(false);  // ������ �������Ƿ� ��Ȱ��ȭ
        settingGimic = interaction.run_Gimic;   // ��� ���� ���� �ʱ�ȭ

        countTakos = 0; // ü���ǿ� ���� Ÿ�ڼ� 0���� �ʱ�ȭ
        // ü���ǿ� ���� �� �ִ� �ִ� Ÿ�ڼ� �ʱ�ȭ
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1) // 1ȸ���� ���
        {
            maxTakos = maxTakos_ending1;
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2) // 2ȸ���� ���
        {
            maxTakos = maxTakos_ending2;
        }
        #region ü���� ����� ����
        // ����� ��ųʸ� �ʱ�ȭ
        CorrectTako_ending1?.Clear();
        CorrectTako_ending2?.Clear();

        // 1ȸ�� ü���� ���� �迭 �ʱ�ȭ
        // ���� Ÿ�� �ε��� �� ��ġ
        CorrectTako_ending1.Add(6, "B3");  // ����
        CorrectTako_ending1.Add(7, "C4");  // Į��
        CorrectTako_ending1.Add(8, "H8");  // Ű�ƶ�

        // 2ȸ�� ü���� ���� �迭 �ʱ�ȭ
        // ���� Ÿ�� �ε��� �� ��ġ
        CorrectTako_ending2.Add(5, "D4");  // ����
        CorrectTako_ending2.Add(1, "G2");  // ������
        CorrectTako_ending2.Add(2, "A1");  // ���̸���
        CorrectTako_ending2.Add(3, "E6");  // ũ�δ�
        CorrectTako_ending2.Add(4, "F5");  // �Ŀ쳪
        #endregion

        answerChessTako = new AnswerTakoInfo[maxTakos];   // ü���ǿ� ���� Ÿ�ڵ��� ������ �޾� ������ ����ü �迭 �ʱ�ȭ

        InitChessBoard?.Invoke();   // ü��ĭ�� �ʱ�ȭ �Լ��� ����

        Setting_SceneStart();       // ���̺� �����Ϳ� ���� ��� ����
    }

    // ���̺� �����Ϳ� ���� �������� ��� ����
    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            if (ameClock.GetComponent<Item18AmeClock>().isGetItem == true)
            {
                // �Ƹ� �ð踦 �̹� ȹ������ ��� ��Ȱ��ȭ ��Ŵ
                ameClock.gameObject.SetActive(false);
                blockBeforeEnding.SetActive(false);
            }
            else
            {
                // �Ƹ� �ð踦 ���� ȹ������ ������ ��� Ȱ��ȭ ��Ŵ
                ameClock.gameObject.SetActive(true);
                blockBeforeEnding.SetActive(true);
            }

            StartCoroutine(EndingGimicStart());             // ���� ���Ǵ޼� Ȯ�� �Լ� ȣ��
        }
        else
        {
            // ����� ���� �������� �ʾ����� setgimic�� false
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���� �� �ı� �׽�Ʈ�� �ڵ�
        if ((countTakos == maxTakos) && !isTakoReachMaxNum)
        {
            isTakoReachMaxNum = true;           // Ÿ�ڰ� �ִ���� �����ߴٰ� ���� ����

            ChessTakoCheck();                   // Ÿ�� ��ġ�� �´��� Ȯ���ϱ� ���� �Լ� ȣ��
        }
    }

    /// <summary>
    /// �ش� �ε����� Ÿ�ڰ� �̹� ü���ǿ� �ִ��� Ȯ���ؼ� bool���� ��ȯ���� �Լ�(true : ������ / false : �������� ����)
    /// </summary>
    /// <param name="_trySummonTakoIndex"></param>
    /// <returns></returns>
    public bool GetAnswerChessTako(int _trySummonTakoIndex)
    {
        bool result = false;

        if (answerChessTako == null)    // �迭�� ������� ���
        {
            // Debug.Log("no takos in array");
            return result;  // ��� ��ȯ
        }

        // �ִ� Ÿ�ڼ� ��ŭ �ݺ��ؼ� �̹� �����Ǿ����� üũ
        for (int i = 0; i < maxTakos; i++)
        {
            // ���� �ش� �ε����� Ÿ�ڰ� �̹� �����Ǿ��� ��� true�� result�� �����ϰ� �ݺ�����
            if (answerChessTako[i].TakoIndex == _trySummonTakoIndex)
            {
                // Debug.Log("that tako is in array");
                result = true;
                break;
            }
        }
        // Debug.Log("no takos in array");

        return result;  // ��� ��ȯ
    }

    // Ÿ�ڵ��� �ùٸ� ��ġ�� �������� Ȯ���ϱ� ���� �Լ�
    private void ChessTakoCheck()
    {
        int TakoCheckNum = 0;   // ����� ������ Ÿ�ڼ��� Ȯ���ϱ� ���� ����

        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1) // 1ȸ���� ���
        {
            for (int i = 0; i < maxTakos; i++)
            {
                // ���� ȸ���� ���� �������� ���� ����� Ÿ���� ��ġ�� ���ϱ� ���� �Լ� ȣ���ؼ� ��� ����
                TakoCheckNum += IsTakoOnCorrectPosition(CorrectTako_ending1, i);
            }

            // �������� ���� Ÿ�ڼ��� �ִ� Ÿ�ڼ����� ���� ��� Ʋ�����Ƿ� ���� ����
            if (TakoCheckNum < maxTakos)
            {
                Debug.Log("����");
                DestroyWrongTakos();    // Ʋ���� ��� Ÿ�ڸ� ������ų �Լ� ȣ��

                return; // �����������Ƿ� ����
            }
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2) // 2ȸ���� ���
        {
            for (int i = 0; i < maxTakos; i++)
            {
                // ���� ȸ���� ���� �������� ���� ����� Ÿ���� ��ġ�� ���ϱ� ���� �Լ� ȣ���ؼ� ��� ����
                TakoCheckNum += IsTakoOnCorrectPosition(CorrectTako_ending2, i);
            }

            // �������� ���� Ÿ�ڼ��� �ִ� Ÿ�ڼ����� ���� ��� Ʋ�����Ƿ� ���� ����
            if (TakoCheckNum < maxTakos)
            {
                Debug.Log("����");
                DestroyWrongTakos();    // Ʋ���� ��� Ÿ�ڸ� ������ų �Լ� ȣ��

                return; // �����������Ƿ� ����
            }
        }
        else 
        {
            Debug.Log("�߸��� ���� ����");
        }

        Debug.Log("����");
        // ���� �� ������ ���� ���丮 ����ϴ� �Լ� ȣ��.

        // �ð� ���� �� ��ƼŬ �� �Ҹ� ���
        ameClockGeneration_Particle.SetActive(true);
        ameClockGeneration_Particle.GetComponent<ParticleSystem>().Play();
        ameClockGeneration_SFX.SetActive(true);
        // �ش� �Լ����� ������ ����Ǵµ��� ȸ�������� ������Ʈ�ϰ� �������� �ʱ�ȭ ��Ű�� �Լ��� ȣ��
        ameClock.gameObject.SetActive(true);            // �Ƹ� ȸ�߽ð踦 Ȱ��ȭ

        // �Ƹ� �ð� ��ȯ �Լ� ȣ��

        StartCoroutine(EndingGimicStart());             // ���� ���Ǵ޼� Ȯ�� �Լ� ȣ��
    }

    // ���� ü���ǿ� �ö�� Ÿ�ڵ��� �������� ������ ��ġ�� �ִ��� Ȯ���ϱ� ���� �Լ�
    private int IsTakoOnCorrectPosition(Dictionary<int, string> _CorrectTako_ending, int _i)
    {
        int result = 0;            // ������� Ȯ���ϱ� ���� ����� ��ȯ�� ���� ����(0: ���� / 1: ����)
        string tempTakoPos = null; // �ӽ÷� �ش� ������ ü���� ����� ��ųʸ����� �˻��ؿ� Ÿ���� �ùٸ� ��ġ�� ������ ����

        // ������ ��ųʸ��� ���� ����� Ÿ���� �ε����� �ִ��� Ȯ���ϱ����� ���ǹ�
        if (_CorrectTako_ending.ContainsKey(answerChessTako[_i].TakoIndex))
        {
            _CorrectTako_ending.TryGetValue(answerChessTako[_i].TakoIndex, out tempTakoPos); // ������ ��ųʸ��� �ش� Ÿ�ڰ� �����Ƿ� ���� ��ġ ���� ã�ƿ�

            // �ش� Ÿ���� ���� ��ġ�� �ùٸ� ��ġ�� �������� Ȯ��
            if (String.Equals(tempTakoPos, answerChessTako[_i].TakoPosition))
            {
                result = 1;    // ����Ȯ�� ���� 1�� ����
            }
        }

        return result;  // ��� ��ȯ
    }

    // Ʋ���� ��� Ÿ�ڸ� ������ų �Լ�
    private void DestroyWrongTakos()
    {
        answerChessTako = new AnswerTakoInfo[maxTakos];   // ü���ǿ� ���� Ÿ�ڵ��� ������ ����� ����ü �迭 �ʱ�ȭ
        countTakos = 0;                     // ���� ���� Ÿ�ڼ��� 0���� �ʱ�ȭ
        isTakoReachMaxNum = false;          // Ÿ�ڰ� �ִ����ŭ ������ �ʾҴٰ� ���� ����

        DestroyWrongPlaceTakos?.Invoke();   // �ش� Action�� ������� ���� ��� ����ִ� ��� �Լ��� ����
        DestroyWrongPlaceTakos = null;      // ���������Ƿ� Action �ʱ�ȭ
    }

    /// <summary>
    /// ü���ǿ��� ������ ������ ��� ������ �Լ� ȣ��(MainHall_ChessBorard���� ����)
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndingGimicStart()
    {
        yield return new WaitUntil(() => ameClock.GetComponent<Item18AmeClock>().isGetItem);   // �Ƹ� �ð踦 ȹ���Ҷ����� ���

        blockBeforeEnding.SetActive(false); // ��Ȱ��ȭ�ؼ� ������ �� �ְ� �ٲ�

        // �ð� ���� �� ��ƼŬ �� �Ҹ� ����
        ameClockGeneration_Particle.GetComponent<ParticleSystem>().Stop();
        ameClockGeneration_SFX.SetActive(false);

        mainHall_EndingExitDoor.DoorOpenForEnding();    // ������ �޼������Ƿ� ���� ���� ������ �Լ� ȣ��
    }
}
