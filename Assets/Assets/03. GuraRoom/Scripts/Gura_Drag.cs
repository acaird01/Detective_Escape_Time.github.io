using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Gura_Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static GameObject draggingEN2 = null;
    Transform EN2Tr;
    Transform StatueHintTr;

    Transform StatueAnswerTr;
    CanvasGroup StatuecanvasGroup;

    Vector3 originPosition; // Added variable to store the original position

    void Start()
    {
        EN2Tr = GetComponent<Transform>();
        StatueHintTr = GameObject.Find("Hint")?.GetComponent<Transform>(); // Add null-conditional operator

        if (StatueHintTr == null)
        {
            return;
        }

        StatueAnswerTr = GameObject.Find("Answer")?.GetComponent<Transform>(); // Add null-conditional operator

        if (StatueAnswerTr == null)
        {
            return;
        }

        StatuecanvasGroup = GetComponent<CanvasGroup>();

        originPosition = EN2Tr.position; // Store the original position
    }

    // 드래그핸들러 인터페이스의 메서드 구현
    // 인터페이스가 가지고 있는 메서드는 반드시 자식 클래스에서 구현해야함
    // 이것이 바로 오버라이딩
    // 마우스 드래그중 일 때 (Stay)
    public void OnDrag(PointerEventData eventData)
    {
        EN2Tr.position = Input.mousePosition;
    }

    // 마우스 드래그가 시작될 때 호출되는 메서드 (Enter)
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 부모를 Inventory로 변경한다
        this.transform.SetParent(StatueHintTr);
        // 드래그가 시작될 때 드래그되는 아이템 정보 저장
        draggingEN2 = this.gameObject;

        // 드래그가 시작될때 다른 UI 이벤트를 받지 않도록 설정함
        StatuecanvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 끝날 때 드래그 아이템을 null로 설정
        draggingEN2 = null;
        // 드래그가 끝나면 다시 UI 이벤트 활성화함
        StatuecanvasGroup.blocksRaycasts = true;

        if (EN2Tr.parent == StatueHintTr)
        {
            Debug.Log("원위치");
            // Slot에 아이템을 놓지 않으면
            // 다시 StatueAnswerTr로 되돌아간다
            EN2Tr.position = originPosition; // Move the object back to the original position
        }
    }
}

