using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_CrystalHintLight : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    [Header("������ ��ġ ������ ������ �� ������Ʈ(��Ʈ ������� �߰�)")]
    [SerializeField]
    private GameObject[] crystalHintLights;                  // ������ ��ġ ������ ������ ���� ������ �迭
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool isHintPlay;    // ���� ��Ʈ�� ���� ������ Ȯ���ϱ� ���� ����
    WaitForSeconds waitOneSeconds = new WaitForSeconds(1f); // 1�ʰ� ��� ��ų�� ����� ����
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");                                             // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();               // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�

        Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    private void Init()
    {
        isHintPlay = false;   // ���� ��Ʈ�� ���� ������ Ȯ���ϱ� ���� ����

        // ��Ʈ ���� ���̴� ���� ����
        for (int i = 0; i < crystalHintLights.Length; i++)
        {
            crystalHintLights[i].SetActive(false);
        }

        StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ����Լ� ȣ��
    }

    // ��ȣ�ۿ�Ǳ� ������ ����� �ڷ�ƾ �Լ�
    private IEnumerator WaitTouch()
    {
        while (player) // �ѹ��ϰ� �ǻ����� �̰Ż���, â�� ���� �ݴ°�ó�� �ݺ��ʿ��ϸ� �̰� �ְ� ����
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // �����ǰ��� ���⼭ ����ϴٰ�

            // ��� ������ ���� ���
            if (interaction.run_Gimic && !isHintPlay)
            {
                isHintPlay = true;    // ��Ʈ ����� �����ߴٰ� ����(�߰� �ݺ��� ����)

                // ���� ��ġ ��Ʈ ���� ���ְ� ȿ���� ���
                for (int i = 0; i < crystalHintLights.Length; i++)
                {
                    crystalHintLights[i].SetActive(true);    // �� Ȱ��ȭ
                    crystalHintLights[i].GetComponent<AudioSource>().Play();    // ȿ���� ���

                    yield return waitOneSeconds;    // 1�ʰ� ���

                    crystalHintLights[i].SetActive(false);    // �� ��Ȱ��ȭ
                }

                isHintPlay = false;    // ��Ʈ ����� �Ϸ�Ǿ����Ƿ� ��������
                interaction.run_Gimic = false;
            }
            else
            {
                interaction.run_Gimic = false; // �ٽ� ��ȣ�ۿ��� �� �ֵ��� ���� ����
            }
        }
    }
}
