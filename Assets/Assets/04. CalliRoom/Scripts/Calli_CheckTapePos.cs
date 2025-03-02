using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calli_CheckTapePos : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    private Interaction_Gimics interaction;         // ��ȣ�ۿ��ϴ� ������� Ȯ���ϱ� ���� ������Ʈ
    private Calli_StoneTableGimic calli_StoneTable; // �� å�� ��ü���� �����ϴ� ��ũ��Ʈ �Ҵ��� ����
    private GameObject player;                      // �÷��̾�
    private GameObject tapeOnTableHole;             // ������ĭ�� �������� �����Ҷ� ����� GameObject ����
    private TextController textController;          // �ؽ�Ʈ�� ����ϴ� UI�� ��ũ��Ʈ
    #endregion

    #region �ش� ��Ϳ��� ����� ���� ����
    private string TableHolePosition;  // �ش� ������ ĭ�� ���° ĭ���� ������ ����
    private bool setTapeOnTableHole;   // �ش� ������ ĭ�� �������� ���Ҵ��� Ȯ���� ����
    private bool settingGimic { get; set; } // ��� ���� ���� ������ ����
    public bool SettingGimic
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }
    #endregion

    private void Awake()
    {
        TableHolePosition = this.gameObject.name;  // �̸��� �̿��� ���� ������ ĭ�� ��ġ�� ����
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�(Start ���)
    {
        player = GameObject.Find("Player");
        calli_StoneTable = GameObject.FindAnyObjectByType<Calli_StoneTableGimic>(); // �� å�� ��ü�� ã�ƿͼ� �Ҵ�

        calli_StoneTable.SetInitTapeHole(Init);    // ü���ǿ��� �ʱ�ȭ �����ϱ� ���� Action�� �Լ� �Ҵ�
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        //calli_StoneTable = GameObject.FindAnyObjectByType<Calli_StoneTableGimic>(); // �� å�� ��ü�� ã�ƿͼ� �Ҵ�

        //calli_StoneTable.SetInitTapeHole(Init);    // ü���ǿ��� �ʱ�ȭ �����ϱ� ���� Action�� �Լ� �Ҵ�
    }

    // �ʱ�ȭ �Լ�(calli_StoneTable���� ����)
    private void Init()
    {
        player = GameObject.Find("Player"); // �÷��̾ ã�ƿͼ� �Ҵ�
        interaction = gameObject.GetComponent<Interaction_Gimics>();                // ��ȣ�ۿ��� ���� Interaction_Gimics �Ҵ�
        //calli_StoneTable = GameObject.FindAnyObjectByType<Calli_StoneTableGimic>(); // �� å�� ��ü�� ã�ƿͼ� �Ҵ�

        //calli_StoneTable.SetInitTapeHole(Init);    // ü���ǿ��� �ʱ�ȭ �����ϱ� ���� Action�� �Լ� �Ҵ�

        textController = GameObject.FindAnyObjectByType<TextController>();  // ��ȣ�ۿ��� �� ������� �ؽ�Ʈ�� �����ϴ� textcontroller

        tapeOnTableHole = null;       // ���� ������ Ÿ�ڰ� ���Բ� �ʱ�ȭ
        settingGimic = interaction.run_Gimic;   // ��� ��ġ���ο� ���� ��� �����ϱ� ���� ���� �ʱ�ȭ

        settingGimic = false;    // ���� ���� �ʾҴٰ� �ʱⰪ ����(���߿� ���嵥���� ������� ���ôٽ� �� ��.)
        // settingGimic = interaction.run_Gimic;
        setTapeOnTableHole = false; // ���� �ش�ĭ�� ���� �ʾҴٰ� ����

        // �ʱ�ȭ �� ����� �����ϱ� ����
        StartCoroutine(WaitTouch());            // ��ȣ�ۿ������� ����ų �ڷ�ƾ �Լ� ȣ��

        Setting_SceneStart();
    }

    // ��ȣ�ۿ�Ǳ� ������ ����� �ڷ�ƾ �Լ�
    private IEnumerator WaitTouch()
    {
        while (player) // �ѹ��ϰ� �ǻ����� �̰Ż���, â�� ���� �ݴ°�ó�� �ݺ��ʿ��ϸ� �̰� �ְ� ����
        {
            // this.GetComponentInChildren<ParticleSystem>().Play();

            yield return new WaitUntil(() => (interaction.run_Gimic) == true);  // �����ǰ��� ���⼭ ����ϴٰ�

            // ��� ������ ���� ���
            if (interaction.run_Gimic && !setTapeOnTableHole)
            {
                setTapeOnTableHole = true; // �������� �ش� ĭ ���� �����Ѵٰ� ���� ����. �߰� �ݺ��� ����

                // �̹� �ִ밹������ ���� ���� �������� ��ȯ�Ǿ���, �ش� ��Ű�� �������� ������ĭ ���� ���� ��쿡�� ��ȯ�� ����
                if (!calli_StoneTable.isTapeReachMaxNum && (!calli_StoneTable.GetAnswerStoneTableTape(ItemManager._instance.hotkeyItemIndex)))
                {
                    Debug.Log("������ ��ȯ �õ�1" + this.gameObject.name);    // ��ȣ�ۿ� Ȯ�ο� �α�

                    // ���� ��Ű�� �������� �������� ��쿡�� ��ȯ
                    if (ItemManager._instance.hotkeyItemIndex >= 21 && ItemManager._instance.hotkeyItemIndex <= 24)
                    // if (ItemManager._instance.hotkeyItemIndex == 21 || ItemManager._instance.hotkeyItemIndex == 22 || ItemManager._instance.hotkeyItemIndex == 23 || ItemManager._instance.hotkeyItemIndex == 24)
                    {
                        SummonTapeOnStoneTable();   // ���� ������ĭ ��ġ�� �������� ��ȯ�ϱ� ���� �Լ� ȣ��
                    }
                    else
                    {
                        setTapeOnTableHole = false;    // ��ȯ���� �ʾ����� ���� ����
                        interaction.run_Gimic = false; //
                    }
                }
                else
                {
                    setTapeOnTableHole = false;    // ��ȯ���� �ʾ����� ���� ����
                    interaction.run_Gimic = false; //
                    // Debug.Log("���� ��ȯ ���� : " + calli_StoneTable.CountTapes);

                    //if (calli_StoneTable.CountTapes > 0)
                    //{
                    //    calli_StoneTable.CountTapes -= 1;     // ��ȯ�� Ÿ�ڼ� -1
                    //}
                    // calli_StoneTable.CountTapes -= 1;     // ��ȯ�� Ÿ�ڼ� -1
                }
            }
            else
            {
            }
        }
    }

    // ������ ĭ ���� Ÿ�� ��ȯ�ϱ� ���� �Լ�
    private void SummonTapeOnStoneTable()
    {
        Debug.Log("������ ��ȯ : " + ItemManager._instance.hotkeyItemName);  // �α� ���

        // Resources Ǯ������ ���� ��Ű�� ��ϵ� ������ �������� ã�ƿ� ����
        tapeOnTableHole = Instantiate(Resources.Load<GameObject>("Items/Prefabs/" + ItemManager._instance.hotkeyItemName));
        tapeOnTableHole.transform.SetParent(this.transform, false);                             // �ش� ������ ĭ�� �ڽ����� ����
        // tapeOnTableHole.transform.position = this.transform.position + (Vector3.up * 0.1f);     // �ش� ������ ĭ ��ġ�� ��¦ ���ְԲ� �̵�
        tapeOnTableHole.transform.position = this.transform.position + (Vector3.up * 0.05f);     // �ش� ������ ĭ ��ġ�� ��¦ ���ְԲ� �̵�
        tapeOnTableHole.transform.localScale = new Vector3(35f, 2.25f, 15.3f);                    // �����Ǵ� �������� ũ�� ����
        tapeOnTableHole.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        calli_StoneTable.SetAnswerCasetteTape(ItemManager._instance.hotkeyItemIndex, TableHolePosition);   // ������ ĭ�� �������Ƿ� �迭�� �����ϱ����� �Լ� ȣ��
        
        // �߰� ��ȣ�ۿ��� ���� �ٽ� �κ��丮�� ���� �ʵ��� collider ��Ȱ��ȭ
        tapeOnTableHole.GetComponent<Collider>().enabled = false;
        this.GetComponent<Collider>().enabled = false;

        calli_StoneTable.SetDestroyWrongPlaceTapes_CallBack(DestroyWrongPlaceTape);    // �߸��� ��ġ�� ���� ��� ������ �Լ� Action�� �־���
    }

    // �� å�󿡼� �߸��� ��ġ�� �������� ������ ��� ������ �Լ�
    private void DestroyWrongPlaceTape()
    {
        // Debug.Log("������ ����");
        Destroy(tapeOnTableHole);       // �ش� ��ġ�� ������ ������ ����
        interaction.run_Gimic = false;  // �ش� ����� ���� ������� ���� ���·� ����
        setTapeOnTableHole = false;     // ��������Ƿ� false�� ����
        this.GetComponent<Collider>().enabled = true;   // �ٽ� ��ȣ�ۿ� �����ϵ��� �ݶ��̴� Ȱ��ȭ
    }

    // �� �������� �� �ʱ� ��ġ�� �����ϱ� ���� �ڷ�ƾ �Լ�
    void Setting_SceneStart()
    {
        // �� �������� �� settingGmimic�� true false���� ���� �ʱ� ��ġ ����
        if (settingGimic)
        {
            
        }
        else
        {

        }
    }
}
