using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Calli_CatchGhost : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private Calli_GhostLantern ghostLantern;            // ��ȥ�� ��Ȯ�ϸ� ���� ���� ����
    private Calli_GhostGimics ghostGimics;              // ��ȥ ��� ���� ��ũ��Ʈ
    private AudioSource ghost_SFX;                      // ��ȥ�� ��Ȯ�ɶ� ȿ������ ����� AudioSource

    [Header("����� ��ƼŬ ����Ʈ")]
    [SerializeField]
    GameObject effectLight;
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    private bool isGhostVisible;    // �̹� ������ ���̰� �ִ��� Ȯ���ϱ� ���� ���ǹ�
    [SerializeField]
    private bool isGhostCatch;      // �ش� ��ȥ�� ��Ȯ�Ǿ����� Ȯ���ϱ� ���� ����
    public bool IsGhostCatch      // �ش� ��ȥ�� ��Ȯ�Ǿ����� Calli_GhostGimics���� Ȯ���ϱ� ���� ������Ƽ
    {
        get
        { 
            return isGhostCatch;
        }
    }
    private bool isGhostWait;        // �ش� ��ȥ�� ���� �������� ��������� Ȯ���ϱ� ���� ����(true : ����� / false : �������)
    private float ghostSoundLength;  // ��ȥ ��Ȯ�ɶ� ���� �Ҹ� ����
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        //interaction = gameObject.GetComponent<Interaction_Gimics>();    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�

        // Init();     // �ʱ�ȭ �Լ� ȣ��
    }

    // �ʱ�ȭ �Լ� ȣ��
    public void Init()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        ghost_SFX = GetComponent<AudioSource>();    // ������ �ִ� AudioSource�� �Ҵ�
        ghostLantern = GameObject.FindAnyObjectByType<Calli_GhostLantern>();    // Calli_DeadBeatSkull�� ã�ƿͼ� �Ҵ�
        ghostGimics = GameObject.FindAnyObjectByType<Calli_GhostGimics>();      // ��ȥ�� �����ϴ� ������Ʈ�� ã�ƿͼ� �Ҵ�

        settingGimic = interaction.run_Gimic;       // ��� ���� ���θ� Ȯ���ؼ� ����
        isGhostVisible = false;     // ���� �Ⱥ��̴� ���·� ����
        isGhostCatch = false;       // ���� �������ٰ� �ʱ�ȭ
        isGhostWait = true;         // ���� ��� ���̶�� ǥ��

        isGhostVisible = ghostGimics.IsGhostVisible;    // Ȱ��ȭ ������ ���� ���̴��� ���� �ֽ�ȭ
        ghostSoundLength = (ghost_SFX.clip.length / 3) * 2; // ���ɼҸ��� 2/3���̷� ����

        effectLight.gameObject.SetActive(false);    // ����Ʈ�� ��Ȱ��ȭ �ص�
        // StartCoroutine(WaitTouch());                    // ��ȣ�ۿ� ��� �ڷ�ƾ �Լ� ����

        Setting_SceneStart();           // ���� ���� �ڷ�ƾ ����
    }

    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            isGhostCatch = true;
            ghostLantern.SetGhostLight();   // �̹� �������Ƿ� ���� �������� �Լ� ȣ��
            this.gameObject.SetActive(false);   // ���� ���� ��Ȱ��ȭ ��Ŵ
        }
        else
        {
            isGhostCatch = false;
            this.gameObject.SetActive(false);   // ���� ���� ��Ȱ��ȭ ��Ŵ
        }
    }

    private void OnEnable()
    {
        isGhostVisible = true;          // Ȱ��ȭ ������ ���� ���̴� ���̶�� ����
        isGhostWait = true;             // �ٽ� ��ȥ�� ��� ��Ű�� ���� true�� ����
    }
    private void OnDisable()
    {
        isGhostVisible = false;         // ��Ȱ��ȭ ������ ���� ������ �ʴ´ٰ� ����
    }

    private void Update()
    {
        if (isGhostVisible && isGhostCatch)   // Ȱ��ȭ��µ� �̹� �ش� ��ȥ�� ������ ���
        {
            isGhostVisible = false;        // ��ȥ ��Ȱ��ȭ�� ���� �Ⱥ��δٰ� ����
            isGhostWait = true;            // �̹� �������Ƿ� �� ����(�ϴ� ��� �Ǳ���)
            SetGhostVisible(isGhostCatch); // Ȱ��ȭ���� ��ȯ �Լ� ����(��Ȱ��ȭ ��Ŵ)
        }

        if (isGhostWait && !isGhostCatch)
        {
            isGhostWait = false;            // �̹� ��� ���̹Ƿ� false�� ����
            StartCoroutine(WaitTouch());    // ��� ������ ���� ��� �ڷ�ƾ �Լ� ȣ��
        }
    }

    IEnumerator WaitTouch()
    {
        yield return new WaitUntil(() => (interaction.run_Gimic) == true);

        if (interaction.run_Gimic)
        {
            isGhostCatch = true;            // ��ȥ�� ��Ȯ�Ǿ��ٰ� ���� ����
            this.gameObject.GetComponent<SphereCollider>().enabled = false; // �̹� �������Ƿ� �Ҹ� ����ϴµ��� ����Բ� �ݶ��̴� ��Ȱ��ȭ

            ghost_SFX.Play();               // ��ȥ ��Ȯ ȿ���� ���
            effectLight.SetActive(true);                          // ����Ʈ Ȱ��ȭ
            effectLight.GetComponent<ParticleSystem>().Play();    // ����Ʈ ���
            ghostLantern.SetGhostLight();   // �������Ƿ� ������ ��⸦ �ø� �Լ� ȣ��

            // yield return new WaitUntil(() => !ghost_SFX.isPlaying); // �Ҹ� ����� ���������� ���
            yield return new WaitForSeconds(ghostSoundLength); // �Ҹ� ��� �ð��� 2/3���� ���

            effectLight.GetComponent<ParticleSystem>().Stop();  // ����Ʈ ����

            calli_ObjectManager.ChangeSceneData_To_GameManager();   // ������ ��Ȯ�Ǿ����Ƿ� ���� 1ȸ ����

            this.gameObject.SetActive(false); ;  // ��ȥ�� ��Ȯ�Ǿ����Ƿ� ��Ȱ��ȭ ��Ŵ
        }
    }

    #region ����� �����ϴ� �Լ� ����
    // Į�� ���� ��Ű���� �����ؼ� ��� ���� ��� ��ȥ�� Ȱ��ȭ���� ������ �Լ�
    private void SetGhostVisible(bool _isGhostVisible)
    {
        // ��ȥ�� bool ���� ���� Ȱ��, ��Ȱ��ȭ��Ŵ(���� ������ �ʾҴٸ� Ȱ��ȭ)
        if (!isGhostCatch)
        {
            this.gameObject.SetActive(!_isGhostVisible);
        }
    }
    #endregion
}
