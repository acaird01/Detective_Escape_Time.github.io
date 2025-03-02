using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTutorialCtrl : MonoBehaviour
{
    #region 오브젝트 및 컨트롤러를 할당할 변수 모음
    private PlayerCtrl player_Ctrl;     // playerCtrl 스크립트
    [Header("플레이어의 canvas")]
    [SerializeField]
    private Canvas playerUI;            // 플레이어가 가진 canvas

    [Header("인벤토리 오브젝트")]
    public GameObject Inventory;     // Inventory GameObject
    [Header("튜토리얼 이미지가 모여있는 오브젝트")]
    public GameObject tutorial_Inventory;     // Tutorial_Inventory GameObject
    [Header("튜토리얼 안내 화살표 이미지")]
    public Image arrowImage;    // 인벤토리 가이드 이미지
    [Header("튜토리얼 인벤토리 이미지")]
    public Image tutorialInventory_Image;    // 인벤토리 가이드 이미지
    [Header("튜토리얼 기믹 아이템 이미지")]
    public Image turorialGimicItem_Image;    // 인벤토리 가이드 이미지
    [Header("튜토리얼 다음 버튼")]
    public Button nextButton;    // 인벤토리 가이드 이미지
    [Header("튜토리얼 끄기 버튼")]
    public Button exitButton;    // 인벤토리 가이드 이미지
    #endregion

    private void Start()
    {
        Init(); // 초기화 함수 실행
    }

    // 초기화 함수
    private void Init()
    {
        player_Ctrl = GameObject.FindAnyObjectByType<PlayerCtrl>();     // playerCtrl을 찾아와서 할당

        tutorial_Inventory.SetActive(false);
        arrowImage.color = new Color(255f, 255f, 255f, 0f); // 화살표 이미지를 비활성화해줌
    }

    private void Update()
    {
        // i 또는 esc 키를 눌렸을 경우
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {
            ExitTutorialImage();
        }
    }

    /// <summary>
    /// 튜토리얼 이미지를 틔워줄 버튼에 할당할 함수
    /// </summary>
    public void ActiveTutorialImage()
    {
        // 인벤토리가 null이 아닌 경우에 실행
        if (Inventory != null)
        {
            // 인벤토리가 비활성화 상태인 경우에 실행
            if (!Inventory.activeSelf)
            {
                Inventory.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Inventory가 null임"); // 인벤토리가 null이라는 에러 로그 출력
        }
        tutorial_Inventory.SetActive(true);  // i버튼이 눌려졌으므로 튜토리얼 이미지 활성화

        // 보여줄 이미지 및 버튼 세팅
        tutorialInventory_Image.gameObject.SetActive(true);
        turorialGimicItem_Image.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(false);

        player_Ctrl.keystrokes = true;    // 키보드 입력막음
    }

    /// <summary>
    /// 튜토리얼 이미지를 다음으로 넘겨줄 버튼에 할당할 함수
    /// </summary>
    public void NextTutorialImage()
    {
        // 보여줄 이미지 및 버튼 세팅
        tutorialInventory_Image.gameObject.SetActive(false);
        turorialGimicItem_Image.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);

        player_Ctrl.keystrokes = false;    // 키보드 입력을 풀어줌
    }

    /// <summary>
    /// 튜토리얼 이미지를 꺼줄 버튼에 할당할 함수
    /// </summary>
    public void ExitTutorialImage()
    {
        if (!player_Ctrl.keystrokes)
        {
            // 보여줄 이미지 및 버튼 세팅
            tutorialInventory_Image.gameObject.SetActive(true);
            turorialGimicItem_Image.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);

            tutorial_Inventory.SetActive(false);  // 튜토리얼 UI를 꺼줌

            // player_Ctrl.keystrokes = false;    // 키보드 입력을 풀어줌 
        }

        //// 복도 씬인 경우에만 실행
        //if (GameManager.instance.nowSceneName == "02. MainHallScene")
        //{
        //    // 플레이어의 켄버스가 활성화되어 있을때만 실행
        //    if (playerUI.isActiveAndEnabled)
        //    {
        //        // 보여줄 이미지 및 버튼 세팅
        //        tutorialInventory_Image.gameObject.SetActive(true);
        //        turorialGimicItem_Image.gameObject.SetActive(true);
        //        nextButton.gameObject.SetActive(true);
        //        exitButton.gameObject.SetActive(true);

        //        tutorial_Inventory.SetActive(false);  // 튜토리얼 UI를 꺼줌

        //        player_Ctrl.keystrokes = false;    // 키보드 입력을 풀어줌 
        //    }
        //}
        //else
        //{
        //    // 보여줄 이미지 및 버튼 세팅
        //    tutorialInventory_Image.gameObject.SetActive(true);
        //    turorialGimicItem_Image.gameObject.SetActive(true);
        //    nextButton.gameObject.SetActive(true);
        //    exitButton.gameObject.SetActive(true);

        //    tutorial_Inventory.SetActive(false);  // 튜토리얼 UI를 꺼줌

        //    player_Ctrl.keystrokes = false;    // 키보드 입력을 풀어줌 
        //}
    }
}
