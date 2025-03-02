using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item17Mumei_Feather : MonoBehaviour, IItem
{
    public int Index { get; set; } = 17;
    public string Name { get; set; } = "Item_17_Kiara_Mumei_Feather";
    public string Icon { get; set; } = "Items/Icons/Mumei_Feather.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "올빼미의 머리깃털\n올빼미의 작은 깃털.\n어째선지 문명을 소생시키는 기운을 뿜는다.";
}
