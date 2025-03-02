using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHall_GetAmeClock : MonoBehaviour
{
    
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private MainHall_ObjectManager mainHall_ObjectManager;     // ������ ������Ʈ �Ŵ���
    private Interaction_Items interaction;                     // �������� ��ȣ�ۿ��� ���� ������Ʈ
    private Item18AmeClock item18AmeClock;                     // �ش� �������� ������ ������ �ִ� ������Ʈ
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� �� ������Ƽ ����
    private bool isGetItem;   // �̹� �������� ������� Ȯ���ϱ� ���� ����
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();          // MainHall_ObjectManager�� ã�ƿͼ� �Ҵ�
        interaction = GetComponent<Interaction_Items>();       // ������ ���� interaction_item ������Ʈ�� �����ͼ� �Ҵ�
    }

    /// <summary>
    /// MainHall_GetAmeClock �ʱ�ȭ �Լ�
    /// </summary>
    public void Init()
    {
        item18AmeClock = GetComponent<Item18AmeClock>();    // �ڽ��� ������ ������ ���� ������Ʈ�� �����ͼ� �Ҵ�

        isGetItem = item18AmeClock.isGetItem;   // �ش� �������� ȸ���Ǿ����� Ȯ���ؼ� ����

        Setting_SceneStart();           // �������� �������ֱ� ���� �Լ� ȣ��
    }

    // �ʱ� ���� �Լ�
    void Setting_SceneStart()
    {
        // �� �������� �� isGetItem�� true false���� ���� �ʱ� ��ġ ����
        if (isGetItem)
        {
            this.gameObject.SetActive(false);   // �ش� ������ ������Ʈ ��Ȱ��ȭ

        }
        else
        {
            this.gameObject.SetActive(true);    // �ش� ������ ������Ʈ Ȱ��ȭ
        }
    }
}
