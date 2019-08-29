namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SkeletonColorInitialize : MonoBehaviour
    {
        public Color skeletonColor = Color.white;
        public List<SlotSettings> slotSettings = new List<SlotSettings>();

        private void ApplySettings()
        {
            ISkeletonComponent component = base.GetComponent<ISkeletonComponent>();
            if (component != null)
            {
                Skeleton skeleton = component.Skeleton;
                skeleton.SetColor(this.skeletonColor);
                foreach (SlotSettings settings in this.slotSettings)
                {
                    Slot slot = skeleton.FindSlot(settings.slot);
                    if (slot != null)
                    {
                        slot.SetColor(settings.color);
                    }
                }
            }
        }

        private void Start()
        {
            this.ApplySettings();
        }

        [Serializable]
        public class SlotSettings
        {
            [SpineSlot("", "", false, true, false)]
            public string slot = string.Empty;
            public Color color = Color.white;
        }
    }
}

