using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class HoldManager : MonoBehaviour
{
    private SoundManager _Sound;
    private LanguageManager _Language;
    private DropManager _Drop;
    private HoldPoolManager _Pool;
    private GuideManager _Guide;
    private int mPreLoadCount = -1;
    private BattleModuleData _BattleData;
    private Vector3 sound_pos = (Vector3.one * 1000f);

    private void Awake()
    {
        GameLogic.SetHold(this);
        RoomGenerateBase.PreloadMap(1);
    }

    public void BattleDataReset()
    {
        this.Drop.Reset();
        this.BattleData.Reset();
        this.mPreLoadCount = -1;
        this.PreLoad(0);
    }

    public void PreLoad(int id)
    {
        Preload_load beanById = LocalModelManager.Instance.Preload_load.GetBeanById(id);
        if ((beanById != null) && (this.mPreLoadCount < id))
        {
            this.mPreLoadCount = id;
            this.PreLoadPlayerBullets(beanById.PlayerBulletsPath);
            this.PreLoadBullets(beanById.BulletsPath);
            this.PreLoadEffects(beanById.EffectsPath);
            this.PreLoadMapEffects(beanById.MapEffectsPath);
            this.PreLoadGoods(beanById.GoodsPath);
            this.PreLoadSounds(beanById.SoundPath);
        }
    }

    private void PreLoadBullet(int BulletID)
    {
        GameObject[] objArray = new GameObject[6];
        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i] = GameLogic.BulletGet(BulletID);
        }
        for (int j = 0; j < objArray.Length; j++)
        {
            GameLogic.BulletCache(BulletID, objArray[j]);
        }
    }

    private void PreLoadBullet(int BulletID, int count)
    {
        GameObject[] objArray = new GameObject[count];
        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i] = GameLogic.BulletGet(BulletID);
        }
        for (int j = 0; j < objArray.Length; j++)
        {
            GameLogic.BulletCache(BulletID, objArray[j]);
        }
    }

    private void PreLoadBullets(string[] s)
    {
        int index = 0;
        int length = s.Length;
        while (index < length)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = s[index].Split(separator);
            int bulletID = int.Parse(strArray[0]);
            int count = int.Parse(strArray[1]);
            this.PreLoadBullet(bulletID, count);
            index++;
        }
    }

    private void PreLoadEffect(string path, int count)
    {
        GameObject[] objArray = new GameObject[count];
        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i] = GameLogic.EffectGet(path);
        }
    }

    private void PreLoadEffects(string[] s)
    {
        int index = 0;
        int length = s.Length;
        while (index < length)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = s[index].Split(separator);
            string path = strArray[0];
            int count = int.Parse(strArray[1]);
            this.PreLoadEffect(path, count);
            index++;
        }
    }

    private void PreLoadGoods(string[] s)
    {
        int index = 0;
        int length = s.Length;
        while (index < length)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = s[index].Split(separator);
            int goodid = int.Parse(strArray[0]);
            int count = int.Parse(strArray[1]);
            this.PreLoadGoods(goodid, count);
            index++;
        }
    }

    private void PreLoadGoods(int goodid, int count)
    {
        GameObject[] objArray = new GameObject[count];
        object[] args = new object[] { "Game/Food/", goodid };
        string key = Utils.GetString(args);
        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i] = GameLogic.EffectGet(key);
        }
        for (int j = 0; j < objArray.Length; j++)
        {
            GameLogic.EffectCache(objArray[j]);
        }
    }

    private void PreLoadMapEffect(string path, int count)
    {
        GameObject[] objArray = new GameObject[count];
        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i] = GameLogic.Release.MapEffect.Get(path);
        }
    }

    private void PreLoadMapEffects(string[] s)
    {
        int index = 0;
        int length = s.Length;
        while (index < length)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = s[index].Split(separator);
            string path = strArray[0];
            int count = int.Parse(strArray[1]);
            this.PreLoadMapEffect(path, count);
            index++;
        }
    }

    public void PreLoadPlayerBullet(int BulletID, int count)
    {
        GameObject[] objArray = new GameObject[count];
        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i] = GameLogic.Release.PlayerBullet.Get(BulletID);
        }
        for (int j = 0; j < objArray.Length; j++)
        {
            GameLogic.Release.PlayerBullet.Cache(BulletID, objArray[j]);
        }
    }

    public void PreLoadPlayerBullets(string[] s)
    {
        int index = 0;
        int length = s.Length;
        while (index < length)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = s[index].Split(separator);
            int bulletID = int.Parse(strArray[0]);
            int count = int.Parse(strArray[1]);
            this.PreLoadPlayerBullet(bulletID, count);
            index++;
        }
    }

    public void PreloadSound(int soundid)
    {
        GameLogic.Hold.Sound.PlayHitted(soundid, this.sound_pos, 0f);
    }

    private void PreLoadSounds(int[] ids)
    {
        int index = 0;
        int length = ids.Length;
        while (index < length)
        {
            this.PreloadSound(ids[index]);
            index++;
        }
    }

    public void SetSound(SoundManager sound)
    {
        this._Sound = sound;
    }

    public SoundManager Sound =>
        this._Sound;

    public LanguageManager Language
    {
        get
        {
            if (this._Language == null)
            {
                this._Language = new LanguageManager();
            }
            return this._Language;
        }
    }

    public DropManager Drop
    {
        get
        {
            if (this._Drop == null)
            {
                this._Drop = new DropManager();
            }
            return this._Drop;
        }
    }

    public HoldPoolManager Pool
    {
        get
        {
            if (this._Pool == null)
            {
                this._Pool = new HoldPoolManager();
            }
            return this._Pool;
        }
    }

    public GuideManager Guide
    {
        get
        {
            if (this._Guide == null)
            {
                this._Guide = new GuideManager();
            }
            return this._Guide;
        }
    }

    public BattleModuleData BattleData
    {
        get
        {
            if (this._BattleData == null)
            {
                this._BattleData = new BattleModuleData();
            }
            return this._BattleData;
        }
    }
}

