using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        // Slot의 자식의 갯수가 0 이라는 의미는
        // 하위 오브젝트가 없는 경우를 말한다
        if (transform.childCount == 0)
        {
           // 드래그 중인 아이템을 slot의 자식으로 등록한다
           Drag.draggingItem.transform.SetParent(this.transform);
        }
        //else
        //{
        //    //기존 칸에 아이템 있는경우 그 아이템을 다시 원래 칸으로 이동
        //    GameObject existingItem = transform.GetChild(0).gameObject;
        //    int existingIndex = existingItem.GetComponent<IItem>().Index;
        //    existingItem.transform.SetParent(ItemManager._instance.inventorySlots[existingIndex].transform);

        //    // 드래그 중인 아이템을 slot의 자식으로 등록한다
        //    Drag.draggingItem.transform.SetParent(this.transform);
        //}
    }


   
}