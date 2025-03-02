using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_CasketGimic : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;   // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;            // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ

    private GameObject calliScythe;                    // Į�� �� ������
    private Calli_DeadBeat deadBeat;                   // ���� �����ִ� ������� �ذ�
    private Animator casketAnimator;                   // ���� ��°�� �ű� Animator ������Ʈ

    private AudioSource casket_AudioSource;            // �簢�� ������ ������ ����� Audio Source ������Ʈ
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    private bool isCasketMove;
    // private bool isCasketMoveDone;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�

        Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    private void Init()
    {
        deadBeat = GameObject.FindAnyObjectByType<Calli_DeadBeat>();    // Calli_DeadBeatSkull�� ã�ƿͼ� �Ҵ�
        calliScythe = GameObject.FindAnyObjectByType<Item12CalliopeScythe>().gameObject;    // Į�� ���� ã�ƿͼ� �Ҵ�
        casketAnimator = GetComponent<Animator>();                      // �ش� ������Ʈ�� ���� Animator ������Ʈ �Ҵ�
        casket_AudioSource = gameObject.GetComponent<AudioSource>();
        settingGimic = interaction.run_Gimic;   // ������ ���� ���� ����

        Setting_SceneStart();           // ���� ���� �ڷ�ƾ ��
    }

    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            // �̹� �Ѳ��� ����� �����̹Ƿ� �������� �ִϸ��̼� ���
            moveCasket("CasketMove");

            if (ItemManager._instance.inventorySlots[12].GetComponent<Item12CalliopeScythe>().isGetItem)
            {
                calliScythe.SetActive(false);   // Į������ ��Ȱ��ȭ �ص�
            }
            else
            {
                calliScythe.SetActive(true);   // Į������ Ȱ��ȭ �ص�
            }
        }
        else
        {
            moveCasket("Idle"); // ������ �ִ� �ִϸ��̼� ���
            calliScythe.SetActive(false);   // Į������ ��Ȱ��ȭ �ص�
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ��������� �Ӹ��� �޾Ұ� ���� ���� �ȿ������� ��� ����
        if (deadBeat.IsDeadBeatAlive && !isCasketMove)
        {
            // if (!isCasketMoveDone)
            {
                // isCasketMoveDone = true;
                isCasketMove = true;

                calliScythe.SetActive(true);   // Į������ Ȱ��ȭ �ص�
                moveCasket("CasketMove");      // �� �����̴� �ִϸ��̼� ����

                StartCoroutine(playMoveCasketSound());// �� �Ѱ� �Ҹ� ���
            }
        }
    }

    // �������̴� �ִϸ��̼��� ����� �Լ�
    private void moveCasket(string _clipName)
    {
        casketAnimator.Play(_clipName); // �Ű������� ���� �̸��� �ִϸ��̼� ���
    }

    private IEnumerator playMoveCasketSound()
    {
        casket_AudioSource.Play();      // �� �Ѱ� �Ҹ� ���

        yield return new WaitForSeconds(3f);  // 3�� �� ����

        casket_AudioSource.Stop();      // �� �Ѱ� �Ҹ� ���
    }
}