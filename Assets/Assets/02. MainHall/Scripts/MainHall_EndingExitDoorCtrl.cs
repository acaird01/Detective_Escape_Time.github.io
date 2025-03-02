using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHall_EndingExitDoorCtrl : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    // 엔딩에서 문 열어줄 애니메이터 컴포넌트 변수
    private Animator EndingDoorAnimator_Left;   // 왼쪽문 Animator 컴포넌트
    private Animator EndingDoorAnimator_Right;  // 오른쪽문 Animator 컴포넌트

    private BoxCollider EndingStart_Collider;                   // 엔딩을 시작할지 확인하기 위한 collider
    private MainHall_ObjectManager mainHall_ObjectManager;      // 복도씬 오브젝트 매니저(현재 테스트용 정보 획득을 위함)
    private MainHall_StartStoryAndTutorial mainHall_Story;      // 복도씬에서 스토리를 출력하는 스크립트
    private Item18AmeClock ameClock_Info;                            // 아메 시계 아이템의 정보 스크립트 
    #endregion

    #region 엔딩을 위한 변수 모음

    #endregion

    // Start is called before the first frame update
    void Start()
    {   
        /*
         * 따로 문들을 적용시켜야 할 경우 사용
        // 자식으로 있는 왼쪽문과 오른쪽문의 이름을 이용해 찾아와 각 변수에 할당
        GameObject LeftDoor = GameObject.Find("DoorGate_Wooden_Left");
        GameObject RightDoor = GameObject.Find("DoorGate_Wooden_Right");

        // 찾아온 왼쪽문과 오른쪽문의 Animator 컴포넌트를 찾아와서 할당
        EndingDoorAnimator_Left = LeftDoor.GetComponent<Animator>();
        EndingDoorAnimator_Right = RightDoor.GetComponent<Animator>();
        */

        // 자식으로 있는 왼쪽문과 오른쪽문의 Animator 컴포넌트를 찾아와서 할당
        // 현재 왼쪽 오른쪽이 바뀌어도 상관없으므로 인덱스를 이용해 찾아옴
        EndingDoorAnimator_Left = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        EndingDoorAnimator_Right = this.gameObject.transform.GetChild(1).GetComponent<Animator>();

        mainHall_Story = GameObject.FindAnyObjectByType<MainHall_StartStoryAndTutorial>();  // 스토리 출력하는 스크립트를 하이어라키에서 찾아와서 할당(Story_Canvas에 붙어있음)
        ameClock_Info = GameObject.FindAnyObjectByType<Item18AmeClock>();                   // 아메시계 오브젝트를 찾아와서 할당
        EndingStart_Collider = gameObject.GetComponent<BoxCollider>();  // 해당 객체의 박스 콜라이더를 찾아와서 할당
        EndingStart_Collider.enabled = false;   // 엔딩 조건 만족하기 전 실행을 막기 위해 비활성화

        // 테스트 용
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>(); // 체스판 본체를 찾아와서 할당// MainHall_ObjectManager를 찾아와서 할당
    }

    // Update is called once per frame
    void Update()
    {
        // 만약 아메의 아이템을 획득 했을 경우
        if (ameClock_Info.isGetItem)
        {
            mainHall_Story.GetComponent<Interaction_Gimics>().run_Gimic = false;    // 엔딩 스토리 출력을 위해 미진행 상태로 변경
        }
    }

    /// <summary>
    /// 엔딩 조건을 만족했을때 출구를 열어줄 함수(체스판에서 엔딩조건 만족시 호출)
    /// </summary>
    public void DoorOpenForEnding()
    {
        // 2회차인 경우 천장에 우주 출력할 함수 호출

        // 엔딩 문 열리는 효과음 재생

        // 엔딩 문 열리는 애니메이션 실행
        EndingDoorAnimator_Left.Play("ExitLeft_DoorOpen");
        EndingDoorAnimator_Right.Play("ExitRight_DoorOpen");

        EndingStart_Collider.enabled = true;   // 엔딩 조건 만족되었으므로 엔딩 실행을 위해 활성화
    }

    // 엔딩 콜라이더 통과할때 엔딩 회차에 따라 엔딩 연출을 출력 할 함수(여기서 엔딩이랑 회차에 따라 세이브 데이터 초기화 해줘야됨)
    private void OnTriggerEnter(Collider other)
    {
        // 콜라이더를 통과하는 대상이 플레이어인 경우에 실행
        if (other.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            // if (GameManager.instance.Episode_Round == 1)    // 1회차
            if (mainHall_ObjectManager.CheckEndingEpisode_Num == 1)
            {
                Debug.Log("1회차 엔딩 출력"); // 로그 출력
                // 엔딩 (코루틴) 실행
                // 회차 정보 업데이트 실행

                mainHall_Story.StartEndingStory();  // 엔딩 스토리 출력 함수 호출
                
                Debug.Log("현재 엔딩 회차 : " + mainHall_ObjectManager.CheckEndingEpisode_Num);   // 테스트용 로그
            }
            // else if (GameManager.instance.Episode_Round == 2)    // 2회차
            else if (mainHall_ObjectManager.CheckEndingEpisode_Num == 2)
            {
                Debug.Log("2회차 엔딩 출력"); // 로그 출력
                // 엔딩 (코루틴) 실행
                SetSanaCelling();   // 천장에 우주 출력할 함수 호출

                mainHall_Story.StartEndingStory();  // 엔딩 스토리 출력 함수 호출

                Debug.Log("현재 엔딩 회차 : " + mainHall_ObjectManager.CheckEndingEpisode_Num);   // 테스트용 로그
            }
            else 
            {
                Debug.Log("잘못된 회차정보 확인됨."); // 로그 출력
            }
        }
    }

    // 2회차 엔딩일 경우 천장에 우주를 출력할 함수
    private void SetSanaCelling()
    {
        Debug.Log("사나나나"); // 로그 출력
    }
}
