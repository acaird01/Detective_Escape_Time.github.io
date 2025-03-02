using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calli_DialLock_BoxOpen : MonoBehaviour
{
    public bool dialLockOpen = false;
    public float rotationDuration = 0.5f; // 회전에 걸리는 시간
    bool GimicMove = false;

    public GameObject Chest_Lid;

    private AudioSource audioSource;            // Audio Source 컴포넌트

    Interaction_Gimics interaction;
    // Start is called before the first frame update
    void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        audioSource = gameObject.GetComponent<AudioSource>();
        GimicMove = interaction.run_Gimic;
        SceneStartSetting_Calli_BoxOpen();
        StartCoroutine(BoxOpenWait());
    }

   
    IEnumerator BoxOpenWait()
    {
        yield return new WaitUntil(() => dialLockOpen == true);
        yield return new WaitForSeconds(0.5f);

        //Chest_Lid.transform.localEulerAngles = new Vector3(-110f, 0, 0);

        audioSource.Play();

        Quaternion startRotation = Chest_Lid.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(-110, 0, 0);

        //        audioSource.Play();
        float elapsedTime = 0;

        while (elapsedTime < rotationDuration)
        {
            Chest_Lid.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Chest_Lid.transform.rotation = endRotation;
        interaction.run_Gimic = true;

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate(); // 기믹 종료 시점에 호출
    }


    public void SceneStartSetting_Calli_BoxOpen() // 여기선 한번만 실행되면 아이템을 얻고 끝이기에 실행 됬는지 여부만 가져올것
    {
        if (GimicMove)
        {
            Chest_Lid.transform.localEulerAngles = new Vector3(-110f, 0, 0);
        }
    }

}
