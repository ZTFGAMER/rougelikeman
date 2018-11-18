using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UGUISpriteAnimation : MonoBehaviour
{
  private Image ImageSource;
  private int mCurFrame = 0;
  private float mDelta = 0;
  public static int FRAMEBASE = 16;

  public static float FPS = 10;
  public List<Sprite> SpriteFrames;
  public bool IsPlaying = false;
  public bool Foward = true;
  public bool AutoPlay = false;
  public bool Loop = false;
  public int CurrentStartFrame = 0;
  public int CurrentEndFrame = 0;
  public string m_SpiteName;
  public AnimState m_AnimState = AnimState.Default;

  public enum AnimState
  {
    RunToward,
    RunBack,
    AttackToward,
    AttackBack,
    Dead,
    Default
  };

  public void InitFrame(string animname)
  {
    InitLoopFrame(animname, "");
    InitLoopFrame(animname, "_dead");
    InitLoopFrame(animname, "_melee");
  }

  void InitLoopFrame(string animname, string special)
  {
    for (int i = 0; i < FRAMEBASE; i++)
    {
      string _str = "_";
      int j = i;
      if (special == "_dead")
      {
        j = (i % 4) * 4 + i / 4;
      }
      if (j < 9)
        _str = "_0";
      SpriteFrames.Add(Resources.Load<Sprite>("Sprites/" + animname + special + _str + (j+1)));
    }
  }

  public void SetAnimState(AnimState animstate)
  {
    if(m_AnimState != animstate)
    { 
      switch (animstate)
      {
        case AnimState.RunBack:
          CurrentStartFrame = 0;
          CurrentEndFrame = FRAMEBASE / 4 - 1;
          break;
        case AnimState.RunToward:
          CurrentStartFrame = FRAMEBASE * 3 / 4;
          CurrentEndFrame = FRAMEBASE - 1;
          break;
        case AnimState.AttackBack:
          CurrentStartFrame = FRAMEBASE * 2;
          CurrentEndFrame = FRAMEBASE * 2 + FRAMEBASE / 4 - 1;
          break;
        case AnimState.AttackToward:
          CurrentStartFrame = FRAMEBASE * 2 + FRAMEBASE * 3 / 4;
          CurrentEndFrame = FRAMEBASE * 3 - 1;
          break;
        case AnimState.Dead:
          CurrentStartFrame = 0 + FRAMEBASE;
          CurrentEndFrame = FRAMEBASE * 2 - 1;
          break;
        default:
          CurrentStartFrame = 0 ;
          CurrentEndFrame = FRAMEBASE * 3 - 1;
          break;
      }
      m_AnimState = animstate;
    }
  }

  public int FrameCount
  {
    get
    {
      return CurrentEndFrame - CurrentStartFrame + 1;
    }
  }

  void Awake()
  {
    ImageSource = GetComponent<Image>();
  }

  void Start()
  {
    if (AutoPlay)
    {
      Play();
    }
    else
    {
      IsPlaying = false;
    }
  }

  private void SetSprite(int idx)
  {
    ImageSource.sprite = SpriteFrames[idx];
    //该部分为设置成原始图片大小，如果只需要显示Image设定好的图片大小，注释掉该行即可。
    //ImageSource.SetNativeSize();
  }

  public void Play()
  {
    IsPlaying = true;
    Foward = true;
  }

  public void PlayReverse()
  {
    IsPlaying = true;
    Foward = false;
  }

  void Update()
  {
    if (!IsPlaying || 0 == FrameCount)
    {
      return;
    }

    mDelta += Time.deltaTime;
    if (mDelta > 1 / FPS)
    {
      mDelta = 0;
      if (Foward)
      {
        mCurFrame++;
      }
      else
      {
        mCurFrame--;
      }

      if (mCurFrame >= CurrentEndFrame)
      {
        if (Loop)
        {
          mCurFrame = CurrentStartFrame;
        }
        else
        {
          IsPlaying = false;
          return;
        }
      }
      else if (mCurFrame < CurrentStartFrame)
      {
        if (Loop)
        {
          mCurFrame = CurrentEndFrame;
        }
        else
        {
          IsPlaying = false;
          return;
        }
      }

      SetSprite(mCurFrame);
    }
  }

  public void Pause()
  {
    IsPlaying = false;
  }

  public void Resume()
  {
    if (!IsPlaying)
    {
      IsPlaying = true;
    }
  }

  public void Stop()
  {
    mCurFrame = CurrentStartFrame;
    SetSprite(mCurFrame);
    IsPlaying = false;
  }

  public void Rewind()
  {
    mCurFrame = CurrentStartFrame;
    SetSprite(mCurFrame);
    Play();
  }
}