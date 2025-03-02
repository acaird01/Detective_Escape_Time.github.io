using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item08TakoKiara : MonoBehaviour, IItem
{
    public int Index { get; set; } = 8;
    public string Name { get; set; } = "Item_08_TakoKiara";
    public string Icon { get; set; } = "Items/Icons/TakoKiara.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "타코 키아라\n키아라의 모습을 한 타코다치다.";
}