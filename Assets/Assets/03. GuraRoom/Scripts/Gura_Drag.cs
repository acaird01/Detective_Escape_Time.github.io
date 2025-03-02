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

    // �巡���ڵ鷯 �������̽��� �޼��� ����
    // �������̽��� ������ �ִ� �޼���� �ݵ�� �ڽ� Ŭ�������� �����ؾ���
    // �̰��� �ٷ� �������̵�
    // ���콺 �巡���� �� �� (Stay)
    public void OnDrag(PointerEventData eventData)
    {
        EN2Tr.position = Input.mousePosition;
    }

    // ���콺 �巡�װ� ���۵� �� ȣ��Ǵ� �޼��� (Enter)
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �θ� Inventory�� �����Ѵ�
        this.transform.SetParent(StatueHintTr);
        // �巡�װ� ���۵� �� �巡�׵Ǵ� ������ ���� ����
        draggingEN2 = this.gameObject;

        // �巡�װ� ���۵ɶ� �ٸ� UI �̺�Ʈ�� ���� �ʵ��� ������
        StatuecanvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�װ� ���� �� �巡�� �������� null�� ����
        draggingEN2 = null;
        // �巡�װ� ������ �ٽ� UI �̺�Ʈ Ȱ��ȭ��
        StatuecanvasGroup.blocksRaycasts = true;

        if (EN2Tr.parent == StatueHintTr)
        {
            Debug.Log("����ġ");
            // Slot�� �������� ���� ������
            // �ٽ� StatueAnswerTr�� �ǵ��ư���
            EN2Tr.position = originPosition; // Move the object back to the original position
        }
    }
}

