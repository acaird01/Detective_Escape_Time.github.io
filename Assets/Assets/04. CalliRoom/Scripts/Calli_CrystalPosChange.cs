using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Calli_CrystalPosChange : MonoBehaviour
{
    #region 오브젝트 및 컴포넌트를 할당할 변수 모음
    private Calli_ObjectManager calli_ObjectManager;        // 칼리씬 오브젝트 매니저
    private Interaction_Gimics interaction;                 // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private GameObject player;                              // 플레이어
    private TextController textController;                  // 상호작용 대사를 출력할 text UI를 컨트롤하는 스크립트

    private Calli_CrystalPosGimic calli_CrystalPosGimic;    // 수정 배치 기믹 관리 스크립트
    #endregion

    #region 해당 스크립트에서 사용할 변수 모음
    private Vector3 crystalOrigin_Position;     // 해당 수정이 원래있던 위치를 저장할 변수
    private bool isCrystalSelect;               // 수정이 선택됬는지 아닌지 확인하기 위한 변수
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

    // 초기화 함수
    private void Init()
    {
        calli_CrystalPosGimic = GameObject.FindAnyObjectByType<Calli_CrystalPosGimic>();    // 수정 배치 기믹 관리 스크립트를 찾아와서 할당

        crystalOrigin_Position = this.transform.position;   // 해당 수정의 현재 위치 저장(최초 위치)
        isCrystalSelect = false;    // 아직 수정이 선택되지 않았다고 변경

        StartCoroutine(WaitTouch());    // 상호작용 대기 코루틴 함수 호출
    }

    // 상호작용되기 전까지 대기할 코루틴 함수
    private IEnumerator WaitTouch()
    {
        while (player) // 한번하고 뽀사지면 이거빼고, 창문 열고 닫는거처럼 반복필요하면 이거 넣고 쓰기
        {
            // this.GetComponentInChildren<ParticleSystem>().Play();

            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // 생성되고난뒤 여기서 대기하다가

            // 기믹 실행이 참인 경우
            if (interaction.run_Gimic && !isCrystalSelect)
            {
                isCrystalSelect = true; // 해당 수정이 선택되었다고 상태 변경. 추가 반복을 막음
                interaction.run_Gimic = false; // 다시 상호작용할 수 있도록 상태 변경

                calli_CrystalPosGimic.SetSelectCrystal(this.gameObject);    // 현재 선택된 수정을 본인으로 변경
            }
            else
            {
                interaction.run_Gimic = false; // 다시 상호작용할 수 있도록 상태 변경
            }
        }
    }

    #region 선택 또는 미선택시 수정의 위치를 조정해주는 함수
    /// <summary>
    /// 해당 수정이 선택되었다고 살짝 위로 올려줄 함수
    /// </summary>
    public void CrystalPosUp()
    {
        // 현재 위치보다 약간 위쪽으로 올려서 선택되었다고 표시해줌
        this.transform.position += Vector3.up * 0.5f;
    }

    /// <summary>
    /// 해당 수정이 더 이상 선택되지 않았으므로 다시 본래 위치로 되돌려줄 함수
    /// </summary>
    public void CrystalReplace()
    {
        isCrystalSelect = false;
        // 미리 저장해둔 본래 위치로 이동
        this.transform.position = crystalOrigin_Position;
    }
    #endregion
}
