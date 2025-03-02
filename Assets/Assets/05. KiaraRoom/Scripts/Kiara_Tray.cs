using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Kiara_Tray : MonoBehaviour
{
    // pos1�� ��Ƣ �Ǵ� �ܹ���
    // pos2�� ��
    // pos3�� �ݶ� �Ǵ� Ŀ��
    public Transform[] setItemPos; // �������� �ν��Ͻ� �� ��ġ

    bool[] posCheck; // ������ ��ġ�� �ùٸ� �������� �ö� �ִ��� üũ
    public bool datasave = false;
    GameObject[] instantiate_Item; // �ش� ��ġ�� �ν��Ͻ��� ������ ������� ����
    int episodeRound;
    string episode1_firstText = "���..�ֹ��� �°� �����غ���.\n��ħ ������ �� �����ִ°� ����.";
    string episode2_firstText = "�̹����� �ֹ��� �°� �����غ���.\n�׷��� ������ �� �����ִ°ǰ�?";
    string Wrong_answer_Text = "���, �̰� �ƴѰ�?";
    string noitemInHotkey = "�̰� ������ �ƴѰ� ������?";
    public GameObject TakoKiara; // �������� �� ��ȯ�� Ű�ƶ� Ÿ��

    public GameObject SpawnMagic;
    Interaction_Gimics interaction;
    GameObject player;
    TextController textController;

    private void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();

        SpawnMagic.gameObject.SetActive(false);
        TakoKiara.gameObject.SetActive(false);
        episodeRound = GameManager.instance.Episode_Round;
        SetRndItem();
        StartSceneBoolSet();
        StartCoroutine(CheckCorrectItem());
        StartCoroutine(SpawnTako());

    }

    void StartSceneBoolSet()
    {
        posCheck = new bool[setItemPos.Length];
        for(int i = 0; i < posCheck.Length; i++)
        {
            posCheck[i] = false;
        }
    }

    /*void SceneStartSetting_KiaraTray() // �޾ƿ� �����Ϳ� ���� �ʱ� ��ġ ����
    {
        if (interaction.run_Gimic)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }*/

    IEnumerator CheckCorrectItem() // ��Ű�� �ִ� �������̶� ���ƾ� �Ǵ� ������ ���ؼ� ������ ����, Ʋ���� �ʱ�ȭ �ϴ°�
    {
        while (player)
        {
            instantiate_Item = new GameObject[setItemPos.Length];
            for (int i = 0; i < setItemPos.Length; i++)
            {
                if (interaction.run_Gimic == false)
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == true); // ��ȣ�ۿ��� ������
                    if (setItemPos[i].name == ItemManager._instance.hotkeyItemName) // ���ƾ� �ϴ� ��ġ(����)�� ��Ű�� ��ϵ� �������� ���ٸ�
                    {
                        posCheck[i] = true; // �ش� ��ġ �Ǻ��ϴ°� Ʈ��
                        instantiate_Item[i] = Instantiate(Resources.Load<GameObject>("Items/Prefabs/" + ItemManager._instance.hotkeyItemName)); // ���ҽ����� ��Ű�� ��ϵ� ������ �����ؼ� instantiate_Item �迭�� ���                                                
                        instantiate_Item[i].gameObject.GetComponent<Kiara_SpawnItem>().dontSetactivefalse = true;
                        instantiate_Item[i].transform.position = setItemPos[i].position; // ��ġ ����
                        instantiate_Item[i].transform.parent = setItemPos[i];
                        instantiate_Item[i].gameObject.GetComponent<Collider>().enabled = false;
                        if (instantiate_Item[i].gameObject.activeSelf == false)
                        {
                            instantiate_Item[i].gameObject.SetActive(true);
                        }
                    }
                    else if (ItemManager._instance.hotkeyItemName == "" || ItemManager._instance.hotkeyItemIndex != 28 && ItemManager._instance.hotkeyItemIndex != 29
                        && ItemManager._instance.hotkeyItemIndex != 30 && ItemManager._instance.hotkeyItemIndex != 31 && ItemManager._instance.hotkeyItemIndex != 32) 
                    {
                        if (i == 0)
                        {
                            if (episodeRound == 1)
                            {
                                StartCoroutine(textController.SendText(episode1_firstText));
                                i = -1;
                                break;
                            }
                            else
                            {
                                StartCoroutine(textController.SendText(episode2_firstText));
                                i = -1;
                                break;
                            }
                        }
                        else
                        {
                            StartCoroutine(textController.SendText(noitemInHotkey));
                            i--;
                        }
                    }                    
                    else
                    {
                        if (i != 0)
                        {
                            StartCoroutine(textController.SendText(Wrong_answer_Text));
                        }
                        for (int j = 0; j < i; j++) // �̶����� �ݺ��� ���� ���Դ� �迭�� �ʱ�ȭ
                        {
                            Destroy(instantiate_Item[j]);
                            posCheck[j] = false;
                        }
                        break; // �� ��� �ڿ� �ݺ��� Ż���� ó������ ���
                    }

                }
                else
                {
                    yield return new WaitUntil(() => interaction.run_Gimic == false); // ��ȣ�ۿ��� ������
                    if (setItemPos[i].name == ItemManager._instance.hotkeyItemName) // ���ƾ� �ϴ� ��ġ(����)�� ��Ű�� ��ϵ� �������� ���ٸ�
                    {
                        posCheck[i] = true; // �ش� ��ġ �Ǻ��ϴ°� Ʈ��
                        instantiate_Item[i] = Instantiate(Resources.Load<GameObject>("Items/Prefabs/" + ItemManager._instance.hotkeyItemName)); // ���ҽ����� ��Ű�� ��ϵ� ������ �����ؼ� instantiate_Item �迭�� ���                                                
                        instantiate_Item[i].gameObject.GetComponent<Kiara_SpawnItem>().dontSetactivefalse = true;
                        instantiate_Item[i].transform.position = setItemPos[i].position; // ��ġ ����
                        instantiate_Item[i].transform.parent = setItemPos[i];
                        instantiate_Item[i].gameObject.GetComponent<Collider>().enabled = false;
                        if (instantiate_Item[i].gameObject.activeSelf == false)
                        {
                            instantiate_Item[i].gameObject.SetActive(true);
                        }
                    }
                    else if (ItemManager._instance.hotkeyItemName == "" || ItemManager._instance.hotkeyItemIndex != 28 && ItemManager._instance.hotkeyItemIndex != 29
                        && ItemManager._instance.hotkeyItemIndex != 30 && ItemManager._instance.hotkeyItemIndex != 31 && ItemManager._instance.hotkeyItemIndex != 32)
                    {
                        if (i == 0)
                        {
                            if (episodeRound == 1)
                            {
                                StartCoroutine(textController.SendText(episode1_firstText));
                                i = -1;
                                break;
                            }
                            else
                            {
                                StartCoroutine(textController.SendText(episode2_firstText));
                                i = -1;
                                break;
                            }
                        }
                        else
                        {
                            StartCoroutine(textController.SendText(noitemInHotkey));
                            i--;
                        }
                    }
                    else
                    {
                        if (i != 0)
                        {
                            StartCoroutine(textController.SendText(Wrong_answer_Text));
                        }
                        for (int j = 0; j < i; j++) // �̶����� �ݺ��� ���� ���Դ� �迭�� �ʱ�ȭ
                        {
                            Destroy(instantiate_Item[j]);
                            posCheck[j] = false;
                        }
                        break; // �� ��� �ڿ� �ݺ��� Ż���� ó������ ���
                    }

                }
            }
        }
    }




    IEnumerator SpawnTako() // 3�� �������� ��� �˸°� ������ Ÿ�� Ű�ƶ� ��ȯ(1ȸ����)
    {
        yield return new WaitUntil(() => posCheck[0] == true && posCheck[1] == true && posCheck[2] == true);
        //        Instantiate(TakoKiara, setItemPos[3]);
        TakoKiara.gameObject.SetActive(true);
        Vector3 targetPos = new Vector3(TakoKiara.transform.position.x, TakoKiara.transform.position.y + 0.5f, TakoKiara.transform.position.z);
        SpawnMagic.gameObject.SetActive(true);
        datasave = true;
        TakoKiara.GetComponent<Collider>().enabled = false;
        while (TakoKiara.transform.localPosition != targetPos)
        {
            TakoKiara.transform.localPosition = Vector3.MoveTowards(TakoKiara.transform.localPosition, targetPos, 0.004f * Time.timeScale);
            yield return null;
        }
        TakoKiara.GetComponent<Collider>().enabled = true;
        SpawnMagic.gameObject.GetComponent<ParticleSystem>().Stop();
        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        gameObject.SetActive(false);
    }

    void SetRndItem() // ������ �������� ���� ���� �޸� ��ũ��Ʈ���� �̰� �޾ƾ���
    {
        int pos1 = Random.Range(0, 2);
        if(pos1 == 0)
        {
            setItemPos[0].name = "Item_28_Kiara_Burger";
        }
        else
        {
            setItemPos[0].name = "Item_29_Kiara_Potatoes";
        }
        setItemPos[1].name = "Item_30_Kiara_Chicken";
        int pos3 = Random.Range(0, 2);
        if (pos3 == 0)
        {
            setItemPos[2].name = "Item_31_Kiara_Coffee";
        }
        else
        {
            setItemPos[2].name = "Item_32_Kiara_Coke";
        }
    }
}
