using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiara_PaperHint : MonoBehaviour
{
    Image Bin_Memo; // �������� �޸� �̹���
    bool image_SetActive; // �̹��� �����ִ��� üũ�� �Ұ�

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
        interaction.run_Gimic = false; // ������ ����x �ܼ��� ��ȣ�ۿ��� ����

        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        playerCtrl = player.GetComponent<PlayerCtrl>();
        textController = player.GetComponentInChildren<TextController>();
        StartCoroutine(WaitTouch());
    }

    IEnumerator WaitTouch() // ������ �� �ڶ� ���¿��� ��ȣ�ۿ� �� ���� �ڶ������ �ݶ��̴� �����ν� ��ȣ�ۿ� x
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (!image_SetActive)
                {
                    textController.SetActiveFalseText();
                    Bin_Memo.gameObject.SetActive(true); // ��ȣ�ۿ� �� ��ġ���� �޸� ����ֱ�
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

    public void CanvasClose() // x��ư�� ����
    {
        Time.timeScale = 1;
        interaction.run_Gimic = false;
        playerCtrl.keystrokes = false;  
        Bin_Memo.gameObject.SetActive(false);
        image_SetActive = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
