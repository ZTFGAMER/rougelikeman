namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using UnityEngine;

    [RequireComponent(typeof(SkeletonRenderer))]
    public class SkeletonGhost : MonoBehaviour
    {
        private const HideFlags GhostHideFlags = HideFlags.HideInHierarchy;
        private const string GhostingShaderName = "Spine/Special/SkeletonGhost";
        public bool ghostingEnabled = true;
        public float spawnRate = 0.05f;
        public Color32 color = new Color32(0xff, 0xff, 0xff, 0);
        [Tooltip("Remember to set color alpha to 0 if Additive is true")]
        public bool additive = true;
        public int maximumGhosts = 10;
        public float fadeSpeed = 10f;
        public Shader ghostShader;
        [Tooltip("0 is Color and Alpha, 1 is Alpha only."), Range(0f, 1f)]
        public float textureFade = 1f;
        [Header("Sorting")]
        public bool sortWithDistanceOnly;
        public float zOffset;
        private float nextSpawnTime;
        private SkeletonGhostRenderer[] pool;
        private int poolIndex;
        private SkeletonRenderer skeletonRenderer;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private readonly Dictionary<Material, Material> materialTable = new Dictionary<Material, Material>();

        private void Ghosting(float val)
        {
            this.ghostingEnabled = val > 0f;
        }

        private static Color32 HexToColor(string hex)
        {
            if (hex.Length < 6)
            {
                return Color.magenta;
            }
            hex = hex.Replace("#", string.Empty);
            byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            byte a = 0xff;
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }

        private void OnDestroy()
        {
            if (this.pool != null)
            {
                for (int i = 0; i < this.maximumGhosts; i++)
                {
                    if (this.pool[i] != null)
                    {
                        this.pool[i].Cleanup();
                    }
                }
            }
            foreach (Material material in this.materialTable.Values)
            {
                UnityEngine.Object.Destroy(material);
            }
        }

        private void OnEvent(TrackEntry trackEntry, Event e)
        {
            if (e.Data.Name.Equals("Ghosting", StringComparison.Ordinal))
            {
                this.ghostingEnabled = e.Int > 0;
                if (e.Float > 0f)
                {
                    this.spawnRate = e.Float;
                }
                if (!string.IsNullOrEmpty(e.stringValue))
                {
                    this.color = HexToColor(e.String);
                }
            }
        }

        private void Start()
        {
            if (this.ghostShader == null)
            {
                this.ghostShader = Shader.Find("Spine/Special/SkeletonGhost");
            }
            this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
            this.meshFilter = base.GetComponent<MeshFilter>();
            this.meshRenderer = base.GetComponent<MeshRenderer>();
            this.nextSpawnTime = Time.time + this.spawnRate;
            this.pool = new SkeletonGhostRenderer[this.maximumGhosts];
            for (int i = 0; i < this.maximumGhosts; i++)
            {
                Type[] components = new Type[] { typeof(SkeletonGhostRenderer) };
                GameObject obj2 = new GameObject(base.gameObject.name + " Ghost", components);
                this.pool[i] = obj2.GetComponent<SkeletonGhostRenderer>();
                obj2.SetActive(false);
                obj2.hideFlags = HideFlags.HideInHierarchy;
            }
            IAnimationStateComponent skeletonRenderer = this.skeletonRenderer as IAnimationStateComponent;
            if (skeletonRenderer != null)
            {
                skeletonRenderer.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.OnEvent);
            }
        }

        private void Update()
        {
            if (this.ghostingEnabled && (Time.time >= this.nextSpawnTime))
            {
                GameObject gameObject = this.pool[this.poolIndex].gameObject;
                Material[] sharedMaterials = this.meshRenderer.sharedMaterials;
                for (int i = 0; i < sharedMaterials.Length; i++)
                {
                    Material material2;
                    Material key = sharedMaterials[i];
                    if (!this.materialTable.ContainsKey(key))
                    {
                        material2 = new Material(key) {
                            shader = this.ghostShader,
                            color = Color.white
                        };
                        if (material2.HasProperty("_TextureFade"))
                        {
                            material2.SetFloat("_TextureFade", this.textureFade);
                        }
                        this.materialTable.Add(key, material2);
                    }
                    else
                    {
                        material2 = this.materialTable[key];
                    }
                    sharedMaterials[i] = material2;
                }
                Transform transform = gameObject.transform;
                transform.parent = base.transform;
                this.pool[this.poolIndex].Initialize(this.meshFilter.sharedMesh, sharedMaterials, this.color, this.additive, this.fadeSpeed, this.meshRenderer.sortingLayerID, !this.sortWithDistanceOnly ? (this.meshRenderer.sortingOrder - 1) : this.meshRenderer.sortingOrder);
                transform.localPosition = new Vector3(0f, 0f, this.zOffset);
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                transform.parent = null;
                this.poolIndex++;
                if (this.poolIndex == this.pool.Length)
                {
                    this.poolIndex = 0;
                }
                this.nextSpawnTime = Time.time + this.spawnRate;
            }
        }
    }
}

