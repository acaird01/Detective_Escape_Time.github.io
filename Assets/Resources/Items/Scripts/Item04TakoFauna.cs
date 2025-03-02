using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item04TakoFauna : MonoBehaviour, IItem
{
    public int Index { get; set; } = 4;
    public string Name { get; set; } = "Item_04_TakoFauna";
    public string Icon { get; set; } = "Items/Icons/TakoFauna.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "Ÿ�� �Ŀ쳪\n�Ŀ쳪�� ����� �� Ÿ�ڴ�ġ��.";
}
