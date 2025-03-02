using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item07TakoCalli : MonoBehaviour, IItem
{
    public int Index { get; set; } = 7;
    public string Name { get; set; } = "Item_07_TakoCalli";
    public string Icon { get; set; } = "Items/Icons/TakoCali.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "타코 칼리\n칼리의 모습을 한 타코다치다.";
}
