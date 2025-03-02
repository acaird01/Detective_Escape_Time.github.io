using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item24CassetteTapeH : MonoBehaviour, IItem
{
    public int Index { get; set; } = 24;
    public string Name { get; set; } = "Item_24_CassetteTape_H";
    public string Icon { get; set; } = "Items/Icons/CassetteTape_H.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "HUGE W\n- Mori Calliope (2022. 3. 20.)";
}