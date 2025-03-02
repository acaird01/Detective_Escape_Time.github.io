using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Intro_UIManager : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private GameObject player;                      // 플레이어 오브젝트
    private Canvas PlayerUI;                        // 플레이어가 지니고 있는 인벤토리 등의 UI Canvas

    private RawImage IntroMV_RawImage;          // 인트로 영상을 출력할 RawImage UI
    private VideoPlayer IntroMV_VideoPlayer;    // 인트로 영상을 재생할 VideoPlayer

    private Intro_AmePC AmeInPC_BackGround;     // 배경으로 쓸 아메가 들어있는 PC 3D 오브젝트

    private Animator AmePC_AnimationCtrl;       // 아메 PC의 Animation Controller
    private AnimationClip GuitarPlayingAme;     // 기타 치는 아메 Animation Clip
    private AnimationClip VibinIsopod;          // 즐기고 있는 콩벌레(등각류) Animation Clip

    public Canvas OptionCanvas;
    public Canvas IntroCanvas;
    AudioSource audioSource;
    AudioManager audioManager;
    #endregion

    #region Intor UI에서 사용할 변수 모음

    bool optionSetactive = false; // 옵션 설정창 켜졌는지 여부
    [SerializeField]
    Button episode2Start_button;
    PlayerCtrl playerCtrl;
    #endregion

    private void Awake()
    {
        Init(); // 초기화 함수 호출
    }

    private void Init()
    {
        IntroMV_RawImage = GameObject.FindAnyObjectByType<RawImage>();          // 하이어라키에 현재 RawImage가 비디오 재생을 위한것 하나이기에 해당 방식으로 찾아옴
        IntroMV_VideoPlayer = GameObject.FindAnyObjectByType<VideoPlayer>();    // 하이어라키에 현재 VideoPlayer가 인트로 영상용으로 하나 뿐이기에 해당 방식으로 찾아옴
        AmeInPC_BackGround = GameObject.FindAnyObjectByType<Intro_AmePC>();     // 배경으로 사용할 아메 PC를 찾아와서 할당
        playerCtrl = GameObject.FindAnyObjectByType<PlayerCtrl>();
        PlayerUI = GameObject.FindAnyObjectByType<ItemManager>().GetComponent<Canvas>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioManager = GameObject.FindAnyObjectByType<AudioManager>();

        AmePC_AnimationCtrl = AmeInPC_BackGround.GetComponent<Animator>();  // 아메 애니메이션 재생을 위한 animator 컴포넌트
        
        AmeInPC_BackGround.gameObject.SetActive(false);     // 시작전 인트로 영상 출력을 위해 오브젝트 비활성화
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;
        playerCtrl.keystrokes = true;
        ShowGameIntroUI();  // 게임 시작 UI를 보여줄 함수 호출
    }

    // Update is called once per frame
    void Update()
    {
        // 스토리 영상이 재생중일때 esc키를 누를 경우 인트로 스토리 스킵
        if (Input.GetKeyDown(KeyCode.Escape) && IntroMV_VideoPlayer.isPlaying)
        {
            IntroMV_VideoPlayer.Stop(); // 영상 재생 종료   
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

    // 인트로 영상이 끝난 뒤 게임 시작 UI를 보여줄 코루틴 함수
    private IEnumerator IntorMovieStart()
    {
        Cursor.lockState = CursorLockMode.Locked;

        IntroMV_VideoPlayer.Play();     // 영상 재생
        yield return new WaitUntil(() => IntroMV_VideoPlayer.isPlaying);    // 영상이 시작되길 기다림(현재 1프레임 대기후 영상을 재생해주기 때문)
        yield return new WaitUntil(() => !IntroMV_VideoPlayer.isPlaying);   // 영상이 끝나길 기다림

        GameManager.instance.PrevSceneName = "01. IntroScene";
        LoadingSceneManager.LoadScene("02. MainHallScene");  // 복도 씬으로 이동
    }

    // 게임 시작 UI를 보여줄 함수
    private void ShowGameIntroUI()
    {
        if (GameManager.instance.Episode_Round == 2)
        {
            playerCtrl.keystrokes = false;
            PlayerUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }

        IntroMV_RawImage.gameObject.SetActive(false);   // RawImage 비활성화
        IntroMV_VideoPlayer.gameObject.SetActive(false);    // VideoPlayer 비활성화

        AmeInPC_BackGround.gameObject.SetActive(true);  // 배경으로 사용할 아메 PC 오브젝트 활성화
        AmePC_AnimationCtrl.Play("amelia_Action_001");  // 기타 치는 아메 Animation Clip 재생
        AmePC_AnimationCtrl.Play("isopod_002_Action");  // 즐기고 있는 콩벌레(등각류) Animation Clip 재생
    }

    // 새 게임
    public void newGameStart()
    {
        audioSource.Stop();
        //AmeInPC_BackGround.gameObject.SetActive(false);  // 배경으로 사용할 아메 PC 오브젝트 비활성화
        IntroMV_RawImage.gameObject.SetActive(true);
        IntroMV_VideoPlayer.gameObject.SetActive(true);    // VideoPlayer 활성화
        //GameManager.instance.Episode_Round = 1;         // 1회차로 변경
        
        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;
        audioManager.SaveVolume();
        if (episode2Start_button.gameObject.activeSelf) // 시계가 켜져있으면 끌것
        {
            episode2Start_button.gameObject.SetActive(false);
        }
        ItemManager._instance.ItemInventroyReset();
        GameManager.instance.NewGameData();
        StartCoroutine(IntorMovieStart());  // 영상이 끝나면 게임 시작 UI를 보여줄 코루틴 함수 호출
        
    }
    // 이어하기
    public void ContinueGame()
    {
        audioSource.Stop();
        GameManager.instance.PrevSceneName = "01. IntroScene";
        ItemManager._instance.ItemInventroyReset();
        GameManager.instance.LoadData();      
        
        //LoadingSceneManager.LoadScene("02. MainHallScene");  // 복도 씬으로 이동
    }

    // 새 게임(에피소드 2)
    public void newGameStart_2()
    {
        audioSource.Stop();
        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;
        Cursor.lockState = CursorLockMode.None;
        audioManager.SaveVolume();

        GameManager.instance.NewGameData(); // 초기화 후에
        GameManager.instance.Episode_Round = 2;// 라운드만 2회차로 변경
        LoadingSceneManager.LoadScene("02. MainHallScene");  // 복도 씬으로 이동

    }

    // 게임 종료
    public void QuitGame()
    {
        GameManager.instance.SaveData();
        Application.Quit();
    }
    // 게임 설정
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
