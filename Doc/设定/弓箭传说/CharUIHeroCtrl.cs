using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class CharUIHeroCtrl : MonoBehaviour
{
    public GameObject child;
    public GameObject[] petsparent;
    private BodyMask m_Body;
    private GameObject weaponobj;
    private GameObject[] pets = new GameObject[2];
    private int weaponid = -1;
    private int clothid = -1;
    private int[] petsid = new int[] { -1, -1 };
    private SequencePool mSeqPool = new SequencePool();
    private bool bChildShow;

    private string GetBodyString(string value)
    {
        object[] args = new object[] { value };
        return Utils.FormatString("Game/Models/{0}", args);
    }

    [DebuggerHidden]
    private IEnumerator init_cloth() => 
        new <init_cloth>c__Iterator0 { $this = this };

    [DebuggerHidden]
    private IEnumerator init_pet(int index) => 
        new <init_pet>c__Iterator1 { 
            index = index,
            $this = this
        };

    public void InitCloth(int clothid)
    {
        if (!((this.clothid == clothid) & (this.m_Body != null)))
        {
            this.clothid = clothid;
            if (this.m_Body != null)
            {
                Object.Destroy(this.m_Body.gameObject);
                this.m_Body = null;
            }
            if (base.gameObject.activeInHierarchy)
            {
                base.StartCoroutine(this.init_cloth());
            }
        }
    }

    public void InitPet(int index, int petid)
    {
        if ((index < 0) || (index >= this.petsid.Length))
        {
            object[] args = new object[] { index, petid };
            SdkManager.Bugly_Report("CharUIHeroCtrl", Utils.FormatString("InitPet index:{0} petid:{1} is out of range.", args));
        }
        else if (this.petsid[index] != petid)
        {
            this.petsid[index] = petid;
            if (this.pets[index] != null)
            {
                Object.Destroy(this.pets[index]);
            }
            if (base.gameObject.activeInHierarchy)
            {
                base.StartCoroutine(this.init_pet(index));
            }
        }
    }

    public void InitWeapon(int weaponid)
    {
        if (this.weaponid != weaponid)
        {
            if (this.weaponobj != null)
            {
                Object.Destroy(this.weaponobj);
                this.weaponobj = null;
            }
            this.weaponid = weaponid;
            if (this.m_Body != null)
            {
                object[] args = new object[] { "Game/WeaponHand/WeaponHand", weaponid };
                GameObject obj2 = GameLogic.EffectGet(Utils.GetString(args));
                if (obj2 != null)
                {
                    Weapon_weapon beanById = LocalModelManager.Instance.Weapon_weapon.GetBeanById(weaponid);
                    obj2.transform.parent = WeaponBase.GetWeaponNode(this.m_Body, beanById.WeaponNode);
                    obj2.transform.localPosition = Vector3.zero;
                    obj2.transform.localScale = Vector3.one;
                    obj2.transform.localRotation = Quaternion.identity;
                    this.weaponobj = obj2;
                    MeshRenderer[] componentsInChildren = this.weaponobj.GetComponentsInChildren<MeshRenderer>(true);
                    this.weaponobj.ChangeChildLayer(LayerManager.UI);
                }
            }
        }
    }

    public void Show(bool value)
    {
        if (this.bChildShow != value)
        {
            this.bChildShow = value;
            if (!value)
            {
                this.mSeqPool.Clear();
                this.child.SetActive(value);
            }
            else
            {
                TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.mSeqPool.Get(), 1f), new TweenCallback(this, this.<Show>m__0));
            }
        }
    }

    [CompilerGenerated]
    private sealed class <init_cloth>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal string <bodystring1>__0;
        internal string <bodystring2>__0;
        internal CharUIHeroCtrl $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        private <init_cloth>c__AnonStorey2 $locvar0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$locvar0 = new <init_cloth>c__AnonStorey2();
                    this.$locvar0.<>f__ref$0 = this;
                    this.$current = new WaitForSeconds(0.7f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.<bodystring1>__0 = LocalModelManager.Instance.Character_Char.GetBeanById(0x3e9).ModelID;
                    this.<bodystring2>__0 = this.$this.GetBodyString(this.$this.clothid.ToString());
                    this.$locvar0.bodystring = string.Empty;
                    if (this.$this.clothid <= 0)
                    {
                        this.$locvar0.bodystring = this.$this.GetBodyString(this.<bodystring1>__0);
                        break;
                    }
                    this.$locvar0.bodystring = this.<bodystring2>__0;
                    break;

                default:
                    goto Label_011E;
            }
            LoadSyncCtrl.Load<GameObject>(this.$locvar0.bodystring, new Action<GameObject>(this.$locvar0.<>m__0));
            this.$PC = -1;
        Label_011E:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;

        private sealed class <init_cloth>c__AnonStorey2
        {
            internal string bodystring;
            internal CharUIHeroCtrl.<init_cloth>c__Iterator0 <>f__ref$0;

            internal void <>m__0(GameObject o2)
            {
                if (o2 == null)
                {
                    object[] args = new object[] { this.bodystring };
                    SdkManager.Bugly_Report("CharUIHeroCtrl", Utils.FormatString("bodystring:{0} is create failed!", args));
                }
                GameObject child = Object.Instantiate<GameObject>(o2);
                child.SetParentNormal(this.<>f__ref$0.$this.child);
                child.ChangeChildLayer(LayerManager.UI);
                if (this.<>f__ref$0.$this.m_Body != null)
                {
                    Object.Destroy(this.<>f__ref$0.$this.m_Body.gameObject);
                    this.<>f__ref$0.$this.m_Body = null;
                }
                this.<>f__ref$0.$this.m_Body = child.GetComponent<BodyMask>();
                int weaponid = this.<>f__ref$0.$this.weaponid;
                this.<>f__ref$0.$this.weaponid = -1;
                this.<>f__ref$0.$this.InitWeapon(weaponid);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <init_pet>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int index;
        internal Skill_skill <skill>__0;
        internal string <bodystring>__0;
        internal CharUIHeroCtrl $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        private <init_pet>c__AnonStorey3 $locvar0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$locvar0 = new <init_pet>c__AnonStorey3();
                    this.$locvar0.<>f__ref$1 = this;
                    this.$locvar0.index = this.index;
                    this.$current = new WaitForSeconds(0.9f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.<skill>__0 = LocalModelManager.Instance.Skill_skill.GetBeanById(this.$this.petsid[this.$locvar0.index]);
                    if (this.<skill>__0 != null)
                    {
                        this.$locvar0.chardata = LocalModelManager.Instance.Character_Char.GetBeanById(int.Parse(this.<skill>__0.Args[0]));
                        Debugger.Log("chardata pet " + this.$locvar0.chardata.CharID);
                        this.<bodystring>__0 = this.$this.GetBodyString(this.$locvar0.chardata.ModelID);
                        LoadSyncCtrl.Load<GameObject>(this.<bodystring>__0, new Action<GameObject>(this.$locvar0.<>m__0));
                        this.$PC = -1;
                        break;
                    }
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;

        private sealed class <init_pet>c__AnonStorey3
        {
            internal int index;
            internal Character_Char chardata;
            internal CharUIHeroCtrl.<init_pet>c__Iterator1 <>f__ref$1;

            internal void <>m__0(GameObject o2)
            {
                GameObject child = Object.Instantiate<GameObject>(o2);
                child.SetParentNormal(this.<>f__ref$1.$this.petsparent[this.index]);
                child.ChangeChildLayer(LayerManager.UI);
                child.transform.localScale = Vector3.one * this.chardata.BodyScale;
                child.GetComponent<BodyMask>().SetTextureWithoutInit(this.chardata.TextureID);
                this.<>f__ref$1.$this.pets[this.index] = child;
            }
        }
    }
}

