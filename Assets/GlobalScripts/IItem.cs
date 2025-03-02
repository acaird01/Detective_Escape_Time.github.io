using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    int Index { get; set; }
    string Name { get; set; }
    string Icon { get; set; }
    bool isGetItem { get; set; }
    string Text { get; set; }
}
