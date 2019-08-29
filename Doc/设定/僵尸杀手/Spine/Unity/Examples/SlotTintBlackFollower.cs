namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class SlotTintBlackFollower : MonoBehaviour
    {
        [SpineSlot("", "", false, true, false), SerializeField]
        protected string slotName;
        [SerializeField]
        protected string colorPropertyName = "_Color";
        [SerializeField]
        protected string blackPropertyName = "_Black";
        public Slot slot;
        private MeshRenderer mr;
        private MaterialPropertyBlock mb;
        private int colorPropertyId;
        private int blackPropertyId;

        public void Initialize(bool overwrite)
        {
            if (overwrite || (this.mb == null))
            {
                this.mb = new MaterialPropertyBlock();
                this.mr = base.GetComponent<MeshRenderer>();
                this.slot = base.GetComponent<ISkeletonComponent>().Skeleton.FindSlot(this.slotName);
                this.colorPropertyId = Shader.PropertyToID(this.colorPropertyName);
                this.blackPropertyId = Shader.PropertyToID(this.blackPropertyName);
            }
        }

        private void OnDisable()
        {
            this.mb.Clear();
            this.mr.SetPropertyBlock(this.mb);
        }

        private void Start()
        {
            this.Initialize(false);
        }

        public void Update()
        {
            Slot s = this.slot;
            if (s != null)
            {
                this.mb.SetColor(this.colorPropertyId, s.GetColor());
                this.mb.SetColor(this.blackPropertyId, s.GetColorTintBlack());
                this.mr.SetPropertyBlock(this.mb);
            }
        }
    }
}

