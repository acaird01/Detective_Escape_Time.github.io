using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecretCode : MonoBehaviour
{
    [SerializeField]
    GameObject Credits;


    void Update()
    {

        if (currentIndex < konamiCode.Length)
        {
            // Check if the current key in the Konami code sequence has been pressed
            if (Input.GetKeyDown(konamiCode[currentIndex]))
            {
                currentIndex++; // Move to the next key in the sequence
            }
            else if (Input.anyKeyDown)
            {
                currentIndex = 0; // Reset the sequence if the wrong key is pressed
            }

            // If all keys in the sequence have been pressed
            if (currentIndex >= konamiCode.Length)
            {
                StartCoroutine(TriggerKonamiCode()); // Call the coroutine
                currentIndex = 0; // Reset the sequence for future use
            }
        }
    }


        private readonly KeyCode[] konamiCode = {
            KeyCode.UpArrow, KeyCode.UpArrow,
            KeyCode.DownArrow, KeyCode.DownArrow,
            KeyCode.LeftArrow, KeyCode.RightArrow,
            KeyCode.LeftArrow, KeyCode.RightArrow,
            KeyCode.B, KeyCode.A
        };

        private int currentIndex = 0;

    // The coroutine that shows the Credits for 5 seconds
    private IEnumerator TriggerKonamiCode()
    {
        Debug.Log("Konami Code Activated!");

        //Instantiate the Credits image
        Credits.gameObject.SetActive(true);


        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Destroy the Credits image
        Credits.gameObject.SetActive(false);


        for (int i = 1; i < 9; i++)
        {
            ItemManager._instance.EarnItem(i);
        }

    }
}
