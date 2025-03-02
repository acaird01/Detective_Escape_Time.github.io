using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_TrayGimic_SaveData : MonoBehaviour
{
    Interaction_Gimics interaction;
    Kiara_Tray kiara_Tray;
    // Start is called before the first frame update
    void Start()
    {
        kiara_Tray = gameObject.GetComponentInChildren<Kiara_Tray>();
        interaction = gameObject.GetComponent<Interaction_Gimics>();

        firstGameSetting();
        StartCoroutine(TraySaveData());
    }

    IEnumerator TraySaveData()
    {
        yield return new WaitUntil(() => kiara_Tray.datasave == true);
        interaction.run_Gimic = true;
    }

    void firstGameSetting()
    {
        if (interaction.run_Gimic == true)
        {
            kiara_Tray.gameObject.SetActive(false);
        }
    }
}
