using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item05TakoBae : MonoBehaviour, IItem
{
    public int Index { get; set; } = 5;
    public string Name { get; set; } = "Item_05_TakoBae";
    public string Icon { get; set; } = "Items/Icons/TakoBae.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "타코 벨즈\n벨즈의 모습을 한 타코다치다.";
}
