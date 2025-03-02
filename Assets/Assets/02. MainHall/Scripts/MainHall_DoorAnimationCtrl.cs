using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHall_DoorAnimationCtrl : MonoBehaviour
{
    Interaction_Gimics interaction;
    GameObject player;
    private MainHall_ObjectManager mainHall_ObjectManager;  // 복도씬의 오브젝트 매니저

    private AudioManager audioManager;
    private bool settingGimic { get; set; }
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    // 애니메이션 출력할 오브젝트의 Animator 컴포넌트
    private Animator animator;

    private void Start()
    {
        player = GameObject.Find("Player");
        mainHall_ObjectManager = GameObject.FindAnyObjectByType<MainHall_ObjectManager>();  // 복도씬의 오브젝트 매니저를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        audioManager = GameObject.FindAnyObjectByType<AudioManager>();

        settingGimic = interaction.run_Gimic;
        // 해당 문이 가진 animator컴포넌트 할당
        animator = GetComponent<Animator>();

        StartCoroutine(WaitTouch());

        Setting_SceneStart();
    }

    // 다음 씬 로딩 함수
    public void NextScene()
    {
        // 맵 이동 전 현재 맵에서의 볼륨값, 기믹 수행여부등을 gamemanager에 저장
        audioManager.SaveVolume();
        mainHall_ObjectManager.ChangeSceneData_To_GameManager();
        GameManager.instance.SaveData();
        GameManager.instance.PrevSceneName = GameManager.instance.nowSceneName;

        // 다음 씬 로드(다음 방으로 이동하기 전 로딩씬 실행)
        LoadingSceneManager.LoadScene(gameObject.name);
    }

    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (settingGimic)
        {
            animator.Play("DoorIdle");
        }
        else
        {

        }
    }

    IEnumerator WaitTouch()
    {
        while (player) // 한번하고 뽀사지면 이거빼고, 창문 열고 닫는거처럼 반복필요하면 이거 넣고 쓰기
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // 기믹 수행해라 여기에

            if (interaction.run_Gimic)
            {
                // 문열리는 애니메이션 재생
                StartCoroutine(opening());
            }
            else
            {
                // 문닫히는 애니메이션 재생
                StartCoroutine(closing());
            }
        }
    }

    IEnumerator opening() // 기믹 작동했을 때 
    {
        // 애니메이션이 플레이중이 아니라면
        animator.SetBool("DoorOpenClose", true);

        yield return new WaitForSeconds(.1f);

        // 만약 해당 문이 시작 방문이 아니라면 다음 씬으로 넘어감.
        if (!String.Equals(gameObject.name, "StartPos_Door"))
        {
            GameManager.instance.PrevSceneName = gameObject.name;   // 이전 씬 정보에 현재 넘어갈 씬의 이름을 저장
            NextScene();
        }
    }

    IEnumerator closing() // 기믹이 되돌아 갈때
    {
        animator.SetBool("DoorOpenClose", false);

        yield return new WaitForSeconds(.1f);
    }
}