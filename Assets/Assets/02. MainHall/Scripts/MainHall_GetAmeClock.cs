using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHall_GetAmeClock : MonoBehaviour
{
    
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private MainHall_ObjectManager mainHall_ObjectManager;     // 복도씬 오브젝트 매니저
    private Interaction_Items interaction;                     // 아이템의 상호작용을 위한 컴포넌트
    private Item18AmeClock item18AmeClock;                     // 해당 아이템의 정보를 가지고 있는 컴포넌트
    #endregion

    #region 해당 스크립트에서 사용할 변수 및 프로퍼티 모음
    private bool isGetItem;   // 이미 아이템을 얻었는지 확인하기 위한 변수
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();          // MainHall_ObjectManager를 찾아와서 할당
        interaction = GetComponent<Interaction_Items>();       // 본인이 지닌 interaction_item 컴포넌트를 가져와서 할당
    }

    /// <summary>
    /// MainHall_GetAmeClock 초기화 함수
    /// </summary>
    public void Init()
    {
        item18AmeClock = GetComponent<Item18AmeClock>();    // 자신의 아이템 정보를 지닌 컴포넌트를 가져와서 할당

        isGetItem = item18AmeClock.isGetItem;   // 해당 아이템이 회수되었는지 확인해서 설정

        Setting_SceneStart();           // 아이템을 세팅해주기 위한 함수 호출
    }

    // 초기 세팅 함수
    void Setting_SceneStart()
    {
        // 씬 시작했을 때 isGetItem의 true false값을 토대로 초기 위치 설정
        if (isGetItem)
        {
            this.gameObject.SetActive(false);   // 해당 아이템 오브젝트 비활성화

        }
        else
        {
            this.gameObject.SetActive(true);    // 해당 아이템 오브젝트 활성화
        }
    }
}
