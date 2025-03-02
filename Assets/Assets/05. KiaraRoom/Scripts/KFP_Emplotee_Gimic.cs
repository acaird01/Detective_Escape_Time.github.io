using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KFP_Emplotee_Gimic : MonoBehaviour
{
    GameObject player;
    bool GimicMove = false; // true�� �� ���� �Ȱ�

    public GameObject kiara_Feather_Prefab; // ���� ������

    TextController textController;
    Interaction_Gimics interaction;
    BoxCollider boxCollider;
    public GameObject FireEffect;

    string interaction_Text = "�����ּż� �����մϴ�.\n�̰��� �帱����."; // ��ȣ�ۿ� �� ����� �ؽ�Ʈ

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        kiara_Feather_Prefab.gameObject.SetActive(false);
        FireEffect.gameObject.SetActive(false);
        boxCollider = gameObject.GetComponent<BoxCollider>();
        //gameObject.GetComponent<Interaction_Gimics>().run_Gimic = false; // �̰� ���߿� �ݵ�� ���� �� �׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        GimicMove = interaction.run_Gimic;
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        SceneStartSetting_KiaraPosBox();
        StartCoroutine(WaitTouch());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        GimicMove = interaction.run_Gimic;

    }
    IEnumerator WaitTouch()
    {
        if (GimicMove == false)
        {
            yield return new WaitUntil(() => GimicMove == true);
            boxCollider.enabled = false;
            StartCoroutine(textController.SendText(interaction_Text));

            FireEffect.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            kiara_Feather_Prefab.gameObject.SetActive(true);
            interaction.run_Gimic = true;
            GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
            yield return new WaitForSeconds(0.5f);
            FireEffect.gameObject.SetActive(false);
            interaction.enabled = false;
        }
    }
    public void SceneStartSetting_KiaraPosBox()
    {
        if (GimicMove)
        {
            interaction.enabled = false;
        }
        else
        {
            interaction.enabled = true;
        }
    }
}
