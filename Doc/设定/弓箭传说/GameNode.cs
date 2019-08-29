using System;
using UnityEngine;

public class GameNode
{
    private static GameObject m_NetP;
    private static Transform m_HPP;
    private static Transform m_RootP;
    private static Transform m_JoyP;
    private static Transform m_UIMainP;
    private static Transform m_InGameP;
    private static Transform m_InGame2P;
    private static Transform m_FrontP;
    private static Transform m_FrontEventP;
    private static Transform m_Front2P;
    private static Transform m_Front3P;
    private static Transform m_FrontMaskP;
    private static Transform m_FrontNetP;
    private static Transform m_TipsUIP;
    private static GameObject m_LightP;
    private static GameObject m_CameraParentP;
    private static Camera m_CameraP;
    private static Animation m_CameraAniP;
    private static Camera m_UICameraP;
    private static Transform m_TipsP;
    private static GameObject m_BattleP;
    private static GameObject m_RoomP;
    private static GameObject m_MonsterP;
    private static GameObject m_SurviveResP;
    private static GameObject m_MainP;
    private static Transform m_PoolParentP;
    private static Transform m_PoolMapParentP;
    private static Transform m_PlayerBulletP;
    private static Transform m_BulletParentP;
    private static AudioSource mBackgroundMusic;
    private static Transform mSoundNode;
    private static Transform mMapCacheNode;

    public static void CameraShake(CameraShakeType type)
    {
        if (GameLogic.Release.Game.RoomState == RoomState.Runing)
        {
            switch (type)
            {
                case CameraShakeType.Crit:
                    m_CameraAni.Play("CamShake_Crit");
                    break;

                case CameraShakeType.BridgeDown:
                    m_CameraAni.Play("CamShake_BridgeDown");
                    break;

                case CameraShakeType.FirstDrop:
                    m_CameraAni.Play("CamShake_FirstDrop");
                    break;

                case CameraShakeType.EquipDrop:
                    m_CameraAni.Play("CamShake_EquipDrop");
                    break;
            }
        }
    }

    public static void DestroyMonsterNode()
    {
        Object.Destroy(m_MonsterP);
        m_MonsterP = null;
    }

    public static GameObject m_Net
    {
        get
        {
            if (m_NetP == null)
            {
                m_NetP = new GameObject("NetNode");
            }
            return m_NetP;
        }
    }

    public static Transform m_HP
    {
        get
        {
            if (m_HPP == null)
            {
                m_HPP = m_Root.Find("InGame/HPParent");
            }
            return m_HPP;
        }
    }

    public static Transform m_Root
    {
        get
        {
            if (m_RootP == null)
            {
                m_RootP = GameObject.Find("UIRoot").transform;
            }
            return m_RootP;
        }
    }

    public static Transform m_Joy
    {
        get
        {
            if (m_JoyP == null)
            {
                m_JoyP = m_Root.Find("InGame/Joy");
            }
            return m_JoyP;
        }
    }

    public static Transform m_UIMain
    {
        get
        {
            if (m_UIMainP == null)
            {
                m_UIMainP = m_Root.Find("Main");
            }
            return m_UIMainP;
        }
    }

    public static Transform m_InGame
    {
        get
        {
            if (m_InGameP == null)
            {
                m_InGameP = m_Root.Find("InGame/GameUI");
            }
            return m_InGameP;
        }
    }

    public static Transform m_InGame2
    {
        get
        {
            if (m_InGame2P == null)
            {
                m_InGame2P = m_Root.Find("InGame/GameUI2");
            }
            return m_InGame2P;
        }
    }

    public static Transform m_Front
    {
        get
        {
            if (m_FrontP == null)
            {
                m_FrontP = GameObject.Find("Front").transform;
            }
            return m_FrontP;
        }
    }

    public static Transform m_FrontEvent
    {
        get
        {
            if (m_FrontEventP == null)
            {
                m_FrontEventP = GameObject.Find("FrontEvent").transform;
            }
            return m_FrontEventP;
        }
    }

    public static Transform m_Front2
    {
        get
        {
            if (m_Front2P == null)
            {
                m_Front2P = GameObject.Find("Front2").transform;
            }
            return m_Front2P;
        }
    }

    public static Transform m_Front3
    {
        get
        {
            if (m_Front3P == null)
            {
                m_Front3P = GameObject.Find("Front3").transform;
            }
            return m_Front3P;
        }
    }

    public static Transform m_FrontMask
    {
        get
        {
            if (m_FrontMaskP == null)
            {
                m_FrontMaskP = GameObject.Find("FrontMask").transform;
            }
            return m_FrontMaskP;
        }
    }

    public static Transform m_FrontNet
    {
        get
        {
            if (m_FrontNetP == null)
            {
                m_FrontNetP = GameObject.Find("FrontNet").transform;
            }
            return m_FrontNetP;
        }
    }

    public static Transform m_TipsUI
    {
        get
        {
            if (m_TipsUIP == null)
            {
                m_TipsUIP = GameObject.Find("FrontTips").transform;
            }
            return m_TipsUIP;
        }
    }

    public static GameObject m_Light
    {
        get
        {
            if (m_LightP == null)
            {
                m_LightP = GameObject.Find("light");
            }
            return m_LightP;
        }
    }

    public static GameObject m_CameraParent
    {
        get
        {
            if (m_CameraParentP == null)
            {
                m_CameraParentP = GameObject.Find("CameraControlM");
            }
            return m_CameraParentP;
        }
    }

    public static Camera m_Camera
    {
        get
        {
            if (m_CameraP == null)
            {
                m_CameraP = GameObject.Find("CameraControlM/Child/Camera").GetComponent<Camera>();
            }
            return m_CameraP;
        }
    }

    private static Animation m_CameraAni
    {
        get
        {
            if (m_CameraAniP == null)
            {
                m_CameraAniP = GameObject.Find("CameraControlM").GetComponent<Animation>();
            }
            return m_CameraAniP;
        }
    }

    public static Camera m_UICamera
    {
        get
        {
            if (m_UICameraP == null)
            {
                m_UICameraP = GameObject.Find("CameraControlM/Child/UICamera").GetComponent<Camera>();
            }
            return m_UICameraP;
        }
    }

    public static Transform m_Tips
    {
        get
        {
            if (m_TipsP == null)
            {
                m_TipsP = GameObject.Find("InGame/Tips").transform;
            }
            return m_TipsP;
        }
    }

    public static GameObject m_Battle
    {
        get
        {
            if (m_BattleP == null)
            {
                m_BattleP = new GameObject();
                m_BattleP.name = "BattleRoot";
            }
            return m_BattleP;
        }
    }

    public static GameObject m_Room
    {
        get
        {
            if (m_RoomP == null)
            {
                m_RoomP = new GameObject();
                m_RoomP.transform.SetParent(m_Battle.transform);
                m_RoomP.name = "Room";
            }
            return m_RoomP;
        }
    }

    public static GameObject m_Monster
    {
        get
        {
            if (m_MonsterP == null)
            {
                m_MonsterP = new GameObject();
                m_MonsterP.transform.SetParent(m_Room.transform);
                m_MonsterP.name = "Monster";
            }
            return m_MonsterP;
        }
    }

    public static GameObject m_SurviveResRoot
    {
        get
        {
            if (m_SurviveResP == null)
            {
                m_SurviveResP = new GameObject();
                m_SurviveResP.transform.parent = m_Battle.transform;
                m_SurviveResP.name = "SurviveResRoot";
            }
            return m_SurviveResP;
        }
    }

    public static GameObject m_MainMapRoot
    {
        get
        {
            if (m_MainP == null)
            {
                m_MainP = new GameObject();
                m_MainP.name = "MainMapRoot";
            }
            return m_MainP;
        }
    }

    public static Transform m_PoolParent
    {
        get
        {
            if (m_PoolParentP == null)
            {
                m_PoolParentP = new GameObject("PoolParent").transform;
            }
            return m_PoolParentP;
        }
    }

    public static Transform m_PoolMapParent
    {
        get
        {
            if (m_PoolMapParentP == null)
            {
                m_PoolMapParentP = new GameObject("PoolMapParent").transform;
            }
            return m_PoolMapParentP;
        }
    }

    public static Transform m_PlayerBullet
    {
        get
        {
            if (m_PlayerBulletP == null)
            {
                m_PlayerBulletP = new GameObject("PlayerBullet").transform;
            }
            return m_PlayerBulletP;
        }
    }

    public static Transform m_BulletParent
    {
        get
        {
            if (m_BulletParentP == null)
            {
                m_BulletParentP = new GameObject("BulletParent").transform;
            }
            return m_BulletParentP;
        }
    }

    public static AudioSource BackgroundMusic
    {
        get
        {
            if (mBackgroundMusic == null)
            {
                mBackgroundMusic = GameObject.Find("CameraControlM/backgroundmusic").GetComponent<AudioSource>();
            }
            return mBackgroundMusic;
        }
    }

    public static Transform SoundNode
    {
        get
        {
            if (mSoundNode == null)
            {
                mSoundNode = new GameObject("SoundNode").transform;
            }
            return mSoundNode;
        }
    }

    public static Transform MapCacheNode
    {
        get
        {
            if (mMapCacheNode == null)
            {
                mMapCacheNode = new GameObject("MapCache").transform;
            }
            return mMapCacheNode;
        }
    }
}

