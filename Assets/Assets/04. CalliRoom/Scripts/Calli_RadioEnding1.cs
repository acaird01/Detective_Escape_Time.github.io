using System.Collections;
using UnityEngine;

public class Calli_RadioEnding1 : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    [Header("����� ������ ȹ���� ������(����)")]
    [SerializeField]
    private Item34ChestKey item34ChestKey;
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private string interactionText;            // ��ȣ�ۿ� �� ����� ���
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
        interaction = GetComponent<Interaction_Gimics>();   // �ڽſ��� �پ��ִ� interaction gimic�� �Ҵ�

        StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ��� �ڷ�ƾ ȣ��
    }

    // ��ȣ�ۿ� ��� �ڷ�ƾ �Լ�
    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // ��� �����ض� ���⿡

            // ���谡 Ȱ��ȭ�Ǿ� �ִٸ� ��� ������ �Ϸ�Ǿ����Ƿ� �ش� ��� ���
            if (item34ChestKey.gameObject.activeSelf)
            {
                interactionText = "������ ������ �μ� �ʿ�� ������.\n�� ����� ����ִ� ���ڸ� �� �� ���� ������?";      // ��ȣ�ۿ� �� ����� ��� ����
                StartCoroutine(textController.SendText(interactionText));   // ��ȣ�ۿ� ��� ���
            }
            else
            {
                interactionText = "���� �ִ� ���ڰ� ������ �ڸ� ��Ʈ �� ������.\n�� �� �뷡 ���� �� ���ڿ��� ���缭 �ֺ���?";      // ��ȣ�ۿ� �� ����� ��� ����
                StartCoroutine(textController.SendText(interactionText));   // ��ȣ�ۿ� ��� ���
            }
            
            interaction.run_Gimic = false;
        }
    }
}
