namespace Spine.Unity.Modules
{
    using Spine.Unity;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SkeletonUtilityKinematicShadow : MonoBehaviour
    {
        [Tooltip("If checked, the hinge chain can inherit your root transform's velocity or position/rotation changes.")]
        public bool detachedShadow;
        public Transform parent;
        public bool hideShadow = true;
        public PhysicsSystem physicsSystem = PhysicsSystem.Physics3D;
        private GameObject shadowRoot;
        private readonly List<TransformPair> shadowTable = new List<TransformPair>();

        private static void DestroyComponents(Component[] components)
        {
            int index = 0;
            int length = components.Length;
            while (index < length)
            {
                UnityEngine.Object.Destroy(components[index]);
                index++;
            }
        }

        private void FixedUpdate()
        {
            if (this.physicsSystem == PhysicsSystem.Physics2D)
            {
                Rigidbody2D component = this.shadowRoot.GetComponent<Rigidbody2D>();
                component.MovePosition(base.transform.position);
                component.MoveRotation(base.transform.rotation.eulerAngles.z);
            }
            else
            {
                Rigidbody component = this.shadowRoot.GetComponent<Rigidbody>();
                component.MovePosition(base.transform.position);
                component.MoveRotation(base.transform.rotation);
            }
            int num = 0;
            int count = this.shadowTable.Count;
            while (num < count)
            {
                TransformPair pair = this.shadowTable[num];
                pair.dest.localPosition = pair.src.localPosition;
                pair.dest.localRotation = pair.src.localRotation;
                num++;
            }
        }

        private void Start()
        {
            this.shadowRoot = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
            UnityEngine.Object.Destroy(this.shadowRoot.GetComponent<SkeletonUtilityKinematicShadow>());
            Transform transform = this.shadowRoot.transform;
            transform.position = base.transform.position;
            transform.rotation = base.transform.rotation;
            Vector3 b = base.transform.TransformPoint(Vector3.right);
            float num = Vector3.Distance(base.transform.position, b);
            transform.localScale = Vector3.one;
            if (!this.detachedShadow)
            {
                if (this.parent == null)
                {
                    transform.parent = base.transform.root;
                }
                else
                {
                    transform.parent = this.parent;
                }
            }
            if (this.hideShadow)
            {
                this.shadowRoot.hideFlags = HideFlags.HideInHierarchy;
            }
            foreach (Joint joint in this.shadowRoot.GetComponentsInChildren<Joint>())
            {
                joint.connectedAnchor *= num;
            }
            SkeletonUtilityBone[] componentsInChildren = base.GetComponentsInChildren<SkeletonUtilityBone>();
            SkeletonUtilityBone[] components = this.shadowRoot.GetComponentsInChildren<SkeletonUtilityBone>();
            foreach (SkeletonUtilityBone bone in componentsInChildren)
            {
                if (bone.gameObject != base.gameObject)
                {
                    Type type = (this.physicsSystem != PhysicsSystem.Physics2D) ? typeof(Rigidbody) : typeof(Rigidbody2D);
                    foreach (SkeletonUtilityBone bone2 in components)
                    {
                        if ((bone2.GetComponent(type) != null) && (bone2.boneName == bone.boneName))
                        {
                            TransformPair item = new TransformPair {
                                dest = bone.transform,
                                src = bone2.transform
                            };
                            this.shadowTable.Add(item);
                            break;
                        }
                    }
                }
            }
            DestroyComponents(components);
            DestroyComponents(base.GetComponentsInChildren<Joint>());
            DestroyComponents(base.GetComponentsInChildren<Rigidbody>());
            DestroyComponents(base.GetComponentsInChildren<Collider>());
        }

        public enum PhysicsSystem
        {
            Physics2D,
            Physics3D
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TransformPair
        {
            public Transform dest;
            public Transform src;
        }
    }
}

