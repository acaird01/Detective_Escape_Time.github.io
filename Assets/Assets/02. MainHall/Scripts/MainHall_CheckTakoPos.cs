using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainHall_CheckTakoPos : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    private Interaction_Gimics interaction; // 상호작용하는 기믹인지 확인하기 위한 컴포넌트
    private MainHall_ChessBoard chessBoard; // 체스판 본체에서 관리하는 스크립트 할당할 변수
    private GameObject player;              // 플레이어
    private GameObject takoOnBoard;           // 체스판에 놓을 타코를 생성할때 사용할 GameObject 변수
    #endregion

    #region 해당 기믹에서 사용할 변수 모음
    private string ChessboardPosition;  // 해당 체스판이 몇번째 칸인지 저장할 변수
    private bool setTakoOnChessboard;   // 해당 체스판에 타코를 놓았는지 확인할 변수
    private bool settingGimic { get; set; } // 기믹 세팅 여부 결정할 변수
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        ChessboardPosition = this.gameObject.name;  // 이름을 이용해 현재 체스판의 위치를 저장
    }

    private void Start()
    {
        player = GameObject.Find("Player"); // 플레이어를 찾아와서 할당
        interaction = gameObject.GetComponent<Interaction_Gimics>();        // 상호작용을 위한 Interaction_Gimics 할당
        chessBoard = GameObject.FindAnyObjectByType<MainHall_ChessBoard>(); // 체스판 본체를 찾아와서 할당

        //settingGimic = false;
        chessBoard.SetInitChessBoard(Init);    // 체스판에서 초기화 진행하기 위해 Action에 함수 할당
    }

    // chessboard에서 실행
    public void Init()
    {
        takoOnBoard = null;       // 현재 생성된 타코가 없게끔 초기화
        settingGimic = interaction.run_Gimic;   // 기믹 설치여부에 따라 기믹 세팅하기 위한 변수 초기화

        setTakoOnChessboard = false;    // 아직 놓지 않았다고 초기값 설정(나중에 저장데이터 기반으로 세팅다시 할 것.)

        // 초기화 후 기믹을 실행하기 시작
        StartCoroutine(WaitTouch());            // 상호작용전까지 대기시킬 코루틴 함수 호출

        // Setting_SceneStart();
    }

    // 상호작용되기 전까지 대기할 코루틴 함수
    private IEnumerator WaitTouch()
    {
        while (player) // 한번하고 뽀사지면 이거빼고, 창문 열고 닫는거처럼 반복필요하면 이거 넣고 쓰기
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // 생성되고난뒤 여기서 대기하다가

            // 기믹 실행이 참인 경우
            if (interaction.run_Gimic && !setTakoOnChessboard)
            {
                // Debug.Log("선택된 체스판 위치 : " + ChessboardPosition);
                setTakoOnChessboard = true; // 아이템이 해당 칸 위에 존재한다고 상태 변경. 추가 반복을 막음

                // 이미 최대갯수보다 적은 수의 타코만 소환되었거나, 해당 핫키의 타코가 체스판 위에 없는 경우에만 소환을 진행
                if (!chessBoard.isTakoReachMaxNum && (!chessBoard.GetAnswerChessTako(ItemManager._instance.hotkeyItemIndex)))
                {
                    // 현재 핫키의 아이템이 타코인 경우에만 소환 실행
                    if (ItemManager._instance.hotkeyItemIndex == 18 || (ItemManager._instance.hotkeyItemIndex >= 1 && ItemManager._instance.hotkeyItemIndex <= 8))   // 벨즈가 0번이라 소환이 안되는듯 함. 확인 요함.
                    {
                        // Debug.Log("summon tako 함수 실행: " + ItemManager._instance.hotkeyItemName); // 타코 소환 확인 로그 출력
                        SummonTakoOnChessboard();   // 현재 체스판 위치에 타코를 소환하기 위한 함수 호출
                    }
                }
                else
                {
                    setTakoOnChessboard = false;    // 소환되지 않았으니 상태 변경
                    interaction.run_Gimic = false;
                }
            }
            else
            {
                setTakoOnChessboard = false;    // 소환되지 않았으니 상태 변경
                interaction.run_Gimic = false;
            }
        }
    }

    // 체스판 위에 타코 소환하기 위한 함수
    private void SummonTakoOnChessboard()
    {
        // Resources 풀더에서 현재 핫키에 등록된 아이템 프리팹을 찾아와 생성
        takoOnBoard = Instantiate(Resources.Load<GameObject>("Items/Prefabs/" + ItemManager._instance.hotkeyItemName));
        takoOnBoard.transform.SetParent(this.transform, false);             // 해당 체스칸의 자식으로 생성
        takoOnBoard.transform.position = this.transform.position + (Vector3.up * 0.2f);           // 해당 체스판 위치에 살짝 떠있게끔 이동
        takoOnBoard.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);   // 생성되는 타코의 크기 조절

        chessBoard.SetAnswerChessTako(ItemManager._instance.hotkeyItemIndex, ChessboardPosition);   // 체스판에 놓았으므로 배열에 저장하기위한 함수 호출
        // chessBoard.CountTakos = chessBoard.CountTakos + 1;// 생성되었으므로 CountTakos를 이용해 체스판에 올라간 타코수를 1증가

        // 추가 상호작용을 통해 다시 인벤토리로 들어가지 않도록 collider 비활성화
        takoOnBoard.GetComponent<Collider>().enabled = false;
        //this.GetComponent<Collider>().enabled = false;

        chessBoard.SetDestroyWrongPlaceTakos_CallBack(DestroyWrongPlaceTako);    // 잘못된 위치에 놓인 경우 실행할 함수 Action에 넣어줌

        // TakoPlace.transform.position = this.transform.position; // 해당 체스판 위치에 생성
        // Debug.Log(ItemManager._instance.hotkeyItemName); // 핫키의 아이템을 제대로 찾아오는지 확인하기 위한 로그 (241001)
    }

    // 체스판에서 잘못된 위치에 타코가 놓였을 경우 실행할 함수
    private void DestroyWrongPlaceTako()
    {
        Destroy(takoOnBoard);           // 해당 위치에 생성된 타코 제거
        interaction.run_Gimic = false;  // 해당 기믹이 아직 실행되지 않은 상태로 변경
        setTakoOnChessboard = false;    // 비워졌으므로 false로 변경
    }
}
