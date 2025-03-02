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
        // ����ȯ�� �� �� �Լ� �ҷ��� sceneName�� ������� �� ������ �ε� �� ����ȯ
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

            if (tako.name == "TakoSana_Loding(Clone)") // ĵ������ ������Ʈ���� �㲿���������� �̰� �߰��� �糪�� �ٸ��ֵ麸�� �� Ŀ�� ���� ����
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

    public float scenetime = 5f; //���ð�
    IEnumerator LoadScene() // 0.1�ʸ��� ���������� �ٷγѾ�°� ��� ����ϱ�����
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; // ����ȯ �ٷεǴ°� ���߰�
        float timer = 0.0f;
        StartCoroutine(TakosMove(!op.isDone));
        while (!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime; 
            if (timer > scenetime) // ������ �ð����� ��� ��
            {
                // ����ٰ� �̺�Ʈ ������ ����

                // �� �Ѿ�� ���� 1ȸ ����
                // GameManager.instance.SaveData(); // 240930 �׽�Ʈ�� ���� ��� �ּ�ó�� > ��¦���� �޾Ƽ� ���ֵ� �ɵ�
                if (nextScene == "01. IntroScene") // �ε��� ������ Ŀ�� ���ְ� �÷��̾� ��� ���¿��� �Ѿ��
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    playerCtrl.keystrokes = false;
                    playerCtrl.characterCanvas.gameObject.SetActive(true); // �� �� �÷��̾� �׼� ��ǲ �� �� Ű�� ĵ���� Ȱ��ȭ
                }


                op.allowSceneActivation = true; // ����ȯ���ֱ�
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
