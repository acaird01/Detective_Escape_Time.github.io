using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

[Serializable] // 직렬화
public class Data // 데이터 한번에 저장하기 위해 모은 class
{
    public int[] saveDatas = new int[12]; // 인트값 4개 따로 저장 할려다가 형 똑같아서 배열로 묶음 그냥
    // 0 : 회차, 1 : 구라씬, 2 : 칼리씬, 3 : 키아라 씬, 4 : 복도 씬, 5 : 아이템 정보, 6 : 마스터 볼륨, 7 : BGM 볼륨, 8 : SFX볼륨, 9: 스토리 진행 상황 저장(아직 추가되지 않음)
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string PrevSceneName = null; // 직전에 플레이어가 위치했던 씬
    public string nowSceneName = null;  // 현재 플레이어가 위치한 씬

    TextController textcontroller;


    int episode_Round = 1;
    public int Episode_Round // 회차 정보
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
    // 각각의 씬 진행상황 저장할 인트값

    int itemData_save { get; set; }
    public int ItemData_save
    {
        get { return itemData_save; } 
        set { itemData_save = value; }  
    } // 아이템 데이터 저장할 인트값

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
    /// 2 : 복도 / 3: 구라1 / 4 : 칼리1 / 5 : 키아라1 / 6: 구라2 / 7 : 칼리2 / 8 : 키아라2
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
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        nowSceneName = SceneManager.GetActiveScene().name; // 이게 매번 씬이 로드되었을 떄 호출될 예정 현제 씬 이름 저장하기
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

    public void LoadData() // 처음 이어하기에서 호출할 데이터 불러오기
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

    public void NewGameData() // 이거 내가 왜만들었을까? 모르겠네
    {
        episode_Round = 1;
        //episode_Round = 2;  // 테스트용으로 변경
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

    public void NextEpisode() // 1회차 끝나는 콜라이더에 이거 달아주셈
    {
        // episode_Round++; // 필요없음 지워도됨. 문에서 라운드 관리하려면 빼고 아님 넣고 ㅇㅇ <- 문에서 관리하기 위해 주석처리함
        guraScene_Save = 0;
        calliScene_Save = 0;
        kiaraScene_save = 0;
        mainHallScene_Save = 0;
        itemData_save = 0;
        isEndingStroryStart = 0;


        nowSceneNum = 0;
        // 볼륨은 초기화 안하고 그대로
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
                LoadingSceneManager.LoadScene("02. MainHallScene");  // 복도 씬으로 이동
                break;
            case 3:
                LoadingSceneManager.LoadScene("03. GuraScene_1");  // 복도 씬으로 이동
                break;
            case 4:
                LoadingSceneManager.LoadScene("04. CalliScene_1");  // 복도 씬으로 이동
                break;
            case 5:
                LoadingSceneManager.LoadScene("05. KiaraScene_1");  // 복도 씬으로 이동
                break;
            case 6:
                LoadingSceneManager.LoadScene("06. GuraScene_2");  // 복도 씬으로 이동
                break;
            case 7:
                LoadingSceneManager.LoadScene("07. CalliScene_2");  // 복도 씬으로 이동
                break;
            case 8:
                LoadingSceneManager.LoadScene("08. KiaraScene_2");  // 복도 씬으로 이동
                break;
            default:
                LoadingSceneManager.LoadScene("02. MainHallScene");  // 복도 씬으로 이동
                break;
        }
    }

    private void OnApplicationQuit() // 어플리케이션 종료될때 자동 저장
    {
        SaveData();
    }

    // 아래는 테스트용
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SaveData();
        }
    }
}

