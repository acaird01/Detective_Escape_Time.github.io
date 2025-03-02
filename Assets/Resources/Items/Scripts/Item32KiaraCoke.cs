using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item32KiaraCoke : MonoBehaviour, IItem
{
    public int Index { get; set; } = 32;
    public string Name { get; set; } = "Item_32_Kiara_Coke";
    public string Icon { get; set; } = "Items/Icons/Coke.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "KFP 특제 콜라\n청량하고 시원한 콜라.";
}
