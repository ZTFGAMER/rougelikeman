namespace DG.Tweening
{
    using DG.Tweening.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("DOTween/DOTween Animation")]
    public class DOTweenAnimation : ABSAnimationComponent
    {
        public float delay;
        public float duration = 1f;
        public Ease easeType = 6;
        public AnimationCurve easeCurve;
        public LoopType loopType;
        public int loops;
        public string id;
        public bool isRelative;
        public bool isFrom;
        public bool isIndependentUpdate;
        public bool autoKill;
        public bool isActive;
        public bool isValid;
        public Component target;
        public DOTweenAnimationType animationType;
        public TargetType targetType;
        public TargetType forcedTargetType;
        public bool autoPlay;
        public bool useTargetAsV3;
        public float endValueFloat;
        public Vector3 endValueV3;
        public Vector2 endValueV2;
        public Color endValueColor;
        public string endValueString;
        public Rect endValueRect;
        public Transform endValueTransform;
        public bool optionalBool0;
        public float optionalFloat0;
        public int optionalInt0;
        public RotateMode optionalRotationMode;
        public ScrambleMode optionalScrambleMode;
        public string optionalString;
        private bool _tweenCreated;
        private int _playCount;

        public DOTweenAnimation()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.easeCurve = new AnimationCurve(keys);
            this.loops = 1;
            this.id = string.Empty;
            this.autoKill = true;
            this.isActive = true;
            this.autoPlay = true;
            this.endValueColor = new Color(1f, 1f, 1f, 1f);
            this.endValueString = string.Empty;
            this.endValueRect = new Rect(0f, 0f, 0f, 0f);
            this._playCount = -1;
        }

        private void Awake()
        {
            if ((this.isActive && this.isValid) && ((this.animationType != 1) || !this.useTargetAsV3))
            {
                this.CreateTween();
                this._tweenCreated = true;
            }
        }

        public void CreateTween()
        {
            if (this.target == null)
            {
                Debug.LogWarning($"{base.gameObject.name} :: This tween's target is NULL, because the animation was created with a DOTween Pro version older than 0.9.255. To fix this, exit Play mode then simply select this object, and it will update automatically", base.gameObject);
                return;
            }
            if (this.forcedTargetType != null)
            {
                this.targetType = this.forcedTargetType;
            }
            if (this.targetType == null)
            {
                this.targetType = TypeToDOTargetType(this.target.GetType());
            }
            switch (this.animationType)
            {
                case 1:
                    if (this.useTargetAsV3)
                    {
                        this.isRelative = false;
                        if (this.endValueTransform == null)
                        {
                            Debug.LogWarning($"{base.gameObject.name} :: This tween's TO target is NULL, a Vector3 of (0,0,0) will be used instead", base.gameObject);
                            this.endValueV3 = Vector3.zero;
                        }
                        else
                        {
                            if (this.targetType != 5)
                            {
                                this.endValueV3 = this.endValueTransform.position;
                                break;
                            }
                            RectTransform endValueTransform = this.endValueTransform as RectTransform;
                            if (endValueTransform == null)
                            {
                                Debug.LogWarning($"{base.gameObject.name} :: This tween's TO target should be a RectTransform, a Vector3 of (0,0,0) will be used instead", base.gameObject);
                                this.endValueV3 = Vector3.zero;
                            }
                            else
                            {
                                RectTransform target = this.target as RectTransform;
                                if (target == null)
                                {
                                    Debug.LogWarning($"{base.gameObject.name} :: This tween's target and TO target are not of the same type. Please reassign the values", base.gameObject);
                                }
                                else
                                {
                                    this.endValueV3 = (Vector3) DOTweenUtils46.SwitchToRectTransform(endValueTransform, target);
                                }
                            }
                        }
                    }
                    break;

                case 2:
                    base.tween = ShortcutExtensions.DOLocalMove(base.transform, this.endValueV3, this.duration, this.optionalBool0);
                    goto Label_09D4;

                case 3:
                {
                    TargetType targetType = this.targetType;
                    if (targetType == 11)
                    {
                        base.tween = ShortcutExtensions.DORotate((Transform) this.target, this.endValueV3, this.duration, this.optionalRotationMode);
                    }
                    else if (targetType == 9)
                    {
                        base.tween = ShortcutExtensions43.DORotate((Rigidbody2D) this.target, this.endValueFloat, this.duration);
                    }
                    else if (targetType == 8)
                    {
                        base.tween = ShortcutExtensions.DORotate((Rigidbody) this.target, this.endValueV3, this.duration, this.optionalRotationMode);
                    }
                    goto Label_09D4;
                }
                case 4:
                    base.tween = ShortcutExtensions.DOLocalRotate(base.transform, this.endValueV3, this.duration, this.optionalRotationMode);
                    goto Label_09D4;

                case 5:
                    base.tween = ShortcutExtensions.DOScale(base.transform, !this.optionalBool0 ? this.endValueV3 : new Vector3(this.endValueFloat, this.endValueFloat, this.endValueFloat), this.duration);
                    goto Label_09D4;

                case 6:
                    this.isRelative = false;
                    switch (this.targetType)
                    {
                        case 3:
                            base.tween = ShortcutExtensions46.DOColor((Image) this.target, this.endValueColor, this.duration);
                            break;

                        case 4:
                            base.tween = ShortcutExtensions.DOColor((Light) this.target, this.endValueColor, this.duration);
                            break;

                        case 6:
                            base.tween = ShortcutExtensions.DOColor(((Renderer) this.target).material, this.endValueColor, this.duration);
                            break;

                        case 7:
                            base.tween = ShortcutExtensions43.DOColor((SpriteRenderer) this.target, this.endValueColor, this.duration);
                            break;

                        case 10:
                            base.tween = ShortcutExtensions46.DOColor((Text) this.target, this.endValueColor, this.duration);
                            break;
                    }
                    goto Label_09D4;

                case 7:
                    this.isRelative = false;
                    switch (this.targetType)
                    {
                        case 2:
                            base.tween = ShortcutExtensions46.DOFade((CanvasGroup) this.target, this.endValueFloat, this.duration);
                            break;

                        case 3:
                            base.tween = ShortcutExtensions46.DOFade((Image) this.target, this.endValueFloat, this.duration);
                            break;

                        case 4:
                            base.tween = ShortcutExtensions.DOIntensity((Light) this.target, this.endValueFloat, this.duration);
                            break;

                        case 6:
                            base.tween = ShortcutExtensions.DOFade(((Renderer) this.target).material, this.endValueFloat, this.duration);
                            break;

                        case 7:
                            base.tween = ShortcutExtensions43.DOFade((SpriteRenderer) this.target, this.endValueFloat, this.duration);
                            break;

                        case 10:
                            base.tween = ShortcutExtensions46.DOFade((Text) this.target, this.endValueFloat, this.duration);
                            break;
                    }
                    goto Label_09D4;

                case 8:
                    if (this.targetType == 10)
                    {
                        base.tween = ShortcutExtensions46.DOText((Text) this.target, this.endValueString, this.duration, this.optionalBool0, this.optionalScrambleMode, this.optionalString);
                    }
                    goto Label_09D4;

                case 9:
                {
                    TargetType targetType = this.targetType;
                    if (targetType == 5)
                    {
                        base.tween = ShortcutExtensions46.DOPunchAnchorPos((RectTransform) this.target, this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
                    }
                    else if (targetType == 11)
                    {
                        base.tween = ShortcutExtensions.DOPunchPosition((Transform) this.target, this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
                    }
                    goto Label_09D4;
                }
                case 10:
                    base.tween = ShortcutExtensions.DOPunchRotation(base.transform, this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
                    goto Label_09D4;

                case 11:
                    base.tween = ShortcutExtensions.DOPunchScale(base.transform, this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
                    goto Label_09D4;

                case 12:
                {
                    TargetType targetType = this.targetType;
                    if (targetType == 5)
                    {
                        base.tween = ShortcutExtensions46.DOShakeAnchorPos((RectTransform) this.target, this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool0, true);
                    }
                    else if (targetType == 11)
                    {
                        base.tween = ShortcutExtensions.DOShakePosition((Transform) this.target, this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool0, true);
                    }
                    goto Label_09D4;
                }
                case 13:
                    base.tween = ShortcutExtensions.DOShakeRotation(base.transform, this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, true);
                    goto Label_09D4;

                case 14:
                    base.tween = ShortcutExtensions.DOShakeScale(base.transform, this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, true);
                    goto Label_09D4;

                case 15:
                    base.tween = ShortcutExtensions.DOAspect((Camera) this.target, this.endValueFloat, this.duration);
                    goto Label_09D4;

                case 0x10:
                    base.tween = ShortcutExtensions.DOColor((Camera) this.target, this.endValueColor, this.duration);
                    goto Label_09D4;

                case 0x11:
                    base.tween = ShortcutExtensions.DOFieldOfView((Camera) this.target, this.endValueFloat, this.duration);
                    goto Label_09D4;

                case 0x12:
                    base.tween = ShortcutExtensions.DOOrthoSize((Camera) this.target, this.endValueFloat, this.duration);
                    goto Label_09D4;

                case 0x13:
                    base.tween = ShortcutExtensions.DOPixelRect((Camera) this.target, this.endValueRect, this.duration);
                    goto Label_09D4;

                case 20:
                    base.tween = ShortcutExtensions.DORect((Camera) this.target, this.endValueRect, this.duration);
                    goto Label_09D4;

                case 0x15:
                    base.tween = ShortcutExtensions46.DOSizeDelta((RectTransform) this.target, !this.optionalBool0 ? this.endValueV2 : new Vector2(this.endValueFloat, this.endValueFloat), this.duration, false);
                    goto Label_09D4;

                default:
                    goto Label_09D4;
            }
            switch (this.targetType)
            {
                case 5:
                    base.tween = ShortcutExtensions46.DOAnchorPos3D((RectTransform) this.target, this.endValueV3, this.duration, this.optionalBool0);
                    break;

                case 8:
                    base.tween = ShortcutExtensions.DOMove((Rigidbody) this.target, this.endValueV3, this.duration, this.optionalBool0);
                    break;

                case 9:
                    base.tween = ShortcutExtensions43.DOMove((Rigidbody2D) this.target, this.endValueV3, this.duration, this.optionalBool0);
                    break;

                case 11:
                    base.tween = ShortcutExtensions.DOMove((Transform) this.target, this.endValueV3, this.duration, this.optionalBool0);
                    break;
            }
        Label_09D4:
            if (base.tween == null)
            {
                return;
            }
            if (this.isFrom)
            {
                TweenSettingsExtensions.From<Tweener>((Tweener) base.tween, this.isRelative);
            }
            else
            {
                TweenSettingsExtensions.SetRelative<Tween>(base.tween, this.isRelative);
            }
            TweenSettingsExtensions.OnKill<Tween>(TweenSettingsExtensions.SetAutoKill<Tween>(TweenSettingsExtensions.SetLoops<Tween>(TweenSettingsExtensions.SetDelay<Tween>(TweenSettingsExtensions.SetTarget<Tween>(base.tween, base.gameObject), this.delay), this.loops, this.loopType), this.autoKill), new TweenCallback(this, this.<CreateTween>m__0));
            if (base.isSpeedBased)
            {
                TweenSettingsExtensions.SetSpeedBased<Tween>(base.tween);
            }
            if (this.easeType == 0x25)
            {
                TweenSettingsExtensions.SetEase<Tween>(base.tween, this.easeCurve);
            }
            else
            {
                TweenSettingsExtensions.SetEase<Tween>(base.tween, this.easeType);
            }
            if (!string.IsNullOrEmpty(this.id))
            {
                TweenSettingsExtensions.SetId<Tween>(base.tween, this.id);
            }
            TweenSettingsExtensions.SetUpdate<Tween>(base.tween, this.isIndependentUpdate);
            if (base.hasOnStart)
            {
                if (base.onStart != null)
                {
                    TweenSettingsExtensions.OnStart<Tween>(base.tween, new TweenCallback(base.onStart, this.Invoke));
                }
            }
            else
            {
                base.onStart = null;
            }
            if (base.hasOnPlay)
            {
                if (base.onPlay != null)
                {
                    TweenSettingsExtensions.OnPlay<Tween>(base.tween, new TweenCallback(base.onPlay, this.Invoke));
                }
            }
            else
            {
                base.onPlay = null;
            }
            if (base.hasOnUpdate)
            {
                if (base.onUpdate != null)
                {
                    TweenSettingsExtensions.OnUpdate<Tween>(base.tween, new TweenCallback(base.onUpdate, this.Invoke));
                }
            }
            else
            {
                base.onUpdate = null;
            }
            if (base.hasOnStepComplete)
            {
                if (base.onStepComplete != null)
                {
                    TweenSettingsExtensions.OnStepComplete<Tween>(base.tween, new TweenCallback(base.onStepComplete, this.Invoke));
                }
            }
            else
            {
                base.onStepComplete = null;
            }
            if (base.hasOnComplete)
            {
                if (base.onComplete != null)
                {
                    TweenSettingsExtensions.OnComplete<Tween>(base.tween, new TweenCallback(base.onComplete, this.Invoke));
                }
            }
            else
            {
                base.onComplete = null;
            }
            if (base.hasOnRewind)
            {
                if (base.onRewind != null)
                {
                    TweenSettingsExtensions.OnRewind<Tween>(base.tween, new TweenCallback(base.onRewind, this.Invoke));
                }
            }
            else
            {
                base.onRewind = null;
            }
            if (this.autoPlay)
            {
                TweenExtensions.Play<Tween>(base.tween);
            }
            else
            {
                TweenExtensions.Pause<Tween>(base.tween);
            }
            if (base.hasOnTweenCreated && (base.onTweenCreated != null))
            {
                base.onTweenCreated.Invoke();
            }
        }

        public override void DOComplete()
        {
            DOTween.Complete(base.gameObject, false);
        }

        public override void DOKill()
        {
            DOTween.Kill(base.gameObject, false);
            base.tween = null;
        }

        public override void DOPause()
        {
            DOTween.Pause(base.gameObject);
        }

        public void DOPauseAllById(string id)
        {
            DOTween.Pause(id);
        }

        public override void DOPlay()
        {
            DOTween.Play(base.gameObject);
        }

        public void DOPlayAllById(string id)
        {
            DOTween.Play(id);
        }

        public override void DOPlayBackwards()
        {
            DOTween.PlayBackwards(base.gameObject);
        }

        public void DOPlayBackwardsAllById(string id)
        {
            DOTween.PlayBackwards(id);
        }

        public void DOPlayBackwardsById(string id)
        {
            DOTween.PlayBackwards(base.gameObject, id);
        }

        public void DOPlayById(string id)
        {
            DOTween.Play(base.gameObject, id);
        }

        public override void DOPlayForward()
        {
            DOTween.PlayForward(base.gameObject);
        }

        public void DOPlayForwardAllById(string id)
        {
            DOTween.PlayForward(id);
        }

        public void DOPlayForwardById(string id)
        {
            DOTween.PlayForward(base.gameObject, id);
        }

        public void DOPlayNext()
        {
            DOTweenAnimation[] components = base.GetComponents<DOTweenAnimation>();
            while (this._playCount < (components.Length - 1))
            {
                this._playCount++;
                DOTweenAnimation animation = components[this._playCount];
                if (((animation != null) && (animation.tween != null)) && (!TweenExtensions.IsPlaying(animation.tween) && !TweenExtensions.IsComplete(animation.tween)))
                {
                    TweenExtensions.Play<Tween>(animation.tween);
                    break;
                }
            }
        }

        public override void DORestart(bool fromHere = false)
        {
            this._playCount = -1;
            if (base.tween == null)
            {
                if (Debugger.logPriority > 1)
                {
                    Debugger.LogNullTween(base.tween);
                }
            }
            else
            {
                if (fromHere && this.isRelative)
                {
                    this.ReEvaluateRelativeTween();
                }
                DOTween.Restart(base.gameObject, true);
            }
        }

        public void DORestartAllById(string id)
        {
            this._playCount = -1;
            DOTween.Restart(id, true);
        }

        public void DORestartById(string id)
        {
            this._playCount = -1;
            DOTween.Restart(base.gameObject, id, true);
        }

        public override void DORewind()
        {
            this._playCount = -1;
            DOTweenAnimation[] components = base.gameObject.GetComponents<DOTweenAnimation>();
            for (int i = components.Length - 1; i > -1; i--)
            {
                Tween tween = components[i].tween;
                if ((tween != null) && TweenExtensions.IsInitialized(tween))
                {
                    TweenExtensions.Rewind(components[i].tween, true);
                }
            }
        }

        public void DORewindAndPlayNext()
        {
            this._playCount = -1;
            DOTween.Rewind(base.gameObject, true);
            this.DOPlayNext();
        }

        public override void DOTogglePause()
        {
            DOTween.TogglePause(base.gameObject);
        }

        public List<Tween> GetTweens()
        {
            List<Tween> list = new List<Tween>();
            foreach (DOTweenAnimation animation in base.GetComponents<DOTweenAnimation>())
            {
                list.Add(animation.tween);
            }
            return list;
        }

        private void OnDestroy()
        {
            if ((base.tween != null) && TweenExtensions.IsActive(base.tween))
            {
                TweenExtensions.Kill(base.tween, false);
            }
            base.tween = null;
        }

        private void ReEvaluateRelativeTween()
        {
            if (this.animationType == 1)
            {
                ((Tweener) base.tween).ChangeEndValue(base.transform.position + this.endValueV3, true);
            }
            else if (this.animationType == 2)
            {
                ((Tweener) base.tween).ChangeEndValue(base.transform.localPosition + this.endValueV3, true);
            }
        }

        private void Start()
        {
            if ((!this._tweenCreated && this.isActive) && this.isValid)
            {
                this.CreateTween();
                this._tweenCreated = true;
            }
        }

        public static TargetType TypeToDOTargetType(Type t)
        {
            string str = t.ToString();
            int num = str.LastIndexOf(".");
            if (num != -1)
            {
                str = str.Substring(num + 1);
            }
            if ((str.IndexOf("Renderer") != -1) && (str != "SpriteRenderer"))
            {
                str = "Renderer";
            }
            return (TargetType) Enum.Parse(typeof(TargetType), str);
        }
    }
}

