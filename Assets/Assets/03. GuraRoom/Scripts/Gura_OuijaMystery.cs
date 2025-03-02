using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_OuijaMystery : MonoBehaviour
{
    private AudioSource audioSource;
    private float minTime = 120f; // 2 minutes in seconds
    private float maxTime = 300f; // 5 minutes in seconds

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayAudioRandomly());
    }

    private IEnumerator PlayAudioRandomly()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);
            audioSource.Play();
        }
    }
}
