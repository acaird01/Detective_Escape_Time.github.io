using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item13KiaraFeather : MonoBehaviour,IItem
{
    public int Index { get; set; } = 13;
    public string Name { get; set; } = "Item13_Kiara_PhoneixFeather";
    public string Icon { get; set; } = "Items/Icons/PhoneixFeather.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "�һ����� ����\n�һ����� ���� �Ѱ���. �ҽ�ð��� ���� ���� �� ����.";
}
