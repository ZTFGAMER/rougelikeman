using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static SoundManager <Instance>k__BackingField;
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource soundSource;
    [SerializeField]
    private AudioSource stepsLoopSource;
    [HideInInspector]
    public bool musicIsMuted;
    [HideInInspector]
    public bool soundIsMuted;
    [HideInInspector]
    public float musicVolume;
    [HideInInspector]
    public float soundVolume;
    public List<MusicInfo> menuMusic;
    public List<MusicInfo> inGameMusic;
    [SerializeField]
    private AudioClip healSound;
    [SerializeField]
    private AudioClip buffSound;
    [Range(0f, 10f), SerializeField]
    private float buffPlayDelay;
    public AudioClip clickSound;
    public AudioClip upgradeSound;
    public AudioClip claimSound;
    public AudioClip gameOverSound;
    public AudioClip newHeroOpened;
    public SusrvivorTakeDamage survivorsTakeDamage;
    public Coroutine musicCor;
    private bool healSoundPlaying;
    private bool buffSoundPlayed;

    private void Awake()
    {
        if ((Instance != null) && (Instance != this))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private float GetMaxVolume(AudioClip clip)
    {
        List<MusicInfo> menuMusic = this.menuMusic;
        menuMusic.AddRange(this.inGameMusic);
        foreach (MusicInfo info in menuMusic)
        {
            if (info.clip == clip)
            {
                return info.maxVolume;
            }
        }
        return 0f;
    }

    public void GetSavedInfo()
    {
        if (!PlayerPrefs.HasKey(StaticConstants.MusicMuted))
        {
            PlayerPrefs.SetInt(StaticConstants.MusicMuted, 0);
            this.musicIsMuted = false;
            PlayerPrefs.SetInt(StaticConstants.SoundMuted, 0);
            this.soundIsMuted = false;
            PlayerPrefs.SetFloat(StaticConstants.MusicVolume, 1f);
            this.musicVolume = 1f;
            this.musicSource.volume = 1f;
            PlayerPrefs.SetFloat(StaticConstants.SoundVolume, 1f);
            this.soundVolume = 1f;
            this.soundSource.volume = 1f;
        }
        else
        {
            this.musicIsMuted = PlayerPrefs.GetInt(StaticConstants.MusicMuted) == 1;
            this.soundIsMuted = PlayerPrefs.GetInt(StaticConstants.SoundMuted) == 1;
            this.musicVolume = PlayerPrefs.GetFloat(StaticConstants.MusicVolume);
            this.soundVolume = PlayerPrefs.GetFloat(StaticConstants.SoundVolume);
        }
        DataLoader.gui.SetMusic(this.musicIsMuted);
        DataLoader.gui.SetSound(this.soundIsMuted);
        this.soundSource.mute = this.soundIsMuted;
        this.musicSource.mute = this.musicIsMuted;
        this.stepsLoopSource.mute = this.soundIsMuted;
        this.PlayRandomMusic();
    }

    public void MuteAll()
    {
        this.musicSource.mute = true;
        this.soundSource.mute = true;
        this.soundIsMuted = true;
        this.stepsLoopSource.mute = true;
    }

    public void MuteMusic()
    {
        this.musicIsMuted = !this.musicIsMuted;
        PlayerPrefs.SetInt(StaticConstants.MusicMuted, !this.musicIsMuted ? 0 : 1);
        PlayerPrefs.Save();
        DataLoader.gui.SetMusic(this.musicIsMuted);
        this.musicSource.mute = this.musicIsMuted;
    }

    public void MuteSound()
    {
        this.soundIsMuted = !this.soundIsMuted;
        PlayerPrefs.SetInt(StaticConstants.SoundMuted, !this.soundIsMuted ? 0 : 1);
        PlayerPrefs.Save();
        DataLoader.gui.SetSound(this.soundIsMuted);
        this.soundSource.mute = this.soundIsMuted;
        this.stepsLoopSource.mute = this.soundIsMuted;
    }

    public void PlayBuffSound()
    {
        if (!this.buffSoundPlayed)
        {
            this.PlaySound(this.buffSound, -1f);
            this.buffSoundPlayed = true;
            base.Invoke("SetBuffSoundPlayed", this.buffPlayDelay);
        }
    }

    public void PlayClickSound()
    {
        this.PlaySound(this.clickSound, -1f);
    }

    public void PlayHealSound()
    {
        if (!this.healSoundPlaying)
        {
            this.PlaySound(this.healSound, -1f);
            this.healSoundPlaying = true;
            base.Invoke("SetHealSoundPlayed", this.healSound.length);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            if (this.musicCor != null)
            {
                base.StopCoroutine(this.musicCor);
            }
            this.musicCor = base.StartCoroutine(this.SmoothMusic(clip));
        }
    }

    public void PlayRandomMusic()
    {
        if (this.menuMusic.Count > 0)
        {
            this.PlayMusic(this.menuMusic[UnityEngine.Random.Range(0, this.menuMusic.Count)].clip);
        }
    }

    public void PlaySound(AudioClip clip, float volume = -1f)
    {
        if (((clip != null) && !this.soundIsMuted) && !this.soundSource.mute)
        {
            this.soundSource.PlayOneShot(clip, (volume != -1f) ? volume : this.soundVolume);
        }
    }

    public void PlayStepsSound(bool play)
    {
        if (play)
        {
            if (!this.stepsLoopSource.isPlaying)
            {
                this.stepsLoopSource.Play();
            }
        }
        else
        {
            this.stepsLoopSource.Stop();
        }
    }

    public void PlaySurvivorTakeDamage(SaveData.HeroData.HeroType type)
    {
        if ((type == SaveData.HeroData.HeroType.MEDIC) || (type == SaveData.HeroData.HeroType.COOK))
        {
            this.PlaySound(this.survivorsTakeDamage.femaleSounds[UnityEngine.Random.Range(0, this.survivorsTakeDamage.femaleSounds.Length)], -1f);
        }
        else
        {
            this.PlaySound(this.survivorsTakeDamage.maleSounds[UnityEngine.Random.Range(0, this.survivorsTakeDamage.maleSounds.Length)], -1f);
        }
    }

    public void SetBuffSoundPlayed()
    {
        this.buffSoundPlayed = false;
    }

    public void SetHealSoundPlayed()
    {
        this.healSoundPlaying = false;
    }

    [DebuggerHidden]
    private IEnumerator SmoothMusic(AudioClip clip) => 
        new <SmoothMusic>c__Iterator0 { 
            clip = clip,
            $this = this
        };

    public void UnMuteAll()
    {
        this.musicSource.mute = this.musicIsMuted;
        this.soundIsMuted = PlayerPrefs.GetInt(StaticConstants.SoundMuted) == 1;
        this.soundSource.mute = this.soundIsMuted;
        this.stepsLoopSource.mute = this.soundIsMuted;
    }

    public static SoundManager Instance
    {
        [CompilerGenerated]
        get => 
            <Instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<Instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <SmoothMusic>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <repeatCount>__0;
        internal AudioClip clip;
        internal float <t>__1;
        internal float <currentMaxVolume>__1;
        internal string <lastClipName>__2;
        internal int <rand>__2;
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
                    this.<repeatCount>__0 = 0;
                    break;

                case 1:
                    goto Label_00CF;

                case 2:
                case 3:
                    while (this.$this.musicSource.volume > 0f)
                    {
                        this.$this.musicSource.volume -= 0.02f;
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                        goto Label_02B6;
                    }
                    if (this.$this.menuMusic.Count > 1)
                    {
                        this.<lastClipName>__2 = this.clip.name;
                        this.<repeatCount>__0++;
                        this.<rand>__2 = UnityEngine.Random.Range(0, this.$this.menuMusic.Count);
                        this.clip = this.$this.menuMusic[this.<rand>__2].clip;
                        if (this.<lastClipName>__2 != this.clip.name)
                        {
                            this.<lastClipName>__2 = this.clip.name;
                            this.<repeatCount>__0 = 0;
                        }
                        else if (this.<repeatCount>__0 >= 3)
                        {
                            int num2 = (UnityEngine.Random.Range(0, 2) * 2) - 1;
                            if (((this.<rand>__2 + num2) >= 0) && ((this.<rand>__2 + num2) < this.$this.menuMusic.Count))
                            {
                                this.clip = this.$this.menuMusic[this.<rand>__2 + num2].clip;
                            }
                            else
                            {
                                this.clip = this.$this.menuMusic[this.<rand>__2 - num2].clip;
                            }
                        }
                    }
                    break;

                default:
                    return false;
            }
            this.<t>__1 = this.clip.length;
            this.$this.musicSource.volume = 0f;
            this.$this.musicSource.clip = this.clip;
            this.<currentMaxVolume>__1 = this.$this.GetMaxVolume(this.clip);
            this.$this.musicSource.Play();
        Label_00CF:
            while (this.$this.musicSource.volume < this.<currentMaxVolume>__1)
            {
                this.$this.musicSource.volume += 0.02f;
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_02B6;
            }
            this.$current = new WaitForSeconds(this.clip.length - 2.5f);
            if (!this.$disposing)
            {
                this.$PC = 2;
            }
        Label_02B6:
            return true;
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
}

