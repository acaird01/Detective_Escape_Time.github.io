using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_StatueTurnCheckCheck : MonoBehaviour
{
    Interaction_Gimics interaction;
    GameObject player;

    [SerializeField]
    GameObject TakoMumei;

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

    }

    void Setting_SceneStart()
    {
        //find GuraStatueTurnCheck
        Gura_StatueTurnCheck statueTurnCheck = gameObject.GetComponent<Gura_StatueTurnCheck>();

        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (interaction.run_Gimic)
        {
            statueTurnCheck.ChestResult();
            if (ItemManager._instance.inventorySlots[1].GetComponent<IItem>().isGetItem == true)
            {
                TakoMumei.SetActive(false);
            }
        }
        else
        {

        }
    }

}


