using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item23CassetteTapeT : MonoBehaviour, IItem
{
    public int Index { get; set; } = 23;
    public string Name { get; set; } = "Item_23_CassetteTape_T";
    public string Icon { get; set; } = "Items/Icons/CassetteTape_T.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "The Grim Reaper is a Live-Streamer\n- Mori Calliope (2021. 8. 2.)";
}