
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    private static volatile GameLauncher _Instance;
    private float touchtime;
    private bool bPause;

    protected void _InitNameGenerator()
    {
    }

    protected void _InitPureMVC()
    {
        new AppFacade();
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = -1;
        GameLogic.Width = 720;
        GameLogic.Height = (int) ((Screen.height * GameLogic.Width) / ((float) Screen.width));
        GameLogic.WidthScale = ((float) GameLogic.Width) / ((float) GameLogic.DesignWidth);
        GameLogic.HeightScale = ((float) GameLogic.Height) / ((float) GameLogic.DesignHeight);
        GameLogic.ScreenSize = new Vector2(((float) (Screen.width * GameLogic.DesignHeight)) / ((float) Screen.height), (float) GameLogic.DesignHeight);
        GameLogic.WidthScaleAll = (GameLogic.WidthScale >= GameLogic.HeightScale) ? 1f : (GameLogic.WidthScale / GameLogic.HeightScale);
        float num = (((float) GameLogic.Width) / ((float) GameLogic.Height)) * GameLogic.DesignHeight;
        GameLogic.WidthReal = MathDxx.Clamp(num, num, (float) GameLogic.DesignWidth);
        GameLogic.ResetMaxResolution();
    }

    private void buy_gold()
    {
        CDiamondToCoin packet = new CDiamondToCoin {
            m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
            m_nCoins = 1,
            m_nDiamonds = 1
        };
        NetManager.SendInternal<CDiamondToCoin>(packet, SendType.eForceOnce, delegate (NetResponse response) {
            if ((response.IsSuccess && (response.data != null)) && (response.data is CRespDimaonToCoin))
            {
                CRespDimaonToCoin data = response.data as CRespDimaonToCoin;
                LocalSave.Instance.UserInfo_SetDiamond((int) data.m_nDiamonds);
                LocalSave.Instance.UserInfo_SetGold((int) data.m_nCoins);
                this.buy_gold();
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
            }
        });
    }

    private void Start()
    {
        _Instance = this;
        ResourceManager.Init();
        GameConfig.Init();
        SdkManager.set_first_setup_time();
        SdkManager.send_event("app_start");
        LocalModelManager.Instance.Stage_Level_chapter1.Init();
        LocalModelManager.Instance.Equip_equip.Init();
        LocalModelManager.Instance.Character_Level.Init();
        LocalModelManager.Instance.Stage_Level_stagechapter.Init();
        LocalModelManager.Instance.Stage_Level_activitylevel.Init();
        LocalModelManager.Instance.Stage_Level_activity.Init();
        LocalModelManager.Instance.Achieve_Achieve.Init();
        LocalModelManager.Instance.Shop_MysticShop.Init();
        CInstance<PlayerPrefsMgr>.Instance.Init();
        AdsRequestHelper.Init();
        AdsRequestHelper.getRewardedAdapter().isLoaded();
        LocalSave.Instance.InitData();
        LocalSave.Instance.BattleIn_CheckInit();
        GameLogic.Hold.Guide.Init();
        this._InitNameGenerator();
        SdkManager.InitSdks();
        Debug.unityLogger.logEnabled = false;
        NetManager.mNetCache.Init();
        NetManager.StartPing();
        WindowUI.Init();
        this._InitPureMVC();
    }

    private void Update()
    {
        if (((Application.platform == RuntimePlatform.Android) || (Application.platform == RuntimePlatform.WindowsPlayer)) && Input.GetKeyDown(KeyCode.Escape))
        {
            if ((Time.time - this.touchtime) > 2f)
            {
                this.touchtime = Time.time;
                CInstance<TipsUIManager>.Instance.Show(GameLogic.Hold.Language.GetLanguageByTID("再按一次退出游戏", Array.Empty<object>()), GameLogic.Height * 0.12f);
            }
            else
            {
                Application.Quit();
            }
        }
    }

    private void update_touch()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            this.bPause = !this.bPause;
            GameLogic.SetPause(this.bPause);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            CameraControlM.CameraFollow = !CameraControlM.CameraFollow;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            this.buy_gold();
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
            Drop_DropModel.DropData item = new Drop_DropModel.DropData {
                type = PropType.eCurrency,
                id = 1,
                count = 0x7d0
            };
            list.Add(item);
            item = new Drop_DropModel.DropData {
                type = PropType.eCurrency,
                id = 2,
                count = 100
            };
            list.Add(item);
            item = new Drop_DropModel.DropData {
                type = PropType.eCurrency,
                id = 3,
                count = 10
            };
            list.Add(item);
            item = new Drop_DropModel.DropData {
                type = PropType.eCurrency,
                id = 4,
                count = 3
            };
            list.Add(item);
            if (MathDxx.RandomBool())
            {
                item = new Drop_DropModel.DropData {
                    type = PropType.eEquip,
                    id = 0xfdee5,
                    count = 1
                };
                list.Add(item);
                item = new Drop_DropModel.DropData {
                    type = PropType.eEquip,
                    id = 0xfdee6,
                    count = 1
                };
                list.Add(item);
                item = new Drop_DropModel.DropData {
                    type = PropType.eEquip,
                    id = 0x7595,
                    count = 5
                };
                list.Add(item);
                item = new Drop_DropModel.DropData {
                    type = PropType.eEquip,
                    id = 0x7597,
                    count = 10
                };
                list.Add(item);
            }
            WindowUI.ShowRewardSimple(list);
        }
    }

    public static GameLauncher Instance =>
        _Instance;
}

