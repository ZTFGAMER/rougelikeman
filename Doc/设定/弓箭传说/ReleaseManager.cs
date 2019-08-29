using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class ReleaseManager : MonoBehaviour
{
    private GameManager _Game;
    private EntityManager _Entity;
    private BulletManager _Bullet;
    private BulletManager _PlayerBullet;
    private EffectManager _Effect;
    private MapEffectManager _MapEffect;
    private EntityCacheManager _EntityCache;
    private GameFormManager _Form;
    private MapCreator _MapCreator;
    private FindPath _Path;
    private GoodsCreateManager _GoodsCreate;
    private ReleaseModeManager _Mode;

    private void Awake()
    {
        GameLogic.SetRelease(this);
    }

    public void Release()
    {
        Time.timeScale = 1f;
        this.Bullet.Release();
        this.PlayerBullet.Release();
        this.Effect.Release();
        this.Form.Release();
        this.GoodsCreate.Release();
        if (this._Path != null)
        {
            this._Path.DeInit();
        }
        this.Game.Release();
        this.EntityCache.Release();
        this.Entity.DeInit();
        this.Mode.DeInit();
        this.MapEffect.Release();
        LocalModelManager.Instance.Drop_Drop.ClearGoldDrop();
        CInstance<TipsManager>.Instance.Clear();
        Updater.GetUpdater().OnRelease();
        Updater.UpdaterDeinit();
        Goods1151.DoorData.DeInit();
        Object.DestroyImmediate(GameNode.m_Battle);
        GameNode.MapCacheNode.DestroyChildren();
        GameLogic.Hold.Sound.DeInit();
        if (this._MapCreator != null)
        {
            this._MapCreator.Deinit();
        }
        if (this._Entity != null)
        {
            Object.DestroyImmediate(this._Entity.gameObject);
            this._Entity = null;
        }
        TimerBase<Timer>.Unregister();
        TimeClock.Clear();
        GameNode.m_HP.DestroyChildren();
        this._Game = null;
        this._Bullet = null;
        this._PlayerBullet = null;
        this._Effect = null;
        this._MapEffect = null;
        this._EntityCache = null;
        this._MapCreator = null;
        this._Path = null;
        this._GoodsCreate = null;
        this._Mode = null;
        this._Form = null;
        Goods1151.DoorData = null;
        GC.Collect();
    }

    public GameManager Game
    {
        get
        {
            if (this._Game == null)
            {
                this._Game = new GameManager();
            }
            return this._Game;
        }
    }

    public EntityManager Entity
    {
        get
        {
            if (this._Entity == null)
            {
                this._Entity = new GameObject("EntityManager").AddComponent<EntityManager>();
            }
            return this._Entity;
        }
    }

    public BulletManager Bullet
    {
        get
        {
            if (this._Bullet == null)
            {
                this._Bullet = new BulletManager(0);
                this._Bullet.parent = GameNode.m_BulletParent;
            }
            return this._Bullet;
        }
    }

    public BulletManager PlayerBullet
    {
        get
        {
            if (this._PlayerBullet == null)
            {
                this._PlayerBullet = new BulletManager(1);
                this._PlayerBullet.parent = GameNode.m_PlayerBullet;
            }
            return this._PlayerBullet;
        }
    }

    public EffectManager Effect
    {
        get
        {
            if (this._Effect == null)
            {
                this._Effect = new EffectManager();
            }
            return this._Effect;
        }
    }

    public MapEffectManager MapEffect
    {
        get
        {
            if (this._MapEffect == null)
            {
                this._MapEffect = new MapEffectManager();
            }
            return this._MapEffect;
        }
    }

    public EntityCacheManager EntityCache
    {
        get
        {
            if (this._EntityCache == null)
            {
                this._EntityCache = new EntityCacheManager();
            }
            return this._EntityCache;
        }
    }

    public GameFormManager Form
    {
        get
        {
            if (this._Form == null)
            {
                this._Form = new GameFormManager();
            }
            return this._Form;
        }
    }

    public MapCreator MapCreatorCtrl
    {
        get
        {
            if (this._MapCreator == null)
            {
                this._MapCreator = new MapCreator();
            }
            return this._MapCreator;
        }
    }

    public FindPath Path
    {
        get
        {
            if (this._Path == null)
            {
                this._Path = new FindPath();
            }
            return this._Path;
        }
    }

    public GoodsCreateManager GoodsCreate
    {
        get
        {
            if (this._GoodsCreate == null)
            {
                this._GoodsCreate = new GoodsCreateManager();
            }
            return this._GoodsCreate;
        }
    }

    public ReleaseModeManager Mode
    {
        get
        {
            if (this._Mode == null)
            {
                this._Mode = new ReleaseModeManager();
            }
            return this._Mode;
        }
    }
}

