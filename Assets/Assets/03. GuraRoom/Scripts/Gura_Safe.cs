using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gura_Safe : MonoBehaviour
{
    Interaction_Gimics interaction;
    GameObject player;
    TextController textController;

    public bool isSafeOpen = false;
    bool isSafeCleared = false;

    [SerializeField]
    private GameObject safe;
    [SerializeField]
    private GameObject wedge;
    [SerializeField]
    private GameObject KeyTrident;
    [SerializeField]
    private GameObject TakoGura;

    [TextArea]
    [SerializeField]
    private string SafeText;

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

        textController = player.GetComponentInChildren<TextController>();

        Setting_SceneStart();

        if (interaction.run_Gimic == true)
        {
            isSafeCleared = true;;
        }
        else if (interaction.run_Gimic == false && interaction != null)
        {
            if (ItemManager._instance.inventorySlots[6].GetComponent<IItem>().isGetItem == false)
            {
                StartCoroutine(WaitTouch());
            }
        }

        Setting_SceneStart();

    }


    void Setting_SceneStart()
    {
        // Initialize or reset logic based on settingGimic value
        if (interaction.run_Gimic)
        {
            wedge.transform.Rotate(0, 0, 135);

            KeyTrident.SetActive(true);

            TakoGura.SetActive(false);

        }
        else
        {

        }
    }

    public IEnumerator WaitTouch()
    {
        yield return new WaitUntil(() => (interaction.run_Gimic) == true);

        if (ItemManager._instance.hotkeyItemIndex == 11)
        {
            isSafeOpen = true;
            StartCoroutine(OpenSafe());
        }
        else
        {
            interaction.run_Gimic = false;

            GetComponent<AudioSource>().Play();

            StartCoroutine(textController.SendText(SafeText));

            StartCoroutine(WaitTouch());
        }
    }


    IEnumerator OpenSafe()
    {
        yield return new WaitUntil(() => isSafeOpen == true);

        if (ItemManager._instance.inventorySlots[6].GetComponent<IItem>().isGetItem == false)
        {
            if (isSafeCleared == false)
            {
                wedge.GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSeconds(0.5f);

            TakoGura.SetActive(true);
        }

        GetComponent<BoxCollider>().enabled = false;

        // StartCoroutine(ShakeCameraEffect());

        wedge.transform.Rotate(0, 0, 135);

        KeyTrident.SetActive(true);   

        StopAllCoroutines();

        interaction.run_Gimic = true;

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
    }

    IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Camera camera = Camera.main;

        Vector3 originalPosition = camera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            camera.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        camera.transform.localPosition = originalPosition;
    }

    IEnumerator ShakeCameraEffect()
    {
        float shakeDuration = 0.5f;
        float shakeMagnitude = 0.1f;

        yield return StartCoroutine(ShakeCamera(shakeDuration, shakeMagnitude));
    }
}
