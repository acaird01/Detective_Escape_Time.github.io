using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gura_NumSafe : MonoBehaviour
{

    Interaction_Gimics interaction;
    GameObject player;

    [SerializeField]
    GameObject NumSafeCanvas;

    [SerializeField]
    GameObject Poseidon;

    bool isNumSafeCleared = false;

    public bool isNumSafeOpen = false;
    public bool isPoseidonUp = false;

    [SerializeField]
    GameObject NumberHintColl;


    private bool settingGimic { get; set; }
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private void Start()
    {
        //settingGimic = false;
        player = GameObject.Find("Player");
        interaction = gameObject.GetComponent<Interaction_Gimics>();

        // ���� �߰��Ѱ�
        /*interaction.hintChild = new GameObject[1];
        interaction.hintChild[0] = GameObject.Find("NumberHint");*/

        // �ٲ�� �Ǵ°�
        interaction.hintChild_OutLine = new cakeslice.Outline[1];
        interaction.hintChild_OutLine[0] = GameObject.Find("NumberHint").GetComponent<cakeslice.Outline>();


        if (interaction != null)
        {
            StartCoroutine(WaitTouch()); // �̰� ���� �ٸ����Ҷ� ������!

        }

        Setting_SceneStart();

        NumSafeCanvas.GetComponent<Gura_NumSafeCanvas>().closeSafeCanvas();
        NumSafeCanvas.SetActive(false);

        StartCoroutine(OpenNumSafe());
    }



    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (interaction.run_Gimic)
        {
            Poseidon.GetComponent<Animator>().Play("PoseidonUp");

            NumberHintColl.GetComponent<BoxCollider>().enabled = true;

            StopAllCoroutines();
        }
        else
        {

        }
    }

    IEnumerator WaitTouch()
    {
        yield return new WaitUntil(() => (interaction.run_Gimic) == true);
        // ��� �����ض� ���⿡

        if (isNumSafeCleared == false)
        {
            NumSafeCanvas.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            player.GetComponentInChildren<PlayerCtrl>().keystrokes = true;
        }

        if (isNumSafeOpen)
        {
            interaction.run_Gimic = true;
            NumSafeCanvas.GetComponent<Gura_NumSafeCanvas>().closeSafeCanvas();

            NumberHintColl.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            interaction.run_Gimic = false;

            StartCoroutine(WaitTouch());
        }

    }

    IEnumerator OpenNumSafe()
    {
        yield return new WaitUntil(() => NumSafeCanvas.GetComponent<Gura_NumSafeCanvas>().CanvasAnswerNum == 123);

        isNumSafeOpen = true;

        GetComponent<Interaction_Gimics>().run_Gimic = true;
        GetComponent<BoxCollider>().enabled = false;

        isPoseidonUp = true;

        NumberHintColl.GetComponent<BoxCollider>().enabled = false;

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();

        NumSafeCanvas.GetComponent<Gura_NumSafeCanvas>().closeSafeCanvas();

    }



}
