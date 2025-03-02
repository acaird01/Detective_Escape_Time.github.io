using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_KroniiSword : MonoBehaviour
{

    [SerializeField]
    GameObject OuijaBoardCanvas;
    [SerializeField]
    GameObject KroniiSword;
    [SerializeField]
    GameObject effectLight;


    void Start()
    {
        StartCoroutine(KroniiSwordUp());
    }

    IEnumerator KroniiSwordUp()
    {
        yield return new WaitUntil(() => OuijaBoardCanvas.GetComponent<Gura_OuijaBoardCanvas>().isKroniiSwordUp == true);

        yield return new WaitForSeconds(1.5f);

        effectLight.SetActive(true);
        effectLight.GetComponent<ParticleSystem>().Play();

        KroniiSword.GetComponent<Animator>().Play("KroniiSword");

        KroniiSword.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(5f);

        effectLight.GetComponent<ParticleSystem>().Stop();

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
    }

    public void KroniiUp()
    {
        KroniiSword.GetComponent<Animator>().Play("KroniiSword");
    }
}
