using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Calli_ClockFace : MonoBehaviour
{
    // AudioSource audioSource;
    //Interaction_Gimics interaction;

    private void Start()
    {
        //interaction = gameObject.GetComponent<Interaction_Gimics>();
        // saudioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        //Time.timeScale = 1;
        if (!isRotating)
        {
            StartCoroutine(RotateClocks());
        }

    }


    public float rotationAngle = 36f;  // 회전할 각도
    public float rotationDuration = 0.5f; // 회전에 걸리는 시간
    private bool isRotating = false; // 회전 중임을 체크할 변수

    // 코루틴을 사용하여 회전
    IEnumerator RotateClocks()
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        //Quaternion endRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(-rotationAngle, 0, 0));
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, rotationAngle);

        //        audioSource.Play();
        float elapsedTime = 0;
        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.rotation = endRotation;
        isRotating = false;
        //interaction.run_Gimic = false;
    }
}
