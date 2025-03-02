using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_TapeSelectEnding2 : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private Calli_WhichSongPlayBook whichSongPlayBook;  // ���� ����ϴ� �뷡�� �������� Ȯ���ϴ� ��ũ��Ʈ
    private Calli_RadioPlay radioPlay;                  // ����� �뷡 ������ �Ѱ��� ��ũ��Ʈ
    [SerializeField]
    private GameObject selectTape;                      // ���õǾ����� ��ġ�� �̵���ų ������ ������Ʈ
    public GameObject SelectTape                        // ���õǾ����� ��ġ�� �̵���ų �������� �ܺο��� Ȯ���� ������Ƽ
    {
        get
        { 
            return selectTape;
        }
    }
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    // private string playSongName = "";          // ���� ������� �뷡 ����
    private string interactionText;            // ��ȣ�ۿ� �� ����� ���
    private Vector3 tapeOrigin_Position;     // �ش� �������� �����ִ� ��ġ�� ������ ����
    private bool isTapeSelect;               // �������� ���É���� �ƴ��� Ȯ���ϱ� ���� ����
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        player = GameObject.Find("Player");
        whichSongPlayBook = GameObject.FindAnyObjectByType<Calli_WhichSongPlayBook>(); // ����� �뷡�� ���� å�� ã�ƿͼ� �Ҵ�

        whichSongPlayBook.SetInitTapeHole(Init);    // �뷡���� å���� �ʱ�ȭ �����ϱ� ���� Action�� �Լ� �Ҵ�
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// �ʱ�ȭ �Լ� (Calli_WhichSongPlayBook���� ����)
    /// </summary>
    /// <param name="_settingGimic"></param>
    private void Init(bool _settingGimic)
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();               // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�
        radioPlay = GameObject.FindAnyObjectByType<Calli_RadioPlay>();                  // �뷡�� ����� ������ ã�ƿͼ� �Ҵ�

        settingGimic = _settingGimic;                  // �뷡 ���� å�� ��� ���� �Ϸ� ���ο� ���� �ʱ�ȭ
        isTapeSelect = false;   // ���õ��� �ʾҴٰ� ����

        Setting_SceneStart();       // ���̺� �����Ϳ� ���� ��� ����
    }

    // ���̺� �����Ϳ� ���� �������� ��� ����
    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            this.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }


    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);

            if (!isTapeSelect)   // �ش� �������� ���� ���õ��� �ʾ������� ����
            {
                // ���� ��ġó�� ����� ���ÿ��� Ȯ�ΰ���
                // ������ ��ȣ�ۿ�� �ش� �������� �뷡 ���
                // �������� �ٽ� ��������.

                isTapeSelect = true; // �ش� ������ ���õǾ��ٰ� ���� ����. �߰� �ݺ��� ����

                radioPlay.SetSelectTape(this.gameObject);    // ���� ���õ� �������� �ִ� ĭ�� ���� ������ ������ ĭ���� ����
            }

            interaction.run_Gimic = false;
        }
    }

    #region ����� �����ϴ� �Լ�
    /// <summary>
    /// �ش� ĭ�� �ִ� ������ �������� ��ġ�� ��ġ�� �Բ� �����ص� �Լ�
    /// </summary>
    /// <param name="_selectTape"></param>
    public void SetTapeSelectCollider(GameObject _selectTape)
    {
        selectTape = _selectTape;   // �ش� ĭ�� ��ȣ�ۿ������� ������ �������� �Ҵ�
        tapeOrigin_Position = selectTape.transform.position;    // �������� ���� ��ġ�� �⺻ ��ġ�� ����

        this.GetComponent<BoxCollider>().enabled = true;        // �ش� ĭ�� �ݶ��̴��� �ٽ� Ȱ��ȭ�ؼ� �������� ������ �� �ְ� �ٲ�

        StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ��� �ڷ�ƾ ȣ��
    }

    #region ���� �Ǵ� �̼��ý� �������� ��ġ�� �������ִ� �Լ�
    /// <summary>
    /// �ش� ������ ���õǾ��ٰ� ��¦ ���� �÷��� �Լ�
    /// </summary>
    public void TapePosUp()
    {
        // ���� ��ġ���� �ణ �������� �÷��� ���õǾ��ٰ� ǥ������
        selectTape.transform.position += Vector3.up * 0.5f;

        this.GetComponent<BoxCollider>().enabled = false;        // �ش� ĭ�� �ݶ��̴��� ��Ȱ��ȭ�ؼ� �������� ������ �� ���� �ٲ�
    }

    /// <summary>
    /// �ش� ������ �� �̻� ���õ��� �ʾ����Ƿ� �ٽ� ���� ��ġ�� �ǵ����� �Լ�
    /// </summary>
    public void TapeReplace()
    {
        isTapeSelect = false;

        this.GetComponent<BoxCollider>().enabled = true;        // �ش� ĭ�� �ݶ��̴��� �ٽ� Ȱ��ȭ�ؼ� �������� ������ �� �ְ� �ٲ�
        // selectTape.gameObject.SetActive(true);

        // �̸� �����ص� ���� ��ġ�� �̵�
        selectTape.transform.position = tapeOrigin_Position;
    }
    #endregion
    #endregion
}
