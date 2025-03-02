using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item03TakoKronii : MonoBehaviour, IItem
{
    public int Index { get; set; } = 3;
    public string Name { get; set; } = "Item_03_TakoKronii";
    public string Icon { get; set; } = "Items/Icons/TakoKronii.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "타코 크로니\n크로니의 모습을 한 타코다치다.";
}
