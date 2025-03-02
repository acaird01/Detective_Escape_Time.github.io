using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_ChestAnimCtrl : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private GameObject item_match;                      // 기믹 수행을 완료하면 제공해줄 아이템(성냥, 27)

    [Header("상자 뚜껑 게임 오브젝트")]
    [SerializeField]
    private GameObject Chest_Lid;                       // 상자 뚜껑 게임 오브젝트
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    private bool isChestUnlock;
    public bool IsChestUnlock
    {
        set
        {
            isChestUnlock = value;
        }
    }
    // private bool isChestOpen;
    private float rotationDuration = 0.5f; // 회전에 걸리는 시간
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");                                             // 플레이어를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();               // 플레이어의 자식에서 TextController를 찾아와서 할당

    }

    // 초기화 함수 (calli_ChestUnlock에서 호출)
    public void Init(bool _isChestUnlock)
    {
        interaction = GetComponent<Interaction_Gimics>();   // 자신에게 붙어있는 interaction gimic을 할당
        item_match = this.GetComponentInChildren<Item27Matches>().gameObject;    // 성냥 아이템을 찾아와서 할당

        settingGimic = interaction.run_Gimic;   // 기믹 수행 여부 초기화
        isChestUnlock = _isChestUnlock;      // 상자를 아직 열수 없다고 설정

        Setting_SceneStart();       // 세이브 데이터에 따라 기믹 세팅
    }

    // 세이브 데이터에 따라 시작전에 기믹 세팅
    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            // 상자 여는 함수 호출
            StartCoroutine(ChestOpen()); // 상자 여는 함수 호출
            item_match.SetActive(true); // 성냥을 활성화 해줌
        }
        else
        {
            // 기믹을 아직 수행하지 않았으면 setgimic이 false
            item_match.SetActive(false); // 성냥을 비활성화 해줌
        }
    }

    IEnumerator ChestOpen() // 기믹 작동했을 때 
    {
        item_match.SetActive(true); // 성냥을 활성화 해줌

        Quaternion startRotation = Chest_Lid.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(-110, 0, 0);

        //        audioSource.Play();
        float elapsedTime = 0;
        while (elapsedTime < rotationDuration)
        {
            Chest_Lid.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Chest_Lid.transform.rotation = endRotation;

        yield return new WaitForSeconds(.1f);
    }

    /// <summary>
    /// 상자를 열어주기 위한 코루틴 함수 실행용 함수
    /// </summary>
    public void OpenChest()
    { 
        StartCoroutine(ChestOpen());
    }
}
