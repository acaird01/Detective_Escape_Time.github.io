using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHall_EndingExitDoorCtrl : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    // �������� �� ������ �ִϸ����� ������Ʈ ����
    private Animator EndingDoorAnimator_Left;   // ���ʹ� Animator ������Ʈ
    private Animator EndingDoorAnimator_Right;  // �����ʹ� Animator ������Ʈ

    private BoxCollider EndingStart_Collider;                   // ������ �������� Ȯ���ϱ� ���� collider
    private MainHall_ObjectManager mainHall_ObjectManager;      // ������ ������Ʈ �Ŵ���(���� �׽�Ʈ�� ���� ȹ���� ����)
    private MainHall_StartStoryAndTutorial mainHall_Story;      // ���������� ���丮�� ����ϴ� ��ũ��Ʈ
    private Item18AmeClock ameClock_Info;                            // �Ƹ� �ð� �������� ���� ��ũ��Ʈ 
    #endregion

    #region ������ ���� ���� ����

    #endregion

    // Start is called before the first frame update
    void Start()
    {   
        /*
         * ���� ������ ������Ѿ� �� ��� ���
        // �ڽ����� �ִ� ���ʹ��� �����ʹ��� �̸��� �̿��� ã�ƿ� �� ������ �Ҵ�
        GameObject LeftDoor = GameObject.Find("DoorGate_Wooden_Left");
        GameObject RightDoor = GameObject.Find("DoorGate_Wooden_Right");

        // ã�ƿ� ���ʹ��� �����ʹ��� Animator ������Ʈ�� ã�ƿͼ� �Ҵ�
        EndingDoorAnimator_Left = LeftDoor.GetComponent<Animator>();
        EndingDoorAnimator_Right = RightDoor.GetComponent<Animator>();
        */

        // �ڽ����� �ִ� ���ʹ��� �����ʹ��� Animator ������Ʈ�� ã�ƿͼ� �Ҵ�
        // ���� ���� �������� �ٲ� ��������Ƿ� �ε����� �̿��� ã�ƿ�
        EndingDoorAnimator_Left = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        EndingDoorAnimator_Right = this.gameObject.transform.GetChild(1).GetComponent<Animator>();

        mainHall_Story = GameObject.FindAnyObjectByType<MainHall_StartStoryAndTutorial>();  // ���丮 ����ϴ� ��ũ��Ʈ�� ���̾��Ű���� ã�ƿͼ� �Ҵ�(Story_Canvas�� �پ�����)
        ameClock_Info = GameObject.FindAnyObjectByType<Item18AmeClock>();                   // �Ƹ޽ð� ������Ʈ�� ã�ƿͼ� �Ҵ�
        EndingStart_Collider = gameObject.GetComponent<BoxCollider>();  // �ش� ��ü�� �ڽ� �ݶ��̴��� ã�ƿͼ� �Ҵ�
        EndingStart_Collider.enabled = false;   // ���� ���� �����ϱ� �� ������ ���� ���� ��Ȱ��ȭ

        // �׽�Ʈ ��
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>(); // ü���� ��ü�� ã�ƿͼ� �Ҵ�// MainHall_ObjectManager�� ã�ƿͼ� �Ҵ�
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �Ƹ��� �������� ȹ�� ���� ���
        if (ameClock_Info.isGetItem)
        {
            mainHall_Story.GetComponent<Interaction_Gimics>().run_Gimic = false;    // ���� ���丮 ����� ���� ������ ���·� ����
        }
    }

    /// <summary>
    /// ���� ������ ���������� �ⱸ�� ������ �Լ�(ü���ǿ��� �������� ������ ȣ��)
    /// </summary>
    public void DoorOpenForEnding()
    {
        // 2ȸ���� ��� õ�忡 ���� ����� �Լ� ȣ��

        // ���� �� ������ ȿ���� ���

        // ���� �� ������ �ִϸ��̼� ����
        EndingDoorAnimator_Left.Play("ExitLeft_DoorOpen");
        EndingDoorAnimator_Right.Play("ExitRight_DoorOpen");

        EndingStart_Collider.enabled = true;   // ���� ���� �����Ǿ����Ƿ� ���� ������ ���� Ȱ��ȭ
    }

    // ���� �ݶ��̴� ����Ҷ� ���� ȸ���� ���� ���� ������ ��� �� �Լ�(���⼭ �����̶� ȸ���� ���� ���̺� ������ �ʱ�ȭ ����ߵ�)
    private void OnTriggerEnter(Collider other)
    {
        // �ݶ��̴��� ����ϴ� ����� �÷��̾��� ��쿡 ����
        if (other.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            // if (GameManager.instance.Episode_Round == 1)    // 1ȸ��
            if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1)
            {
                Debug.Log("1ȸ�� ���� ���"); // �α� ���
                // ���� (�ڷ�ƾ) ����
                // ȸ�� ���� ������Ʈ ����

                mainHall_Story.StartEndingStory();  // ���� ���丮 ��� �Լ� ȣ��
                
                Debug.Log("���� ���� ȸ�� : " + mainHall_ObjectManager.CheckEndingEpisode_Num);   // �׽�Ʈ�� �α�
            }
            // else if (GameManager.instance.Episode_Round == 2)    // 2ȸ��
            else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2)
            {
                Debug.Log("2ȸ�� ���� ���"); // �α� ���
                // ���� (�ڷ�ƾ) ����
                SetSanaCelling();   // õ�忡 ���� ����� �Լ� ȣ��

                mainHall_Story.StartEndingStory();  // ���� ���丮 ��� �Լ� ȣ��

                Debug.Log("���� ���� ȸ�� : " + mainHall_ObjectManager.CheckEndingEpisode_Num);   // �׽�Ʈ�� �α�
            }
            else 
            {
                Debug.Log("�߸��� ȸ������ Ȯ�ε�."); // �α� ���
            }
        }
    }

    // 2ȸ�� ������ ��� õ�忡 ���ָ� ����� �Լ�
    private void SetSanaCelling()
    {
        Debug.Log("�糪����"); // �α� ���
    }
}
