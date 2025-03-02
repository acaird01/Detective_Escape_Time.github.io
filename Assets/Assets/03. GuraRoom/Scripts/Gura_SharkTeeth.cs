using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gura_SharkTeeth : MonoBehaviour
{
    Interaction_Gimics interaction;
    GameObject player;

    [SerializeField]
    AudioSource audioSource;

    public int teethNum;

    private bool settingGimic { get; set; }
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private void OnEnable()
    {
        // This will be called every time the object is activated
        player = GameObject.Find("Player");
        interaction = gameObject.GetComponent<Interaction_Gimics>();

        if (interaction != null)
        {
            StartCoroutine(WaitTouch());
        }

        Setting_SceneStart();
    }


    void Setting_SceneStart()
    {
        // Initialize or reset logic based on settingGimic value
        if (settingGimic)
        {
            // Logic for true settingGimic
        }
        else
        {
            // Logic for false settingGimic
        }
    }

    public IEnumerator WaitTouch()
    {
        yield return new WaitUntil(() => (interaction.run_Gimic) == true);

        // Add value to SharkSafe and deactivate object
        GameObject sharkSafe = GameObject.Find("SharkSafe");
        Gura_SharkSafe sharkSafeScript = sharkSafe.GetComponent<Gura_SharkSafe>();
        sharkSafeScript.SafeAnswerInt += teethNum;
        sharkSafeScript.pressedNum++;

        audioSource.Play();

        interaction.run_Gimic = false;

        this.gameObject.SetActive(false);
    }
}
