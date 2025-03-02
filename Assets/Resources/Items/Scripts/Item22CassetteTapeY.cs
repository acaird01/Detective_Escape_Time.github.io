using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item22CassetteTapeY : MonoBehaviour, IItem
{
    public int Index { get; set; } = 22;
    public string Name { get; set; } = "Item_22_CassetteTape_Y";
    public string Icon { get; set; } = "Items/Icons/CassetteTape_Y.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "You're Not Special\n- Mori Calliope (2023. 8. 17.)";
}