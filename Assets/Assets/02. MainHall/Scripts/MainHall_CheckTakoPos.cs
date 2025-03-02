using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainHall_CheckTakoPos : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    private Interaction_Gimics interaction; // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private MainHall_ChessBoard chessBoard; // ü���� ��ü���� �����ϴ� ��ũ��Ʈ �Ҵ��� ����
    private GameObject player;              // �÷��̾�
    private GameObject takoOnBoard;           // ü���ǿ� ���� Ÿ�ڸ� �����Ҷ� ����� GameObject ����
    #endregion

    #region �ش� ��Ϳ��� ����� ���� ����
    private string ChessboardPosition;  // �ش� ü������ ���° ĭ���� ������ ����
    private bool setTakoOnChessboard;   // �ش� ü���ǿ� Ÿ�ڸ� ���Ҵ��� Ȯ���� ����
    private bool settingGimic { get; set; } // ��� ���� ���� ������ ����
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        ChessboardPosition = this.gameObject.name;  // �̸��� �̿��� ���� ü������ ��ġ�� ����
    }

    private void Start()
    {
        player = GameObject.Find("Player"); // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();        // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        chessBoard = GameObject.FindAnyObjectByType<MainHall_ChessBoard>(); // ü���� ��ü�� ã�ƿͼ� �Ҵ�

        //settingGimic = false;
        chessBoard.SetInitChessBoard(Init);    // ü���ǿ��� �ʱ�ȭ �����ϱ� ���� Action�� �Լ� �Ҵ�
    }

    // chessboard���� ����
    public void Init()
    {
        takoOnBoard = null;       // ���� ������ Ÿ�ڰ� ���Բ� �ʱ�ȭ
        settingGimic = interaction.run_Gimic;   // ��� ��ġ���ο� ���� ��� �����ϱ� ���� ���� �ʱ�ȭ

        setTakoOnChessboard = false;    // ���� ���� �ʾҴٰ� �ʱⰪ ����(���߿� ���嵥���� ������� ���ôٽ� �� ��.)

        // �ʱ�ȭ �� ����� �����ϱ� ����
        StartCoroutine(WaitTouch());            // ��ȣ�ۿ������� ����ų �ڷ�ƾ �Լ� ȣ��

        // Setting_SceneStart();
    }

    // ��ȣ�ۿ�Ǳ� ������ ����� �ڷ�ƾ �Լ�
    private IEnumerator WaitTouch()
    {
        while (player) // �ѹ��ϰ� �ǻ����� �̰Ż���, â�� ���� �ݴ°�ó�� �ݺ��ʿ��ϸ� �̰� �ְ� ����
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // �����ǰ��� ���⼭ ����ϴٰ�

            // ��� ������ ���� ���
            if (interaction.run_Gimic && !setTakoOnChessboard)
            {
                // Debug.Log("���õ� ü���� ��ġ : " + ChessboardPosition);
                setTakoOnChessboard = true; // �������� �ش� ĭ ���� �����Ѵٰ� ���� ����. �߰� �ݺ��� ����

                // �̹� �ִ밹������ ���� ���� Ÿ�ڸ� ��ȯ�Ǿ��ų�, �ش� ��Ű�� Ÿ�ڰ� ü���� ���� ���� ��쿡�� ��ȯ�� ����
                if (!chessBoard.isTakoReachMaxNum && (!chessBoard.GetAnswerChessTako(ItemManager._instance.hotkeyItemIndex)))
                {
                    // ���� ��Ű�� �������� Ÿ���� ��쿡�� ��ȯ ����
                    if (ItemManager._instance.hotkeyItemIndex == 18 || (ItemManager._instance.hotkeyItemIndex >= 1 && ItemManager._instance.hotkeyItemIndex <= 8))   // ��� 0���̶� ��ȯ�� �ȵǴµ� ��. Ȯ�� ����.
                    {
                        // Debug.Log("summon tako �Լ� ����: " + ItemManager._instance.hotkeyItemName); // Ÿ�� ��ȯ Ȯ�� �α� ���
                        SummonTakoOnChessboard();   // ���� ü���� ��ġ�� Ÿ�ڸ� ��ȯ�ϱ� ���� �Լ� ȣ��
                    }
                }
                else
                {
                    setTakoOnChessboard = false;    // ��ȯ���� �ʾ����� ���� ����
                    interaction.run_Gimic = false;
                }
            }
            else
            {
                setTakoOnChessboard = false;    // ��ȯ���� �ʾ����� ���� ����
                interaction.run_Gimic = false;
            }
        }
    }

    // ü���� ���� Ÿ�� ��ȯ�ϱ� ���� �Լ�
    private void SummonTakoOnChessboard()
    {
        // Resources Ǯ������ ���� ��Ű�� ��ϵ� ������ �������� ã�ƿ� ����
        takoOnBoard = Instantiate(Resources.Load<GameObject>("Items/Prefabs/" + ItemManager._instance.hotkeyItemName));
        takoOnBoard.transform.SetParent(this.transform, false);             // �ش� ü��ĭ�� �ڽ����� ����
        takoOnBoard.transform.position = this.transform.position + (Vector3.up * 0.2f);           // �ش� ü���� ��ġ�� ��¦ ���ְԲ� �̵�
        takoOnBoard.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);   // �����Ǵ� Ÿ���� ũ�� ����

        chessBoard.SetAnswerChessTako(ItemManager._instance.hotkeyItemIndex, ChessboardPosition);   // ü���ǿ� �������Ƿ� �迭�� �����ϱ����� �Լ� ȣ��
        // chessBoard.CountTakos = chessBoard.CountTakos + 1;// �����Ǿ����Ƿ� CountTakos�� �̿��� ü���ǿ� �ö� Ÿ�ڼ��� 1����

        // �߰� ��ȣ�ۿ��� ���� �ٽ� �κ��丮�� ���� �ʵ��� collider ��Ȱ��ȭ
        takoOnBoard.GetComponent<Collider>().enabled = false;
        //this.GetComponent<Collider>().enabled = false;

        chessBoard.SetDestroyWrongPlaceTakos_CallBack(DestroyWrongPlaceTako);    // �߸��� ��ġ�� ���� ��� ������ �Լ� Action�� �־���

        // TakoPlace.transform.position = this.transform.position; // �ش� ü���� ��ġ�� ����
        // Debug.Log(ItemManager._instance.hotkeyItemName); // ��Ű�� �������� ����� ã�ƿ����� Ȯ���ϱ� ���� �α� (241001)
    }

    // ü���ǿ��� �߸��� ��ġ�� Ÿ�ڰ� ������ ��� ������ �Լ�
    private void DestroyWrongPlaceTako()
    {
        Destroy(takoOnBoard);           // �ش� ��ġ�� ������ Ÿ�� ����
        interaction.run_Gimic = false;  // �ش� ����� ���� ������� ���� ���·� ����
        setTakoOnChessboard = false;    // ��������Ƿ� false�� ����
    }
}
