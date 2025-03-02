using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item25Torch : MonoBehaviour, IItem
{
    public int Index { get; set; } = 25;
    public string Name { get; set; } = "Item_25_Torch";
    public string Icon { get; set; } = "Items/Icons/Torch.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "횃불\n불이 잘 붙을 것 같은 막대기";
}