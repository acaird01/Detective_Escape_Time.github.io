using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiara_MemoUse : MonoBehaviour
{
    Image MemoBG;
    Text MemoText;

    string RndMomoText = "Order \n ----------- ";
    Kiara_Tray kiara_Tray;

    GameObject player;
    PlayerCtrl playerCtrl;
    TextController textController;

    private void Start()
    {
        kiara_Tray = GameObject.FindObjectOfType<Kiara_Tray>();
        MemoText = gameObject.GetComponentInChildren<Text>();
        MemoBG = gameObject.GetComponentInChildren<Image>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        playerCtrl = player.GetComponent<PlayerCtrl>();

        MemoBG.gameObject.SetActive(false);
        //SetTextOrder();
    }

    public void MemoUse() // 아이템매니저에서 이거 호출
    {
        playerCtrl.keystrokes = true;
        if (RndMomoText == "Order \n ----------- ")
        {
            SetTextOrder();
        }
        textController.SetActiveFalseText();
        MemoBG.gameObject.SetActive(true);
        MemoText.text = RndMomoText;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void CloseItem() // 끌때
    {
        Time.timeScale = 1;
        playerCtrl.keystrokes = false;
        MemoBG.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void SetTextOrder()
    {
        if (kiara_Tray.setItemPos[0].name == "Item_28_Kiara_Burger")
        {
            RndMomoText += "\n\nHambuger";
        }
        else if (kiara_Tray.setItemPos[0].name == "Item_29_Kiara_Potatoes")
        {
            RndMomoText += "\n\nPotatoes";
        }
        RndMomoText += "\n\nChicken";
        if (kiara_Tray.setItemPos[2].name == "Item_31_Kiara_Coffee")
        {
            RndMomoText += "\n\nCoffee";
        }
        else if (kiara_Tray.setItemPos[2].name == "Item_32_Kiara_Coke")  
        {
            RndMomoText += "\n\nCoke";
        }
    }
}
