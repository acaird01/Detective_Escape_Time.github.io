using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiara_BinGimic : MonoBehaviour
{
    Image Bin_Memo; // �������� �޸� �̹���
    bool image_SetActive; // �̹��� �����ִ��� üũ�� �Ұ�

    GameObject player;
    TextController textController;
    PlayerCtrl playerCtrl;
    Interaction_Gimics interaction;
    
    float timer = 0;
    float max_Timer = 1.5f;

    string FirstInteraction_Text = "���� ������ ���� �����ֳ�.\n�̰� ��ġ ü������ ���ĺ��� ���ڰ�����..";
    string SecondInteraction_Text = "��..�߸𸣰ڳ�.\n��? ���� ��Ų �뿡 ���� �ִ� �� ����.";

    // Start is called before the first frame update
    void Start()
    {
        Bin_Memo = gameObject.GetComponentInChildren<Image>();
        Bin_Memo.gameObject.SetActive(false);
        image_SetActive = false;
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        interaction.run_Gimic = false; // ������ ����x �ܼ��� ��ȣ�ۿ��� ����
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
            StartCoroutine(textController.SendText(SecondInteraction_Text)); // ù ��ȣ�ۿ�ÿ��� �ؽ�Ʈ ���

            playerCtrl.keystrokes = true;
            timer = 0f;
            if (timer >= max_Timer)
            {
                textController.SetActiveFalseText();
                Time.timeScale = 0;
                Bin_Memo.gameObject.SetActive(true); // ��ȣ�ۿ� �� ��ġ���� �޸� ����ֱ�
                image_SetActive = true;
                Cursor.lockState = CursorLockMode.None;
            }
                
        }
    }*/

    IEnumerator SecondInteraction()
    {
        if (!image_SetActive)
        {
            StartCoroutine(textController.SendText(SecondInteraction_Text)); // ù ��ȣ�ۿ�ÿ��� �ؽ�Ʈ ���

            playerCtrl.keystrokes = true;
            timer = 0f;
            yield return new WaitUntil(() => timer >= max_Timer);
            textController.SetActiveFalseText();
            Time.timeScale = 0;
            Bin_Memo.gameObject.SetActive(true); // ��ȣ�ۿ� �� ��ġ���� �޸� ����ֱ�
            image_SetActive = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }

    /*void FirstInteraction()
    {
        if (!image_SetActive)
        {
            StartCoroutine(textController.SendText(FirstInteraction_Text)); // ù ��ȣ�ۿ�ÿ��� �ؽ�Ʈ ���

            playerCtrl.keystrokes = true;
            timer = 0f;
            if(timer >= max_Timer)
            {
                textController.SetActiveFalseText();
                Time.timeScale = 0;
                Bin_Memo.gameObject.SetActive(true); // ��ȣ�ۿ� �� ��ġ���� �޸� ����ֱ�
                image_SetActive = true;
                Cursor.lockState = CursorLockMode.None;
            }
            
        }
    }*/

    IEnumerator FirstInteraction()
    {
        if (!image_SetActive)
        {
            StartCoroutine(textController.SendText(FirstInteraction_Text)); // ù ��ȣ�ۿ�ÿ��� �ؽ�Ʈ ���

            playerCtrl.keystrokes = true;
            timer = 0f;

            yield return new WaitUntil(() => timer >= max_Timer);

            textController.SetActiveFalseText();
            Time.timeScale = 0;
            Bin_Memo.gameObject.SetActive(true); // ��ȣ�ۿ� �� ��ġ���� �޸� ����ֱ�
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

    public void CanvasClose() // x��ư�� ����
    {
        Time.timeScale = 1;
        Bin_Memo.gameObject.SetActive(false);
        image_SetActive = false;
        playerCtrl.keystrokes = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
