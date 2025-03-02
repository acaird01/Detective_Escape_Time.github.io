using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHic : MonoBehaviour
{
    private AudioSource audioSource;
    private float minTime = 60f;  // 1 minutes in seconds
    private float maxTime = 180f; // 3 minutes in seconds

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