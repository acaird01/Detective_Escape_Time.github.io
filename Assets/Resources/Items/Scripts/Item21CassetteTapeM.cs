using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item21CassetteTapeM : MonoBehaviour, IItem
{
    public int Index { get; set; } = 21;
    public string Name { get; set; } = "Item_21_CassetteTape_M";
    public string Icon { get; set; } = "Items/Icons/CassetteTape_M.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "MERA MERA\n- Mori Calliope (2022. 5. 18.)";
}