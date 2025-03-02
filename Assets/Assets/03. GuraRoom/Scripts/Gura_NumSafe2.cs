using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gura_NumSafe2 : MonoBehaviour
{

    Interaction_Gimics interaction;
    GameObject player;

    [SerializeField]
    GameObject NumSafeCanvas;

    [SerializeField]
    GameObject BaelzDice;

    bool isNumSafeCleared = false;

    public bool isNumSafeOpen = false;
    public bool isPoseidonUp = false;


    [SerializeField]
    GameObject NumberHintColl;

    [SerializeField]
    private GameObject takoBaelz;
    [SerializeField]
    GameObject lightEffect;


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

        Setting_SceneStart();

        interaction.hintChild_OutLine = new cakeslice.Outline[1];
        interaction.hintChild_OutLine[0] = GameObject.Find("NumberHint").GetComponent<cakeslice.Outline>();


        if (interaction != null)
        {
            if (ItemManager._instance.inventorySlots[14].GetComponent<IItem>().isGetItem == false)
            {
                StartCoroutine(WaitTouch()); // 이건 문만 다른거할때 지우자!
            }
        }

        NumSafeCanvas.SetActive(false);

        StartCoroutine(OpenNumSafe());
    }



    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (interaction.run_Gimic)
        {
            if (ItemManager._instance.inventorySlots[14].GetComponent<IItem>().isGetItem == true)
            {
                GetComponent<BoxCollider>().enabled = false;

                NumberHintColl.GetComponent<BoxCollider>().enabled = false;
            }
            else
            { 
                NumSafeCanvas.GetComponent<Gura_NumSafeCanvas2>().BaelzUp();
                NumberHintColl.GetComponent<BoxCollider>().enabled = false;
            }

            isNumSafeCleared = true;
        }
        else
        {

        }
    }

    IEnumerator WaitTouch()
    {
        yield return new WaitUntil(() => (interaction.run_Gimic) == true);
        // 기믹 수행해라 여기에

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
            NumSafeCanvas.GetComponent<Gura_NumSafeCanvas2>().closeSafeCanvas();

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
        yield return new WaitUntil(() => NumSafeCanvas.GetComponent<Gura_NumSafeCanvas2>().CanvasAnswerNum == 321);


        // 이거 내가 추가한거
        //GameObject.Find("NumberHint").GetComponent<Outline>().enabled = false;

        isNumSafeOpen = true;

        NumberHintColl.GetComponent<BoxCollider>().enabled = false;

        takoBaelz.SetActive(true);

        lightEffect.SetActive(true);
        lightEffect.GetComponent<ParticleSystem>().Play();

        takoBaelz.GetComponent<Animator>().Play("Baelz");
        takoBaelz.GetComponent<AudioSource>().Play();

        GetComponent<Interaction_Gimics>().run_Gimic = true;
        GetComponent<BoxCollider>().enabled = false;

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();

        NumSafeCanvas.GetComponent<Gura_NumSafeCanvas2>().closeSafeCanvas();

    }

}
