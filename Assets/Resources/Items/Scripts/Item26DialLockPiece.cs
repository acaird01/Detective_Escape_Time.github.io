using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item26DialLockPiece : MonoBehaviour, IItem
{
    public int Index { get; set; } = 26;
    public string Name { get; set; } = "Item_26_DialLockPiece";
    public string Icon { get; set; } = "Items/Icons/DialLockPiece.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "�ڹ��� ����\n��򰡿��� ���� ���� �ڹ����� ����";
}