using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainHall_StartStoryAndTutorial : MonoBehaviour
{
    /// <summary>
    /// 1. �ε��� ���� ȸ�� Ȯ��
    /// 2. ȸ���� ���� �ʹ� ���丮 �� ���� �Ϻ� ����
    /// 3. �÷��̾� ã�ƿͼ� ���丮 �����ٶ� ������ UI ��Ȱ��ȭ -> ������ Ȱ��ȭ
    /// </summary>

    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    private GameObject player;                      // �÷��̾� ������Ʈ
    private MainHall_ObjectManager mainHall_ObjectManager;     // �������� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction; // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ

    private Canvas PlayerUI;                        // �÷��̾ ���ϰ� �ִ� �κ��丮 ���� UI Canvas
    private Canvas storyCanvas;                     // ���丮 �� Ʃ�丮�� ������ ������ Canvas
    private Text storyLineCharacter_Text;            // ����Ǵ� ���丮 ĳ������ �̸��� ������ Text UI
    private Text storyLine_Text;                    // ����Ǵ� ���丮 �� Ʃ�丮���� ������ ������ Text UI
    private InventoryTutorialCtrl tutorial_UI;         // Ʃ�丮���� ������ UI�� �����ϴ� ��ũ��Ʈ
    private MainHall_StartRoomDoorWall startRoomDoorWall;   // ���۹��� ���� �޸� ��
    private GameObject mainHall_StartRoomDoor;      // ���� ���� ���۹��� �� ���� ������Ʈ

    // ���丮 ����� Ʋ���� audio source �� audio clip�� ����
    private AudioSource storyAudioSource;           // ���丮 ������ ���带 ������� audio source
    [Header("�ð� ���� ȿ����")]
    [SerializeField]
    private AudioClip timeTravel_SFX;
    [Header("�� ������ �Ҹ�")]
    [SerializeField]
    private AudioClip doorClose_SFX;
    [Header("�ǻ������ �̵������� �ٲ��� BGM")]
    [SerializeField]
    private AudioClip ending2_BGM;
    [Header("������ �⺻ BGM")]
    [SerializeField]
    private AudioClip mainHall_BGM;

    // ��Ʈ ����� ���� ����
    [Header("���丮 ���� �⺻ ��Ʈ")]
    [SerializeField]
    private Font storyDefault_Font;
    [Header("�̳� ��Ʈ(Bold)")]
    [SerializeField]
    private Font inanis_Font;

    // ���丮 ��� �̹��� ����
    private GameObject storyTimeTravelImage;         // 1ȸ�� �������� �ð�����ó���� �����ֱ� ���� Image UI
    private List<Image> character_Image;             // ���丮���� ������ ĳ���� Image UI ���� �迭
    [Header("ī� �ǻ�� �̹���")]
    [SerializeField]
    private GameObject storyEnding2Council_Image;         // 2ȸ�� �������� ������ �ǻ�� Image
    [Header("��� ���̵��ξƿ� �̹���")]
    [SerializeField]
    private Image storyWhiteOut_Image;          // 2ȸ�� �������� �ǻ���� �����ֱ����� ����� Image

    // ���̸��� ������ ���� ���� ����
    [Header("���̸��� �Ӹ����� ���Ϸ�")]
    [SerializeField]
    private GameObject irysHalo_OnHead;
    [Header("���̸��� 1ȸ�� ���Ϸ� �ݶ��̴���")]
    [SerializeField]
    private GameObject irysHalo_ending1;
    [Header("���̸��� 2ȸ�� ���Ϸ� ��ġ")]
    [SerializeField]
    private GameObject irysHalo_ending2;
    [Header("���̸��� ǥ����ȭ�� ���� ���� MeshRenderer")]
    [SerializeField]
    private SkinnedMeshRenderer irysStatue;

    // Ʃ�丮�� ����� ���� ����
    [Header("Ʃ�丮�� �� ���� �ѷ��� ī�޶�")]
    [SerializeField]
    private GameObject tutorialCam;     // Ʃ�丮�� ���� �� ������ ī�޶�
    [Header("Ʃ�丮�� ���� �� ī�޶� ȸ�� ��ų �ִϸ�����")]
    [SerializeField]
    private Animator tutorialCam_Anim;  // Ʃ�丮�� ���� �� ī�޶� ȸ�� ��ų �ִϸ�����
    [Header("���� �� ���̵��ξƿ��� �����ų �ִϸ�����")]
    [SerializeField]
    private Animator production_Anim;  // ������ ������ �ִϸ�����
    #endregion

    #region ���丮 ��¿� ����� ����
    private int currentStoryLine;               // ���� ���丮 ����
    private int maxStoryLine;                   // ������ ���丮 ���� ��ȣ
    private int lastTalkStoryCharacter;          // ���������� ��縦 ���� ĳ����Ȯ�ο� ����(StoryLineClass.StoryCharacter_Enum�� ��ȣ�� ����)
    private Image fadeInOut_Image;              // Fade in, out ���⿡ ����� canvas
    private MainHall_StoryScript storyLine;     // ���丮�� ����� Ŭ���� ����
    private bool isEndingStroryStart = false;   // ���� ���丮 ��� Ȯ�ο� ����(true : ���� ���丮 / false : ���� ���丮)
    private List<StoryLineClass> storyLines;    // ���� ����� ���丮�� ������ ����Ʈ
    public bool isStoryEnd;                     // ���丮 ��� �Ϸ� ���� Ȯ�ο� ����(���丮�� �����̵� ���x)(true : ���丮 ����Ϸ� / false : ���丮 ��� �̿�)
    private bool isWaitingTutorial;    // Ʃ�丮�� ���丮�� �����ؾߵǴ��� Ȯ���ϱ� ���� ����

    private Color activeColor = new Color32(255, 255, 255, 255);      // ��縦 ���ϰ� �ִ� ĳ������ ����
    private Color nonActiveColor = new Color32(63, 63, 63, 220);      // ��縦 ���������� ���� ĳ������ ����

    private WaitForSeconds waitForSecond = new WaitForSeconds(1f);
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        player = GameObject.Find("Player"); // ���̾��Ű���� �÷��̾ ã�ƿͼ� �Ҵ�
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();     // ���� ���� �ִ� ������Ʈ �Ŵ����� �Ҵ�

        //Debug.Log("prev : " + GameManager.instance.PrevSceneName);
        //Debug.Log("now : " + GameManager.instance.nowSceneName);

        if (GameManager.instance.PrevSceneName == "01. IntroScene")
        {
            player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // ������ ��ġ�� �̵�(Ʃ�丮�� ��)
        }

        Init();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �ش� ������ ����� ������ �Ҵ� �� �ʱ�ȭ�ϱ� ���� �Լ�(������Ʈ �Ŵ������� ���)
    public void Init()
    {
        PlayerUI = player.GetComponentInChildren<Canvas>();                 // player�� �ڽ����� �ִ� canvas�� ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();        // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�

        storyTimeTravelImage = GetComponentInChildren<StoryTimeTravelImage>().gameObject;  // 1ȸ�� �������� �ð������ϴ°� �����ֱ� ���� Image�� ã�ƿͼ� �Ҵ�
        startRoomDoorWall = GameObject.FindAnyObjectByType<MainHall_StartRoomDoorWall>();               // ���� ���� ���� �޸� �� ������Ʈ
        mainHall_StartRoomDoor = GameObject.FindAnyObjectByType<MainHall_StartRoomDoor>().gameObject;   // ���۹��� �� ������Ʈ�� ã�ƿͼ� �Ҵ�
        fadeInOut_Image = this.GetComponentInChildren<FadeInOutCanvas>().GetComponent<Image>();            // Fade in, out ���⿡ ����� canvas�� ã�ƿͼ� �Ҵ�

        // �����Ҷ� �Ⱥ��̵��� ��Ȱ��ȭ
        fadeInOut_Image.enabled = false;
        storyTimeTravelImage.SetActive(false);

        storyCanvas = this.GetComponentInChildren<StoryCanvas>().GetComponent<Canvas>();                         // ���丮 �� Ʃ�丮�� ��ũ��Ʈ�� ������ canvas
        storyLineCharacter_Text = GameObject.FindAnyObjectByType<CharacterStoryName_Text>().GetComponent<Text>();  // ���� ��縦 ġ���ִ� ĳ���͸� ������ text ui�� ã�ƿͼ� text ������Ʈ �Ҵ�
        storyLine_Text = GameObject.FindAnyObjectByType<CharacterStoryScript_Text>().GetComponent<Text>();  // ���丮�� ����� text ui�� ã�ƿͼ� text ������Ʈ �Ҵ�
        tutorial_UI = PlayerUI.GetComponent<InventoryTutorialCtrl>();   // turorialUI�� �����ϴ� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�

        storyAudioSource = this.GetComponent<AudioSource>();    // story canvas�� �޷��ִ� ���丮 ����� �Ҹ��� Ʋ���� Audio source�� ã�ƿͼ� �Ҵ�

        // ���丮 ���࿡ ����� �̹����� ã�ƿͼ� ����
        storyWhiteOut_Image.enabled = false; // ��Ȱ��ȭ �ص�
        storyEnding2Council_Image.SetActive(false);

        character_Image = new List<Image>();  // ĳ���� �̹����� ���� ����Ʈ ����
        character_Image?.Clear();             // ����� ����Ʈ �ʱ�ȭ
        character_Image.Add(GetComponentInChildren<StoryAmeImage>().GetComponent<Image>());     // ���丮 ���⿡ ����� �Ƹ� �̹���
        character_Image.Add(GetComponentInChildren<StoryInaImage>().GetComponent<Image>());     // ���丮 ���⿡ ����� �̳� �̹���
        character_Image.Add(GetComponentInChildren<StoryIrysImage>().GetComponent<Image>());    // ���丮 ���⿡ ����� ���̸��� �̹���
        character_Image.Add(GetComponentInChildren<StoryBaeImage>().GetComponent<Image>());     // ���丮 ���⿡ ����� ���� �̹���
        character_Image.Add(GetComponentInChildren<StoryFaunaImage>().GetComponent<Image>());   // ���丮 ���⿡ ����� �Ŀ쳪 �̹���
        character_Image.Add(GetComponentInChildren<StoryKroniiImage>().GetComponent<Image>());  // ���丮 ���⿡ ����� ũ�δ� �̹���
        character_Image.Add(GetComponentInChildren<StoryMumeiImage>().GetComponent<Image>());   // ���丮 ���⿡ ����� ������ �̹���
        character_Image.Add(GetComponentInChildren<StoryTakosImage>().GetComponent<Image>());   // ���丮 ���⿡ ����� Ÿ�ڵ� �̹���

        // ���丮 ���� �� ��� �̹��� ��Ȱ��ȭ
        for (int i = 0; i < character_Image.Count; i++)
        {
            character_Image[i].gameObject.SetActive(false);
        }

        storyLines = null;          // ������ ���丮�� null�� �ʱ�ȭ
        currentStoryLine = 0;       // ���� ������� ���丮 ������ 0��°�� �ʱ�ȭ
        lastTalkStoryCharacter = (int)StoryLineClass.StoryCharacter_Enum.NONE; // ���������� ��縦 ġ�� ĳ���Ͱ� ���ٰ� �ʱ�ȭ
        maxStoryLine = 0;        // ������ ���丮 ������ ���丮�� ����� �迭�� ���̷� �ʱ�ȭ(����� �׽�Ʈ�� ���� 0���� �ʱ�ȭ)
        interaction.run_Gimic = false;  // ���丮 ����� �������� �ʾҴٰ� ���� ����

        // ����Ǿ��ִ� ���丮 ���࿩�ο� ���� ���丮�� �����ϱ� ���� ���� �ʱ�ȭ
        isStoryEnd = interaction.run_Gimic;                 // ���丮�� �̹� �ô��� �ƴ��� ����
        if (GameManager.instance.IsEndingStroryStart == 0)  // ���� ���丮�� �����ϴ��� �ƴ��� ����
        {
            isEndingStroryStart = false;
        }
        else
        {
            isEndingStroryStart = true;
        }

        // ���� ���丮�� ���� �Ⱥð�, 1ȸ���϶� ����
        if (!isStoryEnd && !isEndingStroryStart)
        {
            ItemManager._instance.ResetInventory();
            //if ((GameManager.instance.Episode_Round == 1))
            //{
            //    ItemManager._instance.ResetInventory();
            //}
        }

        PlayerUI.enabled = false;       // ���丮�� �����ֱ� ���� �÷��̾��� ĵ������ ��
        storyCanvas.enabled = false;    // ���丮 ������ ĵ���� ��Ȱ��ȭ

        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = false;
        tutorialCam.GetComponent<Camera>().enabled = false; // Ʃ�丮�� �� ī�޶� ��Ȱ��ȭ

        storyLine = new MainHall_StoryScript();   // ���丮�� ����ϱ� ���� ���丮�� ����� Ŭ���� ����
        storyLine.InitStory();                    // ���丮 �ʱ�ȭ

        Setting_SceneStart();
    }
    #region ���丮 ���� �Լ� ����
    // �� �������� �� ���丮 ������ �����ϱ� ���� �Լ�
    void Setting_SceneStart()
    {
        // ���� ���� ���丮�� �Ⱥ��� ��� ���丮 ����
        if (!isEndingStroryStart)  // �������丮 ���� ���� false + ���丮�� ���� ���������� ���� ��� && !isStoryEnd
        {
            // ȸ���� ���� 
            if (GameManager.instance.Episode_Round == 1)
            {
                irysHalo_OnHead.SetActive(true);
                irysHalo_ending1.SetActive(true);
                irysHalo_ending2.SetActive(false);

                irysStatue.SetBlendShapeWeight(0, 0f);  // ���̸��� ���� �� ���·� ����
            }
            else if (GameManager.instance.Episode_Round == 2)
            {
                irysHalo_OnHead.SetActive(false);
                irysHalo_ending1.SetActive(false);
                irysHalo_ending2.SetActive(true);

                irysStatue.SetBlendShapeWeight(0, 100f);  // ���̸��� ���� ���� ���·� ����
            }
            else
            {
                irysHalo_OnHead.SetActive(true);
                irysHalo_ending1.SetActive(true);
                irysHalo_ending2.SetActive(false);

                irysStatue.SetBlendShapeWeight(0, 0f);  // ���̸��� ���� �� ���·� ����
            }

            StartIntroStory();  // ���丮 ���� �Լ� ȣ��
        }
        else
        {
            if (GameManager.instance.PrevSceneName == "01. IntroScene")
            {
                player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // ������ ��ġ�� �̵�(Ʃ�丮�� ��)
            }

            PlayerUI.gameObject.SetActive(true);    // Ui Ȱ��ȭ
            PlayerUI.enabled = true;
        }

        // ���̸��� ���Ϸθ� ȹ���ߴ��� Ȯ���ؼ� �ʱ�ȭ ����
        if (ItemManager._instance.inventorySlots[10].GetComponent<Item10IrysHalo>().isGetItem)
        {
            mainHall_StartRoomDoor.GetComponent<Interaction_Gimics>().enabled = true; // ���� interaction gimic Ȱ��ȭ
            mainHall_StartRoomDoor.GetComponent<BoxCollider>().enabled = true;
            mainHall_StartRoomDoor.GetComponent<MeshCollider>().enabled = true;
            startRoomDoorWall.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            mainHall_StartRoomDoor.GetComponent<Interaction_Gimics>().enabled = false; // ���� interaction gimic Ȱ��ȭ
            mainHall_StartRoomDoor.GetComponent<BoxCollider>().enabled = false;
            mainHall_StartRoomDoor.GetComponent<MeshCollider>().enabled = false;
            startRoomDoorWall.GetComponent<BoxCollider>().enabled = true;
        }

        // player.GetComponent<PlayerCtrl>().keystrokes = true;    // Ű�Է��� ����
    }

    // ���丮 ���� �Լ�
    private void StartIntroStory()
    {
        player.GetComponent<PlayerCtrl>().keystrokes = true;    // ���丮�� ����Ǵ� ���� Ű���� �Է��� ����

        // Debug.Log("���� ���丮 ���" + mainHall_ObjectManager.CheckEndingEpisode_Num + "/" + isEndingStroryStart);    // �׽�Ʈ�� �α� ���
        Time.timeScale = 0;     // ���丮 ����� ���� ���� �Ͻ� ����
        interaction.run_Gimic = true;   // ���丮 ����� �����ߴٰ� ���� ����

        // ���丮 ������ ���� ���丮 ĵ���� Ȱ��ȭ
        storyCanvas.enabled = true;    // ���丮 ������ ĵ���� Ȱ��ȭ

        // ���� ������ ���� ������ ���丮 ����Ʈ�� �����ؼ� �ش� ���� ����(�׽�Ʈ��)
        // ���ǹ� �߰��ؼ� ���丮�� �̹� �� �������� �ƴ��� Ȯ���ؾߵ�
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1 && !isEndingStroryStart) // 1ȸ��, ���� ���丮 ���
        {
            maxStoryLine = storyLine.StoryLine_Start1.Count - 1;    // �ִ� ���丮 ���μ��� �ش� ���丮 ����Ʈ�� ���� -1�� ����

            // ���� ���丮�� ����� ĳ���� �̹��� Ȱ��ȭ(�����Ҷ� �Ƹ޿� �̳��� Ȱ��ȭ)
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2 && !isEndingStroryStart) // 2ȸ��, ���� ���丮 ���
        {
            maxStoryLine = storyLine.StoryLine_Start2.Count - 1;    // �ִ� ���丮 ���μ��� �ش� ���丮 ����Ʈ�� ���� -1�� ����

            // ���� ���丮�� ����� ĳ���� �̹��� Ȱ��ȭ(2ȸ���� �Ƹ�, �̳�, Ÿ�ڸ� Ȱ��ȭ)
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("�߸��� ���� ��� �õ�");
            // ���� ���� �߰� ���ɺκ�
        }

        // ó�� ������ ������ ��� �÷��̾� ��ġ ����
        player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[4].GetComponent<Transform>().position;
        player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);  // ������ ���� �ְԲ� ī�޶� ����

        Time.timeScale = 0;                         // �Ͻ�����
        Cursor.lockState = CursorLockMode.None;     // ���콺 Ŀ�� ǥ��

        // ���丮 ���� ��ũ��Ʈ ȣ���ؼ� ù ���� ���
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1)
        {
            storyLines = storyLine.StoryLine_Start1;
            PrintStoryScript(storyLines); // 1ȸ�� ���� ���丮
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2)
        {
            storyLines = storyLine.StoryLine_Start2;
            PrintStoryScript(storyLines); // 2ȸ�� ���� ���丮
        }

    }

    /// <summary>
    /// ���� ���丮 ��� �Լ�(MainHall_EndingExitDoorCtrl���� ����)
    /// </summary>
    public void StartEndingStory()
    {
        Debug.Log("���� ���丮 ���" + mainHall_ObjectManager.CheckEndingEpisode_Num + "/" + isEndingStroryStart);    // �׽�Ʈ �α� ���

        Time.timeScale = 0;     // ���丮 ����� ���� ���� �Ͻ� ����
        interaction.run_Gimic = true;   // ���丮 ����� �����ߴٰ� ���� ����

        // ���丮 ������ ���� ���丮 ĵ���� Ȱ��ȭ
        storyCanvas.enabled = true;    // ���丮 ������ ĵ���� Ȱ��ȭ

        Time.timeScale = 0;                         // �Ͻ�����
        Cursor.lockState = CursorLockMode.None;     // ���콺 Ŀ�� ǥ��

        // ���� ������ ���� ������ ���丮 ����Ʈ�� �����ؼ� �ش� ���� ����(�׽�Ʈ��)
        // ���ǹ� �߰��ؼ� ���丮�� �̹� �� �������� �ƴ��� Ȯ���ؾߵ�
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1 && isEndingStroryStart) // 1ȸ��, ���� ���丮 ���
        {
            maxStoryLine = storyLine.StoryLine_Ending1.Count - 1;   // �ִ� ���丮 ���μ��� �ش� ���丮 ����Ʈ�� ���� -1�� ����
            // ���� ���� �� ���丮 ����

            // 1ȸ�� ���� ���丮�� ����� ĳ���� �̹��� Ȱ��ȭ
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2 && isEndingStroryStart) // 2ȸ��, ���� ���丮 ���
        {
            maxStoryLine = storyLine.StoryLine_Ending2.Count - 1;   // �ִ� ���丮 ���μ��� �ش� ���丮 ����Ʈ�� ���� -1�� ����
            // ���� ���� �� ���丮 ����

            // 2ȸ�� ���� ���丮�� ����� ĳ���� �̹��� Ȱ��ȭ(Ÿ������ ������)
            for (int i = 0; i < character_Image.Count - 1; i++)
            {
                character_Image[i].gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("�߸��� ���� ��� �õ�");
            // ���� ���� �߰� ���ɺκ�
        }

        player.GetComponent<PlayerCtrl>().keystrokes = true;    // ���丮�� ����Ǵ� ���� Ű���� �Է��� ����

        // ���丮 ���� ��ũ��Ʈ ȣ���ؼ� ù ���� ���
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1)
        {
            storyLines = storyLine.StoryLine_Ending1;
            PrintStoryScript(storyLines); // 1ȸ�� ���� ���丮
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2)
        {
            storyLines = storyLine.StoryLine_Ending2;
            PrintStoryScript(storyLines); // 2ȸ�� ���� ���丮
        }
    }

    /// <summary>
    /// ������ ���� ���丮�� ������� �Լ�(MainHall_PassAfterTutorial���� ����)
    /// </summary>
    public void StartAfterPreviewStory()
    {
        // ���� ���� ȸ���� ���� ������ ���Ŀ� ������ ���丮 ����
        if (GameManager.instance.Episode_Round == 1)
        {
            storyLines = storyLine.StoryLine_MainHallDoorGuide1;

            // �ش� ���丮�� ����� ĳ���� �̹��� Ȱ��ȭ(1ȸ���� �Ƹ�, �̳��� Ȱ��ȭ)
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
        }
        else if (GameManager.instance.Episode_Round == 2)
        {
            storyLines = storyLine.StoryLine_MainHallDoorGuide2;

            // �ش� ���丮�� ����� ĳ���� �̹��� Ȱ��ȭ(2ȸ���� �Ƹ�, �̳�, Ÿ�ڸ� Ȱ��ȭ)
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
            character_Image[7].gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("���� ȸ�� ���� ����(���� ������ ���� �α�)");
        }

        maxStoryLine = storyLines.Count - 1;    // ���丮 �ִ� ���� �ʱ�ȭ

        player.GetComponent<PlayerCtrl>().keystrokes = true;    // ���丮�� ����Ǵ� ���� Ű���� �Է��� ����

        Time.timeScale = 0;                         // �Ͻ�����
        Cursor.lockState = CursorLockMode.None;     // ���콺 Ŀ�� ǥ��

        storyCanvas.enabled = true;    // ���丮 ������ ĵ���� Ȱ��ȭ
        PlayerUI.enabled = false;       // �÷��̾��� canvas�� ��Ȱ��ȭ

        // player.GetComponent<PlayerCtrl>().keystrokes = false;   // �÷��̾��� Ű�Է��� �����ϰ� ����

        PrintStoryScript(storyLines); // ���� ��� ����� ���� �Լ� ȣ��
    }

    // ���丮 ��縦 ����� �Լ�(�̸� �ۼ��ص� ��ũ��Ʈ�� �迭�� �̿��� ���)
    private void PrintStoryScript(List<StoryLineClass> _storyLine)
    {
        // ���� ���ϴ� ĳ������ �̹����� Ȱ��ȭ ���Ѽ� ������ ���ǹ�
        switch (_storyLine[currentStoryLine].StoryCharacter)
        {
            case StoryLineClass.StoryCharacter_Enum.AME:
                {
                    // ���� ���ϴ� ����� �Ƹ��� ���
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.AME);
                    storyLineCharacter_Text.text = string.Format("<color=#f2bd36>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // ������ ���丮 ĳ���͸� ���
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.INA:
                {
                    // ���� ���ϴ� ����� �̳��� ���
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.INA);
                    storyLineCharacter_Text.text = string.Format("<color=#5E169D>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // ������ ���丮 ĳ���͸� ���
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.IRYS:
                {
                    // ���� ���ϴ� ����� ���̸����� ���
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.IRYS);
                    storyLineCharacter_Text.text = string.Format("<color=#991150>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // ������ ���丮 ĳ���͸� ���
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.BAE:
                {
                    // ���� ���ϴ� ����� ������ ���
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.BAE);
                    storyLineCharacter_Text.text = string.Format("<color=#D72517>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // ������ ���丮 ĳ���͸� ���
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.FAUNA:
                {
                    // ���� ���ϴ� ����� �Ŀ쳪�� ���
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.FAUNA);
                    storyLineCharacter_Text.text = string.Format("<color=#33ca66>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // ������ ���丮 ĳ���͸� ���
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.KRONII:
                {
                    // ���� ���ϴ� ����� ũ�δ��� ���
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.KRONII);
                    storyLineCharacter_Text.text = string.Format("<color=#1d1797>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // ������ ���丮 ĳ���͸� ���
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.MUMEI:
                {
                    // ���� ���ϴ� ����� �������� ���
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.MUMEI);
                    storyLineCharacter_Text.text = string.Format("<color=#c29371>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // ������ ���丮 ĳ���͸� ���
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.TAKOS:
                {
                    // ���� ���ϴ� ����� �������� ���
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.TAKOS);
                    storyLineCharacter_Text.text = string.Format("<color=black>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // ������ ���丮 ĳ���͸� ���
                    break;
                }
            default:
                {
                    Debug.Log("�߸��� ĳ���� �̹��� Ȱ��ȭ �õ�");
                    break;
                }
        }

        storyLine_Text.text = _storyLine[currentStoryLine].StoryScript;     // ������ ���丮 ������ ���
    }

    // ���� ���ϴ� ĳ������ �̹����� Ȱ��ȭ �����ֱ� ���� �Լ�
    private void ActiveTalkingCharacterImage(int _characterNum)
    {
        // ���� ������ ���ϴ� ����� ���� ���(ó�� ���丮�� ��µǰ� �ִ� ���)
        if (lastTalkStoryCharacter == (int)StoryLineClass.StoryCharacter_Enum.NONE)
        {
            for (int i = 0; i < character_Image.Count; i++)
            {
                // ���� ���ϴ� ����� �ƴ� ĳ���͸� ���� ��Ȱ��ȭ
                if (i != _characterNum)
                {
                    character_Image[i].gameObject.SetActive(false);
                }
                else
                {
                    // ���� ���ϰ� �ִ� ĳ���� �̹����� Ȱ��ȭ �������� ����
                    character_Image[i].gameObject.SetActive(true);
                    character_Image[i].color = activeColor;
                }
            }

            lastTalkStoryCharacter = _characterNum; // ���� ��縦 ���ϰ� �ִ� ĳ���͸� ���������� ���� ĳ���ͷ� ����
        }
        else // ���� ������ ���ϴ� ����� �ִ� ���
        {
            // ���� �̹� ��縦 ���ϴ� ĳ���Ϳ� ���� ĳ���Ͱ� ���� ��� �������� ����
            if (_characterNum != lastTalkStoryCharacter)
            {
                for (int i = 0; i < character_Image.Count; i++)
                {
                    // 1. ���� ���ϴ� ����ΰ�
                    // 2. ���� �̹����� Ȱ��ȭ �Ǿ� �ִ°�
                    // 3. ���� �̹����� ������ ���� ����ΰ�
                    if ((character_Image[i].gameObject.activeSelf)) // �ش��̹����� Ȱ��ȭ �Ǿ� �ִ°�
                    {
                        if ((i != _characterNum))   // ���� ���ϴ� ����� �ƴѰ�
                        {
                            if ((i == lastTalkStoryCharacter))  // ���������� ���ߴ� ����ΰ�
                            {
                                // Ȱ��ȭ �ص�ä�� ���������� ���ϰ� �ִ� ĳ���� �̹����� ȸ�������� ����
                                character_Image[i].color = nonActiveColor;

                                //// �Ƹ� ���� ��󳢸� �Դٰ��� �ϴ� ��� ����
                                //if ((i != (int)StoryLineClass.StoryCharacter_Enum.AME) && (lastTalkStoryCharacter == (int)StoryLineClass.StoryCharacter_Enum.AME))  // �Ƹް� �ƴϰų�, ���������� ���ϴ� ����� �Ƹ��� ��� ����
                                //{
                                //    character_Image[i].color = activeColor;
                                //    character_Image[i].gameObject.SetActive(false);
                                //}
                            }
                            else  // ���������� ���ߴ� ����� �ƴѰ�
                            {
                                if (i != (int)StoryLineClass.StoryCharacter_Enum.AME)   // ���������� ���ߴ� ����� �Ƹް� �ƴ� ���
                                {
                                    // ���� ������� ������ ��Ȱ��ȭ ��Ŵ
                                    character_Image[i].color = activeColor;
                                    character_Image[i].gameObject.SetActive(false);
                                }
                                else   // ��Ȱ��ȭ ����� �Ƹ��� ���
                                {
                                    // Ȱ��ȭ �ص�ä�� ���������� ���ϰ� �ִ� ĳ���� �̹����� ȸ�������� ����
                                    character_Image[i].color = nonActiveColor;
                                }
                            }
                        }
                        else
                        {
                            // ���� ���ϴ� ����̴� Ȱ��ȭ
                            character_Image[i].color = activeColor;
                        }
                    }
                    else    // �ش� �̹����� ��Ȱ��ȭ �Ǿ� �ִ°�
                    {
                        if (i == _characterNum) // ���� ���ϴ� ����ΰ�
                        {
                            // ���� ���ϰ� �ִ� ĳ���� �̹��� Ȱ��ȭ
                            character_Image[i].gameObject.SetActive(true);
                            character_Image[i].color = activeColor;
                        }
                    }
                }

                lastTalkStoryCharacter = _characterNum; // ���� ��縦 ���ϰ� �ִ� ĳ���͸� ���������� ���� ĳ���ͷ� ����
            }
        }
    }

    // ���丮 ���� �Լ�
    private void EndStory()
    {
        currentStoryLine = 0;   // ���丮�� �������Ƿ� ���� ������� ���丮 ������ 0���� �ʱ�ȭ
        // player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // ������ ��ġ�� �̵�(Ʃ�丮�� ��)

        lastTalkStoryCharacter = (int)StoryLineClass.StoryCharacter_Enum.NONE;  // ���������� ���� ĳ���͵� ���ٰ� �ʱ�ȭ

        // ���丮�� �������Ƿ� ���丮 ĵ���� ��Ȱ��ȭ �� UI ĵ���� Ȱ��ȭ
        PlayerUI.enabled = true;       // ���丮�� �������Ƿ� �÷��̾��� ĵ���� Ȱ��ȭ
        storyCanvas.enabled = false;   // ���丮 ������ ĵ���� ��Ȱ��ȭ

        if (!isEndingStroryStart)  // ��µǴ� ���丮�� ������ �ƴ� ���
        {
            // 1ȸ���� ��� ���� ���丮 ������ Ʃ�丮������ Ȯ���ؾߵ�
            if (GameManager.instance.Episode_Round == 1) // 1ȸ�� ���� ���丮���� ���
            {
                storyCanvas.enabled = false;    // ���丮 ĵ���� ��Ȱ��ȭ
                player.GetComponent<PlayerCtrl>().keystrokes = false;    // Ű���� �Է��� Ǯ����
                Cursor.lockState = CursorLockMode.Locked;     // ���콺 Ŀ�� ���

                // isEndingStroryStart = true;    // ������ ����ϱ� ���� ���� ����
                isStoryEnd = true;             // ���丮�� ����Ǿ����Ƿ� true�� ����

                if (isWaitingTutorial) // Ʃ�丮�� ������ ����ϰ� ���� ���
                {
                    // ���Ϸ� ȹ����� ����ϰ� ���� ���丮 ���
                    // �� ���丮 ����� ���� ��� ���� ���·� ����
                    StartCoroutine(InventoryTutorialGuide());   // Ʃ�丮�� �Լ� ����
                    StartCoroutine(WaitGetHalo());  // ���Ϸ� ȹ�� ��� �ڷ�ƾ ����
                    isWaitingTutorial = false;
                }
                else // Ʃ�丮���� �ʿ���ų� ���� �Ϸ��� ���
                {
                    // Debug.Log("Ʃ�丮�� ����");   // �α� ���
                }

                // ���� �����䰡 ���� ���� ���丮�� ��µǾ��� ���
                // if ((storyLines == storyLine.StoryLine_MainHallDoorGuide1))
                if (Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_MainHallDoorGuide1))
                {
                    isEndingStroryStart = true;    // ������ ����ϱ� ���� ���� ����
                }
            }
            else if (GameManager.instance.Episode_Round == 2)        // 2ȸ���� ��� �׳� ����
            {
                player.GetComponentInChildren<Camera>().enabled = true;     // �÷��̾��� ����ī�޶� ��
                tutorialCam.GetComponentInChildren<Camera>().enabled = false;// Ʃ�丮��� ī�޶� ��

                player.GetComponent<PlayerCtrl>().keystrokes = false;    // Ű���� �Է��� Ǯ����
                Cursor.lockState = CursorLockMode.Locked;     // ���콺 Ŀ�� ���

                // isEndingStroryStart = true;    // ������ ����ϱ� ���� ���� ����
                isStoryEnd = true;             // ���丮�� ����Ǿ����Ƿ� true�� ����

                if (isWaitingTutorial) // Ʃ�丮�� ������ ����ϰ� ���� ���
                {
                    // ���Ϸ� ȹ����� ����ϰ� ���� ���丮 ���
                    StartCoroutine(WaitGetHalo());  // ���Ϸ� ȹ�� ��� �ڷ�ƾ ����
                    isWaitingTutorial = false;
                }
                else // Ʃ�丮���� �ʿ���ų� ���� �Ϸ��� ���
                {
                    Debug.Log("Ʃ�丮�� ����");   // �α� ���
                }

                // ���� �����䰡 ���� ���� ���丮�� ��µǾ��� ���
                // if ((storyLines == storyLine.StoryLine_MainHallDoorGuide2))
                if (Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_MainHallDoorGuide2))
                {
                    isEndingStroryStart = true;    // ������ ����ϱ� ���� ���� ����
                }

                // player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // ������ ��ġ�� �̵�(Ʃ�丮�� ��)

                // StartCoroutine(WaitGetHalo());  // ���Ϸ� ȹ�� ��� �ڷ�ƾ ����
            }
            else
            {

            }
        }
        else            // ��µǴ� ���丮�� ������ ���
        {
            if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1) // 1ȸ�� �����̿��� ���
            {
                // mainHall_ObjectManager.CheckEndingEpisode_Num = 2; // ���� ȸ���� 2�� �����Ŵ

                PlayEnding1();  // ���� ���� �Լ� ȣ��

                GameManager.instance.Episode_Round = 2; // ���� ȸ���� 2�� �����Ŵ
                // GameManager.instance.Episode_Round = 1; // ���� ȸ���� ���� ������ ����
            }
            else        // 2ȸ�� �����̿��� ���
            {
                // mainHall_ObjectManager.CheckEndingEpisode_Num = 1; // ���� ȸ���� 1�� �����Ŵ
                GameManager.instance.Episode_Round = 1; // ���� ȸ���� 1�� �����Ŵ
            }

            isEndingStroryStart = false;    // �ٽ� ���� ���丮�� ����� �� �ְ� false�� ����(�� ���� ������ ���߿� GameManager�� ȸ�������� �ݿ������ ��)
            isStoryEnd = false;             // ������ �����Ƿ� �ٽ� ���丮�� ����� �� �ֵ��� false�� ����

            GameManager.instance.NextEpisode();

            // �ٽ� ��Ʈ�� ������ ���ư�
            LoadingSceneManager.LoadScene("01. IntroScene");
        }

        // ���� ���丮�� �� �������� Ȯ���ؼ� ����
        if (isEndingStroryStart)
        {
            GameManager.instance.IsEndingStroryStart = 1;
        }
        else
        {
            GameManager.instance.IsEndingStroryStart = 0;
        }

        interaction.run_Gimic = isEndingStroryStart;     // ���丮 ���� ��Ȳ ������Ʈ

        // ���� �������̴� ���丮�� �� �ô��� �ƴ��� Ȯ���ؼ� ����
        if (isStoryEnd)
        {
            interaction.run_Gimic = true;
        }
        else
        {
            interaction.run_Gimic = false;
        }

        mainHall_ObjectManager.ChangeSceneData_To_GameManager();    // �����Լ� ȣ��

        player.GetComponent<PlayerCtrl>().keystrokes = false;    // �ٽ� Ű���� �Է��� �����ϰ� ����

        Time.timeScale = 1;     // �Ͻ� ���� ����
        Cursor.lockState = CursorLockMode.Locked;     // ���콺 Ŀ���� �Ⱥ��̰� ����
    }
    #endregion

    #region ��ư�� �Ҵ��� �Լ� ����(���丮 ���� �� ��ŵ)
    // ��ư�� ������ ���� ���丮�� ����ϱ� ���� �Լ�
    public void NextStory()
    {
        // Debug.Log("������ư Ŭ�� : " + storyLines + ", " + currentStoryLine);

        if (currentStoryLine == maxStoryLine)   // ���� ���� ���丮 ������ �ִ� ���丮 ���ΰ� ���� ��� ���丮�� �������Ƿ� ĵ���� ��Ȱ��ȭ
        {
            // isEndingStroryStart = true; // ���� ���丮�� ����Ǿ��ٰ� ���� ����
            // interaction.run_Gimic = isEndingStroryStart;     // ���丮 ���� ��Ȳ ������Ʈ

            // 1ȸ�� ���� ���丮�� ������̴� ���
            if (storyLines.SequenceEqual(storyLine.StoryLine_Ending1))
            {
                StartCoroutine(PlayEnding1());  // ���� ���� ����
                return;
            }
            // 2ȸ�� ���� ���丮�� ������̴� ���
            else if (storyLines.SequenceEqual(storyLine.StoryLine_Ending2))
            {
                StartCoroutine(PlayEnding2());  // ���� ���� ����
                return;
            }
            else
            {
                EndStory();    // ���丮 ���� �Լ� ȣ��

                return; // ���� �Լ� ����
            }
        }

        if (!isEndingStroryStart)   // ���� ���丮�� �ƴ� ���
        {
            //bool test;
            //test = Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start2);

            if (currentStoryLine == 3) // ���� ���丮���� �� 4��°������ ��� ������ ���� ������ �÷��̾� ��ġ �̵�
            {
                // Time.timeScale = 1;
                if (storyLines.SequenceEqual(storyLine.StoryLine_Start1) || Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start2))
                {
                    fadeInOut_Image.enabled = true;    // ������ ���� ���̵��� Ȱ��ȭ
                    fadeInOut_Image.color = new Color(14f, 14f, 14f, 0f);   // ������ 0���� ����

                    StartCoroutine(WaitFadeInOut());    // ȭ�� ���� ó�� �� �̵� ��ų �Լ�
                }
            }
            else if ((currentStoryLine == 10) && storyLines.SequenceEqual(storyLine.StoryLine_Start1)) // 1ȸ�� ���� ���丮 ������ 10��° ������ ��� ī�޶� ȸ������ ���� �ѷ���
            {
                StartCoroutine(Tutorial_LookAroundStartRoom()); // 1ȸ�� ���丮�� Ʃ�丮���� ���� ���� �ѹ��� �ѷ��� �Լ� ȣ��
            }
            else if ((currentStoryLine == 12) && Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start2)) // 2ȸ�� ���� ���丮 ������ 12��° ������ ��� ī�޶� ȸ������ ���� �ѷ���
            // else if ((currentStoryLine == 12) && test) // 2ȸ�� ���� ���丮 ������ 12��° ������ ��� ī�޶� ȸ������ ���� �ѷ���
            {
                StartCoroutine(LookAroundStartRoomBeforeGetHalo()); // 2ȸ�� ���丮�� ���Ϸ� ȹ���� ���� �������� �Ĵٺ� �Լ� ȣ��
            }
        }
        else // ������ ������ �ִ� ���� ���
        {
            if (GameManager.instance.Episode_Round == 1)    // 1ȸ�� ������ ���
            {
                if (Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Ending1))  // 1ȸ�� �������� �ѹ��� Ȯ��
                {
                    if (currentStoryLine == 18) // ���� ���丮���� �� 19��°������ ��� ��Ʈ ����
                    {
                        storyLine_Text.font = inanis_Font;  // ��Ʈ�� �̳� ��Ʈ�� ����
                        storyLine_Text.fontStyle = FontStyle.Italic;    // ��Ʈ�� ������� ��
                    }
                    else if (currentStoryLine == 19) // ���� ���丮���� �� 20��°������ ��� ��Ʈ ����
                    {
                        storyLine_Text.font = storyDefault_Font;  // ��Ʈ�� �⺻ ��Ʈ�� ����
                        storyLine_Text.fontStyle = FontStyle.Normal;    // ��Ʈ��Ÿ���� �븻�� ����
                    }
                    else if (currentStoryLine == 22)    //  �� ������ �Ҹ� ��� �� �̳� ���� ��Ȱ��ȭ
                    {
                        storyAudioSource.clip = doorClose_SFX;  // �� ������ �Ҹ��� ����
                        storyAudioSource.Play();               // �� ������ �Ҹ� ���

                        lastTalkStoryCharacter = (int)StoryLineClass.StoryCharacter_Enum.NONE;  // ������ ���ϴ� ����� ���ٰ� ���� ����
                    }
                }
            }
            else if (GameManager.instance.Episode_Round == 2)    // 2ȸ�� ������ ���
            {

            }
            else
            {
                // �߸��� ���
            }
        }

        currentStoryLine++; // ���� ������� ���丮������ �������� ����
        PrintStoryScript(storyLines); // ���� ��� ����� ���� �Լ� ȣ��
    }

    // ��ư�� ������ ���丮�� ��ŵ�ϴ� �Լ�
    public void SkipStory()
    {
        player.GetComponentInChildren<Camera>().enabled = true;     // �÷��̾��� ����ī�޶� ��
        tutorialCam.GetComponentInChildren<Camera>().enabled = false;// Ʃ�丮��� ī�޶� ��

        // ���� ���丮���� ��� �÷��̾ �̵���Ŵ
        if (Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start1) || Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start2))
        {
            player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // ������ ��ġ�� �̵�
            player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 180f, 0f);     // Ʃ�丮�� ���� ���� �Ĵٺ��Բ� ���� ����

            isWaitingTutorial = true;   // Ʃ�丮�� ��� ���·� ����
        }

        EndStory();    // ���丮 ���� �Լ� ȣ��
    }
    #endregion

    #region Ʃ�丮�� �� ī�޶� ������ �Լ� ����
    // 1ȸ�� ���丮�� Ʃ�丮���� ���� ���� �ѹ��� �ѷ��� �Լ�
    private IEnumerator Tutorial_LookAroundStartRoom()
    {
        Time.timeScale = 1;     // �Ͻ� ���� ����
        storyCanvas.enabled = false;    // ���丮 ĵ���� �Ͻ��� ��Ȱ��ȭ
        Cursor.lockState = CursorLockMode.Locked;     // ���콺 Ŀ���� �Ⱥ��̰� ����

        player.GetComponentInChildren<Camera>().enabled = false;    // �÷��̾��� ����ī�޶� ��� ��
        tutorialCam.GetComponentInChildren<Camera>().enabled = true;// Ʃ�丮��� ī�޶� ����
        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = true;
        player.gameObject.SetActive(false);
        // ī�޶� ȸ�� �ִϸ��̼� ����
        // tutorialCam_Anim.Play("Start_TutorialCamRotate");
        tutorialCam_Anim.SetBool("isCamRotate", true);

        yield return new WaitForSeconds(10f);    // �ִϸ��̼��� ����� ������ ���

        Time.timeScale = 0;     // �Ͻ� ����
        storyCanvas.enabled = true;    // ���丮 ĵ���� Ȱ��ȭ
        Cursor.lockState = CursorLockMode.None;     // ���콺 Ŀ�� Ȱ��ȭ

        player.gameObject.SetActive(true);
        player.GetComponentInChildren<Camera>().enabled = true;    // �÷��̾��� ����ī�޶� ��
        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = false;
        tutorialCam.GetComponentInChildren<Camera>().enabled = false;// Ʃ�丮��� ī�޶� ��

        isWaitingTutorial = true;
    }

    // 2ȸ�� ���丮�� ���Ϸθ� ���� ���� ���� �ѹ��� �ѷ��� �Լ�
    private IEnumerator LookAroundStartRoomBeforeGetHalo()
    {
        Time.timeScale = 1;     // �Ͻ� ���� ����
        storyCanvas.enabled = false;    // ���丮 ĵ���� �Ͻ��� ��Ȱ��ȭ
        Cursor.lockState = CursorLockMode.Locked;     // ���콺 Ŀ���� �Ⱥ��̰� ����

        player.GetComponentInChildren<Camera>().enabled = false;    // �÷��̾��� ����ī�޶� ��� ��
        tutorialCam.GetComponentInChildren<Camera>().enabled = true;// Ʃ�丮��� ī�޶� ����
        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = true;
        player.gameObject.SetActive(false);

        // ī�޶� ȸ�� �ִϸ��̼� ����
        tutorialCam_Anim.Play("Start_BeforeGetHalo");   // �ȵǸ� �Ʒ� ����� ����� ��
        // tutorialCam_Anim.SetBool("isCamRotate", true);

        yield return new WaitForSeconds(4.5f);    // �ִϸ��̼��� ����� ������ ���

        Time.timeScale = 0;     // �Ͻ� ����
        storyCanvas.enabled = true;    // ���丮 ĵ���� Ȱ��ȭ
        Cursor.lockState = CursorLockMode.None;     // ���콺 Ŀ�� Ȱ��ȭ

        player.gameObject.SetActive(true);
        player.GetComponentInChildren<Camera>().enabled = true;    // �÷��̾��� ����ī�޶� ��
        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = false;
        tutorialCam.GetComponentInChildren<Camera>().enabled = false;// Ʃ�丮��� ī�޶� ��

        isWaitingTutorial = true;
    }

    // ���Ϸ� ȹ�� ������ ����ϴٰ� ȹ���ϸ� ������ ���丮�� ����� �ڷ�ƾ �Լ�
    private IEnumerator WaitGetHalo()
    {
        // ���Ϸθ� ȹ���Ҷ����� ���
        yield return new WaitUntil(() => ItemManager._instance.inventorySlots[10].GetComponent<Item10IrysHalo>().isGetItem);

        mainHall_StartRoomDoor.GetComponent<Interaction_Gimics>().enabled = true; // ���� interaction gimic Ȱ��ȭ
        mainHall_StartRoomDoor.GetComponent<BoxCollider>().enabled = true;
        mainHall_StartRoomDoor.GetComponent<MeshCollider>().enabled = true;
        startRoomDoorWall.GetComponent<BoxCollider>().enabled = false;

        // ���̸��� ���Ϸ� ��Ȱ��ȭ
        irysHalo_OnHead.SetActive(false);
        irysHalo_ending1.SetActive(false);
        irysHalo_ending2.SetActive(false);

        Time.timeScale = 0;     // �Ͻ� ����
        player.GetComponent<PlayerCtrl>().keystrokes = true;    // Ű���� �Է¸���
        PlayerUI.enabled = false;      // �÷��̾��� ĵ���� ��Ȱ��ȭ
        storyCanvas.enabled = true;    // ���丮 ĵ���� Ȱ��ȭ
        Cursor.lockState = CursorLockMode.None;     // ���콺 Ŀ�� Ȱ��ȭ

        if (GameManager.instance.Episode_Round == 1)    // 1ȸ���� ���
        {
            // ���丮 ���
            storyLines = storyLine.AfterTutorial_Scripts;   // ���丮 ����
            maxStoryLine = storyLines.Count - 1;                // ���丮 �ִ� ���� �� ����
            PrintStoryScript(storyLines);
        }
        else if (GameManager.instance.Episode_Round == 2)   // 2ȸ���� ���
        {
            // ���丮 ���
            storyLines = storyLine.AfterGetHaloInEnding2_Scripts;   // ���丮 ����
            maxStoryLine = storyLines.Count - 1;                // ���丮 �ִ� ���� �� ����
            PrintStoryScript(storyLines);
        }
        else    // �߸��� ���
        {
            Debug.Log("�߸��� ���� ������.(���Ϸ� ȹ�� ��� �ڷ�ƾ)");    // �α� ���
            // 1ȸ���� ���丮 ���
            storyLines = storyLine.AfterTutorial_Scripts;   // ���丮 ����
            maxStoryLine = storyLines.Count - 1;                // ���丮 �ִ� ���� �� ����
            PrintStoryScript(storyLines);
        }
    }

    // �κ��丮 Ʃ�丮���� �����ϱ� ���� �ڷ�ƾ �Լ�
    private IEnumerator InventoryTutorialGuide()
    {
        // UI�� I��ư�� ���̶���Ʈ ó��
        // i ��ư ���������� ���
        // ������ Ʃ�丮�� �̹��� ���
        PlayerUI.GetComponent<InventoryTutorialCtrl>().arrowImage.color = new Color(255f, 255f, 255f, 255f); // ȭ��ǥ �̹����� Ȱ��ȭ����;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I));   // i ��ư�� �÷��̾ ���������� ���

        PlayerUI.GetComponent<InventoryTutorialCtrl>().arrowImage.color = new Color(255f, 255f, 255f, 0f); // ȭ��ǥ �̹����� ��Ȱ��ȭ����

        tutorial_UI.ActiveTutorialImage();  // Ʃ�丮�� UI Ȱ��ȭ
    }
    #endregion

    #region �����ϴ� �Լ� ����
    // fade in out �����ϴ� ���� ����Ű�� ���� �Լ�
    private IEnumerator WaitFadeInOut()
    {
        // storyCanvas.enabled = false;    // ���丮 ĵ�ٽ��� ��� ��Ȱ��ȭ

        Time.timeScale = 1;

        production_Anim.Play("FadeInOut_Story");

        yield return waitForSecond;    // 1�ʰ� ���

        // �÷��̾� ��ġ �̵�
        player.GetComponent<Transform>().localPosition = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // ������ ��ġ�� �̵�
        player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 180f, 0f);     // Ʃ�丮�� ���� ���� �Ĵٺ��Բ� ���� ����

        yield return waitForSecond;    // 1�ʰ� ���

        Time.timeScale = 0;

        fadeInOut_Image.enabled = false;    // ������ ���� ��Ȱ��ȭ
    }

    // 1ȸ�� ���� ������ ���� �Լ�
    private IEnumerator PlayEnding1()
    {
        storyTimeTravelImage.SetActive(true);

        Time.timeScale = 1;

        production_Anim.Play("TimeTravel"); // �ð� ���� �ִϸ��̼� ���

        storyAudioSource.clip = timeTravel_SFX; // �ð� ���� �Ҹ��� ����� Ŭ�� ����
        storyAudioSource.Play();    // �ð� ���� �Ҹ� ���

        yield return new WaitForSeconds(2f);    // 2�ʰ� ���

        EndStory();    // ���丮 ���� �Լ� ȣ��
    }

    // 2ȸ�� ���� ������ ���� �Լ�
    private IEnumerator PlayEnding2()
    {
        //storyTimeTravelImage.SetActive(true);

        //Time.timeScale = 1;

        //production_Anim.Play("TimeTravel");

        //storyAudioSource.clip = timeTravel_SFX; // �ð� ���� �Ҹ��� ����� Ŭ�� ����
        //storyAudioSource.Play();    // �ð� ���� �Ҹ� ���

        yield return new WaitForSeconds(2f);    // 2�ʰ� ���

        EndStory();    // ���丮 ���� �Լ� ȣ��
    }
    #endregion
}