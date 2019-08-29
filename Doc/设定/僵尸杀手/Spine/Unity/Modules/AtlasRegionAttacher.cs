namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using Spine.Unity.Modules.AttachmentTools;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AtlasRegionAttacher : MonoBehaviour
    {
        [SerializeField]
        protected AtlasAsset atlasAsset;
        [SerializeField]
        protected bool inheritProperties = true;
        [SerializeField]
        protected List<SlotRegionPair> attachments = new List<SlotRegionPair>();
        private Atlas atlas;

        private void Apply(SkeletonRenderer skeletonRenderer)
        {
            if (base.enabled)
            {
                this.atlas = this.atlasAsset.GetAtlas();
                if (this.atlas != null)
                {
                    float scale = skeletonRenderer.skeletonDataAsset.scale;
                    foreach (SlotRegionPair pair in this.attachments)
                    {
                        Slot slot = skeletonRenderer.Skeleton.FindSlot(pair.slot);
                        Attachment o = slot.Attachment;
                        AtlasRegion atlasRegion = this.atlas.FindRegion(pair.region);
                        if (atlasRegion == null)
                        {
                            slot.Attachment = null;
                        }
                        else if (this.inheritProperties && (o != null))
                        {
                            slot.Attachment = o.GetRemappedClone(atlasRegion, true, true, scale);
                        }
                        else
                        {
                            RegionAttachment attachment2 = atlasRegion.ToRegionAttachment(atlasRegion.name, scale, 0f);
                            slot.Attachment = attachment2;
                        }
                    }
                }
            }
        }

        private void Awake()
        {
            SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
            component.OnRebuild += new SkeletonRenderer.SkeletonRendererDelegate(this.Apply);
            if (component.valid)
            {
                this.Apply(component);
            }
        }

        private void Start()
        {
        }

        [Serializable]
        public class SlotRegionPair
        {
            [SpineSlot("", "", false, true, false)]
            public string slot;
            [SpineAtlasRegion("")]
            public string region;
        }
    }
}

