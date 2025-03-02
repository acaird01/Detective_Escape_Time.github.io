using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_NumSafeHint : MonoBehaviour
{

    TextController textController;
    [TextArea]
    [SerializeField]
    string _text = "";

    bool NumSafeHint = true;

    private void Start()
    {
        textController = GameObject.Find("Player").GetComponentInChildren<TextController>();

    }

    private float cooldown = 10f;
    private bool isCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (NumSafeHint && !isCooldown)
        {
            StartCoroutine(SendText());
            StartCoroutine(StartCooldown());
        }
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }

    IEnumerator SendText() // 아이템(상호작용 하는 오브젝트)에서 텍스트 받아와서 텍스트 출력
    {
        NumSafeHint = false;

        if (_text != "")
        {
            StartCoroutine(textController.SendText(_text));
        }

        yield return new WaitForSeconds(3f);

        NumSafeHint = true;
    }
}
