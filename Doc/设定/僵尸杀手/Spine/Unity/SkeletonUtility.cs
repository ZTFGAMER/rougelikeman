namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    [RequireComponent(typeof(ISkeletonAnimation)), ExecuteInEditMode]
    public class SkeletonUtility : MonoBehaviour
    {
        public Transform boneRoot;
        [HideInInspector]
        public SkeletonRenderer skeletonRenderer;
        [HideInInspector]
        public ISkeletonAnimation skeletonAnimation;
        [NonSerialized]
        public List<SkeletonUtilityBone> utilityBones = new List<SkeletonUtilityBone>();
        [NonSerialized]
        public List<SkeletonUtilityConstraint> utilityConstraints = new List<SkeletonUtilityConstraint>();
        protected bool hasTransformBones;
        protected bool hasUtilityConstraints;
        protected bool needToReprocessBones;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event SkeletonUtilityDelegate OnReset;

        public static PolygonCollider2D AddBoundingBoxAsComponent(BoundingBoxAttachment box, Slot slot, GameObject gameObject, bool isTrigger = true, bool isKinematic = true, float gravityScale = 0f)
        {
            if (box == null)
            {
                return null;
            }
            if ((slot.bone != slot.Skeleton.RootBone) && (gameObject.GetComponent<Rigidbody2D>() == null))
            {
                Rigidbody2D rigidbodyd = gameObject.AddComponent<Rigidbody2D>();
                rigidbodyd.isKinematic = isKinematic;
                rigidbodyd.gravityScale = gravityScale;
            }
            PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
            collider.isTrigger = isTrigger;
            SetColliderPointsLocal(collider, slot, box);
            return collider;
        }

        public static PolygonCollider2D AddBoundingBoxGameObject(string name, BoundingBoxAttachment box, Slot slot, Transform parent, bool isTrigger = true)
        {
            GameObject gameObject = new GameObject("[BoundingBox]" + (!string.IsNullOrEmpty(name) ? name : box.Name));
            Transform transform = gameObject.transform;
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            return AddBoundingBoxAsComponent(box, slot, gameObject, isTrigger, true, 0f);
        }

        public static PolygonCollider2D AddBoundingBoxGameObject(Skeleton skeleton, string skinName, string slotName, string attachmentName, Transform parent, bool isTrigger = true)
        {
            Skin skin = !string.IsNullOrEmpty(skinName) ? skeleton.data.FindSkin(skinName) : skeleton.data.defaultSkin;
            if (skin == null)
            {
                UnityEngine.Debug.LogError("Skin " + skinName + " not found!");
                return null;
            }
            Attachment attachment = skin.GetAttachment(skeleton.FindSlotIndex(slotName), attachmentName);
            if (attachment == null)
            {
                object[] objArray1 = new object[] { slotName, attachmentName, skin.name };
                UnityEngine.Debug.LogFormat("Attachment in slot '{0}' named '{1}' not found in skin '{2}'.", objArray1);
                return null;
            }
            BoundingBoxAttachment box = attachment as BoundingBoxAttachment;
            if (box != null)
            {
                Slot slot = skeleton.FindSlot(slotName);
                return AddBoundingBoxGameObject(box.Name, box, slot, parent, isTrigger);
            }
            object[] args = new object[] { attachmentName };
            UnityEngine.Debug.LogFormat("Attachment '{0}' was not a Bounding Box.", args);
            return null;
        }

        public void CollectBones()
        {
            Skeleton skeleton = this.skeletonRenderer.skeleton;
            if (skeleton != null)
            {
                if (this.boneRoot != null)
                {
                    List<object> list = new List<object>();
                    ExposedList<IkConstraint> ikConstraints = skeleton.IkConstraints;
                    int index = 0;
                    int count = ikConstraints.Count;
                    while (index < count)
                    {
                        list.Add(ikConstraints.Items[index].target);
                        index++;
                    }
                    ExposedList<TransformConstraint> transformConstraints = skeleton.TransformConstraints;
                    int num3 = 0;
                    int num4 = transformConstraints.Count;
                    while (num3 < num4)
                    {
                        list.Add(transformConstraints.Items[num3].target);
                        num3++;
                    }
                    List<SkeletonUtilityBone> utilityBones = this.utilityBones;
                    int num5 = 0;
                    int num6 = utilityBones.Count;
                    while (num5 < num6)
                    {
                        SkeletonUtilityBone bone = utilityBones[num5];
                        if (bone.bone != null)
                        {
                            this.hasTransformBones |= bone.mode == SkeletonUtilityBone.Mode.Override;
                            this.hasUtilityConstraints |= list.Contains(bone.bone);
                        }
                        num5++;
                    }
                    this.hasUtilityConstraints |= this.utilityConstraints.Count > 0;
                    if (this.skeletonAnimation != null)
                    {
                        this.skeletonAnimation.UpdateWorld -= new UpdateBonesDelegate(this.UpdateWorld);
                        this.skeletonAnimation.UpdateComplete -= new UpdateBonesDelegate(this.UpdateComplete);
                        if (this.hasTransformBones || this.hasUtilityConstraints)
                        {
                            this.skeletonAnimation.UpdateWorld += new UpdateBonesDelegate(this.UpdateWorld);
                        }
                        if (this.hasUtilityConstraints)
                        {
                            this.skeletonAnimation.UpdateComplete += new UpdateBonesDelegate(this.UpdateComplete);
                        }
                    }
                    this.needToReprocessBones = false;
                }
                else
                {
                    this.utilityBones.Clear();
                    this.utilityConstraints.Clear();
                }
            }
        }

        public Transform GetBoneRoot()
        {
            if (this.boneRoot == null)
            {
                this.boneRoot = new GameObject("SkeletonUtility-Root").transform;
                this.boneRoot.parent = base.transform;
                this.boneRoot.localPosition = Vector3.zero;
                this.boneRoot.localRotation = Quaternion.identity;
                this.boneRoot.localScale = Vector3.one;
            }
            return this.boneRoot;
        }

        public static Bounds GetBoundingBoxBounds(BoundingBoxAttachment boundingBox, float depth = 0f)
        {
            float[] vertices = boundingBox.Vertices;
            int length = vertices.Length;
            Bounds bounds = new Bounds {
                center = new Vector3(vertices[0], vertices[1], 0f)
            };
            for (int i = 2; i < length; i += 2)
            {
                bounds.Encapsulate(new Vector3(vertices[i], vertices[i + 1], 0f));
            }
            Vector3 size = bounds.size;
            size.z = depth;
            bounds.size = size;
            return bounds;
        }

        private void HandleRendererReset(SkeletonRenderer r)
        {
            if (this.OnReset != null)
            {
                this.OnReset();
            }
            this.CollectBones();
        }

        private void OnDisable()
        {
            this.skeletonRenderer.OnRebuild -= new SkeletonRenderer.SkeletonRendererDelegate(this.HandleRendererReset);
            if (this.skeletonAnimation != null)
            {
                this.skeletonAnimation.UpdateLocal -= new UpdateBonesDelegate(this.UpdateLocal);
                this.skeletonAnimation.UpdateWorld -= new UpdateBonesDelegate(this.UpdateWorld);
                this.skeletonAnimation.UpdateComplete -= new UpdateBonesDelegate(this.UpdateComplete);
            }
        }

        private void OnEnable()
        {
            if (this.skeletonRenderer == null)
            {
                this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
            }
            if (this.skeletonAnimation == null)
            {
                this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
                if (this.skeletonAnimation == null)
                {
                    this.skeletonAnimation = base.GetComponent<SkeletonAnimator>();
                }
            }
            this.skeletonRenderer.OnRebuild -= new SkeletonRenderer.SkeletonRendererDelegate(this.HandleRendererReset);
            this.skeletonRenderer.OnRebuild += new SkeletonRenderer.SkeletonRendererDelegate(this.HandleRendererReset);
            if (this.skeletonAnimation != null)
            {
                this.skeletonAnimation.UpdateLocal -= new UpdateBonesDelegate(this.UpdateLocal);
                this.skeletonAnimation.UpdateLocal += new UpdateBonesDelegate(this.UpdateLocal);
            }
            this.CollectBones();
        }

        public void RegisterBone(SkeletonUtilityBone bone)
        {
            if (!this.utilityBones.Contains(bone))
            {
                this.utilityBones.Add(bone);
                this.needToReprocessBones = true;
            }
        }

        public void RegisterConstraint(SkeletonUtilityConstraint constraint)
        {
            if (!this.utilityConstraints.Contains(constraint))
            {
                this.utilityConstraints.Add(constraint);
                this.needToReprocessBones = true;
            }
        }

        public static void SetColliderPointsLocal(PolygonCollider2D collider, Slot slot, BoundingBoxAttachment box)
        {
            if (box != null)
            {
                if (box.IsWeighted())
                {
                    UnityEngine.Debug.LogWarning("UnityEngine.PolygonCollider2D does not support weighted or animated points. Collider points will not be animated and may have incorrect orientation. If you want to use it as a collider, please remove weights and animations from the bounding box in Spine editor.");
                }
                Vector2[] points = box.GetLocalVertices(slot, null);
                collider.SetPath(0, points);
            }
        }

        public GameObject SpawnBone(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
        {
            GameObject obj2 = new GameObject(bone.Data.Name) {
                transform = { parent = parent }
            };
            SkeletonUtilityBone bone2 = obj2.AddComponent<SkeletonUtilityBone>();
            bone2.skeletonUtility = this;
            bone2.position = pos;
            bone2.rotation = rot;
            bone2.scale = sca;
            bone2.mode = mode;
            bone2.zPosition = true;
            bone2.Reset();
            bone2.bone = bone;
            bone2.boneName = bone.Data.Name;
            bone2.valid = true;
            if (mode == SkeletonUtilityBone.Mode.Override)
            {
                if (rot)
                {
                    obj2.transform.localRotation = Quaternion.Euler(0f, 0f, bone2.bone.AppliedRotation);
                }
                if (pos)
                {
                    obj2.transform.localPosition = new Vector3(bone2.bone.X, bone2.bone.Y, 0f);
                }
                obj2.transform.localScale = new Vector3(bone2.bone.scaleX, bone2.bone.scaleY, 0f);
            }
            return obj2;
        }

        public GameObject SpawnBoneRecursively(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
        {
            GameObject obj2 = this.SpawnBone(bone, parent, mode, pos, rot, sca);
            ExposedList<Bone> children = bone.Children;
            int index = 0;
            int count = children.Count;
            while (index < count)
            {
                Bone bone2 = children.Items[index];
                this.SpawnBoneRecursively(bone2, obj2.transform, mode, pos, rot, sca);
                index++;
            }
            return obj2;
        }

        public GameObject SpawnHierarchy(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
        {
            this.GetBoneRoot();
            Skeleton skeleton = this.skeletonRenderer.skeleton;
            GameObject obj2 = this.SpawnBoneRecursively(skeleton.RootBone, this.boneRoot, mode, pos, rot, sca);
            this.CollectBones();
            return obj2;
        }

        public GameObject SpawnRoot(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
        {
            this.GetBoneRoot();
            Skeleton skeleton = this.skeletonRenderer.skeleton;
            GameObject obj2 = this.SpawnBone(skeleton.RootBone, this.boneRoot, mode, pos, rot, sca);
            this.CollectBones();
            return obj2;
        }

        private void Start()
        {
            this.CollectBones();
        }

        public void UnregisterBone(SkeletonUtilityBone bone)
        {
            this.utilityBones.Remove(bone);
        }

        public void UnregisterConstraint(SkeletonUtilityConstraint constraint)
        {
            this.utilityConstraints.Remove(constraint);
        }

        private void Update()
        {
            Skeleton skeleton = this.skeletonRenderer.skeleton;
            if ((this.boneRoot != null) && (skeleton != null))
            {
                Vector3 one = Vector3.one;
                if (skeleton.FlipX)
                {
                    one.x = -1f;
                }
                if (skeleton.FlipY)
                {
                    one.y = -1f;
                }
                this.boneRoot.localScale = one;
            }
        }

        private void UpdateAllBones(SkeletonUtilityBone.UpdatePhase phase)
        {
            if (this.boneRoot == null)
            {
                this.CollectBones();
            }
            List<SkeletonUtilityBone> utilityBones = this.utilityBones;
            if (utilityBones != null)
            {
                int num = 0;
                int count = utilityBones.Count;
                while (num < count)
                {
                    utilityBones[num].DoUpdate(phase);
                    num++;
                }
            }
        }

        private void UpdateComplete(ISkeletonAnimation anim)
        {
            this.UpdateAllBones(SkeletonUtilityBone.UpdatePhase.Complete);
        }

        private void UpdateLocal(ISkeletonAnimation anim)
        {
            if (this.needToReprocessBones)
            {
                this.CollectBones();
            }
            List<SkeletonUtilityBone> utilityBones = this.utilityBones;
            if (utilityBones != null)
            {
                int num = 0;
                int count = utilityBones.Count;
                while (num < count)
                {
                    utilityBones[num].transformLerpComplete = false;
                    num++;
                }
                this.UpdateAllBones(SkeletonUtilityBone.UpdatePhase.Local);
            }
        }

        private void UpdateWorld(ISkeletonAnimation anim)
        {
            this.UpdateAllBones(SkeletonUtilityBone.UpdatePhase.World);
            int num = 0;
            int count = this.utilityConstraints.Count;
            while (num < count)
            {
                this.utilityConstraints[num].DoUpdate();
                num++;
            }
        }

        public delegate void SkeletonUtilityDelegate();
    }
}

