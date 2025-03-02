using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiara_PaperHint : MonoBehaviour
{
    Image Bin_Memo; // 퀴즈적힌 메모 이미지
    bool image_SetActive; // 이미지 켜져있는지 체크할 불값

    GameObject player;

    Interaction_Gimics interaction;


    TextController textController;
    PlayerCtrl playerCtrl;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Bin_Memo = gameObject.GetComponentInChildren<Image>();
        Bin_Memo.gameObject.SetActive(false);
        image_SetActive = false;
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        interaction.run_Gimic = false; // 데이터 저장x 단순히 상호작용을 위해

        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        playerCtrl = player.GetComponent<PlayerCtrl>();
        textController = player.GetComponentInChildren<TextController>();
        StartCoroutine(WaitTouch());
    }

    IEnumerator WaitTouch() // 나무가 다 자란 상태에서 상호작용 할 예정 자라기전엔 콜라이더 끔으로써 상호작용 x
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (!image_SetActive)
                {
                    textController.SetActiveFalseText();
                    Bin_Memo.gameObject.SetActive(true); // 상호작용 시 위치적힌 메모 띄워주기
                    image_SetActive = true;
                    playerCtrl.keystrokes = true;
                    audioSource.Play();
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0;
                }
            }
            else
            {
                interaction.run_Gimic = false;
            }
        }
    }

    public void CanvasClose() // x버튼에 들어갈꺼
    {
        Time.timeScale = 1;
        interaction.run_Gimic = false;
        playerCtrl.keystrokes = false;  
        Bin_Memo.gameObject.SetActive(false);
        image_SetActive = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
