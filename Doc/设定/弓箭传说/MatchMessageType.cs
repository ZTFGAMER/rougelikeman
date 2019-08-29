using System;

public enum MatchMessageType : short
{
    eInvalid = 0,
    eGameStart = 10,
    eGameEnd = 11,
    eUserComeIn = 12,
    eUserLeave = 13,
    eLearnSkill = 0x15,
    eDead = 0x16,
    eReborn = 0x17,
    eScoreUpdate = 0x18
}

