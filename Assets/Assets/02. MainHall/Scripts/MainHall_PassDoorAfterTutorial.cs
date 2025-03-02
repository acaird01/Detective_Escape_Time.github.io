using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MainHall_PassDoorAfterTutorial : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    private GameObject player;                                 // 플레이어 오브젝트
    private MainHall_ObjectManager mainHall_ObjectManager;     // 복도씬의 오브젝트 매니저
    private TextController textController;                     // 상호작용 대사를 출력하는 스크립트

    private Canvas PlayerUI;                        // 플레이어가 지니고 있는 인벤토리 등의 UI Canvas
    
    //[Header("시네머신 카메라가 따라다닐 빈오브젝트")]
    //[SerializeField]
    //private GameObject mainHallViewCam_position;     // 1회차에서 처음으로 복도씬으로 넘어왔을 때 전체를 보여줄 카메라를 움직일 오브젝트
    [Header("시네머신 카메라")]
    [SerializeField]
    private GameObject mainHallCinemachineCamera;     // 1회차에서 처음으로 복도씬으로 넘어왔을 때 전체를 보여줄 카메라
    [Header("복도씬 프리뷰 시네머신 카메라")]
    [SerializeField]
    private PlayableDirector mainHallDirector;
    [Header("복도씬 프리뷰 시네머신 카메라의 Cinemachinebrain")]
    [SerializeField]
    private Camera mainHallDirector_Brain;
    [Header("시작방 스폰포인트")]
    [SerializeField]
    private Transform startRoom_SponPoint;

    private MainHall_StartStoryAndTutorial mainHall_StartStoryAndTutorial;  // 스토리를 출력하는 스크립트
    private MainHall_StoryHelpAnim mainHall_StoryHelp;      // 복도씬 가이드를 초기화 해줄 스크립트
    private MainHall_TakoMoveCtrl mainHall_TakoMoveCtrl;    // 복도씬에서 타코들을 움직여줄 스크립트
    private Canvas doorInfo_Canvas;                             // 문의 정보를 띄워줄 Canvas
    private Text doorInfo_Text;                                 // 문의 정보를 출력할 Text
    #endregion

    #region 스토리 출력에 사용할 변수
    private bool isPassDoor;                     // 스토리 출력 완료 여부 확인용 변수 (true : 스토리 재생완료 / false : 스토리 재생 미완)
    private string beforePreviewStory;           // 프리뷰를 보여주기전에 띄워줄 스토리
    private WaitForSeconds waitForSecond = new WaitForSeconds(1f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // 플레이어를 찾아와서 할당
        PlayerUI = player.GetComponentInChildren<Canvas>();                 // player의 자식으로 있는 canvas를 찾아와서 할당
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();    // MainHall_ObjectManager를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();           // 플레이어의 자식이 가진 textcontroller를 찾아와서 할당
    }

    /// <summary>
    /// 복도 프리뷰 스크립트의 초기화 함수
    /// </summary>
    public void Init(bool _setting)
    {
        mainHall_StartStoryAndTutorial = GameObject.FindAnyObjectByType<MainHall_StartStoryAndTutorial>();  // 스토리를 출력하는 스크립트를 찾아와서 할당
        mainHall_StoryHelp = GameObject.FindAnyObjectByType<MainHall_StoryHelpAnim>();      // 복도씬 가이드를 관리하는 스크립트를 찾아와서 할당
        mainHall_TakoMoveCtrl = GameObject.FindAnyObjectByType<MainHall_TakoMoveCtrl>();    // 복도씬에서 타코들을 움직여줄 스크립트를 찾아와서 할당
        
        doorInfo_Canvas = GameObject.FindAnyObjectByType<MainHall_DoorGuideCanvas>().GetComponentInChildren<Canvas>();    // 문의 정보를 띄워줄 Canvas를 찾아와서 할당
        doorInfo_Text = doorInfo_Canvas.GetComponentInChildren<Text>();     // 문으로 넘어갈 씬의 대사를 출력할 텍스트

        isPassDoor = _setting;  // 

        if (GameManager.instance.Episode_Round == 1)        // 현재 회차가 1일 경우에 소환할 타코이름으로 초기화
        {
            // isStoryEnd = _setting;
        }
        else if (GameManager.instance.Episode_Round == 2)   // 현재 회차가 2일 경우에 소환할 타코이름으로 초기화
        {

        }
        else
        {
            Debug.Log("엔딩 설정이 잘못됨");
            // summon_TakoName = ending1_TakoName; // 임시로 엔딩1의 타코로 초기화
        }

        // 일단 카메라를 꺼둠
        mainHallDirector_Brain.enabled = false;
        mainHallCinemachineCamera.SetActive(false);
        doorInfo_Canvas.enabled = false; // 캔버스 비활성화

        Setting_SceneStart();   // 최초 세팅해줄 함수 호출
    }

    // 최초 세팅해줄 함수
    private void Setting_SceneStart()
    {
        if (isPassDoor)
        {
            // 이미 지나가서 방 전체를 봤을 경우 재실행을 못하도록 상태 변경
            // this.GetComponent<BoxCollider>().enabled = false;
            this.gameObject.SetActive(false);   // 이미 지나간 적이 있으므로 비활성화 시킴
        }
        else
        {
            this.GetComponent<BoxCollider>().enabled = true;
        }
    }

    // 플레이어가 복도로 나오는 문을 통과했을때 카메라를 움직여줄 함수
    private void OnTriggerExit(Collider other)
    {
        PlayerCtrl isPlayer = other.GetComponent<PlayerCtrl>();

        // 플레이어가 들어왔고 헤일로를 획득 했을 경우 실행
        if ((isPlayer != null) && (ItemManager._instance.inventorySlots[10].GetComponent<IItem>().isGetItem))
        {
            this.GetComponent<BoxCollider>().enabled = false;   // 콜라이더를 꺼줌
            StartCoroutine(playPreview());

        }
        else
        {
            // 아니라면 나오면 안되는 상황에 나와진 것이므로 방으로 되돌려보냄
            player.GetComponent<Transform>().localPosition = startRoom_SponPoint.localPosition;
        }
    }

    // 복도 프리뷰 출력 코루틴 함수
    private IEnumerator playPreview()
    {
        // 플레이어 키보드 입력을 막음
        player.GetComponent<PlayerCtrl>().keystrokes = true;
        PlayerUI.enabled = false;   // 플레이어 캔버스 비활성화

        // 시네머신 출력전에 아메 대사 한줄 출력
        if (GameManager.instance.Episode_Round == 1)
        {
            beforePreviewStory = "응? 저건 뭐지?";
        }
        else if (GameManager.instance.Episode_Round == 2)
        {
            beforePreviewStory = "앗, 저 녀석들은?!";
        }
        else
        {
            Debug.Log("잘못된 엔딩이 설정됨.(복도 프리뷰 이후 스토리 출력되는 부분)");
        }

        doorInfo_Canvas.enabled = true; // 캔버스 활성화
        doorInfo_Text.text = beforePreviewStory;
        // StartCoroutine(textController.SendText(beforePreviewStory));

        // 플레이어 키보드 입력을 막음
        player.GetComponent<PlayerCtrl>().keystrokes = true;

        yield return waitForSecond; // 1초간 대기

        doorInfo_Canvas.enabled = false; // 캔버스 비활성화

        // 대사가 남아있으면 꺼줄 함수호출
        textController.SetActiveFalseText();
        // 플레이어 키보드 입력을 막음
        player.GetComponent<PlayerCtrl>().keystrokes = true;

        // 잠시 대기 후 시네머신 시작

        player.GetComponentInChildren<Camera>().enabled = false;  // 플레이어의 카메라를 꺼줌
        mainHallDirector_Brain.enabled = true;
        mainHallCinemachineCamera.SetActive(true);

        mainHallDirector.Play();    // 카메라 워킹 진행
        mainHall_TakoMoveCtrl.PlayTakoStoryMove();  // 타코 움직임 함수 호출

        yield return new WaitForSeconds(10.2f);

        mainHallCinemachineCamera.SetActive(false);

        mainHallDirector_Brain.enabled = false;
        player.GetComponentInChildren<Camera>().enabled = true;  // 플레이어의 카메라를 켜줌
        player.GetComponentInChildren<Camera>().GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);  // 정면을 보고 있게끔 카메라 조정
        mainHall_StoryHelp.mainHallGuideCamMoveEnd = true;  // 프리뷰가 끝났다고 상태 변경

        PlayerUI.enabled = true;   // 플레이어 캔버스 활성화
        // MainHall_StartStoryAndTutorial의 복도씬 프리뷰 스토리 진행 함수 호출
        mainHall_StartStoryAndTutorial.StartAfterPreviewStory();
    }
}
