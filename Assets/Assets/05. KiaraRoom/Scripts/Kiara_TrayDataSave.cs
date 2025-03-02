using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kiara_TrayDataSave : MonoBehaviour
{
    Kiara_Tray tray;
    Interaction_Gimics interaction;

    GameObject tray_Obj;
    // Start is called before the first frame update
    void Start()
    {
        tray = gameObject.GetComponentInChildren<Kiara_Tray>();
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        tray_Obj = tray.gameObject;
        SceneStartSetting_KiaraTray();
        StartCoroutine(ForSaveData());
    }
    void SceneStartSetting_KiaraTray() // 받아온 데이터에 따른 초기 위치 설정
    {
        if (interaction.run_Gimic)
        {
            tray_Obj.gameObject.SetActive(false);
            gameObject.GetComponent<Kiara_TrayDataSave>().enabled = false;
        }
        else
        {
            tray_Obj.gameObject.SetActive(true);
        }
    }
    IEnumerator ForSaveData()
    {
        yield return new WaitUntil(() => tray.datasave == true);
        {
            interaction.run_Gimic = true;
        }

    }
}
