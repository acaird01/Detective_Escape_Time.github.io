using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_CrystalPosCheck : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;    // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;             // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                          // 플레이어
    private TextController textController;              // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private Calli_CrystalPosGimic calli_CrystalPosGimic;    // 수정 배치 기믹 관리 스크립트
    private GameObject crystalOnFrame;                  // 해당 액자에 배치된 크리스탈의 정보를 저장할 변수
    #endregion

    #region 각 액자에서 사용할 Action
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private bool settingGimic { get; set; }     // 기믹 세팅을 위한 변수
    public bool SettingForObjectToInteration    // 기믹 세팅을 위한 변수 프로퍼티
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    public bool isFramePositionSelect;    // 현재 액자칸이 선택되었는지 확인하기 위한 변수
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");                                             // 플레이어를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // 상호작용을 위한 Interaction_Gimics 할당
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager를 찾아와서 할당
        textController = player.GetComponentInChildren<TextController>();               // 플레이어의 자식에서 TextController를 찾아와서 할당

        Init();     // 초기화 함수 호출
    }

    private void Init()
    {
        calli_CrystalPosGimic = GameObject.FindAnyObjectByType<Calli_CrystalPosGimic>();    // 수정 배치 기믹 관리 스크립트를 찾아와서 할당

        settingGimic = interaction.run_Gimic;   // 기믹이 이미 실행됬는지 아닌지 확인하는 변수
        crystalOnFrame = null;   // 아직 해당 액자에 배치된 수정이 없다고 상태 변경
        isFramePositionSelect = false;   // 해당 액자 칸이 아직 선택되지 않았다고 변경

        StartCoroutine(WaitTouch());    // 상호작용 대기함수 호출
    }

    // 상호작용되기 전까지 대기할 코루틴 함수
    private IEnumerator WaitTouch()
    {
        while (player) // 한번하고 뽀사지면 이거빼고, 창문 열고 닫는거처럼 반복필요하면 이거 넣고 쓰기
        {
            // this.GetComponentInChildren<ParticleSystem>().Play();

            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // 생성되고난뒤 여기서 대기하다가

            // 기믹 실행이 참인 경우
            if (interaction.run_Gimic && !isFramePositionSelect)
            {
                // 현재 선택중인 수정이 있을때만 실행
                if (calli_CrystalPosGimic.selectCrystal != null)
                {
                    isFramePositionSelect = true; // 해당 수정이 선택되었다고 상태 변경. 추가 반복을 막음
                    interaction.run_Gimic = false; // 다시 상호작용할 수 있도록 상태 변경
                                                   // 이미 최대갯수보다 적은 수의 테이프만 소환되었고, 해당 핫키의 테이프가 테이프칸 위에 없는 경우에만 소환을 진행
                    if (!calli_CrystalPosGimic.isCrystalReachMaxNum && (!calli_CrystalPosGimic.GetAnswerCrystalFramePos(calli_CrystalPosGimic.selectCrystal.name)))
                    {
                        MoveCrystalOnFrame(calli_CrystalPosGimic.selectCrystal);   // 현재 액자 위치에 수정을 이동시키기 위한 함수 호출
                    }
                    else
                    {
                        isFramePositionSelect = false;    // 소환되지 않았으니 상태 변경
                        interaction.run_Gimic = false;
                    }
                }
            }
            else
            {
                interaction.run_Gimic = false; // 다시 상호작용할 수 있도록 상태 변경
            }
        }
    }


    // 액자 위에 수정을 이동시키기 위한 함수
    private void MoveCrystalOnFrame(GameObject _crystal)
    {
        crystalOnFrame = _crystal;
        crystalOnFrame.GetComponent<Transform>().position = this.transform.position + (Vector3.up * -0.2f);   // 해당 액자 칸으로 해당 수정을 살짝 떠있게끔 이동 시킴
        //tapeOnTableHole.transform.localScale = new Vector3(35f, 2.25f, 15.3f);                    // 생성되는 테이프의 크기 조절
        //tapeOnTableHole.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        calli_CrystalPosGimic.SetAnswerCrystal(crystalOnFrame.name, this.name);   // 액자에 놓았으므로 배열에 저장하기위한 함수 호출

        // 추가 상호작용을 통해 다시 상호작용 되지 않도록 collider 비활성화
        crystalOnFrame.GetComponent<Collider>().enabled = false;
        this.GetComponent<Collider>().enabled = false;

        calli_CrystalPosGimic.SetReplaceWrongPlaceCrystals_CallBack(ReplaceWrongPlaceCrystal);    // 잘못된 위치에 놓인 경우 실행할 함수 Action에 넣어줌
    }

    // 액자에서 잘못된 위치에 테이프가 놓였을 경우 실행할 함수
    private void ReplaceWrongPlaceCrystal()
    {
        crystalOnFrame.GetComponent<Calli_CrystalPosChange>().CrystalReplace(); // 제자리로 돌아가는 함수 실행
        interaction.run_Gimic = false;  // 해당 기믹이 아직 실행되지 않은 상태로 변경
        isFramePositionSelect = false;     // 비워졌으므로 false로 변경

        // 다시 상호작용 가능하도록 콜라이더 활성화
        crystalOnFrame.GetComponent<Collider>().enabled = true;
        this.GetComponent<Collider>().enabled = true;
    }
}
