using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_GhostGimics : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    private bool isGhostVisible;        // 이미 영혼이 보이고 있는지 확인하기 위한 변수
    public bool IsGhostVisible          // Calli_CatchGhost에서 현재 상태를 확인하기 위한 프로퍼티
    { 
        get { return isGhostVisible; }
    }
    [SerializeField]
    private Calli_CatchGhost[] ghosts;  // 영혼들을 저장할 배열
    private int ghosts_MaxNum;          // 영혼들 배열 길이를 저장할 변수
    #endregion

    #region 해당 스크립트에서 사용할 Action 모음
    private Action GhostInit;   // 수확될 영혼들의 초기화 함수를 담을 Action
    public void SetGhostInit(Action _GhostInit)    // 초기화 함수를 담을 Action의 setter
    { 
        GhostInit += _GhostInit;
    }
    #endregion

    private void Awake()
    {
        // Action 초기화
        GhostInit = null;
    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // 초기화 함수
    private void Init()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();        // 상호작용을 위한 Interaction_Gimics 할당
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;   // 플레이어를 찾아와서 할당

        // ghosts = GetComponentsInChildren<Calli_CatchGhost>();   // 자식으로 있는 Calli_CatchGhost 컴포넌트를 가진 영혼들을 배열에 저장(왜 안되지)
        ghosts_MaxNum = ghosts.Length;                          // ghosts의 길이 저장
        for (int i = 0; i < ghosts_MaxNum; i++)                 // ghosts에 있는 영혼들의 초기화 함수 호출
        {
            ghosts[i].Init();     // 초기화 함수 호출
        }

        // GhostInit?.Invoke();        // 영혼들 초기화 함수 실행
        isGhostVisible = false;     // 현재 안보이는 상태로 설정
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 영혼이 보이지 않고, 핫키에서 선택된 아이템이 필요한 아이템인 경우에 기믹 수행
        if (!isGhostVisible && string.Equals(ItemManager._instance.hotkeyItemName, "Item_12_CalliopeScythe"))
        {
            isGhostVisible = true;  // 영혼이 보이는 상태로 변경해서 update에서 추가 실행이 되지 않도록 막음

            // ghosts에 저장된 영혼들을 전부 활성화 시킴
            for (int i = 0; i < ghosts_MaxNum; i++)
            {
                if (!ghosts[i].IsGhostCatch) // 해당 영혼이 수확이 안됬을 경우에만 활성화 시킴
                {
                    ghosts[i].gameObject.SetActive(isGhostVisible);
                }
            }
        }
        else if (isGhostVisible && !string.Equals(ItemManager._instance.hotkeyItemName, "Item_12_CalliopeScythe"))
        {
            isGhostVisible = false;  // 영혼이 보이지 않는 상태로 변경

            // ghosts에 저장된 영혼들을 전부 비활성화 시킴
            for (int i = 0; i < ghosts_MaxNum; i++)
            {
                ghosts[i].gameObject.SetActive(isGhostVisible);
            }
        }
        else
        {
            // 그 외의 경우는 그냥 종료
            return;
        }
    }

    #region 기믹을 수행하는 함수 모음
    #endregion
}
