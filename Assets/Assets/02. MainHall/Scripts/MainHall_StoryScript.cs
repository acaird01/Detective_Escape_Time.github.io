using System.Collections;
using System.Collections.Generic;

public class StoryLineClass
{
    // �ʱ�ȭ ������
    public StoryLineClass(StoryCharacter_Enum _storyCharactor, string _storyScript)
    {
        storyCharacter = _storyCharactor;   // �ش� ���丮������ ĳ���� �� ����
        storyScript = _storyScript;         // �ش� ���丮������ ��ũ��Ʈ ����
    }

    #region ���丮 ���࿡ �ʿ��� ���� ���� ����
    // ���� ���丮�� �����ϰ� �ִ� ĳ���� ���� enum
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
    public StoryCharacter_Enum StoryCharacter   // �ܺο��� ���� ��縦 ġ���ִ� ĳ���͸� Ȯ���ϱ� ���� ������Ƽ
    {
        get
        {
            return storyCharacter;
        }
    }

    // ����� ���丮 ��� ������ string
    private string storyScript;
    public string StoryScript   // �ܺο��� ���� ���丮 ��縦 Ȯ���ϱ� ���� ������Ƽ
    {
        get
        {
            return storyScript;
        }
    }

    // ���丮 ���� ���� ������ ����ϱ� ���� ����
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
    StoryLineClass storyLineClass;  // ���丮 �ۼ��ؼ� ����Ʈ�� ���� Ŭ����

    public List<StoryLineClass> StoryLine_Start1 = new List<StoryLineClass>();     // ����1�� ���۽��丮 ����Ʈ
    public List<StoryLineClass> StoryLine_Ending1 = new List<StoryLineClass>();    // ����1�� �������丮 ����Ʈ
    public List<StoryLineClass> StoryLine_Start2 = new List<StoryLineClass>();     // ����2�� ���۽��丮 ����Ʈ
    public List<StoryLineClass> StoryLine_Ending2 = new List<StoryLineClass>();    // ����2�� �������丮 ����Ʈ

    public List<StoryLineClass> AfterTutorial_Scripts = new List<StoryLineClass>();           // ó�� ������ �� Ʃ�丮���� ������ ������ ��ũ��Ʈ
    public List<StoryLineClass> AfterGetHaloInEnding2_Scripts = new List<StoryLineClass>();   // ����2���� ���Ϸθ� ó�� ȹ���� ������ ��ũ��Ʈ
    public List<StoryLineClass> StoryLine_MainHallDoorGuide1 = new List<StoryLineClass>();    // ������ ó�� �������� ���� ���� ������ ���丮 ����Ʈ
    public List<StoryLineClass> StoryLine_MainHallDoorGuide2 = new List<StoryLineClass>();    // ������ ó�� �������� ���� ���� ������ ���丮 ����Ʈ

    // ���� ������ �� ȣ���ؼ� �ʱ�ȭ ��Ű�� ���� �Լ�
    public void InitStory()
    {
        // ���� ���丮 �� ���� ���丮
        AddStory_Start1();
        AddStory_Ending1();
        AddStory_Start2();
        AddStory_Ending2();

        AddStory_AfterTutorial();                       // Ʃ�丮�� ���� �� ���丮(1ȸ��)
        AddStory_AfterGetHaloInEnding2();               // ���Ϸθ� ȹ���ѵ� ������ ���丮(2ȸ��)
        AddStroy_IntoTheMainHallFirstTime_Ending1();    // ������ ó�� �������� ������ ���丮(1ȸ��)
        AddStroy_IntoTheMainHallFirstTime_Ending2();    // ������ ó�� �������� ������ ���丮(2ȸ��)
    }

    // ����1�� ���۽��丮 ����
    private void AddStory_Start1()
    {
        // �� �տ��� ����� �κ�
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "���Ⱑ �ٷ� �̳��� �ִ� ���ΰ�..");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��..���� ������ �̻��Ѱ�.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "���� �̳��� ã�ƾ߰ھ�.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��?!");
        StoryLine_Start1.Add(storyLineClass);
        // ������Ű�� �÷��̾� �̵�
        // �濡�� ��� �� ����� �κ�
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��... �Ӹ��� ����..");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�Ƹ�! �Ͼ����!");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�� ��Ҹ���.. �̳�?!\n������? ��ģ ���� ����?");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "��.. �ϴ� �� ������ �� ����.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�����̴�.\n� ������ ���� ���� ���⼭ ������.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "������ ���� ���� ������ ��¼��?");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�ֺ��� ���� �� �� �ִ� �����̳� ��ġ�� �������� ����.");
        StoryLine_Start1.Add(storyLineClass);
        // ī�޶� ������ ���� �ѷ�����.
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�� ������ �Ӹ� ���� ���� �ִ°� ������..\n���� ����� ������");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "......");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "���� ������ �� ������ ����.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�κ��丮�� ��� �� å�� ���� �Ẹ��.");
        StoryLine_Start1.Add(storyLineClass);
    }
    // Ʃ�丮�� ���� �� ���丮 ����(1ȸ��)
    private void AddStory_AfterTutorial()
    {
        // �κ��丮 ��� Ʃ�丮�� �̹��� Ȱ��ȭ
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "����� �̳�!\n�� å�� �׷� �ɷ��� �ִ� ���� �����°�.");
        AfterTutorial_Scripts.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "������ �Ǽ� �⻵.\n���� ������ ������ ������ ��������.");
        AfterTutorial_Scripts.Add(storyLineClass);
    }
    // ������ ������ ���� ó�� ���������� ������� ���丮 ����(1ȸ��)
    private void AddStroy_IntoTheMainHallFirstTime_Ending1()
    {
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!! �� �ð�!\n������ ����� ��������!");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�ƹ����� ��.. �� Ÿ�ڵ��� �����;� ��ã�� �� ���� �� ����.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "������!! ���� �־�� �Ǵµ�..\n� �Ѿư��߰ھ�.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�Ƹ�, �� ���鿡 ������ �ɸ� �� ó�� ����.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��? �׷� �츮�� ���������ٴ� �ž�?");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "...�������� �� �κе� ���� ������ �� ���� �� ����.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "���� ���� �� �տ� ���� ������ ���� �غ���.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "���� �̳�! �׷� �� �༮���� ������ ����!");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
    }
    // ����1�� �������丮 ����
    private void AddStory_Ending1()
    {
        // ���� ���� ����ϰ� ������ ���丮
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��, ���� ���ȳ�.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "������ ���ư��� �̳�!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "...�̾��� �Ƹ�.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "��.. �ƹ����� ���� �� �� ���� �� ����..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��?! ��?");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "��� ������ �������� ��������..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�� å�� ���� ������ ���ؼ� �̰��� �ִ� ��� �Ű� ����߾�.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...�����..?");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�̾��� �Ƹ�..\n������ �� ���Ϸ� ���� �ʱ��� ���⿡ �����ְ� �� �� ������.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�׷� �� ��� ���� ���� �� ����� �����Դ� �ž�..?");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�¾�.. �� ����� ������ �庻������..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "���� ����� �� ��, �� ������ ���� �濡 å�� �ֿ��µ�\n���� �������� ����� �� �־���.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�� �ڿ� �˾����� �� å�� �� ���� ȭ���̱⵵ �ϰŵ�..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�ƴϾ�.. �и� ���� ���ư� ����� ��������..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�Ƹ�, �����̶�� ��� ���� �ʱ��� ��� ���� �� ���� �� �־�.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "������ �̳�! �ʸ� ���� �ΰ� �� �� ����!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�Ƹ�!!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "!...�̾�, �� �ʶ� ������ �����߰ھ�.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��? ���� �װ� ����...");
        StoryLine_Ending1.Add(storyLineClass);
        // �̳� ��Ʈ ����
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "Go for towards the light.");
        StoryLine_Ending1.Add(storyLineClass);
        // ���� ��Ʈ�� �ǵ���
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!? �� ���� �ڴ�� �������ݾ�..!\n�̳�!!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�ʶ� ������ ������ ���ؼ� �� ��� �ۿ� ������..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "���, ������ ��ȸ�� ��!\n�ѹ��� �� ��ȸ�� ��, �̳�!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�� ��, �Ƹ�.");
        StoryLine_Ending1.Add(storyLineClass);
        // �� �ݱ�� �Ҹ�
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�̳�!!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...�̴��..�̴�� ���� �� ����.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�̳�.. �����Ե� ������ �д� �����־�.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�� �ʸ� ���س��ھ�.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...�ð��� �ٽ� �ǵ�������.");
        StoryLine_Ending1.Add(storyLineClass);
        // �ð����� �Ҹ� ���, �̹��� Ȱ��ȭ, �̹��� ������ ����
    }
    // ����2�� ���۽��丮 ����
    private void AddStory_Start2()
    {
        // �� �տ��� ����� �κ�
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "����, �ٽ� ����� ���ƿԾ�.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�̳��� 2�� �濡 �־���.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�̹��� �����ؼ� ���߰ھ�.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "����, ���ڱ� �� �̷��� �������� ����..?");
        StoryLine_Start2.Add(storyLineClass);
        // ����
        // �濡�� ��� �� ����� �κ�
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��..�Ӹ���.. �ᱹ ���Ѱǰ�..?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�Ƹ�! �Ͼ����! ������?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�̳�! �� ������?\n������ �����ִٴ� ������ ����?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "������ �����ִٴ� ����...?\n���� ���� ������ ��Ȳ�� ���ϴ°� �ƴ϶�� ���� �� ����.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�׸��� �ʵ� �˴ٽ��� �� �����ư� �ƴ϶� �̳���.����");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...(����) �׷� ���� �ƴ� �̳��� �´°� ����..");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "���R, �׷��� �� �ٽ� �� �� �־ �⻵.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "���� �⻵. ������ ���� ��� �� ����.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "���� ���� ��ġ�� �и�...��?");
        StoryLine_Start2.Add(storyLineClass);
        // ī�޶� ����
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�� �׷� �Ƹ�? ���� ������ �־�?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�ƴ�.. �ƹ��͵� �Ƴ�.. �ϴ� �� ���Ϸθ� ì��� ���� ��������.");
        StoryLine_Start2.Add(storyLineClass);
    }
    // 2ȸ������ ���Ϸθ� ȹ���� �� ���丮 ����
    private void AddStory_AfterGetHaloInEnding2()
    {
        // �κ��丮 ��� Ʃ�丮�� �̹��� Ȱ��ȭ
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "��! ���� ���� ���Ⱦ�.\n��� �˾Ҿ�, �Ƹ�?");
        AfterGetHaloInEnding2_Scripts.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��, �ֳ��ϸ� �� ���� ģ���� �ְ��� Ž���̱� ��������.\n���� �������� ������ ��������.");
        AfterGetHaloInEnding2_Scripts.Add(storyLineClass);
    }
    // ������ ������ ���� ó�� ���������� ������� ���丮 ����(2ȸ��)
    private void AddStroy_IntoTheMainHallFirstTime_Ending2()
    {
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "������ �� �� �ð踦 ����� ��������!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�ƹ����� ��.. �� Ÿ�ڵ��� �����;� ��ã�� �� ���� �� ����.");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�׷��� �Ƹ� �Ƕ��?\n������ ������ ���� ���� �־�?");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��..�ƹ��͵� �Ƴ�. � �Ѿư���!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.TAKOS, "WAH?");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "��? �Ƹ�, ����� �����ٰǰ���.");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "1���� �ִ� ������ ������ Ǯ���� �� �ִٴ� �� ����.");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�׷�? �׷� ������� ������!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��, ��� ���ٲ����� �ϰ� �;��ϴ� �༮���� ã���� ������!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.TAKOS, "WAH!!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
    }
    // ����2�� �������丮 ����
    private void AddStory_Ending2()
    {
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "����, ���� ���µ� �����߾�!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "(�̹��� å�� ������ ���� �ʰ� ���� ���µ� �����߾�.)");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "(������ �׳� �̷��� �������ٱ�?)");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "���? �Ƹ� �� ������ �� ����?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��? ���Ϸο��� ���ڱ� ���� ���ݾ�..?!");
        StoryLine_Ending2.Add(storyLineClass);
        // �ؽ�Ʈ�ڽ��� ���̰Բ� ��� ���� ���
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�Ƹ�!!!");
        StoryLine_Ending2.Add(storyLineClass);
        // ���� �������� ����
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��.. ���μ�. ��ü ���� ���� �Ͼ ����?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "���, �Ƹ�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�� ������..? ��ü ���� ����?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "�̾�. ���� �ð��� ���̴� ��� �ڼ��� ������ �����ٲ� ����.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "��к��� �������ڸ�, ���п� �ӹڿ��� Ǯ���� �Ǽ�\n���� �� ������ �ְ��� �̰����� �ʴ��߾�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�ӹ��̶��?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "���� ���Ⱑ ����� ���� �������ݾ�. �� �ٺ���!");
        StoryLine_Ending2.Add(storyLineClass);
        // ��� ���̸��� ��ġ�� �Ƹ� ��ġ�� ������ ���� ���ο��� �ǵ���(�Ƹ� �̹����� �����������)
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "�Ͼ�!? �� ���ص� ���� �ҷ��� �߰ŵ�? �ܽ��� ������!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "��??! �� �ܽ��� �ƴϰŵ�?!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "����, �� �� �����ϰ�. ���� �ǻ���̾�.\n��ݱ��� �־��� ������ �ٸ� ��������.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�ǻ���̶��? ��ü �� ���� �ǻ���ΰ���?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "�װſ� ���� ������ ������ ���ֱ� ����� �� ����.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "���� ���� ������ٽ��� ������ �ð��� ���ŵ�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "��� �� �༮.. �츮�� Ÿ���� ���·� �ӹ��صδٴ�.\n�������� ��������! ���ƾ�!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "�и� ���� �Ϻ��Կ� ������ ������ �׷�������.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "�ƹ�ư, �װ� �츮�� ���ǵ� ã���� ���п�\n���� ��ã�� �ӹڿ��� Ǯ���� �� �־���.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "����.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "õ����. ���� �̷��� �� ���� �������� ������..");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "�츮�� Ȯ���ϱ�� ���� ģ���� ���� ��� �ſ��� �����°� ������.\n�� ���� ����, ������?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "��? ��... �̾� ��Ծ���.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "��� ������ �� ������?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "...�¾�. �̳���� �̸��� �������� ������.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!!! ��� �װ�..\n�ƴ�, �̳��� �����ϰ� ������ ���� �� �ְ� ���� �� �� �ִ°ž�?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "�׷�~ �����ְڴٰ� ���ݾ�?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "�츮�� ���� �ǵ��ƿ����� �� �༮�� ���ν�ų �� ��������.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "��ħ å�� ���·� ������ ������ �� �� �����ϰ� �ɲ� ���⵵ �ϰ�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�װ� �����̾�?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "�׷�. �Ƹ� ���ư����� �༮�� ���� ������� ��������\n���� ������ ��Ƶѷ��� �Ҳ���.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "�� ƴ�� Ÿ �츮�� ������ ������ ����.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "��.. ���� �ð��� �� �Ȱ� ����.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "���� ���� ������߰ڳ�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "�� �ð迡 ���� �� ���� ������.. ������ �Ѿ���� �ϰھ�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��.. ������.. ���� ������ �� �𸣰ڴ°�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "�׷� �߰�! �� �ִ� ���ڰ�.");
        StoryLine_Ending2.Add(storyLineClass);

        // ��� ���̵� �� �ƿ�

        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�Ƹ�! �Ƹ�!! ������?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�� �� ������! �̳�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "���ڱ� ���� ������ �ʰ� ������ٰ� �ٽ� ���ƿԾ�!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "(�ð��� ��� ������ǰ�..) �� ������. �׷� ���� ������.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "...��? å���� ���ڱ� ���� ���Ⱑ..!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!! �̳� å���� ������!");
        StoryLine_Ending2.Add(storyLineClass);
        // �ƿ������� ��������, ���� ...�̶� !�� ����
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "�׷����ϰ� �ѱ��!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "���� �츮�� ���⿡ ��Ƶѷ��� �ϴٴ�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "���� ���̿��� ���� ��߰���?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "���δ�.���δ�.���δ�.���δ�.���δ�.���δ�.���δ�.���δ�.\n���δ�.���δ�.���δ�.���δ�.���δ�.���δ�.���δ�.���δ�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "��, ������ ���εǾ� ��߰ھ�!");
        StoryLine_Ending2.Add(storyLineClass);
        // �ƿ������� ��������, ���� ...�̶� !�� ����
        // ��罽 �Ҹ� ���
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "���� ���� ����!\n�׷� �츰 �̸�!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "�ٷ� �����ȳ�..");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "��...�Ƹ�..? �̰� ���� ��Ȳ�̾�?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��..������ڸ� �츮�� ã�� Ÿ�ڵ��� ��� �������ִ� ������̿��� ����.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "��ħ ���� �־��⿡ �츱 ���ͼ�\n�� ��ġ�� �ǽ��� �������ذŰ�.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "�׷� ��.. ���� ������ ���� �� �ִ°ž�..?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "������ ���ư���, �̳�!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "..��!");
        StoryLine_Ending2.Add(storyLineClass);
        // ���� ũ���� ���
    }
}