using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_AmeOnChair : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private AudioSource chair_SFX;                      // �Ƹް� ���� ���� �ö����� ȿ������ ����� AudioSource
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    // private bool isAmeOnChair;       // �Ƹް� ���� ���� �ö󰬴��� Ȯ���ϱ� ���� bool ����
    private float chairSoundLength;  // ���ڿ� ���� �� ���� �Ҹ� ����
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");                                             // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();               // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�

        Init(); // �ʱ�ȭ �Լ� ȣ��
    }

    // �ʱ�ȭ �Լ� ȣ��
    private void Init()
    {
        chair_SFX = GetComponent<AudioSource>();    // ������ �ִ� AudioSource�� �Ҵ�

        // isAmeOnChair = false;     // ���� �Ⱥ��̴� ���·� ����
        // chairSoundLength = (chair_SFX.clip.length / 3) * 2; // ���ɼҸ��� 2/3���̷� ����

        StartCoroutine(WaitTouch());                    // ��ȣ�ۿ� ��� �ڷ�ƾ �Լ� ����
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ��ȣ�ۿ�Ǳ� ������ ����� �ڷ�ƾ �Լ�
    private IEnumerator WaitTouch()
    {
        while (player) // �ѹ��ϰ� �ǻ����� �̰Ż���, â�� ���� �ݴ°�ó�� �ݺ��ʿ��ϸ� �̰� �ְ� ����
        {
            // this.GetComponentInChildren<ParticleSystem>().Play();

            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // �����ǰ��� ���⼭ ����ϴٰ�

            AmeSitChair();

            interaction.run_Gimic = false;
        }
    }

    private void AmeSitChair()
    {
        // ���ڿ� �ö󼭴� ���� ���
        player.transform.position = this.gameObject.transform.position + (Vector3.up * 1f);   // �Ƹ��� ��ġ�� ���� �������� �̵�
        player.transform.rotation = Quaternion.Euler(0f, -90f, 0f);     // ��Ʈ�� �ִ� å���� �Ĵٺ��Բ� ���� ����
    }
}
