using System.Collections;
using System.Collections.Generic;

public class StoryLineClass
{
    // ÃÊ±âÈ­ »ı¼ºÀÚ
    public StoryLineClass(StoryCharacter_Enum _storyCharactor, string _storyScript)
    {
        storyCharacter = _storyCharactor;   // ÇØ´ç ½ºÅä¸®¶óÀÎÀÇ Ä³¸¯ÅÍ ¸í ÀúÀå
        storyScript = _storyScript;         // ÇØ´ç ½ºÅä¸®¶óÀÎÀÇ ½ºÅ©¸³Æ® ÀúÀå
    }

    #region ½ºÅä¸® ÁøÇà¿¡ ÇÊ¿äÇÑ Á¤º¸ º¯¼ö ¸ğÀ½
    // ÇöÀç ½ºÅä¸®¸¦ ÁøÇàÇÏ°í ÀÖ´Â Ä³¸¯ÅÍ °ü¸® enum
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
    public StoryCharacter_Enum StoryCharacter   // ¿ÜºÎ¿¡¼­ ÇöÀç ´ë»ç¸¦ Ä¡°íÀÖ´Â Ä³¸¯ÅÍ¸¦ È®ÀÎÇÏ±â À§ÇÑ ÇÁ·ÎÆÛÆ¼
    {
        get
        {
            return storyCharacter;
        }
    }

    // Ãâ·ÂÇÒ ½ºÅä¸® ´ë»ç ÀúÀåÇÒ string
    private string storyScript;
    public string StoryScript   // ¿ÜºÎ¿¡¼­ ÇöÀç ½ºÅä¸® ´ë»ç¸¦ È®ÀÎÇÏ±â À§ÇÑ ÇÁ·ÎÆÛÆ¼
    {
        get
        {
            return storyScript;
        }
    }

    // ½ºÅä¸® ÁøÇà µµÁß À½¼ºÀ» Ãâ·ÂÇÏ±â À§ÇÑ º¯¼ö
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
    StoryLineClass storyLineClass;  // ½ºÅä¸® ÀÛ¼ºÇØ¼­ ¸®½ºÆ®¿¡ ´ãÀ» Å¬·¡½º

    public List<StoryLineClass> StoryLine_Start1 = new List<StoryLineClass>();     // ¿£µù1ÀÇ ½ÃÀÛ½ºÅä¸® ¸®½ºÆ®
    public List<StoryLineClass> StoryLine_Ending1 = new List<StoryLineClass>();    // ¿£µù1ÀÇ ¿£µù½ºÅä¸® ¸®½ºÆ®
    public List<StoryLineClass> StoryLine_Start2 = new List<StoryLineClass>();     // ¿£µù2ÀÇ ½ÃÀÛ½ºÅä¸® ¸®½ºÆ®
    public List<StoryLineClass> StoryLine_Ending2 = new List<StoryLineClass>();    // ¿£µù2ÀÇ ¿£µù½ºÅä¸® ¸®½ºÆ®

    public List<StoryLineClass> AfterTutorial_Scripts = new List<StoryLineClass>();           // Ã³À½ ½ÃÀÛÇÒ ¶§ Æ©Åä¸®¾óÀ» ÁøÇàÇÑ ÀÌÈÄÀÇ ½ºÅ©¸³Æ®
    public List<StoryLineClass> AfterGetHaloInEnding2_Scripts = new List<StoryLineClass>();   // ¿£µù2¿¡¼­ ÇìÀÏ·Î¸¦ Ã³À½ È¹µæÇÑ ÀÌÈÄÀÇ ½ºÅ©¸³Æ®
    public List<StoryLineClass> StoryLine_MainHallDoorGuide1 = new List<StoryLineClass>();    // º¹µµ¿¡ Ã³À½ ³ª¿ÔÀ»¶§ ¹®¿¡ ´ëÇØ ¼³¸íÇÒ ½ºÅä¸® ¸®½ºÆ®
    public List<StoryLineClass> StoryLine_MainHallDoorGuide2 = new List<StoryLineClass>();    // º¹µµ¿¡ Ã³À½ ³ª¿ÔÀ»¶§ ¹®¿¡ ´ëÇØ ¼³¸íÇÒ ½ºÅä¸® ¸®½ºÆ®

    // °ÔÀÓ ½ÃÀÛÇÒ ¶§ È£ÃâÇØ¼­ ÃÊ±âÈ­ ½ÃÅ°±â À§ÇÑ ÇÔ¼ö
    public void InitStory()
    {
        // ½ÃÀÛ ½ºÅä¸® ¹× ¿£µù ½ºÅä¸®
        AddStory_Start1();
        AddStory_Ending1();
        AddStory_Start2();
        AddStory_Ending2();

        AddStory_AfterTutorial();                       // Æ©Åä¸®¾ó ÁøÇà ÈÄ ½ºÅä¸®(1È¸Â÷)
        AddStory_AfterGetHaloInEnding2();               // ÇìÀÏ·Î¸¦ È¹µæÇÑµÚ ÁøÇàÇÒ ½ºÅä¸®(2È¸Â÷)
        AddStroy_IntoTheMainHallFirstTime_Ending1();    // º¹µµ·Î Ã³À½ ³ª¿ÔÀ»¶§ ÁøÇàÇÒ ½ºÅä¸®(1È¸Â÷)
        AddStroy_IntoTheMainHallFirstTime_Ending2();    // º¹µµ·Î Ã³À½ ³ª¿ÔÀ»¶§ ÁøÇàÇÒ ½ºÅä¸®(2È¸Â÷)
    }

    // ¿£µù1ÀÇ ½ÃÀÛ½ºÅä¸® ÀúÀå
    private void AddStory_Start1()
    {
        // ¹® ¾Õ¿¡¼­ ÁøÇàµÉ ºÎºĞ
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¿©±â°¡ ¹Ù·Î ÀÌ³ª°¡ ÀÖ´Â ¼ºÀÎ°¡..");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "Èì..¹º°¡ ´À³¦ÀÌ ÀÌ»óÇÑ°É.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "»¡¸® ÀÌ³ª¸¦ Ã£¾Æ¾ß°Ú¾î.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "À¹?!");
        StoryLine_Start1.Add(storyLineClass);
        // ¾ÏÀü½ÃÅ°°í ÇÃ·¹ÀÌ¾î ÀÌµ¿
        // ¹æ¿¡¼­ ±ú¾î³­ µÚ ÁøÇàµÉ ºÎºĞ
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "À¸... ¸Ó¸®°¡ ¾ÆÆÄ..");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾Æ¸Ş! ÀÏ¾î³µ±¸³ª!");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÌ ¸ñ¼Ò¸®´Â.. ÀÌ³ª?!\n±¦Âú¾Æ? ´ÙÄ£ °÷Àº ¾ø¾î?");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "ÀÀ.. ÀÏ´Ü ³­ ±¦ÂúÀº °Í °°¾Æ.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "´ÙÇàÀÌ´Ù.\n¾î¼­ ¹üÀÎÀÌ ¿À±â Àü¿¡ ¿©±â¼­ ³ª°¡ÀÚ.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "ÇÏÁö¸¸ ¹®ÀÌ Àá±ä°Å °°Àºµ¥ ¾îÂ¼Áö?");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÁÖº¯¿¡ ¹®À» ¿­ ¼ö ÀÖ´Â ¹°°ÇÀÌ³ª ÀåÄ¡°¡ ÀÖÀ»Áöµµ ¸ô¶ó.");
        StoryLine_Start1.Add(storyLineClass);
        // Ä«¸Ş¶ó¸¦ ¿òÁ÷¿© ¹æÀ» µÑ·¯º»´Ù.
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "Àú Á¶°¢»ó ¸Ó¸® À§¿¡ ¹º°¡ ÀÖ´Â°Å °°Àºµ¥..\n²¨³¾ ¹æ¹ıÀÌ ¾øÀ»±î");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "......");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "³»°¡ µµ¿ÍÁÙ ¼ö ÀÖÀ»²¨ °°¾Æ.");
        StoryLine_Start1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "ÀÎº¥Åä¸®¸¦ ¿­¾î¼­ ÀÌ Ã¥ÀÇ ÈûÀ» ½áº¸ÀÚ.");
        StoryLine_Start1.Add(storyLineClass);
    }
    // Æ©Åä¸®¾ó ÁøÇà ÈÄ ½ºÅä¸® ÀúÀå(1È¸Â÷)
    private void AddStory_AfterTutorial()
    {
        // ÀÎº¥Åä¸® »ç¿ë Æ©Åä¸®¾ó ÀÌ¹ÌÁö È°¼ºÈ­
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "´ë´ÜÇØ ÀÌ³ª!\n±× Ã¥¿¡ ±×·± ´É·ÂÀÌ ÀÖ´Â ÁÙÀº ¸ô¶ú´Â°É.");
        AfterTutorial_Scripts.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "µµ¿òÀÌ µÇ¼­ ±â»µ.\n¹®ÀÌ ¿­¸°°Å °°À¸´Ï ¹ÛÀ¸·Î ³ª°¡º¸ÀÚ.");
        AfterTutorial_Scripts.Add(storyLineClass);
    }
    // º¹µµ·Î ³ª¿À´Â ¹®À» Ã³À½ Áö³µÀ»¶§¸¸ Ãâ·ÂÇØÁÙ ½ºÅä¸® ÀúÀå(1È¸Â÷)
    private void AddStroy_IntoTheMainHallFirstTime_Ending1()
    {
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!! ³» ½Ã°è!\nÀ÷µéÀÌ ¼û±â°í µµ¸Á°¬¾î!");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾Æ¹«·¡µµ À½.. Àú Å¸ÄÚµéÀ» µ¥·Á¿Í¾ß µÇÃ£À» ¼ö ÀÖÀ» ²¨ °°¾Æ.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "À¸À¸À¸!! Àú°Ô ÀÖ¾î¾ß µÇ´Âµ¥..\n¾î¼­ ÂÑ¾Æ°¡¾ß°Ú¾î.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾Æ¸Ş, Àú ¹®µé¿¡ ¸¶¹ıÀÌ °É¸° °Í Ã³·³ º¸¿©.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¹¹? ±×·³ ¿ì¸®°¡ ¸øÁö³ª°£´Ù´Â °Å¾ß?");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "...´ÙÇàÈ÷µµ ÀÌ ºÎºĞµµ ³»°¡ µµ¿ÍÁÙ ¼ö ÀÖÀ» °Í °°¾Æ.");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¿­°í ½ÍÀº ¹® ¾Õ¿¡ °¡¸é ¸¶¹ıÀ» ÇØÁ¦ ÇØº¼²².");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "°í¸¶¿ö ÀÌ³ª! ±×·³ Àú ³à¼®µéÀ» ÀâÀ¸·¯ °¡ÀÚ!");
        StoryLine_MainHallDoorGuide1.Add(storyLineClass);
    }
    // ¿£µù1ÀÇ ¿£µù½ºÅä¸® ÀúÀå
    private void AddStory_Ending1()
    {
        // ¿£µù ¹®À» Åë°úÇÏ°í ÁøÇàÇÒ ½ºÅä¸®
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÈŞ, µåµğ¾î ¿­·È³×.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÁıÀ¸·Î µ¹¾Æ°¡ÀÚ ÀÌ³ª!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "...¹Ì¾ÈÇØ ¾Æ¸Ş.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "³­.. ¾Æ¹«·¡µµ °°ÀÌ °¥ ¼ö ¾øÀ» °Í °°¾Æ..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¹¹?! ¿Ö?");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "»ç½Ç ³ÊÇÑÅ× ¸»ÇØÁÖÁö ¸øÇßÁö¸¸..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "ÀÌ Ã¥ÀÇ ÈûÀ» ºô¸®±â À§ÇØ¼­ ÀÌ°÷¿¡ ÀÖ´Â °í´ë ½Å°ú °è¾àÇß¾î.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...¹¹¶ó°í..?");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¹Ì¾ÈÇØ ¾Æ¸Ş..\nÇÏÁö¸¸ ³¯ ±¸ÇÏ·¯ ¿ÍÁØ ³Ê±îÁö ¿©±â¿¡ ÀâÇôÀÖ°Ô µÑ ¼ø ¾ø¾ú¾î.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "±×·³ ±× °í´ë ½ÅÀÌ ¼³¸¶ ³Î ¿©±â·Î µ¥·Á¿Ô´ø °Å¾ß..?");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¸Â¾Æ.. ³¯ ¿©±â·Î µ¥·Á¿Â Àåº»ÀÎÀÌÁö..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "³»°¡ »ç¶óÁø ±× ³¯, ³Î ¸¸³ª·¯ °¡´Â ±æ¿¡ Ã¥À» ÁÖ¿ü´Âµ¥\nÁ¤½Å Â÷¸®°íº¸´Ï ¿©±â·Î ¿Í ÀÖ¾ú¾î.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "±× µÚ¿¡ ¾Ë¾ÒÁö¸¸ ÀÌ Ã¥Àº ±× ½ÅÀÇ È­½ÅÀÌ±âµµ ÇÏ°Åµç..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¾Æ´Ï¾ß.. ºĞ¸í °°ÀÌ µ¹¾Æ°¥ ¹æ¹ıÀÌ ÀÖÀ»²¨¾ß..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾Æ¸Ş, Áö±İÀÌ¶ó¸é °í´ë ½ÅÀÌ ³Ê±îÁö Àâ±â Àü¿¡ ³Í ³ª°¥ ¼ö ÀÖ¾î.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÇÏÁö¸¸ ÀÌ³ª! ³Ê¸¸ ¿©±â µÎ°í °¥ ¼ø ¾ø¾î!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾Æ¸Ş!!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "!...¹Ì¾È, ³­ ³Ê¶óµµ ¹«»çÈ÷ º¸³»¾ß°Ú¾î.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¹¹? Áö±İ ±×°Ô ¹«½¼...");
        StoryLine_Ending1.Add(storyLineClass);
        // ÀÌ³ª ÆùÆ® ¹®ÀÚ
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "Go for towards the light.");
        StoryLine_Ending1.Add(storyLineClass);
        // Á¤»ó ÆùÆ®·Î µÇµ¹¸²
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!? ³» ¸öÀÌ ¸Ú´ë·Î ¿òÁ÷ÀÌÀİ¾Æ..!\nÀÌ³ª!!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "³Ê¶óµµ ¹«»çÈ÷ º¸³»±â À§ÇØ¼± ÀÌ ¹æ¹ı ¹Û¿¡ ¾ø¾ú¾î..");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "Àá±ñ¸¸, ³ªÇÑÅ× ±âÈ¸¸¦ Áà!\nÇÑ¹ø¸¸ ´õ ±âÈ¸¸¦ Áà, ÀÌ³ª!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "Àß °¡, ¾Æ¸Ş.");
        StoryLine_Ending1.Add(storyLineClass);
        // ¹® ´İ±â´Â ¼Ò¸®
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÌ³ª!!");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...ÀÌ´ë·Î..ÀÌ´ë·Î ³¡³¾ ¼ø ¾ø¾î.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÌ³ª.. ³ª¿¡°Ôµµ ºñÀåÀÇ ÆĞ´Â ³²¾ÆÀÖ¾î.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "²À ³Ê¸¦ ±¸ÇØ³»°Ú¾î.");
        StoryLine_Ending1.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...½Ã°£À» ´Ù½Ã µÇµ¹·Á¼­¶óµµ.");
        StoryLine_Ending1.Add(storyLineClass);
        // ½Ã°£¿©Çà ¼Ò¸® Ãâ·Â, ÀÌ¹ÌÁö È°¼ºÈ­, ÀÌ¹ÌÁö ½ºÄÉÀÏ Á¶Á¤
    }
    // ¿£µù2ÀÇ ½ÃÀÛ½ºÅä¸® ÀúÀå
    private void AddStory_Start2()
    {
        // ¹® ¾Õ¿¡¼­ ÁøÇàµÉ ºÎºĞ
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÁÁ¾Æ, ´Ù½Ã ¿©±â·Î µ¹¾Æ¿Ô¾î.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÌ³ª´Â 2Ãş ¹æ¿¡ ÀÖ¾úÁö.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÌ¹ø¿£ Á¶½ÉÇØ¼­ °¡¾ß°Ú¾î.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¹¹¾ß, °©ÀÚ±â ¿Ö ÀÌ·¸°Ô ¾îÁö·¯¿î °ÅÁö..?");
        StoryLine_Start2.Add(storyLineClass);
        // ¾ÏÀü
        // ¹æ¿¡¼­ ±ú¾î³­ µÚ ÁøÇàµÉ ºÎºĞ
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "À¹..¸Ó¸®¾ß.. °á±¹ ´çÇÑ°Ç°¡..?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾Æ¸Ş! ÀÏ¾î³µ±¸³ª! ±¦Âú¾Æ?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÌ³ª! ³Í ±¦Âú¾Æ?\n¹º°¡¿¡ ÀâÇôÀÖ´Ù´Â ´À³¦Àº ¾ø¾î?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¹º°¡¿¡ ÀâÇôÀÖ´Ù´Â ´À³¦...?\nÁö±İ ¿©±â ÀâÇô¿Â »óÈ²À» ¸»ÇÏ´Â°Ô ¾Æ´Ï¶ó¸é ¾ø´Â °Í °°¾Æ.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "±×¸®°í ³Êµµ ¾Ë´Ù½ÃÇÇ ³­ ±¦Âú¾Æ°¡ ¾Æ´Ï¶ó ÀÌ³ª¶ó±¸.ÇÏÇÏ");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "...(¿¡ÈŞ) ±×·¡ ³»°¡ ¾Æ´Â ÀÌ³ª°¡ ¸Â´Â°Å °°³×..");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "È÷ÆR, ±×·¡µµ ³Î ´Ù½Ã º¼ ¼ö ÀÖ¾î¼­ ±â»µ.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "³ªµµ ±â»µ. ÇÏÁö¸¸ ¹®ÀÌ Àá±ä °Í °°¾Æ.");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¹®À» ¿©´Â ÀåÄ¡°¡ ºĞ¸í...ÀÀ?");
        StoryLine_Start2.Add(storyLineClass);
        // Ä«¸Ş¶ó ¹«ºù
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¿Ö ±×·¡ ¾Æ¸Ş? ¹«½¼ ¹®Á¦¶óµµ ÀÖ¾î?");
        StoryLine_Start2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¾Æ´Ï.. ¾Æ¹«°Íµµ ¾Æ³Ä.. ÀÏ´Ü Àú ÇìÀÏ·Î¸¦ Ã¬±â¸é ¹®ÀÌ ¿­¸±²¨¾ß.");
        StoryLine_Start2.Add(storyLineClass);
    }
    // 2È¸Â÷¿¡¼­ ÇìÀÏ·Î¸¦ È¹µæÇÑ µÚ ½ºÅä¸® ÀúÀå
    private void AddStory_AfterGetHaloInEnding2()
    {
        // ÀÎº¥Åä¸® »ç¿ë Æ©Åä¸®¾ó ÀÌ¹ÌÁö È°¼ºÈ­
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¿Í! Á¤¸» ¹®ÀÌ ¿­·È¾î.\n¾î¶»°Ô ¾Ë¾Ò¾î, ¾Æ¸Ş?");
        AfterGetHaloInEnding2_Scripts.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÈÊ, ¿Ö³ÄÇÏ¸é ´Ï ¿·ÀÇ Ä£±¸°¡ ÃÖ°íÀÇ Å½Á¤ÀÌ±â ¶§¹®ÀÌÁö.\n´©°¡ ¿À±âÀü¿¡ ¹ÛÀ¸·Î ³ª°¡º¸ÀÚ.");
        AfterGetHaloInEnding2_Scripts.Add(storyLineClass);
    }
    // º¹µµ·Î ³ª¿À´Â ¹®À» Ã³À½ Áö³µÀ»¶§¸¸ Ãâ·ÂÇØÁÙ ½ºÅä¸® ÀúÀå(2È¸Â÷)
    private void AddStroy_IntoTheMainHallFirstTime_Ending2()
    {
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "À÷µéÀÌ ¶Ç ³» ½Ã°è¸¦ ¼û±â°í µµ¸Á°¬¾î!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾Æ¹«·¡µµ À½.. Àú Å¸ÄÚµéÀ» µ¥·Á¿Í¾ß µÇÃ£À» ¼ö ÀÖÀ» ²¨ °°¾Æ.");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "±×·±µ¥ ¾Æ¸Ş ¶Ç¶ó´Ï?\nÀÌÀü¿¡ À÷µéÀ» ¸¸³­ ÀûÀÌ ÀÖ¾î?");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¾Æ..¾Æ¹«°Íµµ ¾Æ³Ä. ¾î¼­ ÂÑ¾Æ°¡ÀÚ!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.TAKOS, "WAH?");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "ÀÀ? ¾Æ¸Ş, ¾êµéÀÌ µµ¿ÍÁÙ°Ç°¡ºÁ.");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "1Ãş¿¡ ÀÖ´Â ¹®µéÀÇ ¸¶¹ıÀ» Ç®¾îÁÙ ¼ö ÀÖ´Ù´Â °Í °°¾Æ.");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "±×·¡? ±×·³ »ç¾çÇÏÁö ¾ÊÀ»²²!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÚ, ¾îµğ ¼û¹Ù²ÀÁúÀ» ÇÏ°í ½Í¾îÇÏ´Â ³à¼®µéÀ» Ã£À¸·¯ °¡º¼±î!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.TAKOS, "WAH!!");
        StoryLine_MainHallDoorGuide2.Add(storyLineClass);
    }
    // ¿£µù2ÀÇ ¿£µù½ºÅä¸® ÀúÀå
    private void AddStory_Ending2()
    {
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÁÁ¾Æ, ¹®À» ¿©´Âµ¥ ¼º°øÇß¾î!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "(ÀÌ¹ø¿£ Ã¥ÀÇ µµ¿òÀ» ¹ŞÁö ¾Ê°í ¹®À» ¿©´Âµ¥ ¼º°øÇß¾î.)");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "(ÇÏÁö¸¸ ±×³É ÀÌ·¸°Ô ³»º¸³»ÁÙ±î?)");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾î¶ó? ¾Æ¸Ş ±× ºû³ª´Â °Ç ¹¹¾ß?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÀ? ÇìÀÏ·Î¿¡¼­ °©ÀÚ±â ºûÀÌ ³ªÀİ¾Æ..?!");
        StoryLine_Ending2.Add(storyLineClass);
        // ÅØ½ºÆ®¹Ú½º¸¸ º¸ÀÌ°Ô²û Èò»ö ¹ÙÅÁ Ãâ·Â
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾Æ¸Ş!!!");
        StoryLine_Ending2.Add(storyLineClass);
        // °ËÀº ¹ÙÅÁÀ¸·Î º¯°æ
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "À¹.. ´«ºÎ¼Å. ´ëÃ¼ ¹«½¼ ÀÏÀÌ ÀÏ¾î³­ °ÅÁö?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "¾î¼­¿Í, ¾Æ¸Ş.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "³Í ´©±¸¾ß..? ´ëÃ¼ ¿©±ä ¾îµğ°í?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "¹Ì¾È. Áö±İ ½Ã°£ÀÌ ¸¹ÀÌ´Â ¾ø¾î¼­ ÀÚ¼¼ÇÑ ¼³¸íÀº ¸øÇØÁÙ²¨ °°¾Æ.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "°á·ĞºÎÅÍ ¸»ÇØÁÖÀÚ¸é, ´öºĞ¿¡ ¼Ó¹Ú¿¡¼­ Ç®¸®°Ô µÇ¼­\n°¨»ç °â µµ¿òÀ» ÁÖ°íÀÚ ÀÌ°÷À¸·Î ÃÊ´ëÇß¾î.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¼Ó¹ÚÀÌ¶ó°í?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "Á¤ÀÛ ¿©±â°¡ ¾îµòÁø ¸»À» ¾ÈÇØÁáÀİ¾Æ. ÀÌ ¹Ùº¸¾ß!");
        StoryLine_Ending2.Add(storyLineClass);
        // Àá±ñ ¾ÆÀÌ¸®½º À§Ä¡¸¦ ¾Æ¸Ş À§Ä¡·Î º¯°æÈÄ ´ÙÀ½ ¶óÀÎ¿¡¼­ µÇµ¹¸²(¾Æ¸Ş ÀÌ¹ÌÁöµµ Á¶Á¤ÇØÁà¾ßÇÔ)
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "ÇÏ¾Æ!? ¸» ¾ÈÇØµµ Áö±İ ÇÒ·Á°í Çß°Åµç? ÇÜ½ºÅÍ °°Àº°Ô!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "¹¹??! ³ª ÇÜ½ºÅÍ ¾Æ´Ï°Åµç?!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "ÀÚÀÚ, µÑ ´Ù ÁøÁ¤ÇÏ°í. ¿©±ä ÀÇ»ç´çÀÌ¾ß.\n¹æ±İ±îÁö ÀÖ¾ú´ø ¼º°ú´Â ´Ù¸¥ °ø°£ÀÌÁö.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÇ»ç´çÀÌ¶ó°í? ´ëÃ¼ ¹» À§ÇÑ ÀÇ»ç´çÀÎ°ÅÁö?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "±×°Å¿¡ ´ëÇÑ ¼³¸íÀº Áö±İÀº ÇØÁÖ±â ¾î·Á¿ï °Í °°³×.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "Á¶±İ Àü¿¡ ¸»ÇØÁá´Ù½ÃÇÇ Áö±İÀº ½Ã°£ÀÌ ¾ø°Åµç.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "°í´ë ½Å ³à¼®.. ¿ì¸®¸¦ Å¸ÄÚÀÇ ÇüÅÂ·Î ¼Ó¹ÚÇØµÎ´Ù´Ï.\n°¡¸¸µÎÁö ¾ÊÀ»²¨¾ß! À¸¾Æ¾Æ!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "ºĞ¸í ³ªÀÇ ¿Ïº®ÇÔ¿¡ ÁúÅõ¸¦ ´À²¸¼­ ±×·¨À»²¨¾ß.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "¾Æ¹«Æ°, ³×°¡ ¿ì¸®ÀÇ ¹°°Çµµ Ã£¾ÆÁØ ´öºĞ¿¡\nÈûÀ» µÇÃ£¾Æ ¼Ó¹Ú¿¡¼­ Ç®·Á³¯ ¼ö ÀÖ¾ú¾î.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "°í¸¶¿ö.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "Ãµ¸¸¿¡. ³ªµµ ÀÌ·¸°Ô µÉ ÁÙÀº ¸ô¶ú¾ú±ä ÇÏÁö¸¸..");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "¿ì¸®°¡ È®ÀÎÇÏ±â·Ğ ³ÊÀÇ Ä£±¸°¡ Áö±İ °í´ë ½Å¿¡°Ô ÀâÇô¿Â°Å °°´øµ¥.\n³» ¸»ÀÌ ¸ÂÁö, ¹«¸ŞÀÌ?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "ÀÀ? ¿À... ¹Ì¾È ±î¸Ô¾ú¾î.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "´ë½Å º£¸®¶óµµ Á» ¸ÔÀ»·¡?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "...¸Â¾Æ. ÀÌ³ª¶ó´Â ÀÌ¸§À» °¡Á³´ø°Å °°Àºµ¥.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!!! ¾î¶»°Ô ±×°É..\n¾Æ´Ï, ÀÌ³ª¸¦ ¾ÈÀüÇÏ°Ô µ¥¸®°í ³ª°¥ ¼ö ÀÖ°Ô µµ¿Í ÁÙ ¼ö ÀÖ´Â°Å¾ß?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "±×·³~ µµ¿ÍÁÖ°Ú´Ù°í ÇßÀİ¾Æ?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "¿ì¸®ÀÇ Èûµµ µÇµ¹¾Æ¿ÔÀ¸´Ï ±× ³à¼®À» ºÀÀÎ½ÃÅ³ ¼ö ÀÖÀ»²¨¾ß.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "¸¶Ä§ Ã¥ÀÇ ÇüÅÂ·Î ÇöÇöÇØ ÀÖÀ¸´Ï Á» ´õ ¼ö¿ùÇÏ°Ô µÉ²¨ °°±âµµ ÇÏ°í.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "±×°Ô Á¤¸»ÀÌ¾ß?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "±×·¡. ¾Æ¸¶ µ¹¾Æ°¡°í³ª¸é ³à¼®Àº ¾ÆÁ÷ °è¾àÇÏÁö ¸øÇßÀ¸´Ï\n³ÊÈñ¸¦ °­Á¦·Î Àâ¾ÆµÑ·Á°í ÇÒ²¨¾ß.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "±× Æ´À» Å¸ ¿ì¸®°¡ ºÀÀÎÇØ ¹ö¸®´Â °ÅÁö.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "À½.. ÀÌÁ¨ ½Ã°£ÀÌ ´Ù µÈ°Å °°¾Æ.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "ÀÌÁ¦ µ¹·Á º¸³»Áà¾ß°Ú³×.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "±× ½Ã°è¿¡ °üÇØ ÇÒ ¸»ÀÌ ÀÖÁö¸¸.. Áö±İÀº ³Ñ¾î°¡µµ·Ï ÇÏ°Ú¾î.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "À¹.. ¾ÆÇÏÇÏ.. ¹«½¼ ¸»ÀÎÁö Àß ¸ğ¸£°Ú´Â°É.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "±×·³ Àß°¡! Á» ÀÖ´Ù º¸ÀÚ°í.");
        StoryLine_Ending2.Add(storyLineClass);

        // Èò»ö ÆäÀÌµå ÀÎ ¾Æ¿ô

        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾Æ¸Ş! ¾Æ¸Ş!! ±¦Âú¾Æ?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÀÀ ³­ ±¦Âú¾Æ! ÀÌ³ª.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "°©ÀÚ±â ºûÀÌ ³ª´õ´Ï ³Ê°¡ »ç¶óÁ³´Ù°¡ ´Ù½Ã µ¹¾Æ¿Ô¾î!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "(½Ã°£À» Àá±ñ ¸ØÃè´ø°Ç°¡..) ³­ ±¦Âú¾Æ. ±×·³ ÀÌÁ¦ ³ª°¡ÀÚ.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "...ÀÀ? Ã¥¿¡¼­ °©ÀÚ±â °ËÀº ¿¬±â°¡..!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "!! ÀÌ³ª Ã¥¿¡¼­ ¶³¾îÁ®!");
        StoryLine_Ending2.Add(storyLineClass);
        // ¾Æ¿ÀÂù¿¡¼­ °ËÀº¿¬±â, ´ë»ç´Â ...ÀÌ¶û !°¡ ÀüºÎ
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "±×·¸°ÔÇÏ°Ô µÑ±îº¸³Ä!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.KRONII, "°¨È÷ ¿ì¸®¸¦ ¿©±â¿¡ Àâ¾ÆµÑ·Á°í ÇÏ´Ù´Ï.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.FAUNA, "³ª»Û ¾ÆÀÌ¿¡°Õ ¹úÀ» Áà¾ß°ÚÁö?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.MUMEI, "Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.\nÁ×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.Á×ÀÎ´Ù.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.IRYS, "ÀÚ, ¾äÀüÈ÷ ºÀÀÎµÇ¾î Áà¾ß°Ú¾î!");
        StoryLine_Ending2.Add(storyLineClass);
        // ¾Æ¿ÀÂù¿¡¼­ °ËÀº¿¬±â, ´ë»ç´Â ...ÀÌ¶û !°¡ ÀüºÎ
        // ¼è»ç½½ ¼Ò¸® Ãâ·Â
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.BAE, "ÁÁ¾Æ ºÀÀÎ ¼º°ø!\n±×·³ ¿ì¸° ÀÌ¸¸!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¹Ù·Î °¡¹ö·È³×..");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "¾î...¾Æ¸Ş..? ÀÌ°Ô ¹«½¼ »óÈ²ÀÌ¾ß?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "À½..¿ä¾àÇÏÀÚ¸é ¿ì¸®°¡ Ã£Àº Å¸ÄÚµéÀÌ »ç½Ç ºÙÀâÇôÀÖ´ø Á¸ÀçµéÀÌ¿´´ø °ÅÁö.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "¸¶Ä§ ºúÀÌ ÀÖ¾ú±â¿¡ ¿ì¸± µµ¿Í¼­\n³Î ³³Ä¡ÇÑ ¾Ç½ÅÀ» ºÀÀÎÇØÁØ°Å°í.");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "±×·³ ³ª.. ÀÌÁ¦ ¹«»çÈ÷ ³ª°¥ ¼ö ÀÖ´Â°Å¾ß..?");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.AME, "ÁıÀ¸·Î µ¹¾Æ°¡ÀÚ, ÀÌ³ª!");
        StoryLine_Ending2.Add(storyLineClass);
        storyLineClass = new StoryLineClass(StoryLineClass.StoryCharacter_Enum.INA, "..ÀÀ!");
        StoryLine_Ending2.Add(storyLineClass);
        // ¿£µù Å©·¹µ÷ Àç»ı
    }
}