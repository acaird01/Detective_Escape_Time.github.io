using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_GhostGimics : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    private bool isGhostVisible;        // �̹� ��ȥ�� ���̰� �ִ��� Ȯ���ϱ� ���� ����
    public bool IsGhostVisible          // Calli_CatchGhost���� ���� ���¸� Ȯ���ϱ� ���� ������Ƽ
    { 
        get { return isGhostVisible; }
    }
    [SerializeField]
    private Calli_CatchGhost[] ghosts;  // ��ȥ���� ������ �迭
    private int ghosts_MaxNum;          // ��ȥ�� �迭 ���̸� ������ ����
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� Action ����
    private Action GhostInit;   // ��Ȯ�� ��ȥ���� �ʱ�ȭ �Լ��� ���� Action
    public void SetGhostInit(Action _GhostInit)    // �ʱ�ȭ �Լ��� ���� Action�� setter
    { 
        GhostInit += _GhostInit;
    }
    #endregion

    private void Awake()
    {
        // Action �ʱ�ȭ
        GhostInit = null;
    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // �ʱ�ȭ �Լ�
    private void Init()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();        // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // �÷��̾ ã�ƿͼ� �Ҵ�

        // ghosts = GetComponentsInChildren<Calli_CatchGhost>();   // �ڽ����� �ִ� Calli_CatchGhost ������Ʈ�� ���� ��ȥ���� �迭�� ����(�� �ȵ���)
        ghosts_MaxNum = ghosts.Length;                          // ghosts�� ���� ����
        for (int i = 0; i < ghosts_MaxNum; i++)                 // ghosts�� �ִ� ��ȥ���� �ʱ�ȭ �Լ� ȣ��
        {
            ghosts[i].Init();     // �ʱ�ȭ �Լ� ȣ��
        }

        // GhostInit?.Invoke();        // ��ȥ�� �ʱ�ȭ �Լ� ����
        isGhostVisible = false;     // ���� �Ⱥ��̴� ���·� ����
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ��ȥ�� ������ �ʰ�, ��Ű���� ���õ� �������� �ʿ��� �������� ��쿡 ��� ����
        if (!isGhostVisible && string.Equals(ItemManager._instance.hotkeyItemName, "Item_12_CalliopeScythe"))
        {
            isGhostVisible = true;  // ��ȥ�� ���̴� ���·� �����ؼ� update���� �߰� ������ ���� �ʵ��� ����

            // ghosts�� ����� ��ȥ���� ���� Ȱ��ȭ ��Ŵ
            for (int i = 0; i < ghosts_MaxNum; i++)
            {
                if (!ghosts[i].IsGhostCatch) // �ش� ��ȥ�� ��Ȯ�� �ȉ��� ��쿡�� Ȱ��ȭ ��Ŵ
                {
                    ghosts[i].gameObject.SetActive(isGhostVisible);
                }
            }
        }
        else if (isGhostVisible && !string.Equals(ItemManager._instance.hotkeyItemName, "Item_12_CalliopeScythe"))
        {
            isGhostVisible = false;  // ��ȥ�� ������ �ʴ� ���·� ����

            // ghosts�� ����� ��ȥ���� ���� ��Ȱ��ȭ ��Ŵ
            for (int i = 0; i < ghosts_MaxNum; i++)
            {
                ghosts[i].gameObject.SetActive(isGhostVisible);
            }
        }
        else
        {
            // �� ���� ���� �׳� ����
            return;
        }
    }

    #region ����� �����ϴ� �Լ� ����
    #endregion
}
