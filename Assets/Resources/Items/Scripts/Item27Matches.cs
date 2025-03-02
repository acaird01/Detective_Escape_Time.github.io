using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item27Matches : MonoBehaviour, IItem
{
    public int Index { get; set; } = 27;
    public string Name { get; set; } = "Item_27_Matches";
    public string Icon { get; set; } = "Items/Icons/Matches.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "평범한 성냥\n아직 불을 붙이기에 문제 없을 듯 하다.";
}