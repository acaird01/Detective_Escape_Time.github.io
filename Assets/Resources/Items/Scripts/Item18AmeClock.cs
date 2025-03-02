using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item18AmeClock : MonoBehaviour, IItem
{
    public int Index { get; set; } = 18;
    public string Name { get; set; } = "Item_18_AmeClock";
    public string Icon { get; set; } = "Items/Icons/AmeClock.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "아메의 회중시계\n누군가로부터 빌린 시간여행을 할 수 있게 해주는 시계다.";
}
