using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gura_StatueTurnScirpt : MonoBehaviour
{

    Interaction_Gimics interaction;
    GameObject player;

    [SerializeField]
    int currentWay = 0;
    [SerializeField]
    int correctWay = 0;

    public bool isStatueCorrect = false;

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

        interaction.Init();

        if (interaction != null)
        {
            StartCoroutine(WaitTouch()); // �̰� ���� �ٸ����Ҷ� ������!

        }

        Setting_SceneStart();

        if (currentWay == correctWay)
        {
            isStatueCorrect = true;
        }
        else
        {
            isStatueCorrect = false;
        }
    }



    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {

        }
        else
        {

        }
    }

    IEnumerator WaitTouch()
    {
        yield return new WaitUntil(() => (interaction.run_Gimic) == true);
        // ��� �����ض� ���⿡

        interaction.run_Gimic = false;

        gameObject.transform.Rotate(0, 90, 0);
        // this.gameObject.GetComponentInParent<Transform>().Rotate(0, 90, 0);
        currentWay++;
        if (currentWay == 4)
        {
            currentWay = 0;
        }

        if (currentWay == correctWay)
        { 
            isStatueCorrect = true;
        }
        else
        {
            isStatueCorrect = false;
        }

        StartCoroutine(WaitTouch());
    }








}