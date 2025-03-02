using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item09InaBook : MonoBehaviour, IItem
{
    public int Index { get; set; } = 9;
    public string Name { get; set; } = "Item_09_InaBook";
    public string Icon { get; set; } = "Items/Icons/InaBook.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "AO-chan\n이나의 책이다. 뭔가 이상한 기운이 느껴지는 듯하다.";
}