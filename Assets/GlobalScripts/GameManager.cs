using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

[Serializable] // ����ȭ
public class Data // ������ �ѹ��� �����ϱ� ���� ���� class
{
    public int[] saveDatas = new int[12]; // ��Ʈ�� 4�� ���� ���� �ҷ��ٰ� �� �Ȱ��Ƽ� �迭�� ���� �׳�
    // 0 : ȸ��, 1 : �����, 2 : Į����, 3 : Ű�ƶ� ��, 4 : ���� ��, 5 : ������ ����, 6 : ������ ����, 7 : BGM ����, 8 : SFX����, 9: ���丮 ���� ��Ȳ ����(���� �߰����� ����)
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string PrevSceneName = null; // ������ �÷��̾ ��ġ�ߴ� ��
    public string nowSceneName = null;  // ���� �÷��̾ ��ġ�� ��

    TextController textcontroller;


    int episode_Round = 1;
    public int Episode_Round // ȸ�� ����
    {
        get { return episode_Round; }
        set { episode_Round = value; }
    }

    int guraScene_Save { get; set; }
    public int GuraScene_Save
    {
        get { return guraScene_Save; }
        set { guraScene_Save = value; }
    }

    int calliScene_Save { get; set; }
    public int CalliScene_Save
    {
        get { return calliScene_Save; }
        set { calliScene_Save = value; }
    }
    int kiaraScene_save { get; set; }
    public int KiaraScene_Save
    {
        get { return kiaraScene_save; } 
        set { kiaraScene_save = value; } 
    }

    int mainHallScene_Save { get; set; }
    public int MainHallScene_Save
    {
        get { return mainHallScene_Save; }
        set { mainHallScene_Save = value; }
    }
    // ������ �� �����Ȳ ������ ��Ʈ��

    int itemData_save { get; set; }
    public int ItemData_save
    {
        get { return itemData_save; } 
        set { itemData_save = value; }  
    } // ������ ������ ������ ��Ʈ��

    float masterVolume = 1;
    public float MasterVolume
    {
        get { return masterVolume; }
        set { masterVolume = value; }
    }
    float bgmVolume = 1;
    public float BGMVolume
    {
        get { return bgmVolume; }
        set { bgmVolume = value; }
    }
    float sfxVolume = 1;
    public float SFXVolume
    {
        get { return sfxVolume; }
        set { sfxVolume = value; }
    }
    private int isEndingStroryStart;
    public int IsEndingStroryStart
    {
        get { return isEndingStroryStart; }
        set { isEndingStroryStart = value; }
    }


    private int nowSceneNum;
    /// <summary>
    /// 2 : ���� / 3: ����1 / 4 : Į��1 / 5 : Ű�ƶ�1 / 6: ����2 / 7 : Į��2 / 8 : Ű�ƶ�2
    /// </summary>
    public int NowSceneNum
    {
        get { return nowSceneNum; }
        set { nowSceneNum = value; }
    }

    private float sensitivity = 0.5f;
    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        nowSceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        textcontroller = GameObject.FindAnyObjectByType<TextController>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        nowSceneName = SceneManager.GetActiveScene().name; // �̰� �Ź� ���� �ε�Ǿ��� �� ȣ��� ���� ���� �� �̸� �����ϱ�
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    string GameDataFileName = "GameData.json";
    public Data data = new Data();

    public void SaveData() 
    {
        data.saveDatas[0] = episode_Round;
        if (nowSceneName == "02. MainHallScene" || nowSceneName == "01. IntroScene")
        {
            data.saveDatas[4] = mainHallScene_Save;
        }
        if (nowSceneName == "03. GuraScene_1" || nowSceneName == "06. GuraScene_2" || nowSceneName == "01. IntroScene")
        {
            data.saveDatas[1] = guraScene_Save;
        }
        if (nowSceneName == "04. CalliScene_1" || nowSceneName == "07. CalliScene_2" || nowSceneName == "01. IntroScene")
        {
            data.saveDatas[2] = calliScene_Save;
        }
        if (nowSceneName == "05. KiaraScene_1" || nowSceneName == "08. KiaraScene_2" || nowSceneName == "01. IntroScene")
        {
            data.saveDatas[3] = kiaraScene_save;
        }
        data.saveDatas[5] = itemData_save;
        data.saveDatas[6] = (int)(masterVolume * 10);
        data.saveDatas[7] = (int)(bgmVolume * 10);
        data.saveDatas[8] = (int)(sfxVolume * 10);
        data.saveDatas[9] = (int)isEndingStroryStart;
        NowSceneDataSave();
        data.saveDatas[10] = nowSceneNum;
        data.saveDatas[11] = (int)(sensitivity * 10);
        string ToJsonSave = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + "\\" + GameDataFileName;

        File.WriteAllText(filePath, ToJsonSave); 
    }

    public void LoadData() // ó�� �̾��ϱ⿡�� ȣ���� ������ �ҷ�����
    {
        string filepath = Application.persistentDataPath + "\\" + GameDataFileName;

        if(File.Exists(filepath))
        {
            string FromJsonData = File.ReadAllText(filepath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            
            episode_Round = data.saveDatas[0];
            guraScene_Save = data.saveDatas[1];
            calliScene_Save = data.saveDatas[2];
            kiaraScene_save = data.saveDatas[3];
            mainHallScene_Save = data.saveDatas[4];
            itemData_save = data.saveDatas[5];
            masterVolume = data.saveDatas[6] / 10;
            bgmVolume = data.saveDatas[7] / 10;
            sfxVolume = data.saveDatas[8] / 10;
            isEndingStroryStart = data.saveDatas[9];
            nowSceneNum = data.saveDatas[10];
            sensitivity = data.saveDatas[11] / 10;
        }
        else
        {
            StartCoroutine(textcontroller.SendText(""));
        }
        SaveData();
        Load_LastScenePos();
        ItemManager._instance.LoadInventory();
    }

    public void NewGameData() // �̰� ���� �ָ��������? �𸣰ڳ�
    {
        episode_Round = 1;
        //episode_Round = 2;  // �׽�Ʈ������ ����
        guraScene_Save = 0;
        calliScene_Save = 0;
        kiaraScene_save = 0;
        mainHallScene_Save = 0;
        itemData_save = 0;
        //masterVolume = 10;
        //bgmVolume = 10;
        //sfxVolume = 10;
        isEndingStroryStart = 0;
        nowSceneNum = 0;
        //sensitivity = 5;
    }

    public void NextEpisode() // 1ȸ�� ������ �ݶ��̴��� �̰� �޾��ּ�
    {
        // episode_Round++; // �ʿ���� ��������. ������ ���� �����Ϸ��� ���� �ƴ� �ְ� ���� <- ������ �����ϱ� ���� �ּ�ó����
        guraScene_Save = 0;
        calliScene_Save = 0;
        kiaraScene_save = 0;
        mainHallScene_Save = 0;
        itemData_save = 0;
        isEndingStroryStart = 0;


        nowSceneNum = 0;
        // ������ �ʱ�ȭ ���ϰ� �״��
    }

    void NowSceneDataSave()
    {
        switch(nowSceneName)
        {
            case "02. MainHallScene":
                nowSceneNum = 2;
                break;
            case "03. GuraScene_1":
                nowSceneNum = 3;
                break;
            case "04. CalliScene_1":
                nowSceneNum = 4;
                break;
            case "05. KiaraScene_1":
                nowSceneNum = 5;
                break;
            case "06. GuraScene_2":
                nowSceneNum = 6;
                break;
            case "07. CalliScene_2":
                nowSceneNum = 7;
                break;
            case "08. KiaraScene_2":
                nowSceneNum = 8;
                break;

             default:
                break;
        }
    }

    void Load_LastScenePos()
    {
        switch (nowSceneNum)
        {
            case 2:
                LoadingSceneManager.LoadScene("02. MainHallScene");  // ���� ������ �̵�
                break;
            case 3:
                LoadingSceneManager.LoadScene("03. GuraScene_1");  // ���� ������ �̵�
                break;
            case 4:
                LoadingSceneManager.LoadScene("04. CalliScene_1");  // ���� ������ �̵�
                break;
            case 5:
                LoadingSceneManager.LoadScene("05. KiaraScene_1");  // ���� ������ �̵�
                break;
            case 6:
                LoadingSceneManager.LoadScene("06. GuraScene_2");  // ���� ������ �̵�
                break;
            case 7:
                LoadingSceneManager.LoadScene("07. CalliScene_2");  // ���� ������ �̵�
                break;
            case 8:
                LoadingSceneManager.LoadScene("08. KiaraScene_2");  // ���� ������ �̵�
                break;
            default:
                LoadingSceneManager.LoadScene("02. MainHallScene");  // ���� ������ �̵�
                break;
        }
    }

    private void OnApplicationQuit() // ���ø����̼� ����ɶ� �ڵ� ����
    {
        SaveData();
    }

    // �Ʒ��� �׽�Ʈ��
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SaveData();
        }
    }
}

