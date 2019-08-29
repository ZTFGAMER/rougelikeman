using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BossSpawnManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static BossSpawnManager <instance>k__BackingField;
    public Camera cam;
    private LayerMask groundLayerMask;
    private LayerMask obstacleLayerMask;
    private Vector3[,] viewportPoints = new Vector3[2, 2];
    [SerializeField]
    private GameObject bossSpawnFxPrefab;
    [SerializeField]
    private bool testMode;

    private void Awake()
    {
        instance = this;
    }

    public bool GetSpawnPosition(out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;
        *(this.viewportPoints[0, 0]) = this.GetViewportPoint(0f, 0f);
        *(this.viewportPoints[1, 1]) = this.GetViewportPoint(1f, 1f);
        for (float i = 0.65f; i > 0f; i -= 0.05f)
        {
            if ((i <= 0.05f) || (i >= 0.45f))
            {
                for (float j = 0f; j < 1f; j += 0.05f)
                {
                    spawnPosition = new Vector3(this.GetXPercentage(j, this.viewportPoints[0, 0], this.viewportPoints[1, 1]), 0f, this.GetZPercentage(i, this.viewportPoints[0, 0], this.viewportPoints[1, 1]));
                    if (Physics.OverlapSphere(spawnPosition, 0.5f, (int) this.obstacleLayerMask, QueryTriggerInteraction.Collide).Length == 0)
                    {
                        Vector3 vector = new Vector3(spawnPosition.x, 1f, spawnPosition.z) - new Vector3(base.transform.position.x, 1f, base.transform.position.z);
                        Vector3 direction = vector / vector.magnitude;
                        float maxDistance = Vector3.Distance(new Vector3(spawnPosition.x, 1f, spawnPosition.z), new Vector3(base.transform.position.x, 1f, base.transform.position.z));
                        Ray ray = new Ray(new Vector3(base.transform.position.x, 1f, base.transform.position.z), direction);
                        if (!Physics.Raycast(ray, maxDistance, (int) this.obstacleLayerMask) && (Vector3.Distance(base.transform.position, spawnPosition) > 2.5f))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public Vector3 GetViewportPoint(float x, float y)
    {
        Ray ray = this.cam.ViewportPointToRay(new Vector3(x, y, 0f));
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100f, (int) this.groundLayerMask) && (hit.transform.tag == "Ground"))
        {
            return hit.point;
        }
        UnityEngine.Debug.LogError("Cant find Ground");
        return Vector3.zero;
    }

    public float GetXPercentage(float percentage, Vector3 min, Vector3 max)
    {
        float num = Vector3.Distance(max, min);
        return (min.x + (num * percentage));
    }

    public float GetZPercentage(float percentage, Vector3 min, Vector3 max)
    {
        float num = Vector3.Distance(max, min);
        return (min.z + (num * percentage));
    }

    public void SpawnFx(Vector3 position)
    {
        UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.bossSpawnFxPrefab, position, Quaternion.identity, TransformParentManager.Instance.fx), 3f);
    }

    private void Start()
    {
        this.groundLayerMask = 0x10000;
        this.obstacleLayerMask = 0x1010;
        if (this.testMode)
        {
            base.StartCoroutine(this.TestCoroutine());
        }
    }

    [DebuggerHidden]
    public IEnumerator TestCoroutine() => 
        new <TestCoroutine>c__Iterator0 { $this = this };

    public static BossSpawnManager instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <TestCoroutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Vector3 <spawnVector>__1;
        internal BossSpawnManager $this;
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
                    break;

                case 1:
                    if (this.$this.GetSpawnPosition(out this.<spawnVector>__1))
                    {
                        this.$this.SpawnFx(this.<spawnVector>__1);
                    }
                    break;

                default:
                    return false;
            }
            this.$current = new WaitForSeconds(0.5f);
            if (!this.$disposing)
            {
                this.$PC = 1;
            }
            return true;
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
}

