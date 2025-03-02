using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Calli_CrystalPosChange : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;        // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;                 // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                              // �÷��̾�
    private TextController textController;                  // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private Calli_CrystalPosGimic calli_CrystalPosGimic;    // ���� ��ġ ��� ���� ��ũ��Ʈ
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private Vector3 crystalOrigin_Position;     // �ش� ������ �����ִ� ��ġ�� ������ ����
    private bool isCrystalSelect;               // ������ ���É���� �ƴ��� Ȯ���ϱ� ���� ����
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

    // �ʱ�ȭ �Լ�
    private void Init()
    {
        calli_CrystalPosGimic = GameObject.FindAnyObjectByType<Calli_CrystalPosGimic>();    // ���� ��ġ ��� ���� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�

        crystalOrigin_Position = this.transform.position;   // �ش� ������ ���� ��ġ ����(���� ��ġ)
        isCrystalSelect = false;    // ���� ������ ���õ��� �ʾҴٰ� ����

        StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ��� �ڷ�ƾ �Լ� ȣ��
    }

    // ��ȣ�ۿ�Ǳ� ������ ����� �ڷ�ƾ �Լ�
    private IEnumerator WaitTouch()
    {
        while (player) // �ѹ��ϰ� �ǻ����� �̰Ż���, â�� ���� �ݴ°�ó�� �ݺ��ʿ��ϸ� �̰� �ְ� ����
        {
            // this.GetComponentInChildren<ParticleSystem>().Play();

            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // �����ǰ��� ���⼭ ����ϴٰ�

            // ��� ������ ���� ���
            if (interaction.run_Gimic && !isCrystalSelect)
            {
                isCrystalSelect = true; // �ش� ������ ���õǾ��ٰ� ���� ����. �߰� �ݺ��� ����
                interaction.run_Gimic = false; // �ٽ� ��ȣ�ۿ��� �� �ֵ��� ���� ����

                calli_CrystalPosGimic.SetSelectCrystal(this.gameObject);    // ���� ���õ� ������ �������� ����
            }
            else
            {
                interaction.run_Gimic = false; // �ٽ� ��ȣ�ۿ��� �� �ֵ��� ���� ����
            }
        }
    }

    #region ���� �Ǵ� �̼��ý� ������ ��ġ�� �������ִ� �Լ�
    /// <summary>
    /// �ش� ������ ���õǾ��ٰ� ��¦ ���� �÷��� �Լ�
    /// </summary>
    public void CrystalPosUp()
    {
        // ���� ��ġ���� �ణ �������� �÷��� ���õǾ��ٰ� ǥ������
        this.transform.position += Vector3.up * 0.5f;
    }

    /// <summary>
    /// �ش� ������ �� �̻� ���õ��� �ʾ����Ƿ� �ٽ� ���� ��ġ�� �ǵ����� �Լ�
    /// </summary>
    public void CrystalReplace()
    {
        isCrystalSelect = false;
        // �̸� �����ص� ���� ��ġ�� �̵�
        this.transform.position = crystalOrigin_Position;
    }
    #endregion
}
