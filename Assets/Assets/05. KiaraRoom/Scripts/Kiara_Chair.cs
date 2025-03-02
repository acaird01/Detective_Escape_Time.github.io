using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Chair : MonoBehaviour
{
    public bool GimicMove = false; // true�� �� ���� �Ȱ�
    // ���ڵ��� ��� �迭�����
    // ������ �������� �迭�� ��� ���ڵ� ��ġ
    public float TouchNewRot_Y = 0;
    AudioSource audioSource;
    Interaction_Gimics interaction;

    private void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        //interaction.run_Gimic = false; // ���߿� �����!! �׽�Ʈ��!!!!!!!!!!!!!
        audioSource = GetComponent<AudioSource>();


    }
    private void Update()
    {
        GimicMove = interaction.run_Gimic;
        WaitTouch();
    }

    void SetZeroRot_Y()
    {
        if(transform.localEulerAngles.y < 1 && transform.localEulerAngles.y > 359)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (transform.localEulerAngles.y < 91 && transform.localEulerAngles.y > 89)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        if (transform.localEulerAngles.y < 181 && transform.localEulerAngles.y > 179)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (transform.localEulerAngles.y < 271 && transform.localEulerAngles.y > 269)
        {
            transform.localEulerAngles = new Vector3(0, 270, 0);
        }
    }


    void WaitTouch()
    {
        if (GimicMove == true)
        {
            TouchNewRot_Y += 30;
            transform.localEulerAngles = new Vector3(0, TouchNewRot_Y, 0);
            SetZeroRot_Y();
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            interaction.run_Gimic = false;
        }
    }
}
