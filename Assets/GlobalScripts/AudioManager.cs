using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterSilder;
    [SerializeField] Slider bgmSilder;
    [SerializeField] Slider sfxSilder;
    [SerializeField] Slider mouseSensitivity;

    PlayerCtrl playerctrl;
    MouseCamLook mouseCamLook;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        masterSilder.onValueChanged.AddListener(SetMasterVolume);
        bgmSilder.onValueChanged.AddListener(SetBGMVolume);
        sfxSilder.onValueChanged.AddListener(SetSFXVolume);
        mouseSensitivity.onValueChanged.AddListener(SetMouseSensitivity);
        

        playerctrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        mouseCamLook = GameObject.FindAnyObjectByType<MouseCamLook>();
        SettingSound();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ExitButton()
    {
        mouseCamLook.mouseSensitivity = 500f * mouseSensitivity.value;
        playerctrl.ExitEscButton();
    }

    public void OnClickSaveDate() // 저장 버튼 클릭시 실행할 변수
    {
        SaveVolume();
        if (FindAnyObjectByType<MainHall_ObjectManager>() != null)
        {
            FindAnyObjectByType<MainHall_ObjectManager>().ChangeSceneData_To_GameManager();
        }
        if (FindAnyObjectByType<Gura_ObjectManager>() != null)
        {
            FindAnyObjectByType<Gura_ObjectManager>().ChangeSceneData_To_GamaManager();
        }
        if (FindAnyObjectByType<Calli_ObjectManager>() != null)
        {
            FindAnyObjectByType<Calli_ObjectManager>().ChangeSceneData_To_GameManager();
        }
        if (FindAnyObjectByType<Kiara_ObjectManager>() != null)
        {
            FindAnyObjectByType<Kiara_ObjectManager>().ChangeSceneData_To_GamaManager();
        }
        GameManager.instance.SaveData();
        ItemManager._instance.SaveInventory();
        StartCoroutine(playerctrl.gameObject.GetComponentInChildren<TextController>().SendText("저장되었습니다."));
    }
    public void OnClickSave_Exit()
    {
        SaveVolume();
        if (FindAnyObjectByType<MainHall_ObjectManager>() != null)
        {
            FindAnyObjectByType<MainHall_ObjectManager>().ChangeSceneData_To_GameManager();
        }
        if (FindAnyObjectByType<Gura_ObjectManager>() != null)
        {
            FindAnyObjectByType<Gura_ObjectManager>().ChangeSceneData_To_GamaManager();
        }
        if (FindAnyObjectByType<Calli_ObjectManager>() != null)
        {
            FindAnyObjectByType<Calli_ObjectManager>().ChangeSceneData_To_GameManager();
        }
        if (FindAnyObjectByType<Kiara_ObjectManager>() != null)
        {
            FindAnyObjectByType<Kiara_ObjectManager>().ChangeSceneData_To_GamaManager();
        }
        GameManager.instance.SaveData();   
        ItemManager._instance.SaveInventory();  
        LoadingSceneManager.LoadScene("01. IntroScene");
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
    public void SetMouseSensitivity(float setsitivity)
    {
        mouseSensitivity.value = setsitivity;
    }

    void SettingSound()
    {
        if (GameManager.instance.nowSceneName != "01. IntroScene")
        {
            masterSilder.value = GameManager.instance.MasterVolume / 10;
            bgmSilder.value = GameManager.instance.BGMVolume / 10;
            sfxSilder.value = GameManager.instance.SFXVolume / 10;
            mouseSensitivity.value = GameManager.instance.Sensitivity / 10;
            if (mouseCamLook)
            {
                mouseCamLook.mouseSensitivity = 500f * mouseSensitivity.value;
            }
        }
    }

    public void SaveVolume() // 이거 씬넘어가는 부분에서 저장하자
    {
        GameManager.instance.MasterVolume = (masterSilder.value * 10);
        GameManager.instance.BGMVolume = (bgmSilder.value * 10);
        GameManager.instance.SFXVolume = (sfxSilder.value * 10);
        GameManager.instance.Sensitivity = (mouseSensitivity.value * 10);

    }


    public void SaveSensitivity()
    {
        mouseCamLook.mouseSensitivity = 500f * mouseSensitivity.value;
    }

}
