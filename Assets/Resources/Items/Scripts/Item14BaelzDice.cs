using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item14BaelzDice : MonoBehaviour, IItem
{
        public int Index { get; set; } = 14;
        public string Name { get; set; } = "Item_14_BaelzDice";
        public string Icon { get; set; } = "Items/Icons/BaelzDice.png";
        public bool isGetItem { get; set; } = false;
        public string Text { get; set; } = "ȥ���� �ֻ���\nȥ���� ���� �������� �ֻ���.\n��ĩ�ϴ� ���� �������� �𸥴�.";
    
}
