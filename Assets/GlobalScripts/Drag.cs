using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static GameObject draggingItem = null;

    [SerializeField]
    Transform itemTr;
    [SerializeField]
    Transform cellTr;
    [SerializeField]
    Transform inventoryTr;
    [SerializeField]
    Transform itemListTr;
    CanvasGroup canvasGroup;

    Vector3 positionBefore; // Added variable to store the position before dragging
    Vector3 originalPosition; // Added variable to store the original position

    public int ItemIndex;

    void Start()
    {
        itemTr = GetComponent<Transform>();
        inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
        cellTr = GameObject.Find("Cell").GetComponent<Transform>();
        itemListTr = GameObject.Find("Hotkey").GetComponent<Transform>();

        canvasGroup = GetComponent<CanvasGroup>();

        originalPosition = itemTr.position; // Store the original position


        //find the index of the item
        IItem itemComponent = GetComponent<IItem>();
        if (itemComponent != null)
        {
            ItemIndex = itemComponent.Index;
        }
        if (itemComponent == null)
        {
            // Debug.Log("ItemComponent is null on object: " + gameObject.name);
        }
    }

    // �巡���ڵ鷯 �������̽��� �޼��� ����
    // �������̽��� ������ �ִ� �޼���� �ݵ�� �ڽ� Ŭ�������� �����ؾ���
    // �̰��� �ٷ� �������̵�
    // ���콺 �巡���� �� �� (Stay)
    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }

    // ���콺 �巡�װ� ���۵� �� ȣ��Ǵ� �޼��� (Enter)
    public void OnBeginDrag(PointerEventData eventData)
    {
        StopCoroutine("ItemDescription");
        StartCoroutine(ItemManager._instance.ItemDescription(ItemIndex));

        positionBefore = itemTr.position;

        // �θ� Inventory�� �����Ѵ�
        this.transform.SetParent(inventoryTr);
        // �巡�װ� ���۵� �� �巡�׵Ǵ� ������ ���� ����
        draggingItem = this.gameObject;

        // �巡�װ� ���۵ɶ� �ٸ� UI �̺�Ʈ�� ���� �ʵ��� ������
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�װ� ���� �� �巡�� �������� null�� ����
        draggingItem = null;
        // �巡�װ� ������ �ٽ� UI �̺�Ʈ Ȱ��ȭ��
        canvasGroup.blocksRaycasts = true;


        //if itemTr.parent has already child in it, get child's IItem component's Index and run IfDragError with that Index
        if (itemTr.parent.childCount > 0)
        {
            Transform child = itemTr.parent.GetChild(0); // Get the first child of itemTr's parent
            IItem childItem = child.GetComponent<IItem>(); // Get the IItem component of the child
            if (childItem != null)
            {
                int childIndex = childItem.Index; // Get the index from the IItem component
                ItemManager._instance.IfDragError(childIndex); // Call IfDragError with the child's index
            }
        }

        if (itemTr.parent == inventoryTr)
        {
            Debug.Log("Inventory");
            // Slot�� �������� ���� ������
            // �ٽ� itemListTr�� �ǵ��ư���
            itemTr.position = positionBefore; // Move the object back to the original position

        }
        else if (itemTr.parent == cellTr)
        {
            Debug.Log("Cell");

            this.transform.SetParent(cellTr);

        }
        else if (itemTr.parent == itemListTr)
        {
            Debug.Log("Hotkey");

            this.transform.SetParent(itemListTr);
        }

        ItemManager._instance.IfDragError(ItemIndex);
        ItemManager._instance.ReturnIfMultiItem();

    }

    private void OnDisable()
    {
        ItemManager._instance.IfDragError(ItemIndex);
    }

}

