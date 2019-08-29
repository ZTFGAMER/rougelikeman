using System;
using System.Collections.Generic;
using UnityEngine;

public class KeySpeedAnimation
{
    private List<KeyInfo> keyList = new List<KeyInfo>();
    private int phaseIndex;
    private float timeCount;
    private float speed;
    private Action<int, float, float> onUpdate;

    public KeySpeedAnimation(List<KeyInfo> keyList, Action<int, float, float> onUpdate)
    {
        this.keyList = keyList;
        this.onUpdate = onUpdate;
        this.Reset();
    }

    public float GetTotalLength()
    {
        float num = 0f;
        for (int i = 0; i < this.keyList.Count; i++)
        {
            float spdFrom = this.keyList[i].spdFrom;
            float time = this.keyList[i].time;
            float num5 = (this.keyList[i].spdTo - this.keyList[i].spdFrom) / this.keyList[i].time;
            num += (spdFrom * time) + (((0.5f * num5) * time) * time);
        }
        return num;
    }

    public void Reset()
    {
        this.phaseIndex = 0;
        this.speed = 0f;
        this.timeCount = 0f;
    }

    public bool Update(float deltaTime)
    {
        if (this.phaseIndex >= this.keyList.Count)
        {
            return true;
        }
        this.timeCount += deltaTime;
        KeyInfo info = this.keyList[this.phaseIndex];
        if (this.timeCount < info.time)
        {
            this.speed = Mathf.Lerp(info.spdFrom, info.spdTo, this.timeCount / info.time);
            if (this.onUpdate != null)
            {
                this.onUpdate(this.phaseIndex, this.speed, deltaTime);
            }
        }
        else
        {
            this.timeCount -= info.time;
            this.phaseIndex++;
        }
        return false;
    }

    public class KeyInfo
    {
        public float spdFrom;
        public float spdTo;
        public float time;
    }
}

