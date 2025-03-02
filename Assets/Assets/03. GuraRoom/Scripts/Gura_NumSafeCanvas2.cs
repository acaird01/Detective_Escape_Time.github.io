using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gura_NumSafeCanvas2 : MonoBehaviour
{
    [SerializeField]
    private GameObject NumSafe;
    GameObject player;

    public int CanvasAnswerNum;

    private int FirstNum;
    private int SecondNum;
    private int ThirdNum;

    public Text FirstNumTxt;
    public Text SecondNumTxt;
    public Text ThirdNumTxt;

    [SerializeField]
    private GameObject takoBaelz;

    [SerializeField]
    GameObject lightEffect;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        player = GameObject.Find("Player");

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void BaelzUp()
    {
        takoBaelz.GetComponent<Animator>().Play("Baelz");
    }



    #region NumSafeCanvas 버튼관련

    public void closeSafeCanvas()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        player = GameObject.Find("Player");
        {
            player.GetComponent<PlayerCtrl>().keystrokes = false;
        }
    }

    public void NumPlus1()
    { 
        GetComponent<AudioSource>().Play();

        FirstNum++;
        if (FirstNum == 10)
        {
            FirstNum = 0;
        }
        FirstNumTxt.text = FirstNum.ToString();
    }

    public void NumMinus1()
    {
        GetComponent<AudioSource>().Play();

        FirstNum--;
        if (FirstNum == -1)
        {
            FirstNum = 9;
        }
        FirstNumTxt.text = FirstNum.ToString();
    }

    public void NumPlus2()
    {
        GetComponent<AudioSource>().Play();

        SecondNum++;
        if (SecondNum == 10)
        {
            SecondNum = 0;
        }
        SecondNumTxt.text = SecondNum.ToString();
    }

    public void NumMinus2()
    {
        GetComponent<AudioSource>().Play();

        SecondNum--;
        if (SecondNum == -1)
        {
            SecondNum = 9;
        }
        SecondNumTxt.text = SecondNum.ToString();
    }

    public void NumPlus3()
    {
        GetComponent<AudioSource>().Play();

        ThirdNum++;
        if (ThirdNum == 10)
        {
            ThirdNum = 0;
        }
        ThirdNumTxt.text = ThirdNum.ToString();
    }

    public void NumMinus3()
    {
        GetComponent<AudioSource>().Play();

        ThirdNum--;
        if (ThirdNum == -1)
        {
            ThirdNum = 9;
        }
        ThirdNumTxt.text = ThirdNum.ToString();
    }

    private void Update()
    {
        CanvasAnswerNum = FirstNum * 100 + SecondNum * 10 + ThirdNum;
    }



    #endregion

    
}
