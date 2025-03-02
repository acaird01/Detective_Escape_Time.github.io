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
    /// 1. 로딩전 현재 회차 확인
    /// 2. 회차에 따라 초반 스토리 및 진행 일부 변경
    /// 3. 플레이어 찾아와서 스토리 보여줄땐 나머지 UI 비활성화 -> 끝나면 활성화
    /// </summary>

    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    private GameObject player;                      // 플레이어 오브젝트
    private MainHall_ObjectManager mainHall_ObjectManager;     // 복도씬의 오브젝트 매니저
    private Interaction_Gimics interaction; // 상호작용하는 기믹인지 확인하기 위한 컴포넌트

    private Canvas PlayerUI;                        // 플레이어가 지니고 있는 인벤토리 등의 UI Canvas
    private Canvas storyCanvas;                     // 스토리 및 튜토리얼 내용을 보여줄 Canvas
    private Text storyLineCharacter_Text;            // 진행되는 스토리 캐릭터의 이름을 보여줄 Text UI
    private Text storyLine_Text;                    // 진행되는 스토리 및 튜토리얼의 내용을 보여줄 Text UI
    private InventoryTutorialCtrl tutorial_UI;         // 튜토리얼을 보여줄 UI를 관리하는 스크립트
    private MainHall_StartRoomDoorWall startRoomDoorWall;   // 시작방의 문이 달린 벽
    private GameObject mainHall_StartRoomDoor;      // 복도 씬의 시작방의 문 게임 오브젝트

    // 스토리 재생중 틀어줄 audio source 및 audio clip들 모음
    private AudioSource storyAudioSource;           // 스토리 진행중 사운드를 재생해줄 audio source
    [Header("시간 여행 효과음")]
    [SerializeField]
    private AudioClip timeTravel_SFX;
    [Header("문 닫히는 소리")]
    [SerializeField]
    private AudioClip doorClose_SFX;
    [Header("의사당으로 이동했을때 바꿔줄 BGM")]
    [SerializeField]
    private AudioClip ending2_BGM;
    [Header("복도씬 기본 BGM")]
    [SerializeField]
    private AudioClip mainHall_BGM;

    // 폰트 변경용 변수 모음
    [Header("스토리 진행 기본 폰트")]
    [SerializeField]
    private Font storyDefault_Font;
    [Header("이나 폰트(Bold)")]
    [SerializeField]
    private Font inanis_Font;

    // 스토리 사용 이미지 모음
    private GameObject storyTimeTravelImage;         // 1회차 엔딩에서 시간여행처리로 보여주기 위한 Image UI
    private List<Image> character_Image;             // 스토리에서 보여줄 캐릭터 Image UI 모음 배열
    [Header("카운슬 의사당 이미지")]
    [SerializeField]
    private GameObject storyEnding2Council_Image;         // 2회차 엔딩에서 보여줄 의사당 Image
    [Header("흰색 페이드인아웃 이미지")]
    [SerializeField]
    private Image storyWhiteOut_Image;          // 2회차 엔딩에서 의사당을 보여주기전에 띄워줄 Image

    // 아이리스 조각상 관련 변수 모음
    [Header("아이리스 머리위의 헤일로")]
    [SerializeField]
    private GameObject irysHalo_OnHead;
    [Header("아이리스 1회차 헤일로 콜라이더용")]
    [SerializeField]
    private GameObject irysHalo_ending1;
    [Header("아이리스 2회차 헤일로 위치")]
    [SerializeField]
    private GameObject irysHalo_ending2;
    [Header("아이리스 표정변화를 위한 모델의 MeshRenderer")]
    [SerializeField]
    private SkinnedMeshRenderer irysStatue;

    // 튜토리얼 진행용 변수 모음
    [Header("튜토리얼 전 방을 둘러볼 카메라")]
    [SerializeField]
    private GameObject tutorialCam;     // 튜토리얼 진행 시 보여줄 카메라
    [Header("튜토리얼 진행 시 카메라를 회전 시킬 애니메이터")]
    [SerializeField]
    private Animator tutorialCam_Anim;  // 튜토리얼 진행 시 카메라를 회전 시킬 애니메이터
    [Header("엔딩 및 페이드인아웃을 실행시킬 애니메이터")]
    [SerializeField]
    private Animator production_Anim;  // 연출을 진행할 애니메이터
    #endregion

    #region 스토리 출력에 사용할 변수
    private int currentStoryLine;               // 현재 스토리 라인
    private int maxStoryLine;                   // 마지막 스토리 라인 번호
    private int lastTalkStoryCharacter;          // 마지막으로 대사를 말한 캐릭터확인용 변수(StoryLineClass.StoryCharacter_Enum의 번호에 따름)
    private Image fadeInOut_Image;              // Fade in, out 연출에 사용할 canvas
    private MainHall_StoryScript storyLine;     // 스토리가 저장된 클래스 선언
    private bool isEndingStroryStart = false;   // 엔딩 스토리 출력 확인용 변수(true : 엔딩 스토리 / false : 시작 스토리)
    private List<StoryLineClass> storyLines;    // 현재 출력할 스토리를 저장할 리스트
    public bool isStoryEnd;                     // 스토리 출력 완료 여부 확인용 변수(스토리든 엔딩이든 상관x)(true : 스토리 재생완료 / false : 스토리 재생 미완)
    private bool isWaitingTutorial;    // 튜토리얼 스토리를 진행해야되는지 확인하기 위한 변수

    private Color activeColor = new Color32(255, 255, 255, 255);      // 대사를 말하고 있는 캐릭터의 색상
    private Color nonActiveColor = new Color32(63, 63, 63, 220);      // 대사를 마지막으로 말한 캐릭터의 색상

    private WaitForSeconds waitForSecond = new WaitForSeconds(1f);
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기(Start 대용)
    {
        player = GameObject.Find("Player"); // 하이어라키에서 플레이어를 찾아와서 할당
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();     // 복도 씬에 있는 오브젝트 매니저를 할당

        //Debug.Log("prev : " + GameManager.instance.PrevSceneName);
        //Debug.Log("now : " + GameManager.instance.nowSceneName);

        if (GameManager.instance.PrevSceneName == "01. IntroScene")
        {
            player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // 지정된 위치로 이동(튜토리얼 방)
        }

        Init();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 해당 씬에서 사용할 변수들 할당 및 초기화하기 위한 함수(오브젝트 매니저에서 사용)
    public void Init()
    {
        PlayerUI = player.GetComponentInChildren<Canvas>();                 // player의 자식으로 있는 canvas를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();        // 상호작용을 위한 Interaction_Gimics 할당

        storyTimeTravelImage = GetComponentInChildren<StoryTimeTravelImage>().gameObject;  // 1회차 엔딩볼때 시간여행하는걸 보여주기 위한 Image를 찾아와서 할당
        startRoomDoorWall = GameObject.FindAnyObjectByType<MainHall_StartRoomDoorWall>();               // 시작 방의 문이 달린 벽 오브젝트
        mainHall_StartRoomDoor = GameObject.FindAnyObjectByType<MainHall_StartRoomDoor>().gameObject;   // 시작방의 문 오브젝트를 찾아와서 할당
        fadeInOut_Image = this.GetComponentInChildren<FadeInOutCanvas>().GetComponent<Image>();            // Fade in, out 연출에 사용할 canvas를 찾아와서 할당

        // 시작할땐 안보이도록 비활성화
        fadeInOut_Image.enabled = false;
        storyTimeTravelImage.SetActive(false);

        storyCanvas = this.GetComponentInChildren<StoryCanvas>().GetComponent<Canvas>();                         // 스토리 및 튜토리얼 스크립트를 보여줄 canvas
        storyLineCharacter_Text = GameObject.FindAnyObjectByType<CharacterStoryName_Text>().GetComponent<Text>();  // 현재 대사를 치고있는 캐릭터를 보여줄 text ui를 찾아와서 text 컴포넌트 할당
        storyLine_Text = GameObject.FindAnyObjectByType<CharacterStoryScript_Text>().GetComponent<Text>();  // 스토리를 출력할 text ui를 찾아와서 text 컴포넌트 할당
        tutorial_UI = PlayerUI.GetComponent<InventoryTutorialCtrl>();   // turorialUI를 관리하는 스크립트를 찾아와서 할당

        storyAudioSource = this.GetComponent<AudioSource>();    // story canvas에 달려있는 스토리 재생중 소리를 틀어줄 Audio source를 찾아와서 할당

        // 스토리 진행에 사용할 이미지를 찾아와서 저장
        storyWhiteOut_Image.enabled = false; // 비활성화 해둠
        storyEnding2Council_Image.SetActive(false);

        character_Image = new List<Image>();  // 캐릭터 이미지를 담을 리스트 생성
        character_Image?.Clear();             // 사용할 리스트 초기화
        character_Image.Add(GetComponentInChildren<StoryAmeImage>().GetComponent<Image>());     // 스토리 연출에 사용할 아메 이미지
        character_Image.Add(GetComponentInChildren<StoryInaImage>().GetComponent<Image>());     // 스토리 연출에 사용할 이나 이미지
        character_Image.Add(GetComponentInChildren<StoryIrysImage>().GetComponent<Image>());    // 스토리 연출에 사용할 아이리스 이미지
        character_Image.Add(GetComponentInChildren<StoryBaeImage>().GetComponent<Image>());     // 스토리 연출에 사용할 베이 이미지
        character_Image.Add(GetComponentInChildren<StoryFaunaImage>().GetComponent<Image>());   // 스토리 연출에 사용할 파우나 이미지
        character_Image.Add(GetComponentInChildren<StoryKroniiImage>().GetComponent<Image>());  // 스토리 연출에 사용할 크로니 이미지
        character_Image.Add(GetComponentInChildren<StoryMumeiImage>().GetComponent<Image>());   // 스토리 연출에 사용할 무메이 이미지
        character_Image.Add(GetComponentInChildren<StoryTakosImage>().GetComponent<Image>());   // 스토리 연출에 사용할 타코들 이미지

        // 스토리 진행 전 사용 이미지 비활성화
        for (int i = 0; i < character_Image.Count; i++)
        {
            character_Image[i].gameObject.SetActive(false);
        }

        storyLines = null;          // 진행할 스토리를 null로 초기화
        currentStoryLine = 0;       // 현재 출력중인 스토리 라인을 0번째로 초기화
        lastTalkStoryCharacter = (int)StoryLineClass.StoryCharacter_Enum.NONE; // 마지막으로 대사를 치는 캐릭터가 없다고 초기화
        maxStoryLine = 0;        // 마지막 스토리 라인을 스토리가 저장된 배열의 길이로 초기화(현재는 테스트를 위해 0으로 초기화)
        interaction.run_Gimic = false;  // 스토리 출력을 시작하지 않았다고 상태 변경

        // 저장되어있던 스토리 진행여부에 따라 스토리를 진행하기 위한 변수 초기화
        isStoryEnd = interaction.run_Gimic;                 // 스토리를 이미 봤는지 아닌지 설정
        if (GameManager.instance.IsEndingStroryStart == 0)  // 엔딩 스토리를 봐야하는지 아닌지 설정
        {
            isEndingStroryStart = false;
        }
        else
        {
            isEndingStroryStart = true;
        }

        // 시작 스토리를 아직 안봤고, 1회차일때 실행
        if (!isStoryEnd && !isEndingStroryStart)
        {
            ItemManager._instance.ResetInventory();
            //if ((GameManager.instance.Episode_Round == 1))
            //{
            //    ItemManager._instance.ResetInventory();
            //}
        }

        PlayerUI.enabled = false;       // 스토리를 보여주기 위해 플레이어의 캔버스를 끔
        storyCanvas.enabled = false;    // 스토리 보여줄 캔버스 비활성화

        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = false;
        tutorialCam.GetComponent<Camera>().enabled = false; // 튜토리얼 용 카메라를 비활성화

        storyLine = new MainHall_StoryScript();   // 스토리를 출력하기 위해 스토리가 저장된 클래스 생성
        storyLine.InitStory();                    // 스토리 초기화

        Setting_SceneStart();
    }
    #region 스토리 진행 함수 모음
    // 씬 시작했을 때 스토리 진행을 결정하기 위한 함수
    void Setting_SceneStart()
    {
        // 아직 시작 스토리를 안봤을 경우 스토리 진행
        if (!isEndingStroryStart)  // 엔딩스토리 시작 변수 false + 스토리가 현재 진행중이지 않을 경우 && !isStoryEnd
        {
            // 회차에 따른 
            if (GameManager.instance.Episode_Round == 1)
            {
                irysHalo_OnHead.SetActive(true);
                irysHalo_ending1.SetActive(true);
                irysHalo_ending2.SetActive(false);

                irysStatue.SetBlendShapeWeight(0, 0f);  // 아이리스 눈을 뜬 상태로 설정
            }
            else if (GameManager.instance.Episode_Round == 2)
            {
                irysHalo_OnHead.SetActive(false);
                irysHalo_ending1.SetActive(false);
                irysHalo_ending2.SetActive(true);

                irysStatue.SetBlendShapeWeight(0, 100f);  // 아이리스 눈을 감은 상태로 설정
            }
            else
            {
                irysHalo_OnHead.SetActive(true);
                irysHalo_ending1.SetActive(true);
                irysHalo_ending2.SetActive(false);

                irysStatue.SetBlendShapeWeight(0, 0f);  // 아이리스 눈을 뜬 상태로 설정
            }

            StartIntroStory();  // 스토리 시작 함수 호출
        }
        else
        {
            if (GameManager.instance.PrevSceneName == "01. IntroScene")
            {
                player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // 지정된 위치로 이동(튜토리얼 방)
            }

            PlayerUI.gameObject.SetActive(true);    // Ui 활성화
            PlayerUI.enabled = true;
        }

        // 아이리스 헤일로를 획득했는지 확인해서 초기화 해줌
        if (ItemManager._instance.inventorySlots[10].GetComponent<Item10IrysHalo>().isGetItem)
        {
            mainHall_StartRoomDoor.GetComponent<Interaction_Gimics>().enabled = true; // 문의 interaction gimic 활성화
            mainHall_StartRoomDoor.GetComponent<BoxCollider>().enabled = true;
            mainHall_StartRoomDoor.GetComponent<MeshCollider>().enabled = true;
            startRoomDoorWall.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            mainHall_StartRoomDoor.GetComponent<Interaction_Gimics>().enabled = false; // 문의 interaction gimic 활성화
            mainHall_StartRoomDoor.GetComponent<BoxCollider>().enabled = false;
            mainHall_StartRoomDoor.GetComponent<MeshCollider>().enabled = false;
            startRoomDoorWall.GetComponent<BoxCollider>().enabled = true;
        }

        // player.GetComponent<PlayerCtrl>().keystrokes = true;    // 키입력을 막음
    }

    // 스토리 시작 함수
    private void StartIntroStory()
    {
        player.GetComponent<PlayerCtrl>().keystrokes = true;    // 스토리가 진행되는 동안 키보드 입력을 막음

        // Debug.Log("시작 스토리 출력" + mainHall_ObjectManager.CheckEndingEpisode_Num + "/" + isEndingStroryStart);    // 테스트용 로그 출력
        Time.timeScale = 0;     // 스토리 출력을 위해 게임 일시 정지
        interaction.run_Gimic = true;   // 스토리 출력을 시작했다고 상태 변경

        // 스토리 진행을 위해 스토리 캔버스 활성화
        storyCanvas.enabled = true;    // 스토리 보여줄 캔버스 활성화

        // 현재 엔딩에 따라 진행할 스토리 리스트를 지정해서 해당 길이 저장(테스트중)
        // 조건문 추가해서 스토리를 이미 본 상태인지 아닌지 확인해야됨
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1 && !isEndingStroryStart) // 1회차, 시작 스토리 출력
        {
            maxStoryLine = storyLine.StoryLine_Start1.Count - 1;    // 최대 스토리 라인수를 해당 스토리 리스트의 길이 -1로 저장

            // 시작 스토리에 사용할 캐릭터 이미지 활성화(시작할땐 아메와 이나만 활성화)
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2 && !isEndingStroryStart) // 2회차, 시작 스토리 출력
        {
            maxStoryLine = storyLine.StoryLine_Start2.Count - 1;    // 최대 스토리 라인수를 해당 스토리 리스트의 길이 -1로 저장

            // 시작 스토리에 사용할 캐릭터 이미지 활성화(2회차는 아메, 이나, 타코만 활성화)
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("잘못된 엔딩 출력 시도");
            // 히든 엔딩 추가 가능부분
        }

        // 처음 게임을 시작한 경우 플레이어 위치 조정
        player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[4].GetComponent<Transform>().position;
        player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);  // 정면을 보고 있게끔 카메라 조정

        Time.timeScale = 0;                         // 일시정지
        Cursor.lockState = CursorLockMode.None;     // 마우스 커서 표시

        // 스토리 시작 스크립트 호출해서 첫 라인 출력
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1)
        {
            storyLines = storyLine.StoryLine_Start1;
            PrintStoryScript(storyLines); // 1회차 시작 스토리
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2)
        {
            storyLines = storyLine.StoryLine_Start2;
            PrintStoryScript(storyLines); // 2회차 시작 스토리
        }

    }

    /// <summary>
    /// 엔딩 스토리 출력 함수(MainHall_EndingExitDoorCtrl에서 실행)
    /// </summary>
    public void StartEndingStory()
    {
        Debug.Log("엔딩 스토리 출력" + mainHall_ObjectManager.CheckEndingEpisode_Num + "/" + isEndingStroryStart);    // 테스트 로그 출력

        Time.timeScale = 0;     // 스토리 출력을 위해 게임 일시 정지
        interaction.run_Gimic = true;   // 스토리 출력을 시작했다고 상태 변경

        // 스토리 진행을 위해 스토리 캔버스 활성화
        storyCanvas.enabled = true;    // 스토리 보여줄 캔버스 활성화

        Time.timeScale = 0;                         // 일시정지
        Cursor.lockState = CursorLockMode.None;     // 마우스 커서 표시

        // 현재 엔딩에 따라 진행할 스토리 리스트를 지정해서 해당 길이 저장(테스트중)
        // 조건문 추가해서 스토리를 이미 본 상태인지 아닌지 확인해야됨
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1 && isEndingStroryStart) // 1회차, 엔딩 스토리 출력
        {
            maxStoryLine = storyLine.StoryLine_Ending1.Count - 1;   // 최대 스토리 라인수를 해당 스토리 리스트의 길이 -1로 저장
            // 엔딩 연출 및 스토리 진행

            // 1회차 엔딩 스토리에 사용할 캐릭터 이미지 활성화
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2 && isEndingStroryStart) // 2회차, 엔딩 스토리 출력
        {
            maxStoryLine = storyLine.StoryLine_Ending2.Count - 1;   // 최대 스토리 라인수를 해당 스토리 리스트의 길이 -1로 저장
            // 엔딩 연출 및 스토리 진행

            // 2회차 엔딩 스토리에 사용할 캐릭터 이미지 활성화(타코제외 나머지)
            for (int i = 0; i < character_Image.Count - 1; i++)
            {
                character_Image[i].gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("잘못된 엔딩 출력 시도");
            // 히든 엔딩 추가 가능부분
        }

        player.GetComponent<PlayerCtrl>().keystrokes = true;    // 스토리가 진행되는 동안 키보드 입력을 막음

        // 스토리 엔딩 스크립트 호출해서 첫 라인 출력
        if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1)
        {
            storyLines = storyLine.StoryLine_Ending1;
            PrintStoryScript(storyLines); // 1회차 엔딩 스토리
        }
        else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2)
        {
            storyLines = storyLine.StoryLine_Ending2;
            PrintStoryScript(storyLines); // 2회차 엔딩 스토리
        }
    }

    /// <summary>
    /// 프리뷰 이후 스토리를 출력해줄 함수(MainHall_PassAfterTutorial에서 실행)
    /// </summary>
    public void StartAfterPreviewStory()
    {
        // 현재 엔딩 회차에 따라 프리뷰 이후에 보여줄 스토리 결정
        if (GameManager.instance.Episode_Round == 1)
        {
            storyLines = storyLine.StoryLine_MainHallDoorGuide1;

            // 해당 스토리에 사용할 캐릭터 이미지 활성화(1회차는 아메, 이나만 활성화)
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
        }
        else if (GameManager.instance.Episode_Round == 2)
        {
            storyLines = storyLine.StoryLine_MainHallDoorGuide2;

            // 해당 스토리에 사용할 캐릭터 이미지 활성화(2회차는 아메, 이나, 타코만 활성화)
            character_Image[0].gameObject.SetActive(true);
            character_Image[1].gameObject.SetActive(true);
            character_Image[7].gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("엔딩 회차 설정 오류(복도 프리뷰 이후 로그)");
        }

        maxStoryLine = storyLines.Count - 1;    // 스토리 최대 길이 초기화

        player.GetComponent<PlayerCtrl>().keystrokes = true;    // 스토리가 진행되는 동안 키보드 입력을 막음

        Time.timeScale = 0;                         // 일시정지
        Cursor.lockState = CursorLockMode.None;     // 마우스 커서 표시

        storyCanvas.enabled = true;    // 스토리 보여줄 캔버스 활성화
        PlayerUI.enabled = false;       // 플레이어의 canvas를 비활성화

        // player.GetComponent<PlayerCtrl>().keystrokes = false;   // 플레이어의 키입력이 가능하게 변경

        PrintStoryScript(storyLines); // 다음 대사 출력을 위한 함수 호출
    }

    // 스토리 대사를 출력할 함수(미리 작성해둔 스크립트의 배열을 이용해 출력)
    private void PrintStoryScript(List<StoryLineClass> _storyLine)
    {
        // 현재 말하는 캐릭터의 이미지를 활성화 시켜서 보여줄 조건문
        switch (_storyLine[currentStoryLine].StoryCharacter)
        {
            case StoryLineClass.StoryCharacter_Enum.AME:
                {
                    // 현재 말하는 대상이 아메인 경우
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.AME);
                    storyLineCharacter_Text.text = string.Format("<color=#f2bd36>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // 지정된 스토리 캐릭터를 출력
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.INA:
                {
                    // 현재 말하는 대상이 이나인 경우
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.INA);
                    storyLineCharacter_Text.text = string.Format("<color=#5E169D>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // 지정된 스토리 캐릭터를 출력
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.IRYS:
                {
                    // 현재 말하는 대상이 아이리스인 경우
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.IRYS);
                    storyLineCharacter_Text.text = string.Format("<color=#991150>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // 지정된 스토리 캐릭터를 출력
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.BAE:
                {
                    // 현재 말하는 대상이 베이인 경우
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.BAE);
                    storyLineCharacter_Text.text = string.Format("<color=#D72517>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // 지정된 스토리 캐릭터를 출력
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.FAUNA:
                {
                    // 현재 말하는 대상이 파우나인 경우
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.FAUNA);
                    storyLineCharacter_Text.text = string.Format("<color=#33ca66>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // 지정된 스토리 캐릭터를 출력
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.KRONII:
                {
                    // 현재 말하는 대상이 크로니인 경우
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.KRONII);
                    storyLineCharacter_Text.text = string.Format("<color=#1d1797>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // 지정된 스토리 캐릭터를 출력
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.MUMEI:
                {
                    // 현재 말하는 대상이 무메이인 경우
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.MUMEI);
                    storyLineCharacter_Text.text = string.Format("<color=#c29371>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // 지정된 스토리 캐릭터를 출력
                    break;
                }
            case StoryLineClass.StoryCharacter_Enum.TAKOS:
                {
                    // 현재 말하는 대상이 무메이인 경우
                    ActiveTalkingCharacterImage((int)StoryLineClass.StoryCharacter_Enum.TAKOS);
                    storyLineCharacter_Text.text = string.Format("<color=black>" + _storyLine[currentStoryLine].StoryCharacter.ToString() + "</color>");   // 지정된 스토리 캐릭터를 출력
                    break;
                }
            default:
                {
                    Debug.Log("잘못된 캐릭터 이미지 활성화 시도");
                    break;
                }
        }

        storyLine_Text.text = _storyLine[currentStoryLine].StoryScript;     // 지정된 스토리 라인을 출력
    }

    // 현재 말하는 캐릭터의 이미지를 활성화 시켜주기 위한 함수
    private void ActiveTalkingCharacterImage(int _characterNum)
    {
        // 만약 이전에 말하던 대상이 없는 경우(처음 스토리가 출력되고 있는 경우)
        if (lastTalkStoryCharacter == (int)StoryLineClass.StoryCharacter_Enum.NONE)
        {
            for (int i = 0; i < character_Image.Count; i++)
            {
                // 지금 말하는 대상이 아닌 캐릭터를 전부 비활성화
                if (i != _characterNum)
                {
                    character_Image[i].gameObject.SetActive(false);
                }
                else
                {
                    // 현재 말하고 있는 캐릭터 이미지를 활성화 색상으로 변경
                    character_Image[i].gameObject.SetActive(true);
                    character_Image[i].color = activeColor;
                }
            }

            lastTalkStoryCharacter = _characterNum; // 현재 대사를 말하고 있는 캐릭터를 마지막으로 말한 캐릭터로 설정
        }
        else // 만약 이전에 말하던 대상이 있는 경우
        {
            // 만약 이미 대사를 말하던 캐릭터와 현재 캐릭터가 같을 경우 실행하지 않음
            if (_characterNum != lastTalkStoryCharacter)
            {
                for (int i = 0; i < character_Image.Count; i++)
                {
                    // 1. 지금 말하는 대상인가
                    // 2. 현재 이미지가 활성화 되어 있는가
                    // 3. 현재 이미지가 이전에 말한 대상인가
                    if ((character_Image[i].gameObject.activeSelf)) // 해당이미지가 활성화 되어 있는가
                    {
                        if ((i != _characterNum))   // 현재 말하는 대상이 아닌가
                        {
                            if ((i == lastTalkStoryCharacter))  // 마지막으로 말했던 대상인가
                            {
                                // 활성화 해둔채로 마지막으로 말하고 있던 캐릭터 이미지를 회색톤으로 변경
                                character_Image[i].color = nonActiveColor;

                                //// 아메 외의 대상끼리 왔다갔다 하는 경우 실행
                                //if ((i != (int)StoryLineClass.StoryCharacter_Enum.AME) && (lastTalkStoryCharacter == (int)StoryLineClass.StoryCharacter_Enum.AME))  // 아메가 아니거나, 마지막으로 말하던 대상이 아메인 경우 실행
                                //{
                                //    character_Image[i].color = activeColor;
                                //    character_Image[i].gameObject.SetActive(false);
                                //}
                            }
                            else  // 마지막으로 말했던 대상이 아닌가
                            {
                                if (i != (int)StoryLineClass.StoryCharacter_Enum.AME)   // 마지막으로 말했던 대상이 아메가 아닌 경우
                                {
                                    // 색을 원래대로 돌리고 비활성화 시킴
                                    character_Image[i].color = activeColor;
                                    character_Image[i].gameObject.SetActive(false);
                                }
                                else   // 비활성화 대상이 아메인 경우
                                {
                                    // 활성화 해둔채로 마지막으로 말하고 있던 캐릭터 이미지를 회색톤으로 변경
                                    character_Image[i].color = nonActiveColor;
                                }
                            }
                        }
                        else
                        {
                            // 현재 말하는 대상이니 활성화
                            character_Image[i].color = activeColor;
                        }
                    }
                    else    // 해당 이미지가 비활성화 되어 있는가
                    {
                        if (i == _characterNum) // 현재 말하는 대상인가
                        {
                            // 현재 말하고 있는 캐릭터 이미지 활성화
                            character_Image[i].gameObject.SetActive(true);
                            character_Image[i].color = activeColor;
                        }
                    }
                }

                lastTalkStoryCharacter = _characterNum; // 현재 대사를 말하고 있는 캐릭터를 마지막으로 말한 캐릭터로 설정
            }
        }
    }

    // 스토리 종료 함수
    private void EndStory()
    {
        currentStoryLine = 0;   // 스토리가 끝났으므로 현재 출력중인 스토리 라인을 0으로 초기화
        // player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // 지정된 위치로 이동(튜토리얼 방)

        lastTalkStoryCharacter = (int)StoryLineClass.StoryCharacter_Enum.NONE;  // 마지막으로 말한 캐릭터도 없다고 초기화

        // 스토리가 끝났으므로 스토리 캔버스 비활성화 및 UI 캔버스 활성화
        PlayerUI.enabled = true;       // 스토리가 끝났으므로 플레이어의 캔버스 활성화
        storyCanvas.enabled = false;   // 스토리 보여줄 캔버스 비활성화

        if (!isEndingStroryStart)  // 출력되던 스토리가 엔딩이 아닌 경우
        {
            // 1회차인 경우 메인 스토리 진행후 튜토리얼인지 확인해야됨
            if (GameManager.instance.Episode_Round == 1) // 1회차 시작 스토리였을 경우
            {
                storyCanvas.enabled = false;    // 스토리 캔버스 비활성화
                player.GetComponent<PlayerCtrl>().keystrokes = false;    // 키보드 입력을 풀어줌
                Cursor.lockState = CursorLockMode.Locked;     // 마우스 커서 잠금

                // isEndingStroryStart = true;    // 엔딩을 출력하기 위해 상태 변경
                isStoryEnd = true;             // 스토리가 종료되었으므로 true로 변경

                if (isWaitingTutorial) // 튜토리얼 진행을 대기하고 있을 경우
                {
                    // 헤일로 획득까지 대기하고 이후 스토리 출력
                    // 이 스토리 출력후 엔딩 출력 가능 상태로 변경
                    StartCoroutine(InventoryTutorialGuide());   // 튜토리얼 함수 실행
                    StartCoroutine(WaitGetHalo());  // 헤일로 획득 대기 코루틴 실행
                    isWaitingTutorial = false;
                }
                else // 튜토리얼이 필요없거나 진행 완료한 경우
                {
                    // Debug.Log("튜토리얼 종료");   // 로그 출력
                }

                // 복도 프리뷰가 끝난 이후 스토리가 출력되었던 경우
                // if ((storyLines == storyLine.StoryLine_MainHallDoorGuide1))
                if (Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_MainHallDoorGuide1))
                {
                    isEndingStroryStart = true;    // 엔딩을 출력하기 위해 상태 변경
                }
            }
            else if (GameManager.instance.Episode_Round == 2)        // 2회차인 경우 그냥 종료
            {
                player.GetComponentInChildren<Camera>().enabled = true;     // 플레이어의 메인카메라를 켬
                tutorialCam.GetComponentInChildren<Camera>().enabled = false;// 튜토리얼용 카메라를 끔

                player.GetComponent<PlayerCtrl>().keystrokes = false;    // 키보드 입력을 풀어줌
                Cursor.lockState = CursorLockMode.Locked;     // 마우스 커서 잠금

                // isEndingStroryStart = true;    // 엔딩을 출력하기 위해 상태 변경
                isStoryEnd = true;             // 스토리가 종료되었으므로 true로 변경

                if (isWaitingTutorial) // 튜토리얼 진행을 대기하고 있을 경우
                {
                    // 헤일로 획득까지 대기하고 이후 스토리 출력
                    StartCoroutine(WaitGetHalo());  // 헤일로 획득 대기 코루틴 실행
                    isWaitingTutorial = false;
                }
                else // 튜토리얼이 필요없거나 진행 완료한 경우
                {
                    Debug.Log("튜토리얼 종료");   // 로그 출력
                }

                // 복도 프리뷰가 끝난 이후 스토리가 출력되었던 경우
                // if ((storyLines == storyLine.StoryLine_MainHallDoorGuide2))
                if (Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_MainHallDoorGuide2))
                {
                    isEndingStroryStart = true;    // 엔딩을 출력하기 위해 상태 변경
                }

                // player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // 지정된 위치로 이동(튜토리얼 방)

                // StartCoroutine(WaitGetHalo());  // 헤일로 획득 대기 코루틴 실행
            }
            else
            {

            }
        }
        else            // 출력되던 스토리가 엔딩인 경우
        {
            if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1) // 1회차 엔딩이였을 경우
            {
                // mainHall_ObjectManager.CheckEndingEpisode_Num = 2; // 엔딩 회차를 2로 변경시킴

                PlayEnding1();  // 엔딩 연출 함수 호출

                GameManager.instance.Episode_Round = 2; // 엔딩 회차를 2로 변경시킴
                // GameManager.instance.Episode_Round = 1; // 엔딩 회차를 변경 방지용 라인
            }
            else        // 2회차 엔딩이였을 경우
            {
                // mainHall_ObjectManager.CheckEndingEpisode_Num = 1; // 엔딩 회차를 1로 변경시킴
                GameManager.instance.Episode_Round = 1; // 엔딩 회차를 1로 변경시킴
            }

            isEndingStroryStart = false;    // 다시 시작 스토리를 출력할 수 있게 false로 변경(이 변수 값으로 나중에 GameManager의 회차정보에 반영해줘야 됨)
            isStoryEnd = false;             // 엔딩을 봤으므로 다시 스토리를 재생할 수 있도록 false로 변경

            GameManager.instance.NextEpisode();

            // 다시 인트로 씬으로 돌아감
            LoadingSceneManager.LoadScene("01. IntroScene");
        }

        // 엔딩 스토리를 볼 차례인지 확인해서 설정
        if (isEndingStroryStart)
        {
            GameManager.instance.IsEndingStroryStart = 1;
        }
        else
        {
            GameManager.instance.IsEndingStroryStart = 0;
        }

        interaction.run_Gimic = isEndingStroryStart;     // 스토리 진행 현황 업데이트

        // 현재 진행중이던 스토리를 다 봤는지 아닌지 확인해서 설정
        if (isStoryEnd)
        {
            interaction.run_Gimic = true;
        }
        else
        {
            interaction.run_Gimic = false;
        }

        mainHall_ObjectManager.ChangeSceneData_To_GameManager();    // 저장함수 호출

        player.GetComponent<PlayerCtrl>().keystrokes = false;    // 다시 키보드 입력이 가능하게 변경

        Time.timeScale = 1;     // 일시 정지 해제
        Cursor.lockState = CursorLockMode.Locked;     // 마우스 커서를 안보이게 막음
    }
    #endregion

    #region 버튼에 할당할 함수 모음(스토리 진행 및 스킵)
    // 버튼을 누르면 다음 스토리를 출력하기 위한 함수
    public void NextStory()
    {
        // Debug.Log("다음버튼 클릭 : " + storyLines + ", " + currentStoryLine);

        if (currentStoryLine == maxStoryLine)   // 만약 현재 스토리 라인이 최대 스토리 라인과 같을 경우 스토리가 끝났으므로 캔버스 비활성화
        {
            // isEndingStroryStart = true; // 시작 스토리가 종료되었다고 상태 변경
            // interaction.run_Gimic = isEndingStroryStart;     // 스토리 진행 현황 업데이트

            // 1회차 시작 스토리를 출력중이던 경우
            if (storyLines.SequenceEqual(storyLine.StoryLine_Ending1))
            {
                StartCoroutine(PlayEnding1());  // 엔딩 연출 진행
                return;
            }
            // 2회차 시작 스토리를 출력중이던 경우
            else if (storyLines.SequenceEqual(storyLine.StoryLine_Ending2))
            {
                StartCoroutine(PlayEnding2());  // 엔딩 연출 진행
                return;
            }
            else
            {
                EndStory();    // 스토리 종료 함수 호출

                return; // 현재 함수 종료
            }
        }

        if (!isEndingStroryStart)   // 엔딩 스토리가 아닌 경우
        {
            //bool test;
            //test = Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start2);

            if (currentStoryLine == 3) // 시작 스토리라인 중 4번째라인인 경우 암전후 시작 방으로 플레이어 위치 이동
            {
                // Time.timeScale = 1;
                if (storyLines.SequenceEqual(storyLine.StoryLine_Start1) || Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start2))
                {
                    fadeInOut_Image.enabled = true;    // 연출을 위해 보이도록 활성화
                    fadeInOut_Image.color = new Color(14f, 14f, 14f, 0f);   // 투명도를 0으로 설정

                    StartCoroutine(WaitFadeInOut());    // 화면 암전 처리 및 이동 시킬 함수
                }
            }
            else if ((currentStoryLine == 10) && storyLines.SequenceEqual(storyLine.StoryLine_Start1)) // 1회차 시작 스토리 라인이 10번째 라인인 경우 카메라를 회전시켜 방을 둘러봄
            {
                StartCoroutine(Tutorial_LookAroundStartRoom()); // 1회차 스토리중 튜토리얼을 위해 방을 한바퀴 둘러볼 함수 호출
            }
            else if ((currentStoryLine == 12) && Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start2)) // 2회차 시작 스토리 라인이 12번째 라인인 경우 카메라를 회전시켜 방을 둘러봄
            // else if ((currentStoryLine == 12) && test) // 2회차 시작 스토리 라인이 12번째 라인인 경우 카메라를 회전시켜 방을 둘러봄
            {
                StartCoroutine(LookAroundStartRoomBeforeGetHalo()); // 2회차 스토리중 헤일로 획득을 위해 조각상을 쳐다볼 함수 호출
            }
        }
        else // 엔딩이 나오고 있던 중인 경우
        {
            if (GameManager.instance.Episode_Round == 1)    // 1회차 엔딩인 경우
            {
                if (Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Ending1))  // 1회차 엔딩인지 한번더 확인
                {
                    if (currentStoryLine == 18) // 엔딩 스토리라인 중 19번째라인인 경우 폰트 변경
                    {
                        storyLine_Text.font = inanis_Font;  // 폰트를 이나 폰트로 변경
                        storyLine_Text.fontStyle = FontStyle.Italic;    // 폰트에 기울임을 줌
                    }
                    else if (currentStoryLine == 19) // 엔딩 스토리라인 중 20번째라인인 경우 폰트 변경
                    {
                        storyLine_Text.font = storyDefault_Font;  // 폰트를 기본 폰트로 변경
                        storyLine_Text.fontStyle = FontStyle.Normal;    // 폰트스타일을 노말로 변경
                    }
                    else if (currentStoryLine == 22)    //  문 닫히는 소리 재생 및 이나 사진 비활성화
                    {
                        storyAudioSource.clip = doorClose_SFX;  // 문 닫히는 소리로 설정
                        storyAudioSource.Play();               // 문 닫히는 소리 재생

                        lastTalkStoryCharacter = (int)StoryLineClass.StoryCharacter_Enum.NONE;  // 이전에 말하던 대상이 없다고 상태 변경
                    }
                }
            }
            else if (GameManager.instance.Episode_Round == 2)    // 2회차 엔딩인 경우
            {

            }
            else
            {
                // 잘못된 경우
            }
        }

        currentStoryLine++; // 현재 출력중인 스토리라인을 다음으로 변경
        PrintStoryScript(storyLines); // 다음 대사 출력을 위한 함수 호출
    }

    // 버튼을 누르면 스토리를 스킵하는 함수
    public void SkipStory()
    {
        player.GetComponentInChildren<Camera>().enabled = true;     // 플레이어의 메인카메라를 켬
        tutorialCam.GetComponentInChildren<Camera>().enabled = false;// 튜토리얼용 카메라를 끔

        // 시작 스토리였던 경우 플레이어를 이동시킴
        if (Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start1) || Enumerable.SequenceEqual(storyLines, storyLine.StoryLine_Start2))
        {
            player.GetComponent<Transform>().position = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // 지정된 위치로 이동
            player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 180f, 0f);     // 튜토리얼 가능 벽을 쳐다보게끔 방향 지정

            isWaitingTutorial = true;   // 튜토리얼 대기 상태로 변경
        }

        EndStory();    // 스토리 종료 함수 호출
    }
    #endregion

    #region 튜토리얼 중 카메라를 움직일 함수 모음
    // 1회차 스토리중 튜토리얼을 위해 방을 한바퀴 둘러볼 함수
    private IEnumerator Tutorial_LookAroundStartRoom()
    {
        Time.timeScale = 1;     // 일시 정지 해제
        storyCanvas.enabled = false;    // 스토리 캔버스 일시적 비활성화
        Cursor.lockState = CursorLockMode.Locked;     // 마우스 커서를 안보이게 막음

        player.GetComponentInChildren<Camera>().enabled = false;    // 플레이어의 메인카메라를 잠시 끔
        tutorialCam.GetComponentInChildren<Camera>().enabled = true;// 튜토리얼용 카메라를 켜줌
        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = true;
        player.gameObject.SetActive(false);
        // 카메라 회전 애니메이션 실행
        // tutorialCam_Anim.Play("Start_TutorialCamRotate");
        tutorialCam_Anim.SetBool("isCamRotate", true);

        yield return new WaitForSeconds(10f);    // 애니메이션이 종료될 때까지 대기

        Time.timeScale = 0;     // 일시 정지
        storyCanvas.enabled = true;    // 스토리 캔버스 활성화
        Cursor.lockState = CursorLockMode.None;     // 마우스 커서 활성화

        player.gameObject.SetActive(true);
        player.GetComponentInChildren<Camera>().enabled = true;    // 플레이어의 메인카메라를 켬
        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = false;
        tutorialCam.GetComponentInChildren<Camera>().enabled = false;// 튜토리얼용 카메라를 끔

        isWaitingTutorial = true;
    }

    // 2회차 스토리중 헤일로를 보기 위해 방을 한바퀴 둘러볼 함수
    private IEnumerator LookAroundStartRoomBeforeGetHalo()
    {
        Time.timeScale = 1;     // 일시 정지 해제
        storyCanvas.enabled = false;    // 스토리 캔버스 일시적 비활성화
        Cursor.lockState = CursorLockMode.Locked;     // 마우스 커서를 안보이게 막음

        player.GetComponentInChildren<Camera>().enabled = false;    // 플레이어의 메인카메라를 잠시 끔
        tutorialCam.GetComponentInChildren<Camera>().enabled = true;// 튜토리얼용 카메라를 켜줌
        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = true;
        player.gameObject.SetActive(false);

        // 카메라 회전 애니메이션 실행
        tutorialCam_Anim.Play("Start_BeforeGetHalo");   // 안되면 아래 방식을 사용할 것
        // tutorialCam_Anim.SetBool("isCamRotate", true);

        yield return new WaitForSeconds(4.5f);    // 애니메이션이 종료될 때까지 대기

        Time.timeScale = 0;     // 일시 정지
        storyCanvas.enabled = true;    // 스토리 캔버스 활성화
        Cursor.lockState = CursorLockMode.None;     // 마우스 커서 활성화

        player.gameObject.SetActive(true);
        player.GetComponentInChildren<Camera>().enabled = true;    // 플레이어의 메인카메라를 켬
        tutorialCam.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = false;
        tutorialCam.GetComponentInChildren<Camera>().enabled = false;// 튜토리얼용 카메라를 끔

        isWaitingTutorial = true;
    }

    // 헤일로 획득 전까지 대기하다가 획득하면 마무리 스토리를 출력할 코루틴 함수
    private IEnumerator WaitGetHalo()
    {
        // 헤일로를 획득할때까지 대기
        yield return new WaitUntil(() => ItemManager._instance.inventorySlots[10].GetComponent<Item10IrysHalo>().isGetItem);

        mainHall_StartRoomDoor.GetComponent<Interaction_Gimics>().enabled = true; // 문의 interaction gimic 활성화
        mainHall_StartRoomDoor.GetComponent<BoxCollider>().enabled = true;
        mainHall_StartRoomDoor.GetComponent<MeshCollider>().enabled = true;
        startRoomDoorWall.GetComponent<BoxCollider>().enabled = false;

        // 아이리스 헤일로 비활성화
        irysHalo_OnHead.SetActive(false);
        irysHalo_ending1.SetActive(false);
        irysHalo_ending2.SetActive(false);

        Time.timeScale = 0;     // 일시 정지
        player.GetComponent<PlayerCtrl>().keystrokes = true;    // 키보드 입력막음
        PlayerUI.enabled = false;      // 플레이어의 캔버스 비활성화
        storyCanvas.enabled = true;    // 스토리 캔버스 활성화
        Cursor.lockState = CursorLockMode.None;     // 마우스 커서 활성화

        if (GameManager.instance.Episode_Round == 1)    // 1회차인 경우
        {
            // 스토리 재생
            storyLines = storyLine.AfterTutorial_Scripts;   // 스토리 변경
            maxStoryLine = storyLines.Count - 1;                // 스토리 최대 라인 수 변경
            PrintStoryScript(storyLines);
        }
        else if (GameManager.instance.Episode_Round == 2)   // 2회차인 경우
        {
            // 스토리 재생
            storyLines = storyLine.AfterGetHaloInEnding2_Scripts;   // 스토리 변경
            maxStoryLine = storyLines.Count - 1;                // 스토리 최대 라인 수 변경
            PrintStoryScript(storyLines);
        }
        else    // 잘못된 경우
        {
            Debug.Log("잘못된 엔딩 설정됨.(헤일로 획득 대기 코루틴)");    // 로그 출력
            // 1회차의 스토리 재생
            storyLines = storyLine.AfterTutorial_Scripts;   // 스토리 변경
            maxStoryLine = storyLines.Count - 1;                // 스토리 최대 라인 수 변경
            PrintStoryScript(storyLines);
        }
    }

    // 인벤토리 튜토리얼을 진행하기 위한 코루틴 함수
    private IEnumerator InventoryTutorialGuide()
    {
        // UI의 I버튼에 하이라이트 처리
        // i 버튼 누를때까지 대기
        // 누르면 튜토리얼 이미지 출력
        PlayerUI.GetComponent<InventoryTutorialCtrl>().arrowImage.color = new Color(255f, 255f, 255f, 255f); // 화살표 이미지를 활성화해줌;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I));   // i 버튼을 플레이어가 누를때까지 대기

        PlayerUI.GetComponent<InventoryTutorialCtrl>().arrowImage.color = new Color(255f, 255f, 255f, 0f); // 화살표 이미지를 비활성화해줌

        tutorial_UI.ActiveTutorialImage();  // 튜토리얼 UI 활성화
    }
    #endregion

    #region 연출하는 함수 모음
    // fade in out 연출하는 동안 대기시키기 위한 함수
    private IEnumerator WaitFadeInOut()
    {
        // storyCanvas.enabled = false;    // 스토리 캔바스를 잠시 비활성화

        Time.timeScale = 1;

        production_Anim.Play("FadeInOut_Story");

        yield return waitForSecond;    // 1초간 대기

        // 플레이어 위치 이동
        player.GetComponent<Transform>().localPosition = mainHall_ObjectManager.SponPoints[0].GetComponent<Transform>().position;    // 지정된 위치로 이동
        player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 180f, 0f);     // 튜토리얼 가능 벽을 쳐다보게끔 방향 지정

        yield return waitForSecond;    // 1초간 대기

        Time.timeScale = 0;

        fadeInOut_Image.enabled = false;    // 진행을 위해 비활성화
    }

    // 1회차 엔딩 연출을 위한 함수
    private IEnumerator PlayEnding1()
    {
        storyTimeTravelImage.SetActive(true);

        Time.timeScale = 1;

        production_Anim.Play("TimeTravel"); // 시간 여행 애니메이션 재생

        storyAudioSource.clip = timeTravel_SFX; // 시간 여행 소리로 오디오 클립 변경
        storyAudioSource.Play();    // 시간 여행 소리 재생

        yield return new WaitForSeconds(2f);    // 2초간 대기

        EndStory();    // 스토리 종료 함수 호출
    }

    // 2회차 엔딩 연출을 위한 함수
    private IEnumerator PlayEnding2()
    {
        //storyTimeTravelImage.SetActive(true);

        //Time.timeScale = 1;

        //production_Anim.Play("TimeTravel");

        //storyAudioSource.clip = timeTravel_SFX; // 시간 여행 소리로 오디오 클립 변경
        //storyAudioSource.Play();    // 시간 여행 소리 재생

        yield return new WaitForSeconds(2f);    // 2초간 대기

        EndStory();    // 스토리 종료 함수 호출
    }
    #endregion
}