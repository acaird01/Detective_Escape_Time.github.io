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

    IEnumerator SendText() // ������(��ȣ�ۿ� �ϴ� ������Ʈ)���� �ؽ�Ʈ �޾ƿͼ� �ؽ�Ʈ ���
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
