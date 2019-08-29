namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using Spine.Unity.Modules.AttachmentTools;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CombinedSkin : MonoBehaviour
    {
        [SpineSkin("", "", true, false)]
        public List<string> skinsToCombine;
        private Skin combinedSkin;

        private void Start()
        {
            ISkeletonComponent component = base.GetComponent<ISkeletonComponent>();
            if (component != null)
            {
                Skeleton skeleton = component.Skeleton;
                if (skeleton != null)
                {
                    if (this.combinedSkin == null)
                    {
                    }
                    this.combinedSkin = new Skin("combined");
                    this.combinedSkin.Clear();
                    foreach (string str in this.skinsToCombine)
                    {
                        Skin source = skeleton.Data.FindSkin(str);
                        if (source != null)
                        {
                            this.combinedSkin.Append(source);
                        }
                    }
                    skeleton.SetSkin(this.combinedSkin);
                    skeleton.SetToSetupPose();
                    IAnimationStateComponent component2 = component as IAnimationStateComponent;
                    if (component2 != null)
                    {
                        component2.AnimationState.Apply(skeleton);
                    }
                }
            }
        }
    }
}

