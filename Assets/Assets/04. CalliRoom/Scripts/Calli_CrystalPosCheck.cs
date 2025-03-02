using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_CrystalPosCheck : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private Calli_CrystalPosGimic calli_CrystalPosGimic;    // ���� ��ġ ��� ���� ��ũ��Ʈ
    private GameObject crystalOnFrame;                  // �ش� ���ڿ� ��ġ�� ũ����Ż�� ������ ������ ����
    #endregion

    #region �� ���ڿ��� ����� Action
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    public bool isFramePositionSelect;    // ���� ����ĭ�� ���õǾ����� Ȯ���ϱ� ���� ����
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
        calli_CrystalPosGimic = GameObject.FindAnyObjectByType<Calli_CrystalPosGimic>();    // ���� ��ġ ��� ���� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�

        settingGimic = interaction.run_Gimic;   // ����� �̹� ��������� �ƴ��� Ȯ���ϴ� ����
        crystalOnFrame = null;   // ���� �ش� ���ڿ� ��ġ�� ������ ���ٰ� ���� ����
        isFramePositionSelect = false;   // �ش� ���� ĭ�� ���� ���õ��� �ʾҴٰ� ����

        StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ����Լ� ȣ��
    }

    // ��ȣ�ۿ�Ǳ� ������ ����� �ڷ�ƾ �Լ�
    private IEnumerator WaitTouch()
    {
        while (player) // �ѹ��ϰ� �ǻ����� �̰Ż���, â�� ���� �ݴ°�ó�� �ݺ��ʿ��ϸ� �̰� �ְ� ����
        {
            // this.GetComponentInChildren<ParticleSystem>().Play();

            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // �����ǰ��� ���⼭ ����ϴٰ�

            // ��� ������ ���� ���
            if (interaction.run_Gimic && !isFramePositionSelect)
            {
                // ���� �������� ������ �������� ����
                if (calli_CrystalPosGimic.selectCrystal != null)
                {
                    isFramePositionSelect = true; // �ش� ������ ���õǾ��ٰ� ���� ����. �߰� �ݺ��� ����
                    interaction.run_Gimic = false; // �ٽ� ��ȣ�ۿ��� �� �ֵ��� ���� ����
                                                   // �̹� �ִ밹������ ���� ���� �������� ��ȯ�Ǿ���, �ش� ��Ű�� �������� ������ĭ ���� ���� ��쿡�� ��ȯ�� ����
                    if (!calli_CrystalPosGimic.isCrystalReachMaxNum && (!calli_CrystalPosGimic.GetAnswerCrystalFramePos(calli_CrystalPosGimic.selectCrystal.name)))
                    {
                        MoveCrystalOnFrame(calli_CrystalPosGimic.selectCrystal);   // ���� ���� ��ġ�� ������ �̵���Ű�� ���� �Լ� ȣ��
                    }
                    else
                    {
                        isFramePositionSelect = false;    // ��ȯ���� �ʾ����� ���� ����
                        interaction.run_Gimic = false;
                    }
                }
            }
            else
            {
                interaction.run_Gimic = false; // �ٽ� ��ȣ�ۿ��� �� �ֵ��� ���� ����
            }
        }
    }


    // ���� ���� ������ �̵���Ű�� ���� �Լ�
    private void MoveCrystalOnFrame(GameObject _crystal)
    {
        crystalOnFrame = _crystal;
        crystalOnFrame.GetComponent<Transform>().position = this.transform.position + (Vector3.up * -0.2f);   // �ش� ���� ĭ���� �ش� ������ ��¦ ���ְԲ� �̵� ��Ŵ
        //tapeOnTableHole.transform.localScale = new Vector3(35f, 2.25f, 15.3f);                    // �����Ǵ� �������� ũ�� ����
        //tapeOnTableHole.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        calli_CrystalPosGimic.SetAnswerCrystal(crystalOnFrame.name, this.name);   // ���ڿ� �������Ƿ� �迭�� �����ϱ����� �Լ� ȣ��

        // �߰� ��ȣ�ۿ��� ���� �ٽ� ��ȣ�ۿ� ���� �ʵ��� collider ��Ȱ��ȭ
        crystalOnFrame.GetComponent<Collider>().enabled = false;
        this.GetComponent<Collider>().enabled = false;

        calli_CrystalPosGimic.SetReplaceWrongPlaceCrystals_CallBack(ReplaceWrongPlaceCrystal);    // �߸��� ��ġ�� ���� ��� ������ �Լ� Action�� �־���
    }

    // ���ڿ��� �߸��� ��ġ�� �������� ������ ��� ������ �Լ�
    private void ReplaceWrongPlaceCrystal()
    {
        crystalOnFrame.GetComponent<Calli_CrystalPosChange>().CrystalReplace(); // ���ڸ��� ���ư��� �Լ� ����
        interaction.run_Gimic = false;  // �ش� ����� ���� ������� ���� ���·� ����
        isFramePositionSelect = false;     // ��������Ƿ� false�� ����

        // �ٽ� ��ȣ�ۿ� �����ϵ��� �ݶ��̴� Ȱ��ȭ
        crystalOnFrame.GetComponent<Collider>().enabled = true;
        this.GetComponent<Collider>().enabled = true;
    }
}
