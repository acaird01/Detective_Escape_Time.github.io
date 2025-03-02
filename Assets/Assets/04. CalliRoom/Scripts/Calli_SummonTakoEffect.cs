using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_SummonTakoEffect : MonoBehaviour
{
    //[SerializeField]
    //GameObject OuijaBoardCanvas;
    // [SerializeField]
    GameObject takoPosition;

    [Header("재생할 파티클 이펙트")]
    [SerializeField]
    GameObject effectLight;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        // StartCoroutine(KroniiSwordUp());
        takoPosition = GameObject.FindAnyObjectByType<TakoPosition_Lantern>().gameObject;   // 타코 애니메이터를 가진 오브젝트
        animator = takoPosition.GetComponent<Animator>();
        audioSource = takoPosition.GetComponent<AudioSource>();
    }
    /// <summary>
    /// 타코 위치를 옮겨주는 코루틴 함수 실행
    /// </summary>
    public void animationStart()
    {
        StartCoroutine(TakoPosUp());
    }

    private IEnumerator TakoPosUp()
    {
        // yield return new WaitUntil(() => OuijaBoardCanvas.GetComponent<Gura_OuijaBoardCanvas>().isKroniiSwordUp == true);

        effectLight.SetActive(true);
        effectLight.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(1.5f);

        animator.Play("TakoSummon");

        audioSource.Play();

        yield return new WaitForSeconds(5f);

        effectLight.GetComponent<ParticleSystem>().Stop();
    }
}
