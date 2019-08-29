namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using Spine.Unity.Modules.AttachmentTools;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SpriteAttacher : MonoBehaviour
    {
        public const string DefaultPMAShader = "Spine/Skeleton";
        public const string DefaultStraightAlphaShader = "Sprites/Default";
        public bool attachOnStart = true;
        public bool overrideAnimation = true;
        public Sprite sprite;
        [SpineSlot("", "", false, true, false)]
        public string slot;
        private RegionAttachment attachment;
        private Slot spineSlot;
        private bool applyPMA;
        private static Dictionary<Texture, AtlasPage> atlasPageCache;

        private void AnimationOverrideSpriteAttach(ISkeletonAnimation animated)
        {
            if (this.overrideAnimation && base.isActiveAndEnabled)
            {
                this.Attach();
            }
        }

        public void Attach()
        {
            if (this.spineSlot != null)
            {
                this.spineSlot.Attachment = this.attachment;
            }
        }

        private static AtlasPage GetPageFor(Texture texture, Shader shader)
        {
            if (atlasPageCache == null)
            {
                atlasPageCache = new Dictionary<Texture, AtlasPage>();
            }
            atlasPageCache.TryGetValue(texture, out AtlasPage page);
            if (page == null)
            {
                page = new Material(shader).ToSpineAtlasPage();
                atlasPageCache[texture] = page;
            }
            return page;
        }

        public void Initialize(bool overwrite = true)
        {
            if (overwrite || (this.attachment == null))
            {
                ISkeletonComponent component = base.GetComponent<ISkeletonComponent>();
                SkeletonRenderer renderer = component as SkeletonRenderer;
                if (renderer != null)
                {
                    this.applyPMA = renderer.pmaVertexColors;
                }
                else
                {
                    SkeletonGraphic graphic = component as SkeletonGraphic;
                    if (graphic != null)
                    {
                        this.applyPMA = graphic.MeshGenerator.settings.pmaVertexColors;
                    }
                }
                if (this.overrideAnimation)
                {
                    ISkeletonAnimation animation = component as ISkeletonAnimation;
                    if (animation != null)
                    {
                        animation.UpdateComplete -= new UpdateBonesDelegate(this.AnimationOverrideSpriteAttach);
                        animation.UpdateComplete += new UpdateBonesDelegate(this.AnimationOverrideSpriteAttach);
                    }
                }
                if (this.spineSlot == null)
                {
                }
                this.spineSlot = component.Skeleton.FindSlot(this.slot);
                Shader shader = !this.applyPMA ? Shader.Find("Sprites/Default") : Shader.Find("Spine/Skeleton");
                this.attachment = !this.applyPMA ? this.sprite.ToRegionAttachment(GetPageFor(this.sprite.texture, shader), 0f) : this.sprite.ToRegionAttachmentPMAClone(shader, TextureFormat.RGBA32, false, null, 0f);
            }
        }

        private void OnDestroy()
        {
            ISkeletonAnimation component = base.GetComponent<ISkeletonAnimation>();
            if (component != null)
            {
                component.UpdateComplete -= new UpdateBonesDelegate(this.AnimationOverrideSpriteAttach);
            }
        }

        private void Start()
        {
            this.Initialize(false);
            if (this.attachOnStart)
            {
                this.Attach();
            }
        }
    }
}

