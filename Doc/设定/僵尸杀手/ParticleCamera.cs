using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParticleCamera : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static ParticleCamera <Instance>k__BackingField;
    public Camera camera;
    private RectTransform canvas;
    private float height;
    private float width;
    private int particleCount;

    private void Awake()
    {
        if ((Instance != null) && (Instance != this))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            Instance = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            this.GetObjects();
        }
    }

    public void DecreaseParticles()
    {
        this.particleCount--;
        if (this.particleCount <= 0)
        {
            this.particleCount = 0;
        }
    }

    public Vector3 GetCameraPosition(Vector3 position)
    {
        Vector3 vector = Camera.main.ScreenToViewportPoint(position);
        Vector2 vector2 = new Vector2((vector.x * this.canvas.sizeDelta.x) - (this.canvas.sizeDelta.x / 2f), (vector.y * this.canvas.sizeDelta.y) - (this.canvas.sizeDelta.y / 2f));
        return new Vector3(base.transform.position.x + ((vector2.x * this.width) / this.canvas.sizeDelta.x), base.transform.position.y + ((vector2.y * this.height) / this.canvas.sizeDelta.y), base.transform.position.z + 10f);
    }

    public void GetObjects()
    {
        this.camera = base.GetComponent<Camera>();
        GUI gui = UnityEngine.Object.FindObjectOfType<GUI>();
        this.canvas = gui.GetComponent<RectTransform>();
        RenderTexture texture = new RenderTexture((int) this.canvas.sizeDelta.x, (int) this.canvas.sizeDelta.y, 1);
        this.camera.targetTexture = texture;
        gui.rawImage.set_texture(texture);
        this.height = 2f * this.camera.orthographicSize;
        this.width = this.height * this.camera.aspect;
    }

    public void Play(float playTime)
    {
        this.particleCount++;
        base.Invoke("DecreaseParticles", playTime);
    }

    public static ParticleCamera Instance
    {
        [CompilerGenerated]
        get => 
            <Instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<Instance>k__BackingField = value);
    }
}

