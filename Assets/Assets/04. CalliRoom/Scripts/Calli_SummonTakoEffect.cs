using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_SummonTakoEffect : MonoBehaviour
{
    //[SerializeField]
    //GameObject OuijaBoardCanvas;
    // [SerializeField]
    GameObject takoPosition;

    [Header("����� ��ƼŬ ����Ʈ")]
    [SerializeField]
    GameObject effectLight;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        // StartCoroutine(KroniiSwordUp());
        takoPosition = GameObject.FindAnyObjectByType<TakoPosition_Lantern>().gameObject;   // Ÿ�� �ִϸ����͸� ���� ������Ʈ
        animator = takoPosition.GetComponent<Animator>();
        audioSource = takoPosition.GetComponent<AudioSource>();
    }
    /// <summary>
    /// Ÿ�� ��ġ�� �Ű��ִ� �ڷ�ƾ �Լ� ����
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
