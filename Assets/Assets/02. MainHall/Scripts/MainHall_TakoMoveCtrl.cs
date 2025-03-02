using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHall_TakoMoveCtrl : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    private GameObject player;                              // �÷��̾� ������Ʈ
    private MainHall_ObjectManager mainHall_ObjectManager;  // �������� ������Ʈ �Ŵ���
    // private Interaction_Gimics interaction;                 // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ

    private GameObject takoMoving_Ending1;  // 1ȸ�� Ÿ�ڵ�
    private GameObject takoMoving_Ending2;  // 2ȸ�� Ÿ�ڵ�
    private Animator takoMoving_Animator;       // 1, 2ȸ������ Ÿ�ڵ��� �������� Animator
    // private Animator takoMoving_Ending2_Animator;       // 2ȸ������ Ÿ�ڵ��� �������� Animator
    #endregion

    #region ó�� �����Ҷ� Ÿ�ڵ��� �����ӿ� ����� ����
    private bool isTakoMoveDone;                     // Ÿ�� �ִϸ��̼� ��� ���� Ȯ�ο� ���� (true : ���丮 ����Ϸ� / false : ���丮 ��� �̿�)
    private WaitForSeconds waitForTenMillisecond = new WaitForSeconds(0.1f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // �÷��̾ ã�ƿͼ� �Ҵ�
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();    // MainHall_ObjectManager�� ã�ƿͼ� �Ҵ�
        // interaction = gameObject.GetComponent<Interaction_Gimics>();    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�

        // Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    /// <summary>
    /// Ÿ�ڵ��� �������� �ִϸ��̼� ��� ��ũ��Ʈ�� �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="_setting"></param>
    public void Init(bool _setting)
    {
        takoMoving_Ending1 = GetComponentInChildren<TakoStoryMove_ending1>().gameObject;
        takoMoving_Ending2 = GetComponentInChildren<TakoStoryMove_ending2>().gameObject;
        takoMoving_Animator = this.GetComponent<Animator>();

        isTakoMoveDone = _setting;       // ��� ���� ���θ� Ȯ���ؼ� ����

        Setting_SceneStart();   // ���� �������� �Լ� ȣ��
    }

    // ���� �������� �Լ�
    private void Setting_SceneStart()
    {
        if (isTakoMoveDone)
        {
            // Ÿ�� �ִϸ��̼� ����� ������ ��� ��Ȱ��ȭ
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);

            if (GameManager.instance.Episode_Round == 1)
            {
                takoMoving_Ending1.SetActive(true);
                takoMoving_Ending2.SetActive(false);
            }
            else if (GameManager.instance.Episode_Round == 2)
            {
                takoMoving_Ending1.SetActive(false);
                takoMoving_Ending2.SetActive(true);
            }
        }
    }

    /// <summary>
    /// �÷��̾ ������ ������ ���� ����ؼ� ī�޶� �����϶� �ִϸ��̼��� ������ �Լ�
    /// </summary>
    /// <param name="other"></param>
    public void PlayTakoStoryMove()
    {
        if (GameManager.instance.Episode_Round == 1)        // ���� ȸ���� 1�� ���
        {
            // takoMoving_Ending1_Animator.Play("");
            takoMoving_Animator.Play("TakoMoveEnding1");
        }
        else if (GameManager.instance.Episode_Round == 2)   // ���� ȸ���� 2�� ���
        {
            // takoMoving_Ending2_Animator.Play("");
            takoMoving_Animator.Play("TakoMoveEnding2");
        }
        else
        {
            Debug.Log("���� ������ �߸��� : " + GameManager.instance.Episode_Round);
        }
    }
}
