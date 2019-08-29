using System;

public class BattleLevelWaveData
{
    public bool showui;
    public int currentwave;
    public int maxwave;
    public int lasttime;

    public bool is_last_wave() => 
        (this.currentwave >= this.maxwave);
}

