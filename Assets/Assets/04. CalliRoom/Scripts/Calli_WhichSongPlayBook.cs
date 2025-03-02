using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Calli_WhichSongPlayBook : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private GameObject skullHead;                       //  ����� Ǯ������ ��ȯ���� �ΰ��� GameObject
    [Header("�������� �����ϱ� ���� ���̺� ĭ��")]
    [SerializeField]
    private Calli_TapeSelectEnding2[] tapeSelectCheck;
    [Header("�������� ���� �����Ǿ� �ִ� ���̺� ĭ��")]
    [SerializeField]
    private GameObject[] tapeSummonHoles;
    [Header("������ ������ �̹��� ���׸��� �迭")]
    [SerializeField]
    private Material[] albumImages;                     // (0 : MERA MERA / 1 : You're Not Special / 2 : The Grim Reaper is a Live-Streamer / 3 : HUGE W)
    [Header("������ ������ �뷡���� ���׸��� �迭")]
    [SerializeField]
    private Material[] songNames;                       // (0 : MERA MERA / 1 : You're Not Special / 2 : The Grim Reaper is a Live-Streamer / 3 : HUGE W)
    private MeshRenderer albumImage_Quad;               // �ٹ� ������ ������ �ǳ�
    private MeshRenderer songName_Quad;                 // �뷡 ������ ������ �ǳ�
    private AudioSource audioSource;                    // �ٹ������̳� �뷡 ������ �����ٶ� ȿ������ ������� audio source
    #endregion

    #region 2ȸ�� �� �� ������ ĭ���� ����� Action
    private Action<bool> InitTapeHoles;      // �� ������ ĭ�� �ʱ�ȭ�� �Լ��� ������ Aciton
    public void SetInitTapeHole(Action<bool> _InitTapeHoles)     // InitTapeHoles�� �Լ��� ������ ĭ���� �Ҵ��� set �Լ�
    {
        InitTapeHoles += _InitTapeHoles;
    }
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private string correctSongName = "";        // ���� �뷡 ����
    private string[] allSongNames;              // ��� �뷡 ����

    private string interactionText;        // ��ȣ�ۿ� �� ����� ���

    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private bool isRightSongPlay;               // ���� �뷡�� ������ �ִ��� Ȯ���ϱ� ���� ����
    private bool isTapeGimicEnd;                // ������ ����� �������� Ȯ���ϱ� ���� ����
    /// <summary>
    /// ������ ��ġ ����� ����Ǿ����� �뷡 ��� å���� �������ֱ� ���� ����
    /// </summary>
    public bool IsTapeGimicEnd
    {
        set
        {
            isTapeGimicEnd = value;
        }
    }

    #endregion

    /// <summary>
    /// �����ϸ� �ΰ����� Ȱ��ȭ ������
    /// </summary>

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        player = GameObject.Find("Player");                                             // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = this.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();               // �÷��̾��� �ڽĿ��� TextController�� ã�ƿͼ� �Ҵ�
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// �ʱ�ȭ �Լ�(Calli_StoneTableGimic���� ȣ��)
    /// </summary>
    /// <param name="_settingGimic"></param>
    public void Init(bool _settingGimic)
    {
        skullHead = GameObject.FindAnyObjectByType<Item20Skull>().gameObject;       // �ΰ��� �������� ã�ƿͼ� �Ҵ�
        albumImage_Quad = GetComponentInChildren<Calli_AlbumImage>().GetComponent<MeshRenderer>();   // �ٹ� �׸��� ������ �Ƕ����� meshrenderer�� ã�ƿͼ� �Ҵ�
        songName_Quad = GetComponentInChildren<Calli_SongName>().GetComponent<MeshRenderer>();       // �뷡 ������ ������ �Ƕ����� meshrenderer�� ã�ƿͼ� �Ҵ�
        audioSource = GetComponent<AudioSource>();          // ȿ������ ����� audio source�� �Ҵ�

        settingGimic = _settingGimic;   // ��� ���� ���� �ʱ�ȭ
        isRightSongPlay = false;    // ���� ����뷡�� ������ ���� �ʴٰ� ����

        allSongNames = new string[4];   // �뷡 ������ ������ �迭 ����

        // ��� �뷡 ������ �迭�� �־���
        allSongNames[0] = "MERA MERA";
        allSongNames[1] = "You're Not Special";
        allSongNames[2] = "The Grim Reaper is a Live-Streamer";
        allSongNames[3] = "HUGE W";

        // 0~3 ������ ������ ��ȣ�� �뷡�� �������� ����
        int correctNum = UnityEngine.Random.Range(0, 4);

        correctSongName = allSongNames[correctNum];             // ���� �뷡 ������ ����
        albumImage_Quad.material = albumImages[correctNum];     // ���� �뷡�� �´� �ٹ� �̹����� ����
        songName_Quad.material = songNames[correctNum];         // ���� �뷡�� �´� ���� ���� �̹����� ����

        albumImage_Quad.gameObject.SetActive(false);            // �ٹ��̹��� ��Ȱ��ȭ �ؼ� �Ⱥ��̰� ����
        songName_Quad.gameObject.SetActive(false);              // �뷡�����̹��� ��Ȱ��ȭ �ؼ� �Ⱥ��̰� ����

        Setting_SceneStart();       // ���̺� �����Ϳ� ���� ��� ����
    }

    // ���̺� �����Ϳ� ���� �������� ��� ����
    void Setting_SceneStart()
    {
        InitTapeHoles?.Invoke(settingGimic);   // ������ ���� ĭ�� �ʱ�ȭ �Լ��� ����

        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            // �ΰ����� ȹ���ߴ��� Ȯ��
            if (skullHead.GetComponent<Item20Skull>().isGetItem)
            {
                // �ΰ����� �̹� ȹ������ ��� ��Ȱ��ȭ ��Ŵ
                skullHead.gameObject.SetActive(false);
            }
            else
            {
                // �ΰ����� ���� ȹ������ ������ ��� Ȱ��ȭ ��Ŵ
                skullHead.gameObject.SetActive(true);
            }

            SetPlaySong(settingGimic);             // ����� �뷡�� ��������
        }
        else
        {
            // ����� ���� �������� �ʾ����� setgimic�� false
            skullHead.gameObject.SetActive(false);    // ���� ����� �������� �ʾ����Ƿ� ��Ȱ��ȭ
        }

        isTapeGimicEnd = settingGimic;  // ������ ����� �Ϸ�Ǿ����� Ȯ���ϱ� ���� ���� �ʱ�ȭ

        StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ��� �ڷ�ƾ ȣ��
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // ��� �����ض� ���⿡


            // ������ ��ġ ����� ������ ��� ����
            if (isTapeGimicEnd)
            {
                // ������ ��ġ ��ġ�Ϸ�����Ƿ�
                // �ٽ� ĭ�� �ٸ� ������Ʈ Ȱ��ȭ�ϰ� �� ��ũ��Ʈ Ȱ��ȭ ��Ŵ
                // ���� ��ġó�� ����� ���ÿ��� Ȯ�ΰ���
                // ������ ��ȣ�ۿ�� �ش� �������� �뷡 ���
                // �������� �ٽ� ��������
                interactionText = "���� �׷��� �ִ�. �� �뷡�� �� �� Ʋ���?";        // ��ȣ�ۿ� �� ����� ��� ����
                StartCoroutine(textController.SendText(interactionText));          // ��ȣ�ۿ� ��� ���

                interaction.run_Gimic = false;
            }
            else
            {
                interactionText = "���� �ƹ��͵� �������� ���� å�̴�.";        // ��ȣ�ۿ� �� ����� ��� ����
                StartCoroutine(textController.SendText(interactionText));   // ��ȣ�ۿ� ��� ���

                interaction.run_Gimic = false;
            }
        }
    }

    /// <summary>
    /// ī��Ʈ ���������� ����ؾߵ� �뷡�� �����ϰ� ������ ���� �ݶ��̴��� Ȱ��ȭ ��ų �Լ�
    /// </summary>
    /// <param name="_isTapeGimicEnd"></param>
    public void SetPlaySong(bool _isTapeGimicEnd)
    {
        // ���������� ����� ���� �Ϸ�Ǿ��ٰ� ��������
        isTapeGimicEnd = _isTapeGimicEnd;
        settingGimic = _isTapeGimicEnd;

        // å�� �ٹ� ������ �� ���� �����
        albumImage_Quad.gameObject.SetActive(true);            // �ٹ��̹��� ��Ȱ��ȭ �ؼ� �Ⱥ��̰� ����
        songName_Quad.gameObject.SetActive(true);              // �뷡�����̹��� ��Ȱ��ȭ �ؼ� �Ⱥ��̰� ����
        // å�� ���ڰ� ���̴� �Ҹ� ���
        audioSource.Play();

        // �� ������ ĭ�� �ݶ��̴� Ȱ��ȭ �Լ� ����
        if (tapeSelectCheck[0].gameObject != null)
        {
            tapeSelectCheck[0].SetTapeSelectCollider(tapeSummonHoles[0].GetComponentInChildren<Item23CassetteTapeT>().gameObject);
        }
        if (tapeSelectCheck[1].gameObject != null)
        {
            tapeSelectCheck[1].SetTapeSelectCollider(tapeSummonHoles[1].GetComponentInChildren<Item24CassetteTapeH>().gameObject);
        }
        if (tapeSelectCheck[2].gameObject != null)
        {
            tapeSelectCheck[2].SetTapeSelectCollider(tapeSummonHoles[2].GetComponentInChildren<Item21CassetteTapeM>().gameObject);
        }
        if (tapeSelectCheck[3].gameObject != null)
        {
            tapeSelectCheck[3].SetTapeSelectCollider(tapeSummonHoles[3].GetComponentInChildren<Item22CassetteTapeY>().gameObject);
        }
    }

    /// <summary>
    /// ����� ������ �뷡�� ����ϰ� �ִ��� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <param name="_answerSongName"></param>
    public void IsPlaySongCorrectCheck(string _answerSongName)
    {
        // ���� ������ ������ �ʾҰ� �뷡 ������ ���� ��� ����
        if (!isRightSongPlay && (string.Equals(correctSongName, _answerSongName)))
        {
            isRightSongPlay = true;
            skullHead.gameObject.SetActive(true);   // �ΰ����� Ȱ��ȭ ������
        }
    }
}
