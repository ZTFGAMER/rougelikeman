namespace Spine.Unity.Playables
{
    using Spine;
    using System;
    using UnityEngine;
    using UnityEngine.Playables;

    public class SpineSkeletonFlipMixerBehaviour : PlayableBehaviour
    {
        private bool defaultFlipX;
        private bool defaultFlipY;
        private SpinePlayableHandleBase playableHandle;
        private bool m_FirstFrameHappened;

        public override void OnGraphStop(Playable playable)
        {
            this.m_FirstFrameHappened = false;
            if (this.playableHandle != null)
            {
                Skeleton skeleton = this.playableHandle.Skeleton;
                skeleton.flipX = this.defaultFlipX;
                skeleton.flipY = this.defaultFlipY;
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            this.playableHandle = playerData as SpinePlayableHandleBase;
            if (this.playableHandle != null)
            {
                Skeleton skeleton = this.playableHandle.Skeleton;
                if (!this.m_FirstFrameHappened)
                {
                    this.defaultFlipX = skeleton.flipX;
                    this.defaultFlipY = skeleton.flipY;
                    this.m_FirstFrameHappened = true;
                }
                int inputCount = playable.GetInputCount<Playable>();
                float num2 = 0f;
                float num3 = 0f;
                int num4 = 0;
                for (int i = 0; i < inputCount; i++)
                {
                    float inputWeight = playable.GetInputWeight<Playable>(i);
                    SpineSkeletonFlipBehaviour behaviour = ((ScriptPlayable<SpineSkeletonFlipBehaviour>) playable.GetInput<Playable>(i)).GetBehaviour();
                    num2 += inputWeight;
                    if (inputWeight > num3)
                    {
                        skeleton.flipX = behaviour.flipX;
                        skeleton.flipY = behaviour.flipY;
                        num3 = inputWeight;
                    }
                    if (!Mathf.Approximately(inputWeight, 0f))
                    {
                        num4++;
                    }
                }
                if ((num4 != 1) && ((1f - num2) > num3))
                {
                    skeleton.flipX = this.defaultFlipX;
                    skeleton.flipY = this.defaultFlipY;
                }
            }
        }
    }
}

