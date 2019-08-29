using Dxx.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private Dictionary<string, AudioClip> _soundDictionary;
    private Dictionary<string, Queue<SoundData>> _soundObjDic;
    private AudioSource audioSourceEffect;
    private const string SoundPath = "Sound/";
    private AnimationCurve animationCurve;
    private bool bSound;
    private int walk_walk;
    private string walk_path;
    private float walk_Time;
    private bool walk_Start;
    private bool bMusic;
    private static Dictionary<BackgroundMusicType, BGMusicData> mBGList;

    static SoundManager()
    {
        Dictionary<BackgroundMusicType, BGMusicData> dictionary = new Dictionary<BackgroundMusicType, BGMusicData>();
        BGMusicData data = new BGMusicData {
            path = "uibg",
            volume = 1f
        };
        dictionary.Add(BackgroundMusicType.eMain, data);
        data = new BGMusicData {
            path = "battlebg",
            volume = 0.5f
        };
        dictionary.Add(BackgroundMusicType.eBattle, data);
        mBGList = dictionary;
    }

    private void Awake()
    {
        GameLogic.Hold.SetSound(this);
        this.InitSound();
        this.InitMusic();
        this._soundDictionary = new Dictionary<string, AudioClip>();
        this._soundObjDic = new Dictionary<string, Queue<SoundData>>();
        this.audioSourceEffect = base.GetComponent<AudioSource>();
        Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 1f, 0f, -0.5f), new Keyframe(1.7f, 0.5f, -0.2f, -0.15f), new Keyframe(10f, 0f, 0f, 0f) };
        this.animationCurve = new AnimationCurve(keys);
    }

    public bool ChangeMusic()
    {
        this.SetMusic(!this.bMusic);
        return this.bMusic;
    }

    public bool ChangeSound()
    {
        this.SetSound(!this.bSound);
        return this.bSound;
    }

    public void DeInit()
    {
        this._soundDictionary.Clear();
        this._soundObjDic.Clear();
        GameNode.SoundNode.DestroyChildren();
    }

    private void DespawnSoundCallback(string sound, SoundData data)
    {
        if (data != null)
        {
            Queue<SoundData> queue = null;
            if (!this._soundObjDic.TryGetValue(sound, out queue))
            {
                queue = new Queue<SoundData>();
                this._soundObjDic.Add(sound, queue);
            }
            if (data.obj != null)
            {
                queue.Enqueue(data);
                data.obj.SetActive(false);
            }
        }
    }

    public bool GetMusic() => 
        this.bMusic;

    private bool GetMusicLocal() => 
        PlayerPrefsEncrypt.GetBool("Music", true);

    public bool GetSound() => 
        this.bSound;

    private bool GetSoundLocal() => 
        PlayerPrefsEncrypt.GetBool("Sound", true);

    private void InitMusic()
    {
        this.bMusic = this.GetMusicLocal();
        this.UpdateMusicVolume();
    }

    private void InitSound()
    {
        this.bSound = this.GetSoundLocal();
    }

    public void PauseBackgroundMusic()
    {
        GameNode.BackgroundMusic.Pause();
    }

    private GameObject PlayAtPoint(string audioEffectName, Vector3 pos, float volume = 1f)
    {
        if (this.bSound)
        {
            audioEffectName = "Sound/" + audioEffectName;
            if (this._soundDictionary.ContainsKey(audioEffectName))
            {
                return this.PlayAudio(audioEffectName, pos, volume);
            }
            AudioClip clip = ResourceManager.Load<AudioClip>(audioEffectName);
            if (clip != null)
            {
                this._soundDictionary.Add(audioEffectName, clip);
                return this.PlayAudio(audioEffectName, pos, volume);
            }
        }
        return null;
    }

    private GameObject PlayAudio(string sound, Vector3 pos, float volume)
    {
        Queue<SoundData> queue = null;
        SoundData data = null;
        if (this._soundObjDic.TryGetValue(sound, out queue))
        {
            while (queue.Count > 0)
            {
                data = queue.Dequeue();
                if (data.Valid)
                {
                    break;
                }
            }
        }
        if (data == null)
        {
            if (queue == null)
            {
                queue = new Queue<SoundData>();
                this._soundObjDic.Add(sound, queue);
            }
            data = new SoundData {
                obj = GameLogic.HoldGet("Sound/AudioSource")
            };
            data.obj.name = sound;
            AutoDespawnSound component = data.obj.GetComponent<AutoDespawnSound>();
            component.callback = new Action<string, SoundData>(this.DespawnSoundCallback);
            component.sounddata = data;
            component.SetDespawnTime(this._soundDictionary[sound].length);
            data.audio = data.obj.GetComponent<AudioSource>();
        }
        data.obj.SetActive(true);
        data.audio.transform.position = pos;
        data.audio.clip = this._soundDictionary[sound];
        data.audio.volume = volume;
        data.audio.Play();
        return data.obj;
    }

    public GameObject PlayAudioAttack(string path, Vector3 pos, float volumn = 1f)
    {
        if (!this.bSound)
        {
            return null;
        }
        object[] args = new object[] { "attack/", path };
        path = Utils.GetString(args);
        return this.PlayAtPoint(path, pos, volumn);
    }

    public void PlayAudioSource(string audioEffectName, float volumn = 1f)
    {
        if (this.bSound)
        {
            object[] args = new object[] { "Sound/", audioEffectName };
            audioEffectName = Utils.GetString(args);
            if (this._soundDictionary.ContainsKey(audioEffectName))
            {
                this.audioSourceEffect.clip = this._soundDictionary[audioEffectName];
                this.audioSourceEffect.volume = volumn;
                this.audioSourceEffect.Play();
            }
            else
            {
                AudioClip clip = ResourceManager.Load<AudioClip>(audioEffectName);
                if (clip != null)
                {
                    this._soundDictionary.Add(audioEffectName, clip);
                    this.audioSourceEffect.clip = this._soundDictionary[audioEffectName];
                    this.audioSourceEffect.volume = volumn;
                    this.audioSourceEffect.Play();
                }
            }
        }
    }

    public void PlayBackgroundMusic(BackgroundMusicType type)
    {
        string path = mBGList[type].path;
        if (this._soundDictionary.TryGetValue(path, out AudioClip clip))
        {
            GameNode.BackgroundMusic.clip = clip;
            GameNode.BackgroundMusic.Play();
        }
        else
        {
            object[] args = new object[] { path };
            clip = ResourceManager.Load<AudioClip>(Utils.FormatString("Sound/BG/{0}", args));
            if (clip != null)
            {
                this._soundDictionary.Add(path, clip);
                GameNode.BackgroundMusic.clip = clip;
                GameNode.BackgroundMusic.volume = !this.bMusic ? 0f : mBGList[type].volume;
                GameNode.BackgroundMusic.Play();
            }
            else
            {
                GameNode.BackgroundMusic.Stop();
            }
        }
    }

    public GameObject PlayBattleSpecial(int id, Vector3 pos)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        return this.PlayAtPoint(beanById.Path, pos, beanById.Volumn);
    }

    public GameObject PlayBodyHit(int id, Vector3 pos)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        object[] args = new object[] { beanById.Path, id };
        return this.PlayAtPoint(Utils.GetString(args), pos, beanById.Volumn);
    }

    public GameObject PlayBulletCreate(int id, Vector3 pos)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        object[] args = new object[] { beanById.Path, id };
        return this.PlayAtPoint(Utils.GetString(args), pos, beanById.Volumn);
    }

    public GameObject PlayBulletDead(int id, Vector3 pos)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        object[] args = new object[] { beanById.Path, id };
        return this.PlayAtPoint(Utils.GetString(args), pos, beanById.Volumn);
    }

    public GameObject PlayBulletHitWall(int id, Vector3 pos)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        object[] args = new object[] { beanById.Path, id };
        return this.PlayAtPoint(Utils.GetString(args), pos, beanById.Volumn);
    }

    public GameObject PlayEntityDead(int id, Vector3 pos)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        object[] args = new object[] { beanById.Path, id };
        return this.PlayAtPoint(Utils.GetString(args), pos, beanById.Volumn);
    }

    public GameObject PlayGetGoods(int id, Vector3 pos)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        object[] args = new object[] { beanById.Path, id };
        return this.PlayAtPoint(Utils.GetString(args), pos, beanById.Volumn);
    }

    public GameObject PlayHitted(int id, Vector3 pos, float volumn = -1f)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        if (volumn < 0f)
        {
            volumn = beanById.Volumn;
        }
        return this.PlayAtPoint(beanById.Path, pos, volumn);
    }

    public GameObject PlayMonsterSkill(int id, Vector3 pos)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        return this.PlayAtPoint(beanById.Path, pos, beanById.Volumn);
    }

    public void PlayUI(SoundUIType type)
    {
        this.PlayUI((int) type);
    }

    public void PlayUI(int id)
    {
        Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
        this.PlayAtPoint(beanById.Path, Vector3.zero, beanById.Volumn);
    }

    public void PlayWalk()
    {
        this.walk_Start = true;
    }

    public void PreloadSound(string path)
    {
    }

    public void ResumeBackgroundMusic()
    {
        GameNode.BackgroundMusic.UnPause();
    }

    public void SetBackgroundVolume(float volume)
    {
        GameNode.BackgroundMusic.volume = volume;
    }

    public void SetMusic(bool music)
    {
        PlayerPrefsEncrypt.SetBool("Music", music);
        this.bMusic = music;
        this.UpdateMusicVolume();
    }

    public void SetSound(bool sound)
    {
        PlayerPrefsEncrypt.SetBool("Sound", sound);
        this.bSound = sound;
    }

    [DebuggerHidden]
    private IEnumerator Start() => 
        new <Start>c__Iterator0 { $this = this };

    public void StopBackgroundMusic()
    {
        GameNode.BackgroundMusic.Stop();
    }

    public void StopWalk()
    {
        this.walk_Start = false;
    }

    private void Update()
    {
        if (this.bSound)
        {
            this.WalkUpdate();
        }
    }

    private void UpdateMusicVolume()
    {
        GameNode.BackgroundMusic.volume = !this.bMusic ? 0f : 1f;
    }

    private void WalkUpdate()
    {
        if (this.walk_Start && ((Updater.AliveTime - this.walk_Time) > 0.3f))
        {
            this.walk_walk = GameLogic.Random(1, 8);
            object[] args = new object[] { "walk/footstep0", this.walk_walk };
            this.walk_path = Utils.GetString(args);
            this.PlayAudioSource(this.walk_path, 0.5f);
            this.walk_Time = Updater.AliveTime;
        }
    }

    [CompilerGenerated]
    private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal SoundManager $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

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
                    this.$current = new WaitForSecondsRealtime(0.5f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.PlayAudioSource(this.$this.walk_path, 0f);
                    this.$PC = -1;
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
    }

    public enum BackgroundMusicType
    {
        eMain,
        eBattle
    }

    private class BGMusicData
    {
        public string path;
        public float volume;
    }

    public class SoundData
    {
        public GameObject obj;
        public AudioSource audio;

        public bool Valid =>
            ((this.obj != null) && (this.audio != null));
    }
}

