using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item02TakoIRyS : MonoBehaviour, IItem
{
    public int Index { get; set; } = 2;
    public string Name { get; set; } = "Item_02_TakoIRyS";
    public string Icon { get; set; } = "Items/Icons/TakoIRyS.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "타코 아이리스\n아이리스의 모습을 한 타코다치다.";
}
