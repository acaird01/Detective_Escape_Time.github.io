using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Intro_UIManager : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private GameObject player;                      // �÷��̾� ������Ʈ
    private Canvas PlayerUI;                        // �÷��̾ ���ϰ� �ִ� �κ��丮 ���� UI Canvas

    private RawImage IntroMV_RawImage;          // ��Ʈ�� ������ ����� RawImage UI
    private VideoPlayer IntroMV_VideoPlayer;    // ��Ʈ�� ������ ����� VideoPlayer

    private Intro_AmePC AmeInPC_BackGround;     // ������� �� �Ƹް� ����ִ� PC 3D ������Ʈ

    private Animator AmePC_AnimationCtrl;       // �Ƹ� PC�� Animation Controller
    private AnimationClip GuitarPlayingAme;     // ��Ÿ ġ�� �Ƹ� Animation Clip
    private AnimationClip VibinIsopod;          // ���� �ִ� �����(���) Animation Clip

    public Canvas OptionCanvas;
    public Canvas IntroCanvas;
    AudioSource audioSource;
    AudioManager audioManager;
    #endregion

    #region Intor UI���� ����� ���� ����

    bool optionSetactive = false; // �ɼ� ����â �������� ����
    [SerializeField]
    Button episode2Start_button;
    PlayerCtrl playerCtrl;
    #endregion

    private void Awake()
    {
        Init(); // �ʱ�ȭ �Լ� ȣ��
    }

    private void Init()
    {
        IntroMV_RawImage = GameObject.FindAnyObjectByType<RawImage>();          // ���̾��Ű�� ���� RawImage�� ���� ����� ���Ѱ� �ϳ��̱⿡ �ش� ������� ã�ƿ�
        IntroMV_VideoPlayer = GameObject.FindAnyObjectByType<VideoPlayer>();    // ���̾��Ű�� ���� VideoPlayer�� ��Ʈ�� ��������� �ϳ� ���̱⿡ �ش� ������� ã�ƿ�
        AmeInPC_BackGround = GameObject.FindAnyObjectByType<Intro_AmePC>();     // ������� ����� �Ƹ� PC�� ã�ƿͼ� �Ҵ�
        playerCtrl = GameObject.FindAnyObjectByType<PlayerCtrl>();
        PlayerUI = GameObject.FindAnyObjectByType<ItemManager>().GetComponent<Canvas>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioManager = GameObject.FindAnyObjectByType<AudioManager>();

        AmePC_AnimationCtrl = AmeInPC_BackGround.GetComponent<Animator>();  // �Ƹ� �ִϸ��̼� ����� ���� animator ������Ʈ
        
        AmeInPC_BackGround.gameObject.SetActive(false);     // ������ ��Ʈ�� ���� ����� ���� ������Ʈ ��Ȱ��ȭ
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;
        playerCtrl.keystrokes = true;
        ShowGameIntroUI();  // ���� ���� UI�� ������ �Լ� ȣ��
    }

    // Update is called once per frame
    void Update()
    {
        // ���丮 ������ ������϶� escŰ�� ���� ��� ��Ʈ�� ���丮 ��ŵ
        if (Input.GetKeyDown(KeyCode.Escape) && IntroMV_VideoPlayer.isPlaying)
        {
            IntroMV_VideoPlayer.Stop(); // ���� ��� ����   
        }

        if (GameManager.instance.Episode_Round == 2)
        {
            episode2Start_button.gameObject.SetActive(true);
        }
        else 
        {
            episode2Start_button.gameObject.SetActive(false);
        }
    }

    // ��Ʈ�� ������ ���� �� ���� ���� UI�� ������ �ڷ�ƾ �Լ�
    private IEnumerator IntorMovieStart()
    {
        Cursor.lockState = CursorLockMode.Locked;

        IntroMV_VideoPlayer.Play();     // ���� ���
        yield return new WaitUntil(() => IntroMV_VideoPlayer.isPlaying);    // ������ ���۵Ǳ� ��ٸ�(���� 1������ ����� ������ ������ֱ� ����)
        yield return new WaitUntil(() => !IntroMV_VideoPlayer.isPlaying);   // ������ ������ ��ٸ�

        GameManager.instance.PrevSceneName = "01. IntroScene";
        LoadingSceneManager.LoadScene("02. MainHallScene");  // ���� ������ �̵�
    }

    // ���� ���� UI�� ������ �Լ�
    private void ShowGameIntroUI()
    {
        if (GameManager.instance.Episode_Round == 2)
        {
            playerCtrl.keystrokes = false;
            PlayerUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }

        IntroMV_RawImage.gameObject.SetActive(false);   // RawImage ��Ȱ��ȭ
        IntroMV_VideoPlayer.gameObject.SetActive(false);    // VideoPlayer ��Ȱ��ȭ

        AmeInPC_BackGround.gameObject.SetActive(true);  // ������� ����� �Ƹ� PC ������Ʈ Ȱ��ȭ
        AmePC_AnimationCtrl.Play("amelia_Action_001");  // ��Ÿ ġ�� �Ƹ� Animation Clip ���
        AmePC_AnimationCtrl.Play("isopod_002_Action");  // ���� �ִ� �����(���) Animation Clip ���
    }

    // �� ����
    public void newGameStart()
    {
        audioSource.Stop();
        //AmeInPC_BackGround.gameObject.SetActive(false);  // ������� ����� �Ƹ� PC ������Ʈ ��Ȱ��ȭ
        IntroMV_RawImage.gameObject.SetActive(true);
        IntroMV_VideoPlayer.gameObject.SetActive(true);    // VideoPlayer Ȱ��ȭ
        //GameManager.instance.Episode_Round = 1;         // 1ȸ���� ����
        
        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;
        audioManager.SaveVolume();
        if (episode2Start_button.gameObject.activeSelf) // �ð谡 ���������� ����
        {
            episode2Start_button.gameObject.SetActive(false);
        }
        ItemManager._instance.ItemInventroyReset();
        GameManager.instance.NewGameData();
        StartCoroutine(IntorMovieStart());  // ������ ������ ���� ���� UI�� ������ �ڷ�ƾ �Լ� ȣ��
        
    }
    // �̾��ϱ�
    public void ContinueGame()
    {
        audioSource.Stop();
        GameManager.instance.PrevSceneName = "01. IntroScene";
        ItemManager._instance.ItemInventroyReset();
        GameManager.instance.LoadData();      
        
        //LoadingSceneManager.LoadScene("02. MainHallScene");  // ���� ������ �̵�
    }

    // �� ����(���Ǽҵ� 2)
    public void newGameStart_2()
    {
        audioSource.Stop();
        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;
        Cursor.lockState = CursorLockMode.None;
        audioManager.SaveVolume();

        GameManager.instance.NewGameData(); // �ʱ�ȭ �Ŀ�
        GameManager.instance.Episode_Round = 2;// ���常 2ȸ���� ����
        LoadingSceneManager.LoadScene("02. MainHallScene");  // ���� ������ �̵�

    }

    // ���� ����
    public void QuitGame()
    {
        GameManager.instance.SaveData();
        Application.Quit();
    }
    // ���� ����
    public void OptionSetting()
    {
        optionSetactive = !optionSetactive;
        if (optionSetactive)
        {
            OptionCanvas.gameObject.SetActive(true);
            IntroCanvas.gameObject.SetActive(false);
        }
        else
        {
            OptionCanvas.gameObject.SetActive(false);
            IntroCanvas.gameObject.SetActive(true);
        }
    }
}
