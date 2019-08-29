namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [RequireComponent(typeof(SkeletonRenderer))]
    public class SkeletonRagdoll : MonoBehaviour
    {
        private static Transform parentSpaceHelper;
        [Header("Hierarchy"), SpineBone("", "", true, false)]
        public string startingBoneName = string.Empty;
        [SpineBone("", "", true, false)]
        public List<string> stopBoneNames = new List<string>();
        [Header("Parameters")]
        public bool applyOnStart;
        [Tooltip("Warning!  You will have to re-enable and tune mix values manually if attempting to remove the ragdoll system.")]
        public bool disableIK = true;
        public bool disableOtherConstraints;
        [Space(18f), Tooltip("Set RootRigidbody IsKinematic to true when Apply is called.")]
        public bool pinStartBone;
        [Tooltip("Enable Collision between adjacent ragdoll elements (IE: Neck and Head)")]
        public bool enableJointCollision;
        public bool useGravity = true;
        [Tooltip("If no BoundingBox Attachment is attached to a bone, this becomes the default Width or Radius of a Bone's ragdoll Rigidbody")]
        public float thickness = 0.125f;
        [Tooltip("Default rotational limit value.  Min is negative this value, Max is this value.")]
        public float rotationLimit = 20f;
        public float rootMass = 20f;
        [Tooltip("If your ragdoll seems unstable or uneffected by limits, try lowering this value."), Range(0.01f, 1f)]
        public float massFalloffFactor = 0.4f;
        [Tooltip("The layer assigned to all of the rigidbody parts.")]
        public int colliderLayer;
        [Range(0f, 1f)]
        public float mix = 1f;
        private ISkeletonAnimation targetSkeletonComponent;
        private Skeleton skeleton;
        private Dictionary<Bone, Transform> boneTable = new Dictionary<Bone, Transform>();
        private Transform ragdollRoot;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Rigidbody <RootRigidbody>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Bone <StartingBone>k__BackingField;
        private Vector3 rootOffset;
        private bool isActive;

        public void Apply()
        {
            this.isActive = true;
            this.mix = 1f;
            this.StartingBone = this.skeleton.FindBone(this.startingBoneName);
            this.RecursivelyCreateBoneProxies(this.StartingBone);
            this.RootRigidbody = this.boneTable[this.StartingBone].GetComponent<Rigidbody>();
            this.RootRigidbody.isKinematic = this.pinStartBone;
            this.RootRigidbody.mass = this.rootMass;
            List<Collider> list = new List<Collider>();
            foreach (KeyValuePair<Bone, Transform> pair in this.boneTable)
            {
                Transform ragdollRoot;
                Bone key = pair.Key;
                Transform transform = pair.Value;
                list.Add(transform.GetComponent<Collider>());
                if (key == this.StartingBone)
                {
                    this.ragdollRoot = new GameObject("RagdollRoot").transform;
                    this.ragdollRoot.SetParent(base.transform, false);
                    if (key == this.skeleton.RootBone)
                    {
                        this.ragdollRoot.localPosition = new Vector3(key.WorldX, key.WorldY, 0f);
                        this.ragdollRoot.localRotation = Quaternion.Euler(0f, 0f, GetPropagatedRotation(key));
                    }
                    else
                    {
                        this.ragdollRoot.localPosition = new Vector3(key.Parent.WorldX, key.Parent.WorldY, 0f);
                        this.ragdollRoot.localRotation = Quaternion.Euler(0f, 0f, GetPropagatedRotation(key.Parent));
                    }
                    ragdollRoot = this.ragdollRoot;
                    this.rootOffset = transform.position - base.transform.position;
                }
                else
                {
                    ragdollRoot = this.boneTable[key.Parent];
                }
                Rigidbody component = ragdollRoot.GetComponent<Rigidbody>();
                if (component != null)
                {
                    HingeJoint joint = transform.gameObject.AddComponent<HingeJoint>();
                    joint.connectedBody = component;
                    Vector3 vector = ragdollRoot.InverseTransformPoint(transform.position);
                    vector.x *= 1f;
                    joint.connectedAnchor = vector;
                    joint.axis = Vector3.forward;
                    joint.GetComponent<Rigidbody>().mass = joint.connectedBody.mass * this.massFalloffFactor;
                    JointLimits limits = new JointLimits {
                        min = -this.rotationLimit,
                        max = this.rotationLimit
                    };
                    joint.limits = limits;
                    joint.useLimits = true;
                    joint.enableCollision = this.enableJointCollision;
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (i != j)
                    {
                        Physics.IgnoreCollision(list[i], list[j]);
                    }
                }
            }
            SkeletonUtilityBone[] componentsInChildren = base.GetComponentsInChildren<SkeletonUtilityBone>();
            if (componentsInChildren.Length > 0)
            {
                List<string> list2 = new List<string>();
                foreach (SkeletonUtilityBone bone2 in componentsInChildren)
                {
                    if (bone2.mode == SkeletonUtilityBone.Mode.Override)
                    {
                        list2.Add(bone2.gameObject.name);
                        UnityEngine.Object.Destroy(bone2.gameObject);
                    }
                }
                if (list2.Count > 0)
                {
                    string message = "Destroyed Utility Bones: ";
                    for (int j = 0; j < list2.Count; j++)
                    {
                        message = message + list2[j];
                        if (j != (list2.Count - 1))
                        {
                            message = message + ",";
                        }
                    }
                    UnityEngine.Debug.LogWarning(message);
                }
            }
            if (this.disableIK)
            {
                ExposedList<IkConstraint> ikConstraints = this.skeleton.IkConstraints;
                int index = 0;
                int count = ikConstraints.Count;
                while (index < count)
                {
                    ikConstraints.Items[index].mix = 0f;
                    index++;
                }
            }
            if (this.disableOtherConstraints)
            {
                ExposedList<TransformConstraint> transformConstraints = this.skeleton.transformConstraints;
                int index = 0;
                int count = transformConstraints.Count;
                while (index < count)
                {
                    transformConstraints.Items[index].rotateMix = 0f;
                    transformConstraints.Items[index].scaleMix = 0f;
                    transformConstraints.Items[index].shearMix = 0f;
                    transformConstraints.Items[index].translateMix = 0f;
                    index++;
                }
                ExposedList<PathConstraint> pathConstraints = this.skeleton.pathConstraints;
                int num9 = 0;
                int num10 = pathConstraints.Count;
                while (num9 < num10)
                {
                    pathConstraints.Items[num9].rotateMix = 0f;
                    pathConstraints.Items[num9].translateMix = 0f;
                    num9++;
                }
            }
            this.targetSkeletonComponent.UpdateWorld += new UpdateBonesDelegate(this.UpdateSpineSkeleton);
        }

        private List<Collider> AttachBoundingBoxRagdollColliders(Bone b)
        {
            Skin defaultSkin;
            List<Collider> list = new List<Collider>();
            Transform transform = this.boneTable[b];
            GameObject gameObject = transform.gameObject;
            Skin skin1 = this.skeleton.Skin;
            if (skin1 != null)
            {
                defaultSkin = skin1;
            }
            else
            {
                defaultSkin = this.skeleton.Data.DefaultSkin;
            }
            List<Attachment> attachments = new List<Attachment>();
            foreach (Slot slot in this.skeleton.Slots)
            {
                if (slot.Bone == b)
                {
                    defaultSkin.FindAttachmentsForSlot(this.skeleton.Slots.IndexOf(slot), attachments);
                    foreach (Attachment attachment in attachments)
                    {
                        BoundingBoxAttachment boundingBox = attachment as BoundingBoxAttachment;
                        if ((boundingBox != null) && attachment.Name.ToLower().Contains("ragdoll"))
                        {
                            BoxCollider item = gameObject.AddComponent<BoxCollider>();
                            Bounds boundingBoxBounds = SkeletonUtility.GetBoundingBoxBounds(boundingBox, this.thickness);
                            item.center = boundingBoxBounds.center;
                            item.size = boundingBoxBounds.size;
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        private static float GetPropagatedRotation(Bone b)
        {
            Bone parent = b.Parent;
            float appliedRotation = b.AppliedRotation;
            while (parent != null)
            {
                appliedRotation += parent.AppliedRotation;
                parent = parent.parent;
            }
            return appliedRotation;
        }

        public Rigidbody GetRigidbody(string boneName)
        {
            Bone key = this.skeleton.FindBone(boneName);
            return (((key == null) || !this.boneTable.ContainsKey(key)) ? null : this.boneTable[key].GetComponent<Rigidbody>());
        }

        private void RecursivelyCreateBoneProxies(Bone b)
        {
            string name = b.data.name;
            if (!this.stopBoneNames.Contains(name))
            {
                GameObject obj2 = new GameObject(name) {
                    layer = this.colliderLayer
                };
                Transform transform = obj2.transform;
                this.boneTable.Add(b, transform);
                transform.parent = base.transform;
                transform.localPosition = new Vector3(b.WorldX, b.WorldY, 0f);
                transform.localRotation = Quaternion.Euler(0f, 0f, b.WorldRotationX - b.shearX);
                transform.localScale = new Vector3(b.WorldScaleX, b.WorldScaleY, 1f);
                if (this.AttachBoundingBoxRagdollColliders(b).Count == 0)
                {
                    float length = b.Data.Length;
                    if (length == 0f)
                    {
                        obj2.AddComponent<SphereCollider>().radius = this.thickness * 0.5f;
                    }
                    else
                    {
                        BoxCollider collider2 = obj2.AddComponent<BoxCollider>();
                        collider2.size = new Vector3(length, this.thickness, this.thickness);
                        collider2.center = new Vector3(length * 0.5f, 0f);
                    }
                }
                obj2.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
                foreach (Bone bone in b.Children)
                {
                    this.RecursivelyCreateBoneProxies(bone);
                }
            }
        }

        public void Remove()
        {
            this.isActive = false;
            foreach (Transform transform in this.boneTable.Values)
            {
                UnityEngine.Object.Destroy(transform.gameObject);
            }
            UnityEngine.Object.Destroy(this.ragdollRoot.gameObject);
            this.boneTable.Clear();
            this.targetSkeletonComponent.UpdateWorld -= new UpdateBonesDelegate(this.UpdateSpineSkeleton);
        }

        public void SetSkeletonPosition(Vector3 worldPosition)
        {
            if (!this.isActive)
            {
                UnityEngine.Debug.LogWarning("Can't call SetSkeletonPosition while Ragdoll is not active!");
            }
            else
            {
                Vector3 vector = worldPosition - base.transform.position;
                base.transform.position = worldPosition;
                foreach (Transform transform in this.boneTable.Values)
                {
                    transform.position -= vector;
                }
                this.UpdateSpineSkeleton(null);
                this.skeleton.UpdateWorldTransform();
            }
        }

        public Coroutine SmoothMix(float target, float duration) => 
            base.StartCoroutine(this.SmoothMixCoroutine(target, duration));

        [DebuggerHidden]
        private IEnumerator SmoothMixCoroutine(float target, float duration) => 
            new <SmoothMixCoroutine>c__Iterator1 { 
                target = target,
                duration = duration,
                $this = this
            };

        [DebuggerHidden]
        private IEnumerator Start() => 
            new <Start>c__Iterator0 { $this = this };

        private void UpdateSpineSkeleton(ISkeletonAnimation skeletonRenderer)
        {
            bool flipX = this.skeleton.flipX;
            bool flipY = this.skeleton.flipY;
            bool flag3 = flipX ^ flipY;
            bool flag4 = flipX || flipY;
            foreach (KeyValuePair<Bone, Transform> pair in this.boneTable)
            {
                Bone key = pair.Key;
                Transform transform = pair.Value;
                bool flag5 = key == this.StartingBone;
                Transform transform2 = !flag5 ? this.boneTable[key.Parent] : this.ragdollRoot;
                Vector3 position = transform2.position;
                Quaternion rotation = transform2.rotation;
                parentSpaceHelper.position = position;
                parentSpaceHelper.rotation = rotation;
                parentSpaceHelper.localScale = transform2.localScale;
                Vector3 vector2 = transform.position;
                Vector3 vector3 = parentSpaceHelper.InverseTransformDirection(transform.right);
                Vector3 vector4 = parentSpaceHelper.InverseTransformPoint(vector2);
                float b = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
                if (flag4)
                {
                    if (flag5)
                    {
                        if (flipX)
                        {
                            vector4.x *= -1f;
                        }
                        if (flipY)
                        {
                            vector4.y *= -1f;
                        }
                        b *= !flag3 ? 1f : -1f;
                        if (flipX)
                        {
                            b += 180f;
                        }
                    }
                    else if (flag3)
                    {
                        b *= -1f;
                        vector4.y *= -1f;
                    }
                }
                key.x = Mathf.Lerp(key.x, vector4.x, this.mix);
                key.y = Mathf.Lerp(key.y, vector4.y, this.mix);
                key.rotation = Mathf.Lerp(key.rotation, b, this.mix);
            }
        }

        public Rigidbody RootRigidbody { get; private set; }

        public Bone StartingBone { get; private set; }

        public Vector3 RootOffset =>
            this.rootOffset;

        public bool IsActive =>
            this.isActive;

        public Rigidbody[] RigidbodyArray
        {
            get
            {
                if (!this.isActive)
                {
                    return new Rigidbody[0];
                }
                Rigidbody[] rigidbodyArray = new Rigidbody[this.boneTable.Count];
                int index = 0;
                foreach (Transform transform in this.boneTable.Values)
                {
                    rigidbodyArray[index] = transform.GetComponent<Rigidbody>();
                    index++;
                }
                return rigidbodyArray;
            }
        }

        public Vector3 EstimatedSkeletonPosition =>
            (this.RootRigidbody.position - this.rootOffset);

        [CompilerGenerated]
        private sealed class <SmoothMixCoroutine>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal float <startTime>__0;
            internal float <startMix>__0;
            internal float target;
            internal float duration;
            internal SkeletonRagdoll $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<startTime>__0 = Time.time;
                        this.<startMix>__0 = this.$this.mix;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B8;
                }
                if (this.$this.mix > 0f)
                {
                    this.$this.skeleton.SetBonesToSetupPose();
                    this.$this.mix = Mathf.SmoothStep(this.<startMix>__0, this.target, (Time.time - this.<startTime>__0) / this.duration);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$PC = -1;
            Label_00B8:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal SkeletonRagdoll $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (SkeletonRagdoll.parentSpaceHelper == null)
                        {
                            SkeletonRagdoll.parentSpaceHelper = new GameObject("Parent Space Helper").transform;
                            SkeletonRagdoll.parentSpaceHelper.hideFlags = HideFlags.HideInHierarchy;
                        }
                        this.$this.targetSkeletonComponent = this.$this.GetComponent<SkeletonRenderer>() as ISkeletonAnimation;
                        if (this.$this.targetSkeletonComponent == null)
                        {
                            UnityEngine.Debug.LogError("Attached Spine component does not implement ISkeletonAnimation. This script is not compatible.");
                        }
                        this.$this.skeleton = this.$this.targetSkeletonComponent.Skeleton;
                        if (!this.$this.applyOnStart)
                        {
                            break;
                        }
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.Apply();
                        break;

                    default:
                        goto Label_00DD;
                }
                this.$PC = -1;
            Label_00DD:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        public class LayerFieldAttribute : PropertyAttribute
        {
        }
    }
}

