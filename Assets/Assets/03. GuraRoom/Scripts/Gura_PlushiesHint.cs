using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_PlushiesHint : MonoBehaviour
{
    GameObject player;
    GameObject textBox;

    [SerializeField]
    private GameObject hintImage; // Image to be shown

    private void Start()
    {
        player = GameObject.Find("Player");
        textBox = player.GetComponentInChildren<TextController>().gameObject;

        hintImage.SetActive(false); // Ensure the image is hidden at the start
    }

    private void OnMouseOver()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist < 10)
        {
            if (textBox.activeSelf)

            {
                hintImage.SetActive(true);
            }
            else
            {
                hintImage.SetActive(false);
            }
        }
    }

    private void OnMouseExit()
    {
        hintImage.SetActive(false);
    }
}









