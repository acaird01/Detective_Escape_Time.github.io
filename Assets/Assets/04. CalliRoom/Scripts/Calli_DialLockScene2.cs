using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Calli_DialLockScene2 : MonoBehaviour
{
    public bool GimicMove = false; // true�� �� ���� �Ȱ�

    [SerializeField]
    Calli_ClockFace[] clocks;

    //AudioSource audioSource;
    //Animator animator;

    public bool fixClock_Check = false; // ���� ���̾� ��������
    bool[] Clocks_Right_Answer; // clock���� �������� �ƴ��� ������ ���� ����
    Interaction_Gimics interaction;

    [SerializeField]
    int[] clocks_Answer; // 5�� ��й�ȣ ����
    public GameObject[] clocks_Answer_Text; // ���� 5���� ǥ���� text ��ġ�� ��Ʈ������Ʈ ���� ��

    public GameObject DialLock_box;
    private GameObject TakoKronii;

    Calli_DialLock_BoxOpen dialLock_Check;
    Calli_DialCameraCtrl cameraCtrl;
    void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>(); // ��� ���� ���θ� üũ
        GimicMove = interaction.run_Gimic; 

        clocks = gameObject.GetComponentsInChildren<Calli_ClockFace>();
        dialLock_Check = gameObject.GetComponentInParent<Calli_DialLock_BoxOpen>();
        cameraCtrl = gameObject.GetComponentInParent<Calli_DialCameraCtrl>();
        TakoKronii = GameObject.FindAnyObjectByType<Item03TakoKronii>().gameObject;

        SceneStartSetting_Calli_DiallockScene(); // �����Ϳ��� �ҷ��� ��� ���¿� ���� �ʱ���ġ ����

        Clocks_Right_Answer = new bool[clocks.Length];
        clocks_Answer = new int[clocks.Length];
        FirstClocks_BoolSet(); // �ʱ�ȭ�� Clocks_Right_Answer �迭 bool�� ����
        FirstClocks_AnswerSet(); // �ʱ�ȭ�� clocks_Answer �迭 ���� -> clocks�� ���� ���� �� ��Ʈ��ġ�� �� ����
        StartCoroutine(WaitTouch()); // ��ȣ�ۿ� ���
    }

    private void Update()
    {
        CheckClocks_RotData(); // ���� ���ߴ°� �迭 �� bool������ ���� true���� üũ

    }
    IEnumerator WaitTouch() // clock�� ���� ��� ������ �� true���� ���� �� ��ũ��Ʈ�� ����� �� gimicmove�� true�� �ɰ�
    {
        if (GimicMove == false)
        {
            clocks[0].gameObject.SetActive(false);
            yield return new WaitUntil(() => fixClock_Check == true); // ���̾� ���� ��ġ�°� ���
            clocks[0].gameObject.SetActive(true);
            // �ڹ��� ���� �κ��丮�� �ǵ�����
            ItemManager._instance.ReturnItem(26);
            ItemManager._instance.DeactivateItem(26);
            yield return new WaitUntil(() => GimicMove == true); // ������ߴ°� ���
            TakoKronii.SetActive(true);
            transform.localRotation = Quaternion.Euler(transform.localRotation.x - 90f, transform.localRotation.y, transform.localRotation.z + 90f);
            StartCoroutine(MoveBox());
        }
    }

    public float rotationDuration = 1f; // �ڽ� �����̴µ� �ɸ��� �ð�

    IEnumerator MoveBox()
    // �ڷ�ƾ�� ����Ͽ� ȸ�� �� �ڹ��� Ǯ���� ���? + �Ҹ� ���� �������
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


    void FirstClocks_BoolSet() // �ʱ� bool�� ����
    {
        for (int i = 0; i < clocks.Length; i++)
        {
            Clocks_Right_Answer[i] = false;
        }
    }

    void FirstClocks_AnswerSet()
    {
        // �� ����
        clocks_Answer[0] = 6;   // guyrys = 6����
        clocks_Answer[1] = 4;   // nemu = 4����
        clocks_Answer[2] = 6;   // friend = 6����
        clocks_Answer[3] = 5;   // boros = 5����
        clocks_Answer[4] = 9;   // mr. squerks = 9����(.�� ���� ����)*
    }

    void CheckClocks_RotData() // clocks�� ���������� üũ
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


    public void SceneStartSetting_Calli_DiallockScene() // ���⼱ �ѹ��� ����Ǹ� �������� ��� ���̱⿡ ���� ����� ���θ� �����ð�
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
