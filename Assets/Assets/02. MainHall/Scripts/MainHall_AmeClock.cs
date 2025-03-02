using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MainHall_AmeClock : MonoBehaviour
{
    [Header("�Ƹ� �ð踦 �������� animator")]
    [SerializeField]
    private Animator animator;
    [Header("�Ƹ� �ð谡 ��ȯ�� �� ����� ����Ʈ")]
    [SerializeField]
    private ParticleSystem particle;
    [Header("�Ƹ� �ð谡 ��ȯ�� �� ����� Audio Source")]
    [SerializeField]
    private AudioSource audioSource;
    [Header("�Ƹ� �ð� ���� ������Ʈ")]
    [SerializeField]
    private GameObject ameClock;

    /// <summary>
    /// �Ƹ� �ð谡 ��ȯ�ɶ� �ִϸ��̼� �� ����Ʈ�� ����� �Լ�
    /// </summary>
    public IEnumerator ameAnimationStart()
    {
        particle.gameObject.SetActive(true);    // ��ƼŬ �ý��� Ȱ��ȭ

        animator.Play("");
        particle.Play();
        audioSource.Play();

        yield return new WaitForSeconds(2f);    // 2�ʰ� ���

        particle.Stop();
    }
}
