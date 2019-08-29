using System;

public class EventAggregator
{
    public static NewRoundEvent NewRound = new NewRoundEvent();
    public static GameOverEvent YouWin = new GameOverEvent();
}

