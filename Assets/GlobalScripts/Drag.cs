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

    // 드래그핸들러 인터페이스의 메서드 구현
    // 인터페이스가 가지고 있는 메서드는 반드시 자식 클래스에서 구현해야함
    // 이것이 바로 오버라이딩
    // 마우스 드래그중 일 때 (Stay)
    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }

    // 마우스 드래그가 시작될 때 호출되는 메서드 (Enter)
    public void OnBeginDrag(PointerEventData eventData)
    {
        StopCoroutine("ItemDescription");
        StartCoroutine(ItemManager._instance.ItemDescription(ItemIndex));

        positionBefore = itemTr.position;

        // 부모를 Inventory로 변경한다
        this.transform.SetParent(inventoryTr);
        // 드래그가 시작될 때 드래그되는 아이템 정보 저장
        draggingItem = this.gameObject;

        // 드래그가 시작될때 다른 UI 이벤트를 받지 않도록 설정함
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 끝날 때 드래그 아이템을 null로 설정
        draggingItem = null;
        // 드래그가 끝나면 다시 UI 이벤트 활성화함
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
            // Slot에 아이템을 놓지 않으면
            // 다시 itemListTr로 되돌아간다
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

