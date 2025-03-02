using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class OneStepDrawingOnOff : MonoBehaviour
{
    public GameObject NodePrefab; // �Ѻױ׸��� ������ ����ִ� ������
    Interaction_Gimics interaction;
    GameObject player;
    Gimic_One_StepDrawing stepDrawing;

    TextController textConteroller;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        stepDrawing = gameObject.GetComponentInChildren<Gimic_One_StepDrawing>();
        player = GameObject.Find("Player");
        textConteroller = player.GetComponentInChildren<TextController>();
        StartCoroutine(WaitTouch());
        NodePrefab.SetActive(false);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                textConteroller.SetActiveFalseText();
                NodePrefab.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            else
            {
                yield return new WaitUntil(() => interaction.run_Gimic == false);
            }
        }
    }
    public void CanvasClose() // x��ư�� ����
    {
        Time.timeScale = 1;
        NodePrefab.SetActive(false);
        stepDrawing.ResetData();
        Cursor.lockState = CursorLockMode.Locked;
        interaction.run_Gimic = false;
    }
}
