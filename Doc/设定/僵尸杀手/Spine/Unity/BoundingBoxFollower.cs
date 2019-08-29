namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class BoundingBoxFollower : MonoBehaviour
    {
        internal static bool DebugMessages = true;
        public SkeletonRenderer skeletonRenderer;
        [SpineSlot("", "skeletonRenderer", true, true, false)]
        public string slotName;
        public bool isTrigger;
        public bool clearStateOnDisable = true;
        private Spine.Slot slot;
        private BoundingBoxAttachment currentAttachment;
        private string currentAttachmentName;
        private PolygonCollider2D currentCollider;
        public readonly Dictionary<BoundingBoxAttachment, PolygonCollider2D> colliderTable = new Dictionary<BoundingBoxAttachment, PolygonCollider2D>();
        public readonly Dictionary<BoundingBoxAttachment, string> nameTable = new Dictionary<BoundingBoxAttachment, string>();

        private void AddSkin(Skin skin, int slotIndex)
        {
            if (skin != null)
            {
                List<string> names = new List<string>();
                skin.FindNamesForSlot(slotIndex, names);
                foreach (string str in names)
                {
                    Attachment attachment = skin.GetAttachment(slotIndex, str);
                    BoundingBoxAttachment key = attachment as BoundingBoxAttachment;
                    if ((DebugMessages && (attachment != null)) && (key == null))
                    {
                        Debug.Log("BoundingBoxFollower tried to follow a slot that contains non-boundingbox attachments: " + this.slotName);
                    }
                    if ((key != null) && !this.colliderTable.ContainsKey(key))
                    {
                        PolygonCollider2D colliderd = SkeletonUtility.AddBoundingBoxAsComponent(key, this.slot, base.gameObject, this.isTrigger, true, 0f);
                        colliderd.enabled = false;
                        colliderd.hideFlags = HideFlags.NotEditable;
                        colliderd.isTrigger = this.IsTrigger;
                        this.colliderTable.Add(key, colliderd);
                        this.nameTable.Add(key, str);
                    }
                }
            }
        }

        public void ClearState()
        {
            if (this.colliderTable != null)
            {
                foreach (PolygonCollider2D colliderd in this.colliderTable.Values)
                {
                    colliderd.enabled = false;
                }
            }
            this.currentAttachment = null;
            this.currentAttachmentName = null;
            this.currentCollider = null;
        }

        private void DisposeColliders()
        {
            PolygonCollider2D[] components = base.GetComponents<PolygonCollider2D>();
            if (components.Length != 0)
            {
                if (Application.isEditor)
                {
                    if (Application.isPlaying)
                    {
                        foreach (PolygonCollider2D colliderd in components)
                        {
                            if (colliderd != null)
                            {
                                UnityEngine.Object.Destroy(colliderd);
                            }
                        }
                    }
                    else
                    {
                        foreach (PolygonCollider2D colliderd2 in components)
                        {
                            if (colliderd2 != null)
                            {
                                UnityEngine.Object.DestroyImmediate(colliderd2);
                            }
                        }
                    }
                }
                else
                {
                    foreach (PolygonCollider2D colliderd3 in components)
                    {
                        if (colliderd3 != null)
                        {
                            UnityEngine.Object.Destroy(colliderd3);
                        }
                    }
                }
                this.slot = null;
                this.currentAttachment = null;
                this.currentAttachmentName = null;
                this.currentCollider = null;
                this.colliderTable.Clear();
                this.nameTable.Clear();
            }
        }

        private void HandleRebuild(SkeletonRenderer sr)
        {
            this.Initialize(false);
        }

        public void Initialize(bool overwrite = false)
        {
            if (this.skeletonRenderer != null)
            {
                this.skeletonRenderer.Initialize(false);
                if (!string.IsNullOrEmpty(this.slotName) && (((overwrite || (this.colliderTable.Count <= 0)) || ((this.slot == null) || (this.skeletonRenderer.skeleton != this.slot.Skeleton))) || (this.slotName != this.slot.data.name)))
                {
                    this.DisposeColliders();
                    Skeleton skeleton = this.skeletonRenderer.skeleton;
                    this.slot = skeleton.FindSlot(this.slotName);
                    int slotIndex = skeleton.FindSlotIndex(this.slotName);
                    if (this.slot == null)
                    {
                        if (DebugMessages)
                        {
                            Debug.LogWarning($"Slot '{this.slotName}' not found for BoundingBoxFollower on '{base.gameObject.name}'. (Previous colliders were disposed.)");
                        }
                    }
                    else
                    {
                        if (base.gameObject.activeInHierarchy)
                        {
                            foreach (Skin skin in skeleton.Data.Skins)
                            {
                                this.AddSkin(skin, slotIndex);
                            }
                            if (skeleton.skin != null)
                            {
                                this.AddSkin(skeleton.skin, slotIndex);
                            }
                        }
                        if (DebugMessages && (this.colliderTable.Count == 0))
                        {
                            if (base.gameObject.activeInHierarchy)
                            {
                                Debug.LogWarning("Bounding Box Follower not valid! Slot [" + this.slotName + "] does not contain any Bounding Box Attachments!");
                            }
                            else
                            {
                                Debug.LogWarning("Bounding Box Follower tried to rebuild as a prefab.");
                            }
                        }
                    }
                }
            }
        }

        private void LateUpdate()
        {
            if ((this.slot != null) && (this.slot.Attachment != this.currentAttachment))
            {
                this.MatchAttachment(this.slot.Attachment);
            }
        }

        private void MatchAttachment(Attachment attachment)
        {
            BoundingBoxAttachment key = attachment as BoundingBoxAttachment;
            if ((DebugMessages && (attachment != null)) && (key == null))
            {
                Debug.LogWarning("BoundingBoxFollower tried to match a non-boundingbox attachment. It will treat it as null.");
            }
            if (this.currentCollider != null)
            {
                this.currentCollider.enabled = false;
            }
            if (key == null)
            {
                this.currentCollider = null;
                this.currentAttachment = null;
                this.currentAttachmentName = null;
            }
            else
            {
                this.colliderTable.TryGetValue(key, out PolygonCollider2D colliderd);
                if (colliderd != null)
                {
                    this.currentCollider = colliderd;
                    this.currentCollider.enabled = true;
                    this.currentAttachment = key;
                    this.currentAttachmentName = this.nameTable[key];
                }
                else
                {
                    this.currentCollider = null;
                    this.currentAttachment = key;
                    this.currentAttachmentName = null;
                    if (DebugMessages)
                    {
                        object[] args = new object[] { key.Name };
                        Debug.LogFormat("Collider for BoundingBoxAttachment named '{0}' was not initialized. It is possibly from a new skin. currentAttachmentName will be null. You may need to call BoundingBoxFollower.Initialize(overwrite: true);", args);
                    }
                }
            }
        }

        private void OnDisable()
        {
            if (this.clearStateOnDisable)
            {
                this.ClearState();
            }
        }

        private void OnEnable()
        {
            if (this.skeletonRenderer != null)
            {
                this.skeletonRenderer.OnRebuild -= new SkeletonRenderer.SkeletonRendererDelegate(this.HandleRebuild);
                this.skeletonRenderer.OnRebuild += new SkeletonRenderer.SkeletonRendererDelegate(this.HandleRebuild);
            }
            this.Initialize(false);
        }

        private void Start()
        {
            this.Initialize(false);
        }

        public Spine.Slot Slot =>
            this.slot;

        public BoundingBoxAttachment CurrentAttachment =>
            this.currentAttachment;

        public string CurrentAttachmentName =>
            this.currentAttachmentName;

        public PolygonCollider2D CurrentCollider =>
            this.currentCollider;

        public bool IsTrigger =>
            this.isTrigger;
    }
}

