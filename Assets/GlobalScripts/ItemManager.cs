using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager _instance;

    // �κ�UI
    public GameObject inventoryPanel;  // �κ��丮 UI �г�
    public GameObject[] inventoryPages; // �κ��丮 4���� ������
    public GameObject[] inventorySlots; // �κ��丮 ���� 40��
    public GameObject[] inventorySlotsParent; // �κ��丮 ���� ��ġ

    public GameObject[] hotkeySlots; // 4���� ��Ű ����

    public Button prevButton; // ���� ������ ��ư
    public Button nextButton; // ���� ������ ��ư

    [SerializeField]
    private int currentPageIndex = 0; // ���� ������
    [SerializeField]
    private int selectedHotkeySlot = -1; // ���õ� ��Ű ����
    private Dictionary<int, GameObject> hotkeyToItemMap = new Dictionary<int, GameObject>(); // ��Ű -> ������ ����

    PlayerCtrl playerCtrl;
    public GameObject interaction_F;

    [SerializeField]
    GameObject usingItem;

    public string[] hotkeyItemNames;
    public string hotkeyItemName;
    public int[] hotkeyItemIndexes;
    public int hotkeyItemIndex;

    [SerializeField]
    private GameObject hotkeyItemDescriptionPanel;
    [SerializeField]
    private Text hotkeyItemDescriptionTxT;
    private bool isItemDescriptionTxtRunning = false;

    public IItem[] itemComponent;    // �׽�Ʈ�� ����

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        interaction_F = GameObject.Find("F");
    }


    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    //}

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    //{
    //    LoadInventory();
    //    Debug.Log("ItemData_load");
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}




    private void Start()
    {
        //�κ��丮 ���۽� ���߱�
        inventoryPanel.SetActive(false);

        UpdateInventoryPage();

        interaction_F.gameObject.SetActive(false);

        //PlayerCtrl ��ũ��Ʈ ��������
        playerCtrl = gameObject.GetComponent<PlayerCtrl>();


        itemComponent = new IItem[hotkeySlots.Length];


        //set every inventorySlot's component "Drop" to enabled = false
        foreach (GameObject slot in inventorySlots)
        {
            if (slot != null)
            {
                Drag dragComponent = slot.GetComponent<Drag>();
                if (dragComponent != null)
                {
                    dragComponent.enabled = false;
                }
            }
        }
        hotkeyItemDescriptionPanel.SetActive(false);

        // CollectItem(GameObject.Find("Item_09_InaBook"));

        //if (GameManager.instance.Episode_Round == 2)    // 2ȸ���� ���
        //{
        //    ResetInventory();

        //    CollectItem(GameObject.Find("Item_09_InaBook"));

        //    // en 1����� Ÿ�ڵ��� ȹ�� �Ϸ� ó�� �� ��� �Ϸ� ó���� �����ص�
        //    // 2ȸ���� ��� �� ���� �� �ֵ��� �صα� ����
        //    CollectItem(GameObject.FindAnyObjectByType<Item06TakoGura>().gameObject);
        //    CollectItem(GameObject.FindAnyObjectByType<Item07TakoCalli>().gameObject);
        //    CollectItem(GameObject.FindAnyObjectByType<Item08TakoKiara>().gameObject);
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UseHotkeyItem();
        }

    }


    #region ������ ����

    //�������̶� ��ȣ�ۿ� ��
    public void CollectItem(GameObject item)
    {
        IItem itemComponent = item.GetComponent<IItem>();
        int itemIndex = itemComponent.Index;

        //�κ��丮 �� ������ Ȱ��ȭ
        inventorySlots[itemIndex].GetComponent<Drag>().enabled = true;
        inventorySlots[itemIndex].GetComponent<Image>().color = Color.white;
        inventorySlots[itemIndex].GetComponent<IItem>().isGetItem = true;
        itemComponent.isGetItem = true;

        switch (itemIndex)
        {
            case 1:     //Ÿ�� ������
                GameObject.FindAnyObjectByType<Number_if_Gimic>().TakoGet_TextUpdate(itemIndex);
                break;
            case 2:     //Ÿ�� ���̸���
                ReturnItem(12);
                DeactivateItem(12);
                GameObject.FindAnyObjectByType<Number_if_Gimic>().TakoGet_TextUpdate(itemIndex);
                break;
            case 3:     //Ÿ�� ũ�δ�
                GameObject.FindAnyObjectByType<Number_if_Gimic>().TakoGet_TextUpdate(itemIndex);
                break;
            case 4:     //Ÿ�� �Ŀ쳪
                for (int i = 28; i < 34; i++)
                {
                    ReturnItem(i);
                    DeactivateItem(i);
                }
                GameObject.FindAnyObjectByType<Number_if_Gimic>().TakoGet_TextUpdate(itemIndex);
                break;
            case 5:     //Ÿ�ں�
                ReturnItem(14);
                DeactivateItem(14);
                GameObject.FindAnyObjectByType<Number_if_Gimic>().TakoGet_TextUpdate(itemIndex);
                break;
            case 6:     //Ÿ�� ����
                ReturnItem(11);
                DeactivateItem(11); // ��Ȱ��ȭó��
                GameObject.FindAnyObjectByType<Number_if_Gimic>().TakoGet_TextUpdate(itemIndex);

                break;
            case 7:     //Ÿ�� Į��
                ReturnItem(12);
                DeactivateItem(12); // ��Ȱ��ȭó��
                GameObject.FindAnyObjectByType<Number_if_Gimic>().TakoGet_TextUpdate(itemIndex);

                break;
            case 8:     //Ÿ�� Ű�ƶ�
                for (int i = 28; i < 34; i++)
                {
                    ReturnItem(i);
                    DeactivateItem(i);
                }
                GameObject.FindAnyObjectByType<Number_if_Gimic>().TakoGet_TextUpdate(itemIndex);

                break;
            case 9:     // �̳� å
                if (GameManager.instance.Episode_Round == 2)
                {
                    // 2ȸ�� �ΰ�� ��Ȱ��ȭ ��Ŵ
                    DeactivateItem(9);
                }
                break;
            case 10:     // ���Ϸ�
                ReturnItem(9);
                DeactivateItem(9);

                break;
            case 12:    // ��

                ReturnItem(20); // �ΰ���
                DeactivateItem(20);
                break;
            case 15:
                ReturnItem(13);
                DeactivateItem(13);
                break;
            case 18:    //�Ƹ� �ð�
                for (int i = 1; i < 9; i++)
                {
                    ReturnItem(i);
                    DeactivateItem(i);
                }
                break;
        }
    }

    public void ReturnItem(int itemIndex)
    {
        inventorySlots[itemIndex].transform.SetParent(inventorySlotsParent[itemIndex].transform, false);

        inventorySlots[itemIndex].transform.localScale = new Vector3(1, 1, 1);

        if (hotkeyItemIndex == itemIndex)
        {
            hotkeyItemName = "";
            hotkeyItemIndex = -1;
        }
    }

    public void ReturnIfMultiItem()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            var parent = inventorySlots[i].transform.parent;
            if (parent != inventorySlotsParent[i].transform &&
                parent != hotkeySlots[0].transform &&
                parent != hotkeySlots[1].transform &&
                parent != hotkeySlots[2].transform &&
                parent != hotkeySlots[3].transform &&
                parent != hotkeySlots[4].transform)
            {
                ReturnItem(i);
            }
        }
    }


    public void DeactivateItem(int itemIndex)
    {
        inventorySlots[itemIndex].GetComponent<Drag>().enabled = false;
        inventorySlots[itemIndex].GetComponent<Image>().color = new Color(1, 1, 1, 50);
    }

    #endregion


    #region ��Ű ����
    // ��Ű ���� �ϱ�
    public void SelectHotkeySlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < hotkeySlots.Length)
        {
            selectedHotkeySlot = slotIndex;
            UpdateHotkeyHighlight();

            if (!isItemDescriptionTxtRunning)
            {
                StartCoroutine(ItemDescriptionPopUp());
            }
        }
    }

    //��Ű ���� �� ���̶���Ʈ
    public void UpdateHotkeyHighlight()
    {
        for (int i = 0; i < hotkeySlots.Length; i++)
        {
            if (i == selectedHotkeySlot)
            {
                hotkeySlots[i].GetComponent<Image>().color = new Color(1, 0.92f, 0.016f, 0.4f);
            }
            else
            {
                hotkeySlots[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
            }

            if (hotkeySlots[i].transform.childCount > 0)
            {
                usingItem = hotkeySlots[i].transform.GetChild(0).gameObject;
                if (usingItem != null)
                {
                    itemComponent[i] = usingItem.GetComponent<IItem>();
                    Debug.Log("ItemComponent: " + itemComponent[i].Name);

                    //hotkeyItemNames[i] = "";
                    //hotkeyItemName = "";
                    //hotkeyItemIndexes[i] = -1;
                    //hotkeyItemIndex = -1;

                    if (itemComponent != null)
                    {
                        if (i == selectedHotkeySlot)
                        {
                            hotkeyItemNames[i] = itemComponent[i].Name;
                            hotkeyItemName = hotkeyItemNames[i];
                            Debug.Log("HotkeyItemName: " + hotkeyItemName);
                            hotkeyItemIndexes[i] = itemComponent[i].Index;
                            hotkeyItemIndex = hotkeyItemIndexes[i];
                            Debug.Log("HotkeyItemIndex: " + hotkeyItemIndex);
                        }
                        else
                        {
                            hotkeyItemNames[i] = "";
                            //hotkeyItemName = "";
                            Debug.Log("reset non-Hotkey ItemName");
                            hotkeyItemIndexes[i] = -1;
                            //hotkeyItemIndex = -1;
                            Debug.Log("reset non-Hotkey ItemIndex");
                        }
                    }

                }
                else // ������� ��� �� �̸� ����
                {
                    hotkeyItemNames[i] = "";
                    // hotkeyItemName = "";
                    hotkeyItemIndexes[i] = -1;
                    // hotkeyItemIndex = -1;
                }
            }
            else
            {
                hotkeyItemNames[i] = "";
                // hotkeyItemName = "";
                hotkeyItemIndexes[i] = -1;
                // hotkeyItemIndex = -1;
            }
        }
    }

    private IEnumerator ItemDescriptionPopUp()
    {
        // hotkeyItemDescriptionPanel.SetActive(false);
        // StopCoroutine(ItemDescriptionPopUp());
        if (hotkeyItemDescriptionPanel.activeSelf)
        {
            StopCoroutine("ItemDescriptionPopUp");
        }
        

        if (itemComponent != null)
        {
            if (hotkeySlots[selectedHotkeySlot].transform.childCount > 0)
            {
                int itemIndex = hotkeyItemIndexes[selectedHotkeySlot];

                if (itemComponent[selectedHotkeySlot].Index == itemIndex)
                {
                    isItemDescriptionTxtRunning = true;

                    hotkeyItemDescriptionTxT.text = itemComponent[selectedHotkeySlot].Text;
                    hotkeyItemDescriptionPanel.SetActive(true);
                }
                yield return new WaitForSecondsRealtime(2f);
                hotkeyItemDescriptionTxT.text = "";
                isItemDescriptionTxtRunning = false;
                hotkeyItemDescriptionPanel.SetActive(false);
            }
        }

    }

    private void OnDisable()
    {
        hotkeyItemDescriptionPanel.SetActive(false);
        StopCoroutine(ItemDescriptionPopUp());
    }


    // ��Ű ���Կ� �ִ� ������ ���
    private void UseHotkeyItem()
    {
        UpdateHotkeyHighlight();

        if (selectedHotkeySlot != -1 && selectedHotkeySlot < hotkeySlots.Length)
        {
            if (hotkeySlots[selectedHotkeySlot].transform.childCount > 0)
            {
                if (usingItem != null)
                {
                    if (itemComponent != null)
                    {
                        switch (itemComponent[selectedHotkeySlot].Index)
                        {
                            case 0:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 1:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 2:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 3:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 4:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 5:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 6:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 7:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 8:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 9:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 10:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);

                                //// f�� �ȶ㶧�� ����
                                //if (!interaction_F.activeSelf)
                                //{
                                //    Item_Irys_Halo halo = GameObject.FindAnyObjectByType<Item_Irys_Halo>();
                                //    halo.HaloUse();
                                //}

                                break;
                            case 11:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 12:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 13:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 14:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 15:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 16:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 17:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 18:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 19:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 20:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 21:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 22:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 23:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 24:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 25:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 26:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 27:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 28:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 29:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 30:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 31:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 32:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 33:
                                //if (!interaction_F.activeSelf)
                                //{
                                //    Kiara_MemoUse memoUse = GameObject.FindAnyObjectByType<Kiara_MemoUse>();
                                //    memoUse.MemoUse();
                                //}

                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            case 34:
                                Debug.Log("Current Hotkey:" + selectedHotkeySlot + "Used item: " + itemComponent[selectedHotkeySlot].Name);
                                break;
                            default:
                                Debug.Log("No item in this slot");
                                break;
                        }
                    }
                    else
                    {
                        Debug.Log("Item component is null");
                    }
                }
                else
                {
                    Debug.Log("Using item is null");
                }
            }
            else
            {
                Debug.Log("No item in this slot");
            }
        }
    }

    #endregion


    #region �κ��丮 ������ ����
    // �κ��丮 ������ �̵� ��ũ��Ʈ��
    public void ShowPreviousPage()
    {
        if (currentPageIndex == 0)
        {
            currentPageIndex = inventoryPages.Length - 1;
        }
        else
        {
            currentPageIndex--;
        }
        UpdateInventoryPage();
    }
    public void ShowNextPage()
    {
        if (currentPageIndex == inventoryPages.Length - 1)
        {
            currentPageIndex = 0;
        }
        else
        {
            currentPageIndex++;
        }
        UpdateInventoryPage();
    }

    // ���� �κ��丮 ������ �ε�
    private void UpdateInventoryPage()
    {
        for (int i = 0; i < inventoryPages.Length; i++)
        {
            //deactivate every pages except for current page
            inventoryPages[i].SetActive(i == currentPageIndex);
        }
    }

    #endregion


    public IEnumerator ItemDescription(int index)
    {

        IItem itemComponent = inventorySlots[index].GetComponent<IItem>();
        if (itemComponent != null)
        {
            hotkeyItemDescriptionTxT.text = itemComponent.Text;
            hotkeyItemDescriptionPanel.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(2f);

        hotkeyItemDescriptionTxT.text = "";
        hotkeyItemDescriptionPanel.SetActive(false);

    }


    // �κ��丮 ����
    public void SaveInventory()
    {
        {
            // ȹ���� �����۵� ã�ƿ���
            List<IItem> collectedItems = new List<IItem>();

            foreach (GameObject slot in inventorySlots)
            {
                IItem itemComponent = slot.GetComponent<IItem>();

                if (itemComponent != null && itemComponent.isGetItem)
                {
                    collectedItems.Add(itemComponent);
                }
            }

            // ȹ���۵� �ε��� ã�ƿ���
            List<int> collectedItemIndexes = new List<int>();
            foreach (IItem item in collectedItems)
            {
                collectedItemIndexes.Add(item.Index);
            }

            // �������� ��ȯ
            string binaryString = "1";
            for (int i = 0; i < 34; i++)
            {
                if (collectedItemIndexes.Contains(i))
                {
                    binaryString += "1";
                }
                else
                {
                    binaryString += "0";
                }
            }

            // long���� ��ȯ
            long binaryLong = System.Convert.ToInt64(binaryString, 2);

            // int�� ��ȯ
            int binaryInt = (int)binaryLong;

            GameManager.instance.ItemData_save = binaryInt;
            Debug.Log("ItemData_save: " + GameManager.instance.ItemData_save);
        }
    }

    // �κ��丮 �ε�
    public void LoadInventory()
    {
        int binaryInt = GameManager.instance.ItemData_save;

        //string binaryString = System.Convert.ToString(binaryInt, 2);

        string tempSave = Convert.ToString(binaryInt, 2);
        Debug.Log("tempSave: " + tempSave);

        //�޾ƿ� int�� string���� ��ȯ �� �� �� 1 ����
        string binarySystem_LoadData_String = tempSave;
        binarySystem_LoadData_String = binarySystem_LoadData_String.Substring(1);
        Debug.Log("binarySystem_LoadData_String: " + binarySystem_LoadData_String);

        //string�� char[]�� ��ȯ �� ������ char�� int�� ��ȯ�Ͽ� �迭�� ����
        char[] binarySystem_LoadData_Char = binarySystem_LoadData_String.ToCharArray();

        for (int i = 0; i < binarySystem_LoadData_Char.Length; i++)
        {
            if (binarySystem_LoadData_Char[i] == '1')
            {
                IItem itemComponent = inventorySlots[i].GetComponent<IItem>();
                if (itemComponent != null)
                {
                    int itemIndex = itemComponent.Index;


                    //�κ��丮 �� ������ Ȱ��ȭ
                    inventorySlots[itemIndex].GetComponent<Drag>().enabled = true;
                    inventorySlots[itemIndex].GetComponent<Image>().color = Color.white;

                    inventorySlots[itemIndex].GetComponent<IItem>().isGetItem = true;

                    itemComponent.isGetItem = true;
                }

            }
        }
    }

    // �κ��丮 ����
    public void ResetInventory()
    {
        IItem temp = null;

        for (int i = 1; i < inventorySlots.Length; i++)
        {
            ReturnItem(i);
            inventorySlots[i].GetComponent<Drag>().enabled = false;
            inventorySlots[i].GetComponent<Image>().color = new Color(0, 0, 0, 255);

            temp = inventorySlots[i].GetComponent<IItem>();
            if (temp != null)
            {
                inventorySlots[i].GetComponent<IItem>().isGetItem = false;
            }

            if (GameManager.instance.Episode_Round == 2)
            {
                if (i != 6 || i != 7 || i != 8)
                {
                    inventorySlots[i].GetComponent<Image>().color = new Color(0, 0, 0, 255);
                }
            }
        }
        ItemInventroyReset();
        CollectItem(GameObject.Find("Item_09_InaBook"));

        //if (GameManager.instance.Episode_Round == 2)    // 2ȸ���� ���
        //{
        //    // en 1����� Ÿ�ڵ��� ȹ�� �Ϸ� ó�� �� ��� �Ϸ� ó���� �����ص�
        //    // 2ȸ���� ��� �� ���� �� �ֵ��� �صα� ����
        //    CollectItem(GameObject.FindAnyObjectByType<Item06TakoGura>().gameObject);
        //    CollectItem(GameObject.FindAnyObjectByType<Item07TakoCalli>().gameObject);
        //    CollectItem(GameObject.FindAnyObjectByType<Item08TakoKiara>().gameObject);
        //}
    }

    // �巡�� ������ ������ ���� ��ġ�� ���ư���
    public void IfDragError(int ItemIndex)
    {
        if (inventorySlots[ItemIndex].transform.parent == inventoryPanel.transform)
        {
            ReturnItem(ItemIndex);
        }

    }

    /// <summary>
    /// int �־�θ� ������ ȹ�� �Լ�
    /// </summary>
    /// <param ������ �ε�����ȣ="i"></param>
    public void EarnItem(int i)
    {
        IItem itemComponent = inventorySlots[i].GetComponent<IItem>();
        if (itemComponent != null)
        {
            int itemIndex = itemComponent.Index;


            //�κ��丮 �� ������ Ȱ��ȭ
            inventorySlots[itemIndex].GetComponent<Drag>().enabled = true;
            inventorySlots[itemIndex].GetComponent<Image>().color = Color.white;

            inventorySlots[itemIndex].GetComponent<IItem>().isGetItem = true;

            itemComponent.isGetItem = true;
        }
    }

    public void ItemInventroyReset() // �κ��丮 ù�������� ����
    {
        for (int i = 0; i < inventoryPages.Length; i++)
        {
            if(i == 0)
            {
                inventoryPages[i].gameObject.SetActive(true);
            }
            else
            {
                inventoryPages[i].gameObject.SetActive(false);
            }
        }
    }
}