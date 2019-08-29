namespace Spine.Unity.Playables
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;
    using UnityEngine.Playables;

    public class SpineAnimationStateMixerBehaviour : PlayableBehaviour
    {
        private float[] lastInputWeights;

        public void PreviewEditModePose(Playable playable, SkeletonAnimation spineComponent)
        {
            if (!Application.isPlaying && (spineComponent != null))
            {
                int inputCount = playable.GetInputCount<Playable>();
                int inputPort = -1;
                for (int i = 0; i < inputCount; i++)
                {
                    if (playable.GetInputWeight<Playable>(i) >= 1f)
                    {
                        inputPort = i;
                    }
                }
                if (inputPort != -1)
                {
                    ScriptPlayable<SpineAnimationStateBehaviour> input = (ScriptPlayable<SpineAnimationStateBehaviour>) playable.GetInput<Playable>(inputPort);
                    SpineAnimationStateBehaviour behaviour = input.GetBehaviour();
                    Skeleton skeleton = spineComponent.Skeleton;
                    if ((behaviour.animationReference != null) && (spineComponent.SkeletonDataAsset.GetSkeletonData(true) != behaviour.animationReference.SkeletonDataAsset.GetSkeletonData(true)))
                    {
                        object[] args = new object[] { spineComponent.SkeletonDataAsset, behaviour.animationReference.SkeletonDataAsset };
                        Debug.LogWarningFormat("SpineAnimationStateMixerBehaviour tried to apply an animation for the wrong skeleton. Expected {0}. Was {1}", args);
                    }
                    Spine.Animation from = null;
                    float time = 0f;
                    bool loop = false;
                    if ((inputPort != 0) && (inputCount > 1))
                    {
                        ScriptPlayable<SpineAnimationStateBehaviour> playable3 = (ScriptPlayable<SpineAnimationStateBehaviour>) playable.GetInput<Playable>((inputPort - 1));
                        SpineAnimationStateBehaviour behaviour2 = playable3.GetBehaviour();
                        from = behaviour2.animationReference.Animation;
                        time = (float) playable3.GetTime<ScriptPlayable<SpineAnimationStateBehaviour>>();
                        loop = behaviour2.loop;
                    }
                    Spine.Animation animation = behaviour.animationReference.Animation;
                    float num6 = (float) input.GetTime<ScriptPlayable<SpineAnimationStateBehaviour>>();
                    float mixDuration = behaviour.mixDuration;
                    if (!behaviour.customDuration && (from != null))
                    {
                        mixDuration = spineComponent.AnimationState.Data.GetMix(from, animation);
                    }
                    if (((from != null) && (mixDuration > 0f)) && (num6 < mixDuration))
                    {
                        skeleton.SetToSetupPose();
                        float alpha = 1f - (num6 / mixDuration);
                        alpha = (alpha <= 0.5f) ? (alpha * 2f) : 1f;
                        from.Apply(skeleton, 0f, time, loop, null, alpha, MixPose.Setup, MixDirection.Out);
                        animation.Apply(skeleton, 0f, num6, behaviour.loop, null, num6 / mixDuration, MixPose.Current, MixDirection.In);
                    }
                    else
                    {
                        skeleton.SetToSetupPose();
                        animation.PoseSkeleton(skeleton, num6, behaviour.loop);
                    }
                }
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            SkeletonAnimation spineComponent = playerData as SkeletonAnimation;
            if (spineComponent != null)
            {
                Skeleton skeleton = spineComponent.Skeleton;
                Spine.AnimationState animationState = spineComponent.AnimationState;
                if (!Application.isPlaying)
                {
                    this.PreviewEditModePose(playable, spineComponent);
                }
                else
                {
                    int inputCount = playable.GetInputCount<Playable>();
                    if ((this.lastInputWeights == null) || (this.lastInputWeights.Length < inputCount))
                    {
                        this.lastInputWeights = new float[inputCount];
                        for (int j = 0; j < inputCount; j++)
                        {
                            this.lastInputWeights[j] = 0f;
                        }
                    }
                    float[] lastInputWeights = this.lastInputWeights;
                    for (int i = 0; i < inputCount; i++)
                    {
                        float num4 = lastInputWeights[i];
                        float inputWeight = playable.GetInputWeight<Playable>(i);
                        bool flag = inputWeight > num4;
                        lastInputWeights[i] = inputWeight;
                        if (flag)
                        {
                            SpineAnimationStateBehaviour behaviour = ((ScriptPlayable<SpineAnimationStateBehaviour>) playable.GetInput<Playable>(i)).GetBehaviour();
                            if (behaviour.animationReference == null)
                            {
                                float mixDuration = !behaviour.customDuration ? animationState.Data.DefaultMix : behaviour.mixDuration;
                                animationState.SetEmptyAnimation(0, mixDuration);
                            }
                            else if (behaviour.animationReference.Animation != null)
                            {
                                TrackEntry entry = animationState.SetAnimation(0, behaviour.animationReference.Animation, behaviour.loop);
                                entry.EventThreshold = behaviour.eventThreshold;
                                entry.DrawOrderThreshold = behaviour.drawOrderThreshold;
                                entry.AttachmentThreshold = behaviour.attachmentThreshold;
                                if (behaviour.customDuration)
                                {
                                    entry.MixDuration = behaviour.mixDuration;
                                }
                            }
                            spineComponent.Update(0f);
                            spineComponent.LateUpdate();
                        }
                    }
                }
            }
        }
    }
}

