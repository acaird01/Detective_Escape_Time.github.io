using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_TorchGimic : MonoBehaviour
{
    #region ������Ʈ �� ������Ʈ�� �Ҵ��� ���� ����
    private Calli_ObjectManager calli_ObjectManager;    // Į���� ������Ʈ �Ŵ���
    private Interaction_Gimics interaction;             // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private GameObject player;                          // �÷��̾�
    private TextController textController;              // ��ȣ�ۿ� ��縦 ����� text UI�� ��Ʈ���ϴ� ��ũ��Ʈ

    private AudioSource torchLightOnSound_AudioSource;   // ȶ���� ���� �� �Ҹ��� ����� AudioSource
    private Light[] torch_Lights;                        // ���ɰ� ��ȣ�ۿ��ϸ� ���� ���� Light ������Ʈ��
    public GameObject takoPosHint;                       // ȶ�� ����� Ǯ������ ������ Ÿ�� ��ġ ��Ʈ
    private Calli_TorchLightOn calli_TorchLightOn;       // ȶ���� ���� �ɾ��ְ� ������� ���θ� Ȯ���� ��ũ��Ʈ

    [Header("����� ��ƼŬ ����Ʈ")]
    [SerializeField]
    GameObject effectLight;
    private AudioSource audioSource;        // ����Ʈ �����Ҷ� ����� audio source
    #endregion

    #region �ش� ��ũ��Ʈ���� ����� ���� ����
    private bool settingGimic { get; set; }     // ��� ������ ���� ����(ȶ�� ����Ⱑ Ȱ��ȭ �Ǿ����� Ȯ�ο�)
    public bool SettingForObjectToInteration    // ��� ������ ���� ���� ������Ƽ
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        calli_ObjectManager = GameObject.FindAnyObjectByType<Calli_ObjectManager>();    // Calli_ObjectManager�� ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();                    // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        player = GameObject.FindAnyObjectByType<PlayerCtrl>().gameObject;               // �÷��̾ ã�ƿͼ� �Ҵ�
        textController = player.GetComponentInChildren<TextController>();                         // ��ȣ�ۿ�� ��縦 ����� ��ũ��Ʈ

        torch_Lights = GetComponentsInChildren<Light>();      // ȶ�ҵ��� �ڽĵ��� ������ �ִ� Light ������Ʈ�� ã�ƿͼ� �Ҵ�
        
        takoPosHint = GameObject.FindAnyObjectByType<Calli_TakoPosCeilingHint>().gameObject; // ȶ�� ����� Ǯ������ ������ Ÿ�� ��ġ ��Ʈ�� ã�ƿͼ� �Ҵ�
        calli_TorchLightOn = GetComponentInChildren<Calli_TorchLightOn>();  // ȶ���� ���� �ɾ��ְ� ������� ���θ� Ȯ���� ��ũ��Ʈ�� �ڽĿ��� ã�ƿͼ� �Ҵ�
        audioSource = takoPosHint.GetComponent<AudioSource>();              // ����� audio source�� �Ҵ�

        settingGimic = interaction.run_Gimic;       // ��� ���� ���θ� Ȯ���ؼ� ����

        Setting_SceneStart();   // ���� �Լ� ȣ��
    }

    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            // ȶ�ҵ��� ���� ����
            for (int i = 0; i < torch_Lights.Length; i++)
            {
                torch_Lights[i].gameObject.SetActive(true);
            }

            takoPosHint.gameObject.SetActive(true); // Ÿ�� ��ġ ��Ʈ Ȱ��ȭ
        }
        else
        {
            // ȶ�ҵ��� ���ܵ�
            for (int i = 0; i < torch_Lights.Length; i++)
            {
                // ���� ��������� ���Ǵ� ȶ���� ��� ���⼭ ��Ȱ��ȭ ��Ű�� ����.
                if (torch_Lights[i].name == "TorchLight (14)")
                {
                    continue;
                }

                torch_Lights[i].gameObject.SetActive(false);
            }

            takoPosHint.gameObject.SetActive(false); // Ÿ�� ��ġ ��Ʈ ��Ȱ��ȭ
        }

        calli_TorchLightOn.Init();  // Calli_TorchLightOn�� �ʱ�ȭ �Լ� ����
    }

    #region ��� �����ϴ� �Լ� ����
    // õ���� ��Ʈ�� Ȱ��ȭ ���� �Լ�
    public IEnumerator CeilingTakoPos()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(0.5f);

        // ȶ�ҵ��� ���� ����
        for (int i = 0; i < torch_Lights.Length; i++)
        {
            // ȶ���� ���� ����
            torch_Lights[i].gameObject.SetActive(true);

            if (torch_Lights[i].name != "TorchLight (14)")
            {
                // ��ȯ �Ҹ� ���
                torchLightOnSound_AudioSource = torch_Lights[i].gameObject.GetComponent<AudioSource>();  // ȶ���� ���� �� �Ҹ��� ����� AudioSource
                torchLightOnSound_AudioSource.Play();
            }

            yield return waitSeconds;  // 0.2�� ���

            // ī�޶� ��ŷ �õ�
        }

        // ������ ����Ʈ ����
        effectLight.SetActive(true);
        effectLight.GetComponent<ParticleSystem>().Play();
        audioSource.Play();

        yield return waitSeconds;  // 0.2�� ���

        takoPosHint.gameObject.SetActive(true); // Ÿ�� ��ġ ��Ʈ Ȱ��ȭ

        yield return waitSeconds;  // 0.2�� ���

        // ������ ����Ʈ ����
        effectLight.GetComponent<ParticleSystem>().Stop();

        interaction.run_Gimic = true;

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate(); // ��� ���� ������ ȣ��
    }
    #endregion
}
