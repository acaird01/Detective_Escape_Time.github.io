using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_CasketAnimCtrl : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;   // Į���� ������Ʈ �Ŵ���(���� �׽�Ʈ�� ���� ȹ���� ����)
    private Interaction_Gimics interaction;            // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                         // �÷��̾�

    private Calli_DeadBeat deadBeat;                   // ������� �ذ� ������Ʈ
    private Animator CasketAnimator;                   // �簢���� ���� ���� Animator ������Ʈ
    private AudioSource casket_AudioSource;            // �簢�� ������ ������ ����� Audio Source ������Ʈ
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();        // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�

        casket_AudioSource = gameObject.GetComponent<AudioSource>();

        deadBeat = GameObject.FindAnyObjectByType<Calli_DeadBeat>();    // Calli_DeadBeat�� ã�ƿͼ� �Ҵ�
        CasketAnimator = this.gameObject.GetComponent<Animator>();  // �ش� ������Ʈ�� ���� animator�� ã�ƿͼ� �Ҵ�
        settingGimic = interaction.run_Gimic;

        StartCoroutine(WaitTouch());    // ��� �ڷ�ƾ ����

        Setting_SceneStart();           // ���� ���� �ڷ�ƾ ����
    }


    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            // �̹� �Ѳ��� ����� �����̹Ƿ� �������� �ִϸ��̼� ���
            StartCoroutine(opening());
        }
        else
        {
            CasketAnimator.Play("Idle");
            deadBeat.GetComponent<CapsuleCollider>().enabled = false;    // ��ȣ�ۿ��� �Ұ����ϰԲ� �ذ��� �ݶ��̴� ��Ȱ��ȭ
        }
    }

    IEnumerator WaitTouch()
    {
        while (player) // �ѹ��ϰ� �ǻ����� �̰Ż���, â�� ���� �ݴ°�ó�� �ݺ��ʿ��ϸ� �̰� �ְ� ����
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // ��� �����ض� ���⿡

            if (interaction.run_Gimic)
            {
                // �������� �ִϸ��̼� ���
                StartCoroutine(opening());
            }
            else
            {
                // �������� �ִϸ��̼� ���
                StartCoroutine(closing());
            }
        }
    }

    IEnumerator opening() // ��� �۵����� �� 
    {
        deadBeat.GetComponent<CapsuleCollider>().enabled = true;    // ��ȣ�ۿ��� �����ϰԲ� �ذ��� �ݶ��̴� Ȱ��ȭ

        // CasketAnimator.Play("CasketOpen");
        CasketAnimator.SetBool("isOpenCasket", true);

        if (casket_AudioSource.isPlaying)   // �̹� ��� ���̴� �Ҹ��� ���� ���
        { 
            casket_AudioSource.Stop();      // �� �Ѱ� �Ҹ� ����
        }
        casket_AudioSource.Play();      // �� �Ѱ� �Ҹ� ���

        yield return new WaitForSeconds(.5f);
    }

    IEnumerator closing() // ����� �ǵ��� ����
    {
        deadBeat.GetComponent<CapsuleCollider>().enabled = false;    // ��ȣ�ۿ��� �Ұ����ϰԲ� �ذ��� �ݶ��̴� ��Ȱ��ȭ

        // CasketAnimator.Play("CasketClose");
        CasketAnimator.SetBool("isOpenCasket", false);

        if (casket_AudioSource.isPlaying)   // �̹� ��� ���̴� �Ҹ��� ���� ���
        {
            casket_AudioSource.Stop();      // �� �Ѱ� �Ҹ� ����
        }
        casket_AudioSource.Play();      // �� �Ѱ� �Ҹ� ���

        yield return new WaitForSeconds(.5f);
    }
}
