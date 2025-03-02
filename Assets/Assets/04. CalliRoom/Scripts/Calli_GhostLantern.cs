using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_GhostLantern : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private AudioSource ghostLanternSound_AudioSource;  // ��ȥ�� ��Ȯ�Ǿ����� �Ҹ��� ����� AudioSource
    private Light ghostLight;                           // ��Ȯ�Ǹ� ���� ���� Light ������Ʈ
    private GameObject tako;                            // ���� ����� Ǯ������ ��ȯ���� Ÿ�� GameObject
    private Calli_SummonTakoEffect calli_SummonTakoEffect;  // Ÿ�� ��ȯ ����Ʈ ��� �� �ִϸ��̼� ����� ���� ��ũ��Ʈ

    [Header("��ȥ ��Ȯ ���θ� ������ interaction gimic")]
    [SerializeField]
    private Interaction_Gimics save_Interaction;        // ����� �����ϱ� ���� Interaction gimic(�θ� ����)
    [Header("��ȥ�� ��Ȯ�ɶ� ����� audioclip(�Ҹ� ���� Ȯ�ο�)")]
    [SerializeField]
    private AudioClip ghostCatch_SFX;                   // ��ȥ�� ��Ȯ�ɶ� ����� audioclip(�Ҹ� ���� Ȯ�ο�)
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private const int MaxGhostNum = 5;  // ��ƾ��� �ִ� ��ȥ�� ���� ��������� ����(���� 5)
    private bool isAllGhostCatch;       // ��ȥ�� �ִ�ġ���� ��Ȯ�Ǿ����� Ȯ���ϱ� ���� bool ����(true : ��� ���� / false : �� ������)
    private int currGhostCount;         // ���� ��ȥ�� �󸶳� ��Ȯ�Ǿ����� Ȯ���ϱ� ���� ����
    public int CurrentGhostCount        // ��ȥ���� ������ ��Ȯ�Ǿ��ٰ� üũ���ֱ� ���� ������Ƽ
    {
        set 
        {
            // ���� ��ȥ�� ���� �ִ�ġ�� �ƴ� ��쿡�� set
            if (currGhostCount < MaxGhostNum)
            { 
                currGhostCount = value;
            }
        }
    }
    private const string ending1_TakoName = "Item_06_TakoCalli";    // ���� ����ȸ���� 1�� ��� ��ȯ�� Ÿ���̸�
    private const string ending2_TakoName = "Item_02_TakoIRyS";    // ���� ����ȸ���� 2�� ��� ��ȯ�� Ÿ���̸�
    private float ghostSoundLength;    // ��ȥ ��Ȯ�ɶ� ���� �Ҹ� ����
    #endregion


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�

        Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Init()
    {
        ghostLight = GetComponentInChildren<Light>();    // Calli_GhostLight�� Light ������Ʈ�� ã�ƿͼ� �Ҵ�
        ghostLanternSound_AudioSource = GetComponentInChildren<AudioSource>();  // ��ȥ�� ��Ȯ�Ϸ�Ǹ� �Ҹ��� ����� AudioSource�� �Ҵ�
        calli_SummonTakoEffect = GetComponentInChildren<Calli_SummonTakoEffect>();  // Ÿ�� ��ȯ �ִϸ��̼� ����� ��ũ��Ʈ�� ã�ƿͼ� �Ҵ�
        settingGimic = save_Interaction.run_Gimic;       // ��� ���� ���θ� Ȯ���ؼ� ����
        textController = player.GetComponentInChildren<TextController>();

        currGhostCount = 0;             // ���� ��ȥ�� �ϳ��� ��Ȯ���� �ʾҴٰ� �ʱ�ȭ
        isAllGhostCatch = false;        // ���� ��� ��ȥ�� ��Ȯ���� �ʾҴٰ� �ʱ�ȭ

        ghostSoundLength = (ghostCatch_SFX.length / 3) * 2; // ��ȥ �Ҹ��� 2/3���̷� ����

        if (GameManager.instance.Episode_Round == 1)        // ���� ȸ���� 1�� ��쿡 ��ȯ�� Ÿ���̸����� �ʱ�ȭ
        {
            tako = GetComponentInChildren<Item07TakoCalli>().gameObject;    // ��ȯ���� Ÿ�ڸ� ã�ƿͼ� �Ҵ�(Į��)
        }
        else if (GameManager.instance.Episode_Round == 2)   // ���� ȸ���� 2�� ��쿡 ��ȯ�� Ÿ���̸����� �ʱ�ȭ
        {
            tako = GetComponentInChildren<Item02TakoIRyS>().gameObject;     // ��ȯ���� Ÿ�ڸ� ã�ƿͼ� �Ҵ�(���̸���)
        }
        else 
        {
            Debug.Log("���� ������ �߸���");
        }

        tako.SetActive(false);                   // Ÿ�ڸ� ��Ȱ��ȭ

        StartCoroutine(WaitTouch());    // ��ȣ�ۿ� ����� �ڷ�ƾ �Լ� ȣ��

        Setting_SceneStart();           // ���� ���� �ڷ�ƾ ��
    }

    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            isAllGhostCatch = true;
        }
        else
        {
            isAllGhostCatch = false;
        }
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);
            // ��� �����ض� ���⿡

            // Į�� ���� ȹ������ ���� ���
            if (!ItemManager._instance.inventorySlots[12].GetComponent<IItem>().isGetItem)
            {
                StartCoroutine(textController.SendText("�������� ������ ��� �����̳�.\n��ġ ��ȥ�̶� ���ֵα� ���� ������ �͸� ����.."));

                // StartCoroutine(textController.SendText("��ȥ���� ��Ȯ�ϸ� ���� ������ ������?"));

                interaction.run_Gimic = false;
            }
            else // Į�� ���� ȹ���� ���
            {
                if (isAllGhostCatch) // ��� ��ȥ�� ���� ���
                {
                    StartCoroutine(textController.SendText("����, ��ȥ���� ��� ��Ҿ�!\n���� Ÿ�ڸ� ��������"));
                }
                else
                {
                    StartCoroutine(textController.SendText("��ȥ���� ��� ��Ȯ�ϸ� ���� ������ ������?"));
                }

                interaction.run_Gimic = false;
            }
        }
    }

    #region ����� �����ϴ� �Լ� ����
    // ��ȥ�� ��Ȯ�� ��� ���� ������ �Լ�(Calli_CatchGhost���� ȣ���ؼ� ���)
    public void SetGhostLight()
    {
        // �ִ�ġ���� ������ �ʾҴٸ� ��ȥ�� ��Ȯ�Ǿ����Ƿ� ���� 1�ܰ� �÷���
        if (!isAllGhostCatch)
        {
            if (currGhostCount < MaxGhostNum)
            { 
                currGhostCount++;       // ���� ��ȥ���� 1����
            }
            
            ghostLight.range += 20; // ��⸦ 20 ����
        }

        if (currGhostCount == MaxGhostNum)
        {
            isAllGhostCatch = true; // ���� �������ٸ� ��� ��Ʈ�� �����ٰ� ����
            save_Interaction.run_Gimic = isAllGhostCatch;

            StartCoroutine(SummonTako());   // ���������Ƿ� Ÿ�� �����Լ� ȣ��    
        }
    }

    // Ÿ�� ��ȯ�ϴ� �ڷ�ƾ �Լ�
    private IEnumerator SummonTako()
    {
        yield return new WaitForSeconds(ghostSoundLength);  //��ȥ�� ��Ȯ�ɶ� ����� audioclip�� 2/3���̸�ŭ ��� �� ��ȯ

        ghostLanternSound_AudioSource.Play();   // Ÿ�� ��ȯ ȿ���� ���

        tako.SetActive(true);                   // Ÿ�ڸ� ��ȯ(Ȱ��ȭ)

        // ���⼭ �ִϸ��̼� ����
        calli_SummonTakoEffect.animationStart();

        // ���൵ ������Ʈ
        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
    }
    #endregion
}
