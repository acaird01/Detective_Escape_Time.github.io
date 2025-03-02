using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MainHall_PassDoorAfterTutorial : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    private GameObject player;                                 // �÷��̾� ������Ʈ
    private MainHall_ObjectManager mainHall_ObjectManager;     // �������� ������Ʈ �Ŵ���
    private TextController textController;                     // ��ȣ�ۿ� ��縦 ����ϴ� ��ũ��Ʈ

    private Canvas PlayerUI;                        // �÷��̾ ���ϰ� �ִ� �κ��丮 ���� UI Canvas
    
    //[Header("�ó׸ӽ� ī�޶� ����ٴ� �������Ʈ")]
    //[SerializeField]
    //private GameObject mainHallViewCam_position;     // 1ȸ������ ó������ ���������� �Ѿ���� �� ��ü�� ������ ī�޶� ������ ������Ʈ
    [Header("�ó׸ӽ� ī�޶�")]
    [SerializeField]
    private GameObject mainHallCinemachineCamera;     // 1ȸ������ ó������ ���������� �Ѿ���� �� ��ü�� ������ ī�޶�
    [Header("������ ������ �ó׸ӽ� ī�޶�")]
    [SerializeField]
    private PlayableDirector mainHallDirector;
    [Header("������ ������ �ó׸ӽ� ī�޶��� Cinemachinebrain")]
    [SerializeField]
    private Camera mainHallDirector_Brain;
    [Header("���۹� ��������Ʈ")]
    [SerializeField]
    private Transform startRoom_SponPoint;

    private MainHall_StartStoryAndTutorial mainHall_StartStoryAndTutorial;  // ���丮�� ����ϴ� ��ũ��Ʈ
    private MainHall_StoryHelpAnim mainHall_StoryHelp;      // ������ ���̵带 �ʱ�ȭ ���� ��ũ��Ʈ
    private MainHall_TakoMoveCtrl mainHall_TakoMoveCtrl;    // ���������� Ÿ�ڵ��� �������� ��ũ��Ʈ
    private Canvas doorInfo_Canvas;                             // ���� ������ ����� Canvas
    private Text doorInfo_Text;                                 // ���� ������ ����� Text
    #endregion

    #region ���丮 ��¿� ����� ����
    private bool isPassDoor;                     // ���丮 ��� �Ϸ� ���� Ȯ�ο� ���� (true : ���丮 ����Ϸ� / false : ���丮 ��� �̿�)
    private string beforePreviewStory;           // �����並 �����ֱ����� ����� ���丮
    private WaitForSeconds waitForSecond = new WaitForSeconds(1f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // �÷��̾ ã�ƿͼ� �Ҵ�
        PlayerUI = player.GetComponentInChildren<Canvas>();                 // player�� �ڽ����� �ִ� canvas�� ã�ƿͼ� �Ҵ�
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();    // MainHall_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();           // �÷��̾��� �ڽ��� ���� textcontroller�� ã�ƿͼ� �Ҵ�
    }

    /// <summary>
    /// ���� ������ ��ũ��Ʈ�� �ʱ�ȭ �Լ�
    /// </summary>
    public void Init(bool _setting)
    {
        mainHall_StartStoryAndTutorial = GameObject.FindAnyObjectByType<MainHall_StartStoryAndTutorial>();  // ���丮�� ����ϴ� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�
        mainHall_StoryHelp = GameObject.FindAnyObjectByType<MainHall_StoryHelpAnim>();      // ������ ���̵带 �����ϴ� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�
        mainHall_TakoMoveCtrl = GameObject.FindAnyObjectByType<MainHall_TakoMoveCtrl>();    // ���������� Ÿ�ڵ��� �������� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�
        
        doorInfo_Canvas = GameObject.FindAnyObjectByType<MainHall_DoorGuideCanvas>().GetComponentInChildren<Canvas>();    // ���� ������ ����� Canvas�� ã�ƿͼ� �Ҵ�
        doorInfo_Text = doorInfo_Canvas.GetComponentInChildren<Text>();     // ������ �Ѿ ���� ��縦 ����� �ؽ�Ʈ

        isPassDoor = _setting;  // 

        if (GameManager.instance.Episode_Round == 1)        // ���� ȸ���� 1�� ��쿡 ��ȯ�� Ÿ���̸����� �ʱ�ȭ
        {
            // isStoryEnd = _setting;
        }
        else if (GameManager.instance.Episode_Round == 2)   // ���� ȸ���� 2�� ��쿡 ��ȯ�� Ÿ���̸����� �ʱ�ȭ
        {

        }
        else
        {
            Debug.Log("���� ������ �߸���");
            // summon_TakoName = ending1_TakoName; // �ӽ÷� ����1�� Ÿ�ڷ� �ʱ�ȭ
        }

        // �ϴ� ī�޶� ����
        mainHallDirector_Brain.enabled = false;
        mainHallCinemachineCamera.SetActive(false);
        doorInfo_Canvas.enabled = false; // ĵ���� ��Ȱ��ȭ

        Setting_SceneStart();   // ���� �������� �Լ� ȣ��
    }

    // ���� �������� �Լ�
    private void Setting_SceneStart()
    {
        if (isPassDoor)
        {
            // �̹� �������� �� ��ü�� ���� ��� ������� ���ϵ��� ���� ����
            // this.GetComponent<BoxCollider>().enabled = false;
            this.gameObject.SetActive(false);   // �̹� ������ ���� �����Ƿ� ��Ȱ��ȭ ��Ŵ
        }
        else
        {
            this.GetComponent<BoxCollider>().enabled = true;
        }
    }

    // �÷��̾ ������ ������ ���� ��������� ī�޶� �������� �Լ�
    private void OnTriggerExit(Collider other)
    {
        PlayerCtrl isPlayer = other.GetComponent<PlayerCtrl>();

        // �÷��̾ ���԰� ���Ϸθ� ȹ�� ���� ��� ����
        if ((isPlayer != null) && (ItemManager._instance.inventorySlots[10].GetComponent<IItem>().isGetItem))
        {
            this.GetComponent<BoxCollider>().enabled = false;   // �ݶ��̴��� ����
            StartCoroutine(playPreview());

        }
        else
        {
            // �ƴ϶�� ������ �ȵǴ� ��Ȳ�� ������ ���̹Ƿ� ������ �ǵ�������
            player.GetComponent<Transform>().localPosition = startRoom_SponPoint.localPosition;
        }
    }

    // ���� ������ ��� �ڷ�ƾ �Լ�
    private IEnumerator playPreview()
    {
        // �÷��̾� Ű���� �Է��� ����
        player.GetComponent<PlayerCtrl>().keystrokes = true;
        PlayerUI.enabled = false;   // �÷��̾� ĵ���� ��Ȱ��ȭ

        // �ó׸ӽ� ������� �Ƹ� ��� ���� ���
        if (GameManager.instance.Episode_Round == 1)
        {
            beforePreviewStory = "��? ���� ����?";
        }
        else if (GameManager.instance.Episode_Round == 2)
        {
            beforePreviewStory = "��, �� �༮����?!";
        }
        else
        {
            Debug.Log("�߸��� ������ ������.(���� ������ ���� ���丮 ��µǴ� �κ�)");
        }

        doorInfo_Canvas.enabled = true; // ĵ���� Ȱ��ȭ
        doorInfo_Text.text = beforePreviewStory;
        // StartCoroutine(textController.SendText(beforePreviewStory));

        // �÷��̾� Ű���� �Է��� ����
        player.GetComponent<PlayerCtrl>().keystrokes = true;

        yield return waitForSecond; // 1�ʰ� ���

        doorInfo_Canvas.enabled = false; // ĵ���� ��Ȱ��ȭ

        // ��簡 ���������� ���� �Լ�ȣ��
        textController.SetActiveFalseText();
        // �÷��̾� Ű���� �Է��� ����
        player.GetComponent<PlayerCtrl>().keystrokes = true;

        // ��� ��� �� �ó׸ӽ� ����

        player.GetComponentInChildren<Camera>().enabled = false;  // �÷��̾��� ī�޶� ����
        mainHallDirector_Brain.enabled = true;
        mainHallCinemachineCamera.SetActive(true);

        mainHallDirector.Play();    // ī�޶� ��ŷ ����
        mainHall_TakoMoveCtrl.PlayTakoStoryMove();  // Ÿ�� ������ �Լ� ȣ��

        yield return new WaitForSeconds(10.2f);

        mainHallCinemachineCamera.SetActive(false);

        mainHallDirector_Brain.enabled = false;
        player.GetComponentInChildren<Camera>().enabled = true;  // �÷��̾��� ī�޶� ����
        player.GetComponentInChildren<Camera>().GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);  // ������ ���� �ְԲ� ī�޶� ����
        mainHall_StoryHelp.mainHallGuideCamMoveEnd = true;  // �����䰡 �����ٰ� ���� ����

        PlayerUI.enabled = true;   // �÷��̾� ĵ���� Ȱ��ȭ
        // MainHall_StartStoryAndTutorial�� ������ ������ ���丮 ���� �Լ� ȣ��
        mainHall_StartStoryAndTutorial.StartAfterPreviewStory();
    }
}
