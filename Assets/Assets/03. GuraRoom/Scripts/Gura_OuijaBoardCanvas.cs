using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gura_OuijaBoardCanvas : MonoBehaviour
{
    GameObject player;

    [SerializeField]
    Button[] OuijaBoardButtons;
    [SerializeField]
    GameObject OuijaBoard;
    [SerializeField]
    GameObject effectLight;

    [SerializeField]
    GameObject OuijaHint;

    public bool isAnswer = false;

    public bool isOuijaCleared = false;

    public bool isKroniiSwordUp = false;


    private void Start()
    {
        player = GameObject.Find("Player");
    }


    public void WrongAnswer()   //틀린 버튼 누를떄
    {
        isAnswer = false;

        foreach (Button button in OuijaBoardButtons)    //빼고 다 투명하게
        {
            button.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }

        //set this button's image color's alpha to 255
        Button thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        thisButton.GetComponent<Image>().color = new Color(255, 255, 255, 255);
  
    }

    public void RightAnswer()   //맞는 버튼 누를때
    {
        isAnswer = true;

        foreach (Button button in OuijaBoardButtons)    //빼고 다 투명하게
        {
            button.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }

        Button thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        thisButton.GetComponent<Image>().color = new Color(255, 255, 255, 255);

    }


    public void closeOuijaCanvas()  //캔버스 닫을때 정답 확인 + 정답시 이벤트 발생
    {

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponentInChildren<PlayerCtrl>().keystrokes = false;

        if (isAnswer)
        {
            OuijaBoard.GetComponent<Collider>().enabled = false;

            isOuijaCleared = true;

            //find OuijaBoard's child object with Animator component and play animation
            OuijaBoard.GetComponentInChildren<Animator>().Play("OuijaMove");

            OuijaBoard.GetComponent<AudioSource>().Play();

            isKroniiSwordUp = true;

            OuijaBoard.GetComponent<Interaction_Gimics>().run_Gimic = true;

            HintOff();

        }

        gameObject.SetActive(false);
    }



    public void HintOff()
    { 
        OuijaHint.SetActive(false);
    }

}
