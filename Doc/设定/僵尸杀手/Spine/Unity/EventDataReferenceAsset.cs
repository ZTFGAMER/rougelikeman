namespace Spine.Unity
{
    using Spine;
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName="Spine/EventData Reference Asset")]
    public class EventDataReferenceAsset : ScriptableObject
    {
        private const bool QuietSkeletonData = true;
        [SerializeField]
        protected SkeletonDataAsset skeletonDataAsset;
        [SerializeField, SpineEvent("", "skeletonDataAsset", true, false)]
        protected string eventName;
        private Spine.EventData eventData;

        public void Initialize()
        {
            if (this.skeletonDataAsset != null)
            {
                this.eventData = this.skeletonDataAsset.GetSkeletonData(true).FindEvent(this.eventName);
                if (this.eventData == null)
                {
                    object[] args = new object[] { this.eventName, this.skeletonDataAsset.name };
                    Debug.LogWarningFormat("Event Data '{0}' not found in SkeletonData : {1}.", args);
                }
            }
        }

        public static implicit operator Spine.EventData(EventDataReferenceAsset asset) => 
            asset.EventData;

        public Spine.EventData EventData
        {
            get
            {
                if (this.eventData == null)
                {
                    this.Initialize();
                }
                return this.eventData;
            }
        }
    }
}

