using System.Collections;
using System.Collections.Generic;

public class StoryLineClass
{
    // 초기화 생성자
    public StoryLineClass(StoryCharacter_Enum _storyCharactor, string _storyScript)
    {
        storyCharacter = _storyCharactor;   // 해당 스토리라인의 캐릭터 명 저장
        storyScript = _storyScript;         // 해당 스토리라인의 스크립트 저장
    }

    #region 스토리 진행에 필요한 정보 변수 모음
    // 현재 스토리를 진행하고 있는 캐릭터 관리 enum
    public enum StoryCharacter_Enum
    {
        AME,
        INA,
        IRYS,
        BAE,
        FAUNA,
        KRONII,
        MUMEI,
        TAKOS,
        NONE,
        LENGTH,
    };
    private StoryCharacter_Enum storyCharacter;
    public StoryCharacter_Enum StoryCharacter   // 외부에서 현재 대사를 치고있는 캐릭터를 확인하기 위한 프로퍼티
    {
        get
        {
            return storyCharacter;
        }
    }

    // 출력할 스토리 대사 저장할 string
    private string storyScript;
    public string StoryScript   // 외부에서 현재 스토리 대사를 확인하기 위한 프로퍼티
    {
        get
        {
            return storyScript;
        }
    }

    // 스토리 진행 도중 음성을 출력하기 위한 변수
    private string characterAudioClipName;
    public string CharacterAudioClipName
    {
        get
        {
            return characterAudioClipName;
        }
    }
    #endregion
}

public class MainHall_StoryScript
{
    StoryLineClass storyLineClass;  // 스토리 작성해서 리스트에 담을 클래스

    public List<StoryLineClass> StoryLine_Start1 = new List<StoryLineClass>();     // 엔딩1의 시작스토리 리스트
    public List<StoryLineClass> StoryLine_Ending1 = new List<StoryLineClass>();    // 엔딩1의 엔딩스토리 리스트
    public List<StoryLineClass> StoryLine_Start2 = new List<StoryLineClass>();     // 엔딩2의 시작스토리 리스트
    public List<StoryLineClass> StoryLine_Ending2 = new List<StoryLineClass>();    // 엔딩2의 엔딩스토리 리스트

    public List<StoryLineClass> AfterTutorial_Scripts = new List<StoryLineClass>();           // 처음 시작할 때 튜토리얼을 진행한 이후의 스크립트
    public List<StoryLineClass> AfterGetHaloInEnding2_Scripts = new List<StoryLineClass>();   // 엔딩2에서 헤일로를 처음 획득한 이후의 스크립트
    public List<StoryLineClass> StoryLine_MainHallDoorGuide1 = new List<StoryLineClass>();    // 복도에 처음 나왔을때 문에 대해 설명할 스토리 리스트
    public List<StoryLineClass> StoryLine_MainHallDoorGuide2 = new List<StoryLineClass>();    // 복도에 처음 나왔을때 문에 대해 설명할 스토리 리스트

    // 게임 시작할 때 호출해서 초기화 시키기 위한 함수
    public void InitStory()
    {
        // 시작 스토리 및 엔딩 스토리
        AddStory_Start1();
        AddStory_Ending1();
        AddStory_Start2();
        AddStory_Ending2();

        AddStory_AfterTutorial();                       // 튜토리얼 진행 후 스토리(1회차)
        AddStory_AfterGetHaloInEnding2();               // 헤일로를 획득한뒤 진행할 스토리(2회차)
        AddStroy_IntoTheMainHallFirstTime_Ending1();    // 복도로 처음 나왔을때 진행할 스토리(1회차)
        AddStroy_IntoTheMainHallFirstTime_Ending2();    // 복도로 처음 나왔을때 진행할 스토리(2회차)
    }

    // 엔딩1의 시작스토리 저장
    private void AddStory_Start1()
    {
        // 문 앞에서 진행될 부분
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "여기가 바로 이나가 있는 성인가..");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "흠..뭔가 느낌이 이상한걸.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "빨리 이나를 찾아야겠어.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "윽?!");
        StoryLine_Start1.Add(storyLineClass);
        // 암전시키고 플레이어 이동
        // 방에서 깨어난 뒤 진행될 부분
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "으... 머리가 아파..");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "아메! 일어났구나!");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "이 목소리는.. 이나?!\n괜찮아? 다친 곳은 없어?");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "응.. 일단 난 괜찮은 것 같아.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "다행이다.\n어서 범인이 오기 전에 여기서 나가자.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "하지만 문이 잠긴거 같은데 어쩌지?");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "주변에 문을 열 수 있는 물건이나 장치가 있을지도 몰라.");
        StoryLine_Start1.Add(storyLineClass);
        // 카메라를 움직여 방을 둘러본다.
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "저 조각상 머리 위에 뭔가 있는거 같은데..\n꺼낼 방법이 없을까");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "......");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "내가 도와줄 수 있을꺼 같아.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "인벤토리를 열어서 이 책의 힘을 써보자.");
        StoryLine_Start1.Add(storyLineClass);
    }
    // 튜토리얼 진행 후 스토리 저장(1회차)
    private void AddStory_AfterTutorial()
    {
        // 인벤토리 사용 튜토리얼 이미지 활성화
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "대단해 이나!\n그 책에 그런 능력이 있는 줄은 몰랐는걸.");
        AfterTutorial_Scripts.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "도움이 되서 기뻐.\n문이 열린거 같으니 밖으로 나가보자.");
        AfterTutorial_Scripts.Add(storyLineClass);
    }
    // 복도로 나오는 문을 처음 지났을때만 출력해줄 스토리 저장(1회차)
    private void AddStroy_IntoTheMainHallFirstTime_Ending1()
    {
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!! 내 시계!\n쟤들이 숨기고 도망갔어!");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "아무래도 음.. 저 타코들을 데려와야 되찾을 수 있을 꺼 같아.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "으으으!! 저게 있어야 되는데..\n어서 쫓아가야겠어.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "아메, 저 문들에 마법이 걸린 것 처럼 보여.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "뭐? 그럼 우리가 못지나간다는 거야?");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "...다행히도 이 부분도 내가 도와줄 수 있을 것 같아.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "열고 싶은 문 앞에 가면 마법을 해제 해볼께.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "고마워 이나! 그럼 저 녀석들을 잡으러 가자!");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
    }
    // 엔딩1의 엔딩스토리 저장
    private void AddStory_Ending1()
    {
        // 엔딩 문을 통과하고 진행할 스토리
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "휴, 드디어 열렸네.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "집으로 돌아가자 이나!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "...미안해 아메.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "난.. 아무래도 같이 갈 수 없을 것 같아..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "뭐?! 왜?");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "사실 너한테 말해주지 못했지만..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "이 책의 힘을 빌리기 위해서 이곳에 있는 고대 신과 계약했어.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...뭐라고..?");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "미안해 아메..\n하지만 날 구하러 와준 너까지 여기에 잡혀있게 둘 순 없었어.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "그럼 그 고대 신이 설마 널 여기로 데려왔던 거야..?");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "맞아.. 날 여기로 데려온 장본인이지..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "내가 사라진 그 날, 널 만나러 가는 길에 책을 주웠는데\n정신 차리고보니 여기로 와 있었어.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "그 뒤에 알았지만 이 책은 그 신의 화신이기도 하거든..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "아니야.. 분명 같이 돌아갈 방법이 있을꺼야..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "아메, 지금이라면 고대 신이 너까지 잡기 전에 넌 나갈 수 있어.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "하지만 이나! 너만 여기 두고 갈 순 없어!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "아메!!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "!...미안, 난 너라도 무사히 보내야겠어.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "뭐? 지금 그게 무슨...");
        StoryLine_Ending1.Add(storyLineClass);
        // 이나 폰트 문자
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "Go for towards the light.");
        StoryLine_Ending1.Add(storyLineClass);
        // 정상 폰트로 되돌림
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!? 내 몸이 멋대로 움직이잖아..!\n이나!!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "너라도 무사히 보내기 위해선 이 방법 밖에 없었어..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "잠깐만, 나한테 기회를 줘!\n한번만 더 기회를 줘, 이나!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "잘 가, 아메.");
        StoryLine_Ending1.Add(storyLineClass);
        // 문 닫기는 소리
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "이나!!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...이대로..이대로 끝낼 순 없어.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "이나.. 나에게도 비장의 패는 남아있어.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "꼭 너를 구해내겠어.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...시간을 다시 되돌려서라도.");
        StoryLine_Ending1.Add(storyLineClass);
        // 시간여행 소리 출력, 이미지 활성화, 이미지 스케일 조정
    }
    // 엔딩2의 시작스토리 저장
    private void AddStory_Start2()
    {
        // 문 앞에서 진행될 부분
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "좋아, 다시 여기로 돌아왔어.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "이나는 2층 방에 있었지.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "이번엔 조심해서 가야겠어.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "뭐야, 갑자기 왜 이렇게 어지러운 거지..?");
        StoryLine_Start2.Add(storyLineClass);
        // 암전
        // 방에서 깨어난 뒤 진행될 부분
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "윽..머리야.. 결국 당한건가..?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "아메! 일어났구나! 괜찮아?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "이나! 넌 괜찮아?\n뭔가에 잡혀있다는 느낌은 없어?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "뭔가에 잡혀있다는 느낌...?\n지금 여기 잡혀온 상황을 말하는게 아니라면 없는 것 같아.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "그리고 너도 알다시피 난 괜찮아가 아니라 이나라구.하하");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...(에휴) 그래 내가 아는 이나가 맞는거 같네..");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "히힣, 그래도 널 다시 볼 수 있어서 기뻐.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "나도 기뻐. 하지만 문이 잠긴 것 같아.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "문을 여는 장치가 분명...응?");
        StoryLine_Start2.Add(storyLineClass);
        // 카메라 무빙
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "왜 그래 아메? 무슨 문제라도 있어?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "아니.. 아무것도 아냐.. 일단 저 헤일로를 챙기면 문이 열릴꺼야.");
        StoryLine_Start2.Add(storyLineClass);
    }
    // 2회차에서 헤일로를 획득한 뒤 스토리 저장
    private void AddStory_AfterGetHaloInEnding2()
    {
        // 인벤토리 사용 튜토리얼 이미지 활성화
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "와! 정말 문이 열렸어.\n어떻게 알았어, 아메?");
        AfterGetHaloInEnding2_Scripts.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "훗, 왜냐하면 니 옆의 친구가 최고의 탐정이기 때문이지.\n누가 오기전에 밖으로 나가보자.");
        AfterGetHaloInEnding2_Scripts.Add(storyLineClass);
    }
    // 복도로 나오는 문을 처음 지났을때만 출력해줄 스토리 저장(2회차)
    private void AddStroy_IntoTheMainHallFirstTime_Ending2()
    {
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "쟤들이 또 내 시계를 숨기고 도망갔어!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "아무래도 음.. 저 타코들을 데려와야 되찾을 수 있을 꺼 같아.");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "그런데 아메 또라니?\n이전에 쟤들을 만난 적이 있어?");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "아..아무것도 아냐. 어서 쫓아가자!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.TAKOS, "WAH?");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "응? 아메, 얘들이 도와줄건가봐.");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "1층에 있는 문들의 마법을 풀어줄 수 있다는 것 같아.");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "그래? 그럼 사양하지 않을께!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "자, 어디 숨바꼭질을 하고 싶어하는 녀석들을 찾으러 가볼까!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.TAKOS, "WAH!!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
    }
    // 엔딩2의 엔딩스토리 저장
    private void AddStory_Ending2()
    {
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "좋아, 문을 여는데 성공했어!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "(이번엔 책의 도움을 받지 않고 문을 여는데 성공했어.)");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "(하지만 그냥 이렇게 내보내줄까?)");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "어라? 아메 그 빛나는 건 뭐야?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "응? 헤일로에서 갑자기 빛이 나잖아..?!");
        StoryLine_Ending2.Add(storyLineClass);
        // 텍스트박스만 보이게끔 흰색 바탕 출력
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "아메!!!");
        StoryLine_Ending2.Add(storyLineClass);
        // 검은 바탕으로 변경
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "윽.. 눈부셔. 대체 무슨 일이 일어난 거지?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "어서와, 아메.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "넌 누구야..? 대체 여긴 어디고?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "미안. 지금 시간이 많이는 없어서 자세한 설명은 못해줄꺼 같아.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "결론부터 말해주자면, 덕분에 속박에서 풀리게 되서\n감사 겸 도움을 주고자 이곳으로 초대했어.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "속박이라고?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "정작 여기가 어딘진 말을 안해줬잖아. 이 바보야!");
        StoryLine_Ending2.Add(storyLineClass);
        // 잠깐 아이리스 위치를 아메 위치로 변경후 다음 라인에서 되돌림(아메 이미지도 조정해줘야함)
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "하아!? 말 안해도 지금 할려고 했거든? 햄스터 같은게!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "뭐??! 나 햄스터 아니거든?!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "자자, 둘 다 진정하고. 여긴 의사당이야.\n방금까지 있었던 성과는 다른 공간이지.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "의사당이라고? 대체 뭘 위한 의사당인거지?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "그거에 대한 설명은 지금은 해주기 어려울 것 같네.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "조금 전에 말해줬다시피 지금은 시간이 없거든.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "고대 신 녀석.. 우리를 타코의 형태로 속박해두다니.\n가만두지 않을꺼야! 으아아!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "분명 나의 완벽함에 질투를 느껴서 그랬을꺼야.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "아무튼, 네가 우리의 물건도 찾아준 덕분에\n힘을 되찾아 속박에서 풀려날 수 있었어.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "고마워.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "천만에. 나도 이렇게 될 줄은 몰랐었긴 하지만..");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "우리가 확인하기론 너의 친구가 지금 고대 신에게 잡혀온거 같던데.\n내 말이 맞지, 무메이?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "응? 오... 미안 까먹었어.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "대신 베리라도 좀 먹을래?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "...맞아. 이나라는 이름을 가졌던거 같은데.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!!! 어떻게 그걸..\n아니, 이나를 안전하게 데리고 나갈 수 있게 도와 줄 수 있는거야?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "그럼~ 도와주겠다고 했잖아?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "우리의 힘도 되돌아왔으니 그 녀석을 봉인시킬 수 있을꺼야.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "마침 책의 형태로 현현해 있으니 좀 더 수월하게 될꺼 같기도 하고.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "그게 정말이야?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "그래. 아마 돌아가고나면 녀석은 아직 계약하지 못했으니\n너희를 강제로 잡아둘려고 할꺼야.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "그 틈을 타 우리가 봉인해 버리는 거지.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "음.. 이젠 시간이 다 된거 같아.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "이제 돌려 보내줘야겠네.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "그 시계에 관해 할 말이 있지만.. 지금은 넘어가도록 하겠어.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "윽.. 아하하.. 무슨 말인지 잘 모르겠는걸.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "그럼 잘가! 좀 있다 보자고.");
        StoryLine_Ending2.Add(storyLineClass);

        // 흰색 페이드 인 아웃

        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "아메! 아메!! 괜찮아?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "응 난 괜찮아! 이나.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "갑자기 빛이 나더니 너가 사라졌다가 다시 돌아왔어!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "(시간을 잠깐 멈췄던건가..) 난 괜찮아. 그럼 이제 나가자.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "...응? 책에서 갑자기 검은 연기가..!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!! 이나 책에서 떨어져!");
        StoryLine_Ending2.Add(storyLineClass);
        // 아오찬에서 검은연기, 대사는 ...이랑 !가 전부
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "그렇게하게 둘까보냐!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "감히 우리를 여기에 잡아둘려고 하다니.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "나쁜 아이에겐 벌을 줘야겠지?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "죽인다.죽인다.죽인다.죽인다.죽인다.죽인다.죽인다.죽인다.\n죽인다.죽인다.죽인다.죽인다.죽인다.죽인다.죽인다.죽인다.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "자, 얌전히 봉인되어 줘야겠어!");
        StoryLine_Ending2.Add(storyLineClass);
        // 아오찬에서 검은연기, 대사는 ...이랑 !가 전부
        // 쇠사슬 소리 출력
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "좋아 봉인 성공!\n그럼 우린 이만!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "바로 가버렸네..");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "어...아메..? 이게 무슨 상황이야?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "음..요약하자면 우리가 찾은 타코들이 사실 붙잡혀있던 존재들이였던 거지.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "마침 빚이 있었기에 우릴 도와서\n널 납치한 악신을 봉인해준거고.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "그럼 나.. 이제 무사히 나갈 수 있는거야..?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "집으로 돌아가자, 이나!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "..응!");
        StoryLine_Ending2.Add(storyLineClass);
        // 엔딩 크레딧 재생
    }
}