using cakeslice;
using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_Poseidon : MonoBehaviour
{
    [SerializeField]
    private GameObject NumSafe;
    [SerializeField]
    private GameObject NumSafeCanvas;
    [SerializeField]
    private Camera PoseidonCam;
    [SerializeField]
    GameObject smokePrefab;

    void Start()
    {
        GetComponent<Outline>().enabled = false;

        StartCoroutine(PoseidonUp());

    }

    IEnumerator PoseidonUp()
    {
        yield return new WaitUntil(() => NumSafe.GetComponent<Gura_NumSafe>().isPoseidonUp == true);

        PoseidonCam.gameObject.SetActive(true);

        //play smoke effect
        smokePrefab.SetActive(true);
        smokePrefab.GetComponent<ParticleSystem>().Play();

        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().Play("PoseidonUp");

        StartCoroutine(ShakePoseidonCam());
        yield return new WaitForSeconds(7f);
        GetComponent<AudioSource>().Stop();

        PoseidonCam.gameObject.SetActive(false);

        GetComponent<Outline>() .enabled = true;
    }

    IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPosition = PoseidonCam.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            PoseidonCam.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        PoseidonCam.transform.localPosition = originalPosition;
    }

    IEnumerator ShakePoseidonCam()
    {
        float shakeDuration = 5f;
        float shakeMagnitude = 0.1f;

        yield return StartCoroutine(ShakeCamera(shakeDuration, shakeMagnitude));
    }
}
