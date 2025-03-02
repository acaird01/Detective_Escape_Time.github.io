using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Calli_DialLockScene2 : MonoBehaviour
{
    public bool GimicMove = false; // true일 때 실행 된거

    [SerializeField]
    Calli_ClockFace[] clocks;

    //AudioSource audioSource;
    //Animator animator;

    public bool fixClock_Check = false; // 빠진 다이얼 끼웠는지
    bool[] Clocks_Right_Answer; // clock들이 정답인지 아닌지 판정을 담을 변수
    Interaction_Gimics interaction;

    [SerializeField]
    int[] clocks_Answer; // 5개 비밀번호 정답
    public GameObject[] clocks_Answer_Text; // 정답 5개가 표현될 text 배치될 힌트오브젝트 넣을 곳

    public GameObject DialLock_box;
    private GameObject TakoKronii;

    Calli_DialLock_BoxOpen dialLock_Check;
    Calli_DialCameraCtrl cameraCtrl;
    void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>(); // 기믹 수행 여부만 체크
        GimicMove = interaction.run_Gimic; 

        clocks = gameObject.GetComponentsInChildren<Calli_ClockFace>();
        dialLock_Check = gameObject.GetComponentInParent<Calli_DialLock_BoxOpen>();
        cameraCtrl = gameObject.GetComponentInParent<Calli_DialCameraCtrl>();
        TakoKronii = GameObject.FindAnyObjectByType<Item03TakoKronii>().gameObject;

        SceneStartSetting_Calli_DiallockScene(); // 데이터에서 불러온 기믹 상태에 따른 초기위치 설정

        Clocks_Right_Answer = new bool[clocks.Length];
        clocks_Answer = new int[clocks.Length];
        FirstClocks_BoolSet(); // 초기화한 Clocks_Right_Answer 배열 bool값 셋팅
        FirstClocks_AnswerSet(); // 초기화한 clocks_Answer 배열 셋팅 -> clocks들 정답 셋팅 및 힌트위치에 값 셋팅
        StartCoroutine(WaitTouch()); // 상호작용 대기
    }

    private void Update()
    {
        CheckClocks_RotData(); // 정답 맞추는거 배열 내 bool값들이 전부 true인지 체크

    }
    IEnumerator WaitTouch() // clock들 정답 모두 맞으면 의 true값이 들어와 이 스크립트가 실행될 때 gimicmove가 true가 될것
    {
        if (GimicMove == false)
        {
            clocks[0].gameObject.SetActive(false);
            yield return new WaitUntil(() => fixClock_Check == true); // 다이얼 끼워 고치는거 대기
            clocks[0].gameObject.SetActive(true);
            // 자물쇠 조각 인벤토리로 되돌려줌
            ItemManager._instance.ReturnItem(26);
            ItemManager._instance.DeactivateItem(26);
            yield return new WaitUntil(() => GimicMove == true); // 정답맞추는거 대기
            TakoKronii.SetActive(true);
            transform.localRotation = Quaternion.Euler(transform.localRotation.x - 90f, transform.localRotation.y, transform.localRotation.z + 90f);
            StartCoroutine(MoveBox());
        }
    }

    public float rotationDuration = 1f; // 박스 움직이는데 걸리는 시간

    IEnumerator MoveBox()
    // 코루틴을 사용하여 회전 후 자물쇠 풀리는 모션? + 소리 내고 사라지게
    {
        float elapsedTime = 0;
        float moveSpeed = 0.2f;
        //audioSource.Play();
        while (elapsedTime < rotationDuration)
        {
            DialLock_box.transform.Translate(-Vector3.up * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        interaction.run_Gimic = GimicMove;
        dialLock_Check.dialLockOpen = true;
        cameraCtrl.ExitButton();
        gameObject.SetActive(false);
    }


    void FirstClocks_BoolSet() // 초기 bool값 셋팅
    {
        for (int i = 0; i < clocks.Length; i++)
        {
            Clocks_Right_Answer[i] = false;
        }
    }

    void FirstClocks_AnswerSet()
    {
        // 답 세팅
        clocks_Answer[0] = 6;   // guyrys = 6글자
        clocks_Answer[1] = 4;   // nemu = 4글자
        clocks_Answer[2] = 6;   // friend = 6글자
        clocks_Answer[3] = 5;   // boros = 5글자
        clocks_Answer[4] = 9;   // mr. squerks = 9글자(.과 띄어쓰기 제외)*
    }

    void CheckClocks_RotData() // clocks가 제방향인지 체크
    {
        bool CheckGimicMove = false;

        for (int i = 0; i < clocks.Length; i++)
        {
            float chairRotation = clocks[i].transform.localEulerAngles.z;
            float targetRotationX;

            if (0 <= i && i <= 4)
            {
                targetRotationX = clocks_Answer[i] * 36;
            }
            else
            {
                targetRotationX = (10 - clocks_Answer[i]) * -36;
            }


            if (Mathf.Approximately(chairRotation, targetRotationX))
            {
                if (!Clocks_Right_Answer[i])
                {
                    CheckGimicMove = true;
                    Clocks_Right_Answer[i] = true;
                }
            }
            else
            {
                if (Clocks_Right_Answer[i])
                {
                    Clocks_Right_Answer[i] = false;
                }
            }

        }

        if (CheckGimicMove)
        {
            GimicMove = Clocks_Right_Answer.All(n => n == true);
        }

    }


    public void SceneStartSetting_Calli_DiallockScene() // 여기선 한번만 실행되면 아이템을 얻고 끝이기에 실행 됬는지 여부만 가져올것
    {
        if (GimicMove)
        {
            dialLock_Check.dialLockOpen = true;
            if (ItemManager._instance.inventorySlots[3].GetComponent<IItem>().isGetItem)
            {
                TakoKronii.SetActive(true);
            }

            transform.gameObject.SetActive(false);
        }
        else
        {
            dialLock_Check.dialLockOpen = false;
            TakoKronii.SetActive(false);
        }
    }
}
