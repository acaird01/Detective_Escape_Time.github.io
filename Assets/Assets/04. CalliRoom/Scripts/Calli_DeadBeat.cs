using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_DeadBeat : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    [Header("�� ���� �Ϸ� ����� interaction gimic")]
    [SerializeField]
    private Interaction_Gimics save_Interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ

    private GameObject deadBeat_Skull;                  // ������� �ΰ��� �κ�
    private GameObject deadBeat_Cloth;                  // ������� �� �κ�
    private GameObject deadBeat_HeadScarf;              // ������� ��ī�� �κ�
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private bool isDeadBeatAlive;       // ����������� �Ӹ��� �ǵ�������� Ȯ���ϱ� ���� ����
    public bool IsDeadBeatAlive         // ����������� �Ӹ��� �ǵ�������� Calli_CasketGimic���� Ȯ���ϱ� ���� ������Ƽ
    {
        get 
        { 
            return isDeadBeatAlive;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();     // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�

        Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    // �ʱ�ȭ �Լ�
    private void Init()
    {
        settingGimic = save_Interaction.run_Gimic;   // ��� ���� ��Ȳ ����

        deadBeat_Skull = GetComponentInChildren<Calli_DeadBeatSkull>().gameObject;          // ������� �ΰ����� �ڽĿ��� ã�ƿͼ� �Ҵ�
        deadBeat_Cloth = GetComponentInChildren<Calli_DeadBeatCloth>().gameObject;          // ������� ���� �ڽĿ��� ã�ƿͼ� �Ҵ�
        deadBeat_HeadScarf = GetComponentInChildren<Calli_DeadBeatHeadScarf>().gameObject;  // ������� ��ī���� �ڽĿ��� ã�ƿͼ� �Ҵ�

        StartCoroutine(WaitTouch());    // ��� �ڷ�ƾ ����

        Setting_SceneStart();           // ���� ���� �ڷ�ƾ ����
    }

    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            // ����� �̹� ����� �����̹Ƿ� ���� ���̰� ����
            deadBeat_Skull.SetActive(true);
            deadBeat_Cloth.SetActive(true);
            deadBeat_HeadScarf.SetActive(true);

            isDeadBeatAlive = true;     // ��������� �Ӹ��� �޾Ҵٰ� ����
        }
        else
        {
            // ����� ���� ������� �ʾ����Ƿ� ������� ���̰Բ� ����
            deadBeat_Skull.SetActive(false);
            deadBeat_Cloth.SetActive(false);
            deadBeat_HeadScarf.SetActive(false);

            isDeadBeatAlive = false;     // ��������� ���� �Ӹ��� �� �޾Ҵٰ� ����
        }
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // ��� �����ض� ���⿡

            // ���� ��Ű���� ���õ� �������� �ΰ����̰� ��ͼ����� �Ϸ���� ���� ��쿡 ��� ����
            if (string.Equals(ItemManager._instance.hotkeyItemName, "Item_20_Skull") && (!save_Interaction.run_Gimic))
            {
                // ������ ��� �Լ� ȣ��
                SetSkullToDeadBeat();

                interaction.run_Gimic = false;  // ����� ������� �ʾ����Ƿ� false�� �ٽ� ����
            }
            else if (save_Interaction.run_Gimic)    // ��� ������ �Ϸ�� ���  ��� ���
            {
                // ��ȣ�ۿ� ���� ����Լ� ȣ��
                StartCoroutine(textController.SendText("��� ſ�ϱ�... �Ӹ��� �ǵ��ƿͼ� �⻵����."));

                interaction.run_Gimic = false;  // ����� ������� �ʾ����Ƿ� false�� �ٽ� ����
            }
            else  // �ƴ϶�� �ؽ�Ʈâ�� ��ȣ�ۿ� ���� ���
            {
                // ��ȣ�ۿ� ���� ����Լ� ȣ��
                StartCoroutine(textController.SendText("���, �ذ��� �Ӹ��� �����?"));

                interaction.run_Gimic = false;  // ����� ������� �ʾ����Ƿ� false�� �ٽ� ����
            }
        }
    }

    #region ����� �����ϴ� �Լ� ����
    // �ΰ����� �����ϰ� ����������� ��ȣ�ۿ����� ��� �ΰ����� �����ְ� ���� ���� �Լ�
    private void SetSkullToDeadBeat()
    {
        // �ΰ����� �κ��丮�� �ǵ�����
        ItemManager._instance.ReturnItem(20);
        ItemManager._instance.DeactivateItem(20);

        deadBeat_Skull.SetActive(true);     // �ذ��� �ٽ� ������

        save_Interaction.run_Gimic = true;  // ����� ���� �Ϸ� �Ǿ��ٰ� ���� ����
        
        StartCoroutine(MoveCasket());       // �ذ� �������� �� �������� �ڷ�ƾ ����
    }

    private IEnumerator MoveCasket()
    {
        yield return new WaitForSeconds(0.3f);  // 0.3�� ���

        // �ʰ����� �ǵ�����
        deadBeat_Cloth.SetActive(true);
        deadBeat_HeadScarf.SetActive(true);

        // �ִϸ��̼ǵ� ����ұ�..
        // ���� ������ �̵����� Į������ ȹ�� �� �� �ְ� Calli_CasketGimic�� ����
        isDeadBeatAlive = true; // �ΰ����� �����޾Ҵٰ� ����

        save_Interaction.run_Gimic = true;   // ��� ���� �Ϸ� ó��

        // 1ȸ���϶��� ����
        if (GameManager.instance.Episode_Round == 1)
        {
            // ���൵ ������Ʈ
            GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        }
        calli_ObjectManager.ChangeSceneData_To_GameManager();   // Į�� ���� �������Ƿ� ��Ȯ�Ǿ����Ƿ� ���� 1ȸ ����
    }
    #endregion
}
