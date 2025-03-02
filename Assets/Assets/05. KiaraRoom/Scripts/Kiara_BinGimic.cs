using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiara_BinGimic : MonoBehaviour
{
    Image Bin_Memo; // 퀴즈적힌 메모 이미지
    bool image_SetActive; // 이미지 켜져있는지 체크할 불값

    GameObject player;
    TextController textController;
    PlayerCtrl playerCtrl;
    Interaction_Gimics interaction;
    
    float timer = 0;
    float max_Timer = 1.5f;

    string FirstInteraction_Text = "여기 쪽지에 무언가 적혀있네.\n이건 마치 체스판의 알파벳과 숫자같은데..";
    string SecondInteraction_Text = "흠..잘모르겠네.\n응? 왼쪽 냅킨 통에 뭔가 있는 것 같아.";

    // Start is called before the first frame update
    void Start()
    {
        Bin_Memo = gameObject.GetComponentInChildren<Image>();
        Bin_Memo.gameObject.SetActive(false);
        image_SetActive = false;
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        interaction.run_Gimic = false; // 데이터 저장x 단순히 상호작용을 위해
        if (GameManager.instance.Episode_Round == 1)
        {
            interaction.hintChild_OutLine = new cakeslice.Outline[1];
            interaction.hintChild_OutLine[0] = GameObject.FindAnyObjectByType<Kiara_NapkinHolder>().GetComponent<cakeslice.Outline>();
        }
        player = GameObject.Find("Player");
        playerCtrl = player.GetComponent<PlayerCtrl>();
        textController = player.GetComponentInChildren<TextController>();

        StartCoroutine(WaitTouch());
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    /*void SecondInteraction()
    {
        if (!image_SetActive)
        {
            StartCoroutine(textController.SendText(SecondInteraction_Text)); // 첫 상호작용시에만 텍스트 출력

            playerCtrl.keystrokes = true;
            timer = 0f;
            if (timer >= max_Timer)
            {
                textController.SetActiveFalseText();
                Time.timeScale = 0;
                Bin_Memo.gameObject.SetActive(true); // 상호작용 시 위치적힌 메모 띄워주기
                image_SetActive = true;
                Cursor.lockState = CursorLockMode.None;
            }
                
        }
    }*/

    IEnumerator SecondInteraction()
    {
        if (!image_SetActive)
        {
            StartCoroutine(textController.SendText(SecondInteraction_Text)); // 첫 상호작용시에만 텍스트 출력

            playerCtrl.keystrokes = true;
            timer = 0f;
            yield return new WaitUntil(() => timer >= max_Timer);
            textController.SetActiveFalseText();
            Time.timeScale = 0;
            Bin_Memo.gameObject.SetActive(true); // 상호작용 시 위치적힌 메모 띄워주기
            image_SetActive = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }

    /*void FirstInteraction()
    {
        if (!image_SetActive)
        {
            StartCoroutine(textController.SendText(FirstInteraction_Text)); // 첫 상호작용시에만 텍스트 출력

            playerCtrl.keystrokes = true;
            timer = 0f;
            if(timer >= max_Timer)
            {
                textController.SetActiveFalseText();
                Time.timeScale = 0;
                Bin_Memo.gameObject.SetActive(true); // 상호작용 시 위치적힌 메모 띄워주기
                image_SetActive = true;
                Cursor.lockState = CursorLockMode.None;
            }
            
        }
    }*/

    IEnumerator FirstInteraction()
    {
        if (!image_SetActive)
        {
            StartCoroutine(textController.SendText(FirstInteraction_Text)); // 첫 상호작용시에만 텍스트 출력

            playerCtrl.keystrokes = true;
            timer = 0f;

            yield return new WaitUntil(() => timer >= max_Timer);

            textController.SetActiveFalseText();
            Time.timeScale = 0;
            Bin_Memo.gameObject.SetActive(true); // 상호작용 시 위치적힌 메모 띄워주기
            image_SetActive = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    IEnumerator WaitTouch() 
    {
        if (GameManager.instance.Episode_Round == 1)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                StartCoroutine(FirstInteraction());
                //FirstInteraction();
            }
            while (player)
            {
                if (interaction.run_Gimic == false)
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == true);
                    //SecondInteraction();
                    StartCoroutine(SecondInteraction());
                }
                else
                {
                    interaction.run_Gimic = false;
                }
            }
        }
    }

    public void CanvasClose() // x버튼에 들어갈꺼
    {
        Time.timeScale = 1;
        Bin_Memo.gameObject.SetActive(false);
        image_SetActive = false;
        playerCtrl.keystrokes = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
