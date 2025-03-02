using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item30KiaraChicken : MonoBehaviour, IItem
{
    public int Index { get; set; } = 30;
    public string Name { get; set; } = "Item_30_Kiara_Chicken";
    public string Icon { get; set; } = "Items/Icons/Chicken.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "KFP 치킨\n맛있어 보이는 치킨이다.\n(저희는 직원을 튀긴 적이 없습니다.)";
}
