using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MainHall_AmeClock : MonoBehaviour
{
    [Header("아메 시계를 움직여줄 animator")]
    [SerializeField]
    private Animator animator;
    [Header("아메 시계가 소환될 때 재생할 이펙트")]
    [SerializeField]
    private ParticleSystem particle;
    [Header("아메 시계가 소환될 때 재생할 Audio Source")]
    [SerializeField]
    private AudioSource audioSource;
    [Header("아메 시계 게임 오브젝트")]
    [SerializeField]
    private GameObject ameClock;

    /// <summary>
    /// 아메 시계가 소환될때 애니메이션 및 이펙트를 재생할 함수
    /// </summary>
    public IEnumerator ameAnimationStart()
    {
        particle.gameObject.SetActive(true);    // 파티클 시스템 활성화

        animator.Play("");
        particle.Play();
        audioSource.Play();

        yield return new WaitForSeconds(2f);    // 2초간 대기

        particle.Stop();
    }
}
