using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHall_TakoMoveCtrl : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    private GameObject player;                              // 플레이어 오브젝트
    private MainHall_ObjectManager mainHall_ObjectManager;  // 복도씬의 오브젝트 매니저
    // private Interaction_Gimics interaction;                 // 상호작용하는 기믹인지 확인하기 위한 컴포넌트

    private GameObject takoMoving_Ending1;  // 1회차 타코들
    private GameObject takoMoving_Ending2;  // 2회차 타코들
    private Animator takoMoving_Animator;       // 1, 2회차에서 타코들을 움직여줄 Animator
    // private Animator takoMoving_Ending2_Animator;       // 2회차에서 타코들을 움직여줄 Animator
    #endregion

    #region 처음 시작할때 타코들의 움직임에 사용할 변수
    private bool isTakoMoveDone;                     // 타코 애니메이션 재생 여부 확인용 변수 (true : 스토리 재생완료 / false : 스토리 재생 미완)
    private WaitForSeconds waitForTenMillisecond = new WaitForSeconds(0.1f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // 플레이어를 찾아와서 할당
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();    // MainHall_ObjectManager를 찾아와서 할당
        // interaction = gameObject.GetComponent<Interaction_Gimics>();    // 상호작용을 위한 Interaction_Gimics 할당

        // Init();     // 초기화 함수 호출
    }

    /// <summary>
    /// 타코들을 움직여줄 애니메이션 재생 스크립트의 초기화 함수
    /// </summary>
    /// <param name="_setting"></param>
    public void Init(bool _setting)
    {
        takoMoving_Ending1 = GetComponentInChildren<TakoStoryMove_ending1>().gameObject;
        takoMoving_Ending2 = GetComponentInChildren<TakoStoryMove_ending2>().gameObject;
        takoMoving_Animator = this.GetComponent<Animator>();

        isTakoMoveDone = _setting;       // 기믹 실행 여부를 확인해서 저장

        Setting_SceneStart();   // 최초 세팅해줄 함수 호출
    }

    // 최초 세팅해줄 함수
    private void Setting_SceneStart()
    {
        if (isTakoMoveDone)
        {
            // 타코 애니메이션 재생이 끝났을 경우 비활성화
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
    /// 플레이어가 복도로 나오는 문을 통과해서 카메라가 움직일때 애니메이션을 실행할 함수
    /// </summary>
    /// <param name="other"></param>
    public void PlayTakoStoryMove()
    {
        if (GameManager.instance.Episode_Round == 1)        // 현재 회차가 1일 경우
        {
            // takoMoving_Ending1_Animator.Play("");
            takoMoving_Animator.Play("TakoMoveEnding1");
        }
        else if (GameManager.instance.Episode_Round == 2)   // 현재 회차가 2일 경우
        {
            // takoMoving_Ending2_Animator.Play("");
            takoMoving_Animator.Play("TakoMoveEnding2");
        }
        else
        {
            Debug.Log("엔딩 설정이 잘못됨 : " + GameManager.instance.Episode_Round);
        }
    }
}
