using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Calli_RadioPlay : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private Animator animator;                          // �뷡�� ���ö� ������ �������� �ִϸ�����
    private AudioSource audioSource;                    // �뷡�� ����� audioSource
    [Header("BGM���� ����ϴ� AudioClip ����")]
    [SerializeField]
    private AudioClip[] bgmAudioClips;
    private Calli_WhichSongPlayBook whichSongPlayBook;  // ���� ����ϴ� �뷡�� �������� Ȯ���ϴ� ��ũ��Ʈ
    [SerializeField]
    private GameObject selectTape;                      // ���õǾ����� ��ġ�� �̵���ų ������ ������Ʈ
    [SerializeField]
    private GameObject prevTape;                        // ������ ������ ������ ������Ʈ
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private string playSongName = "";          // ���� ������� �뷡 ����
    private string interactionText;            // ��ȣ�ۿ� �� ����� ���
    private bool isBGMChange;                  // BGM�� �ٲ㼭 ����ϴ� �ڷ�ƾ�� ���ư��� �ִ��� Ȯ���ϱ� ���� ����
    private int playSongNum;                   // ���� ������� �뷡 ��ȣ(0 : �⺻ / 21 : MERA MERA / 22 : You're Not Special / 23 : The Grim Reaper is a Live-Streamer / 24 : HUGE W)
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
        interaction = GetComponent<Interaction_Gimics>();   // �ڽſ��� �پ��ִ� interaction gimic�� �Ҵ�
        animator = GetComponent<Animator>();                // �ڽ��� �ִϸ����� �Ҵ�
        audioSource = calli_ObjectManager.GetComponentInChildren<AudioSource>();          // �뷡�� �����ų �ڽ��� audio source �Ҵ�
        whichSongPlayBook = GameObject.FindAnyObjectByType<Calli_WhichSongPlayBook>();  // ���� ����ϴ� �뷡�� �������� Ȯ���ϴ� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�

        playSongName = "";      // ���� �������� �뷡 ������ ��ĭ���� �ʱ�ȭ
        isBGMChange = false;    // ���� BGM�� �ٲٴ� �ڷ�ƾ�� ������� �ƴ϶�� �ʱ�ȭ
        playSongNum = 0;        // �⺻ BGM�� ������̶�� ����

        StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ��� �ڷ�ƾ ȣ��
    }

    // ��ȣ�ۿ� ��� �ڷ�ƾ �Լ�
    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // ��� �����ض� ���⿡

            // ���õ� �������� null�� �ƴϰ�, ������ ��ġ ����� ������ ��� ����
            if ((selectTape != null) && (whichSongPlayBook.SettingForObjectToInteration == true))
            {

                if ((selectTape != null))
                {
                    // ������ ������Ʈ�� �������� ������ �������� ��쿡�� ����
                    if ((selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.GetComponent<IItem>() != null))
                    {
                        SetPlaySongName(); // ���õ� �������� ������ ���� �뷡�� ����� �� �ֵ��� ����
                    }
                    else
                    {
                        Debug.Log("���õ� ������Ʈ�� ������(������)�� �ƴ�");
                    }
                }
                else
                {
                    interactionText = "���⼭ å�� ������ �뷡�� Ʋ �� ������ ����.";      // ��ȣ�ۿ� �� ����� ��� ����
                    StartCoroutine(textController.SendText(interactionText));   // ��ȣ�ۿ� ��� ���   
                }

                interaction.run_Gimic = false;
            }
            else // ������ ��ġ ����� ������ �ʾ��� ��� ����
            {
                interactionText = "���⼭ å�� ������ �뷡�� Ʋ �� ������ ����.";      // ��ȣ�ۿ� �� ����� ��� ����
                StartCoroutine(textController.SendText(interactionText));   // ��ȣ�ۿ� ��� ���

                interaction.run_Gimic = false;
            }
        }
    }

    /// <summary>
    /// ���� �������� �������� �����ϰ� ������ �������̴��� �ִٸ� �ٽ� �������� �Լ�
    /// </summary>
    /// <param name="_currSelectTape"></param>
    public void SetSelectTape(GameObject _currSelectTape)
    {
        if (selectTape != null)      // ���� ������ �������̴� �������� ���� ���
        {
            if (selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.activeSelf)  // ���� ������ �������� Ȱ��ȭ ���¶��
            {
                selectTape.GetComponent<Calli_TapeSelectEnding2>().TapeReplace();  // �������� �ٽ� ���ڸ��� �ǵ�����.
            }

            prevTape = selectTape;        // �����ϰ� �ִ� �������� ������ �����ߴ� �������� ����
            selectTape = _currSelectTape; // ���� ������ �������� �������� �������� ����

            selectTape.GetComponent<Calli_TapeSelectEnding2>().TapePosUp();    // ���õǾ��ٰ� ǥ������ �Լ� ȣ��
        }
        else    // ���ٸ� ���� ���� ���� �������� �ٷ� �Ҵ�
        {
            prevTape = _currSelectTape;
            selectTape = _currSelectTape;

            selectTape.GetComponent<Calli_TapeSelectEnding2>().TapePosUp();    // ���õǾ��ٰ� ǥ������ �Լ� ȣ��
        }
    }

    /// <summary>
    /// ������ �������� �ε����� Ȯ���ؼ� �ش� �뷡�� �����ϰ� Ʋ����
    /// </summary>
    private void SetPlaySongName()
    {
        // �̹� BGM�� �ٲ��ִ� �ڷ�ƾ�� ���ư��� ���� ���
        if (isBGMChange)
        {
            StopCoroutine("PlayBGMByRadio");    // �ش� �̸��� ���� �ڷ�ƾ ����
            isBGMChange = false;
        }

        // ������ �������� �ε����� Ȯ���ؼ� �ش� �뷡�� �����ϰ� Ʋ����
        switch (selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.GetComponent<IItem>().Index)
        {
            case 21:
                {
                    // ���� ������� �뷡�� �ٸ� ��쿡�� ����
                    if (playSongNum != 21)
                    {
                        playSongNum = 21;
                        playSongName = "MERA MERA";
                        StartCoroutine(PlayBGMByRadio(bgmAudioClips[1]));   // �ش� AudioClip���� BGM�� �ٲ㼭 ����ϱ� ���� �Լ� ȣ��
                    }

                    break;
                }
            case 22:
                {
                    // ���� ������� �뷡�� �ٸ� ��쿡�� ����
                    if (playSongNum != 22)
                    {
                        playSongNum = 22;
                        playSongName = "You're Not Special";
                        StartCoroutine(PlayBGMByRadio(bgmAudioClips[2]));   // �ش� AudioClip���� BGM�� �ٲ㼭 ����ϱ� ���� �Լ� ȣ��
                    }

                    break;
                }
            case 23:
                {
                    // ���� ������� �뷡�� �ٸ� ��쿡�� ����
                    if (playSongNum != 23)
                    {
                        playSongNum = 23;
                        playSongName = "The Grim Reaper is a Live-Streamer";
                        StartCoroutine(PlayBGMByRadio(bgmAudioClips[3]));   // �ش� AudioClip���� BGM�� �ٲ㼭 ����ϱ� ���� �Լ� ȣ��
                    }

                    break;
                }
            case 24:
                {
                    // ���� ������� �뷡�� �ٸ� ��쿡�� ����
                    if (playSongNum != 24)
                    {
                        playSongNum = 24;
                        playSongName = "HUGE W";
                        StartCoroutine(PlayBGMByRadio(bgmAudioClips[4]));   // �ش� AudioClip���� BGM�� �ٲ㼭 ����ϱ� ���� �Լ� ȣ��
                    }

                    break;
                }
            default:
                {
                    Debug.Log("�߸��� ������ �Է�");
                    break;
                }
        }


        if (prevTape != selectTape)
        {
            prevTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.gameObject.SetActive(true); // ������ �����ߴ� �������� Ȱ��ȭ ���� ���̰� �������
            prevTape.GetComponent<Calli_TapeSelectEnding2>().TapeReplace();
        }
        selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.gameObject.SetActive(false); // �������� ��Ȱ��ȭ ���� �Ⱥ��̰� �������

        whichSongPlayBook.IsPlaySongCorrectCheck(playSongName);   // ���� �뷡�� ����ϰ� �ִ��� Ȯ���ϱ� ���� �Լ� ȣ��
    }

    /// <summary>
    /// ���õ� BGM Clip���� BGM�� �ٲ㼭 ������� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="_bgmClip"></param>
    private IEnumerator PlayBGMByRadio(AudioClip _bgmClip)
    {
        isBGMChange = true; // �ش� �ڷ�ƾ�� �������̶�� ���� ����

        audioSource.Stop(); // ���� ��� ���̴� BGM�� ����

        audioSource.clip = _bgmClip;    // BGM�� Audio clip�� ������ �뷡�� ����
        audioSource.loop = false;   // loop�� ���� �ʵ��� ����

        audioSource.Play(); // �ش� Ŭ������ BGM�� �ٽ� ���

        yield return new WaitUntil(() => !audioSource.isPlaying);   // �ش� �뷡�� ������ ������ ���

        selectTape.GetComponent<Calli_TapeSelectEnding2>().SelectTape.gameObject.SetActive(true); // �뷡�� �������Ƿ� �������� Ȱ��ȭ ���� ���̰� �������

        // �뷡�� �������� �Ѵ� �ٽ� �����
        selectTape = null;
        prevTape = null;
        playSongNum = 0;    // ���� ��� ���� �뷡�� �⺻BGM�̶�� ��ȣ ����

        audioSource.clip = bgmAudioClips[0];    // BGM�� Audio clip�� ���� ����ϰ� �ִ� �뷡�� ����
        audioSource.loop = true;   // loop�� ������ ����

        audioSource.Play(); // �ش� Ŭ������ BGM�� �ٽ� ���
    }
}
