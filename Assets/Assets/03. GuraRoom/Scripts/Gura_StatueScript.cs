using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gura_StatueScript : MonoBehaviour
{

    Interaction_Gimics interaction;
    GameObject player;

    [SerializeField]
    GameObject StatueAnswerCanvas;


    [SerializeField]
    Transform[] StatueModelPos;
    [SerializeField]
    Transform[] StatuePos;

    [SerializeField]
    GameObject[] AnswerStatueModels;

    public bool isStatueCorrect2 = false;

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

        if (interaction != null)
        {
            if (gameObject.activeSelf) // Check if the this obj self is active
            {
                StartCoroutine(WaitTouch()); // Start the coroutine only if 'StatueAnswerCanvas' is active
            }
        }

        StatueAnswerCanvas.SetActive(false);
    }



    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (interaction.run_Gimic)
        {
            isStatueCorrect2 = true;

            GetComponent<BoxCollider>().enabled = false;

            for (int i = 0; i < AnswerStatueModels.Length; i++)
            {
                AnswerStatueModels[i].SetActive(true);
            }

            StatueAnswerCanvas.GetComponent<Gura_StatueAnswerScript>().StatueResult();

            gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < AnswerStatueModels.Length; i++)
            {
                AnswerStatueModels[i].SetActive(false);
            }
        }
    }

    IEnumerator WaitTouch()
    {
        yield return new WaitUntil(() => (interaction.run_Gimic) == true);

        //�� Ʋ���� �ٽ� ����ڷ�ƾ
        Gura_StatueAnswerScript statueAnswerScript = StatueAnswerCanvas.GetComponent<Gura_StatueAnswerScript>();
        if (!statueAnswerScript.isStatueCorrect)
        {
            if (isStatueCorrect2 == false)
            {
                StatueAnswerCanvas.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                player.GetComponentInChildren<PlayerCtrl>().keystrokes = true;

                interaction.run_Gimic = false;
                StartCoroutine(WaitTouch());
            }   
        }
    }








}