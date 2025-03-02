using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    public List<GameObject> takos;

    public Transform[] spawnPos;

    GameObject player;
    PlayerCtrl playerCtrl;


    void Start()
    {
        RndSpawn_Tako();
        StartCoroutine(LoadScene());
        player = GameObject.Find("Player");
        player.transform.position = new Vector3(10000, 10000, 500);
        playerCtrl = player.GetComponent<PlayerCtrl>();
        playerCtrl.LoadingSceneCloseAll();
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        GameManager.instance.nowSceneName = sceneName;
        SceneManager.LoadScene("09. LoadingScene");
        // 씬전환할 때 이 함수 불러서 sceneName에 가고싶은 씬 적으면 로딩 후 씬전환
    }

    void RndSpawn_Tako()
    {
        for (int i = 0; i < 3; i++)
        {
            int rndTako = Random.Range(0, takos.Count);
            GameObject tako = Instantiate(takos[rndTako]);
            tako.transform.position = spawnPos[i].transform.position;
            tako.transform.parent = spawnPos[i].transform;
            tako.transform.Rotate(tako.transform.rotation.x, tako.transform.rotation.y, tako.transform.rotation.z + (i * 6));

            if (tako.name == "TakoSana_Loding(Clone)") // 캔버스라 오브젝트들이 쥐꼬리만해져서 이거 추가함 사나는 다른애들보다 더 커서 따로 뺴고
            {
                tako.transform.localScale = new Vector3(50, 50, 50);
            }
            else
            {
                tako.transform.localScale = new Vector3(100, 100, 100);
            }
            
            takos.RemoveAt(rndTako);
        }
    }

    public float scenetime = 5f; //대기시간
    IEnumerator LoadScene() // 0.1초만에 다음씬으로 바로넘어가는거 잠깐 대기하기위해
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; // 씬전환 바로되는거 멈추고
        float timer = 0.0f;
        StartCoroutine(TakosMove(!op.isDone));
        while (!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime; 
            if (timer > scenetime) // 지정한 시간동안 대기 후
            {
                // 여기다가 이벤트 띄우려면 띄우고

                // 씬 넘어갈때 저장 1회 진행
                // GameManager.instance.SaveData(); // 240930 테스트를 위해 잠시 주석처리 > 문짝에다 달아서 없애도 될듯
                if (nextScene == "01. IntroScene") // 로딩씬 갈때만 커서 켜주고 플레이어 잠긴 상태에서 넘어가기
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    playerCtrl.keystrokes = false;
                    playerCtrl.characterCanvas.gameObject.SetActive(true); // 그 외 플레이어 액션 인풋 및 핫 키등 캔버스 활성화
                }


                op.allowSceneActivation = true; // 씬전환해주기
                yield break;

            }

        }
    }

    public float takoMoveSpeed = 0.1f;

    IEnumerator TakosMove(bool SceneLoad)
    {
        while(SceneLoad)
        {
            for(int i = 0; i < spawnPos.Length; i++)
            {
                Vector3 targetPos = new Vector3(0, 0.3f, 0);
                spawnPos[i].GetChild(0).transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * takoMoveSpeed);
                yield return new WaitForSeconds(0.1f);                
            }
            for (int i = 0; i < spawnPos.Length; i++)
            {
                Vector3 targetPos = new Vector3(0, -0.3f, 0);
                spawnPos[i].GetChild(0).transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime* takoMoveSpeed);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
