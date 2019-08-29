using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSurvivorPointer : MonoBehaviour
{
    [SerializeField]
    private Transform arrow;
    [SerializeField]
    private RectTransform CanvasRect;
    [SerializeField]
    private Text distanceText;
    [SerializeField]
    public float showDistance = 20f;
    private Animation showPointerAnim;
    private RectTransform pointerImage;
    private Transform cameraTarget;
    private float borderSpace;

    private void FixedUpdate()
    {
        List<Vector3> list = new List<Vector3>();
        foreach (SurvivorSpawn spawn in SpawnManager.instance.survivorSpawns)
        {
            if (spawn.newSurvivor != null)
            {
                list.Add(spawn.transform.position);
            }
        }
        if (list.Count <= 0)
        {
            this.pointerImage.gameObject.SetActive(false);
            this.distanceText.enabled = false;
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i; j < list.Count; j++)
                {
                    if (Vector3.Distance(this.cameraTarget.position, list[j]) < Vector3.Distance(this.cameraTarget.position, list[i]))
                    {
                        Vector3 vector = list[j];
                        list[j] = list[i];
                        list[i] = vector;
                    }
                }
            }
            if (Vector3.Distance(this.cameraTarget.position, list[0]) <= this.showDistance)
            {
                if (!this.pointerImage.gameObject.activeSelf && (PlayerPrefs.HasKey(StaticConstants.AbilityTutorialCompleted) || GameManager.instance.isTutorialNow))
                {
                    this.pointerImage.gameObject.SetActive(true);
                    this.showPointerAnim.Play();
                    base.Invoke("ShowDistance", 0.5f);
                }
                Vector2 vector2 = Camera.main.WorldToViewportPoint(list[0]);
                vector2.x -= 0.5f;
                vector2.y -= 0.5f;
                Vector2 from = new Vector2(vector2.x * this.CanvasRect.sizeDelta.x, vector2.y * this.CanvasRect.sizeDelta.y);
                Vector3 vector6 = list[0];
                if (((vector6.z - this.cameraTarget.position.z) < 0f) && (from.y > 0f))
                {
                    from *= -1f;
                }
                if ((Mathf.Abs(from.x) > ((this.CanvasRect.sizeDelta.x / 2f) - this.borderSpace)) || (Mathf.Abs(from.y) > ((this.CanvasRect.sizeDelta.y / 2f) - this.borderSpace)))
                {
                    float introduced21 = Mathf.Abs((float) (from.x / ((this.CanvasRect.sizeDelta.x / 2f) - this.borderSpace)));
                    if (introduced21 > Mathf.Abs((float) (from.y / ((this.CanvasRect.sizeDelta.y / 2f) - this.borderSpace))))
                    {
                        float x = from.x;
                        from.x = Mathf.Sign(from.x) * ((this.CanvasRect.sizeDelta.x / 2f) - this.borderSpace);
                        from.y *= Mathf.Abs((float) (from.x / x));
                    }
                    else
                    {
                        float y = from.y;
                        from.y = Mathf.Sign(from.y) * ((this.CanvasRect.sizeDelta.y / 2f) - this.borderSpace);
                        from.x *= Mathf.Abs((float) (from.y / y));
                    }
                    this.arrow.gameObject.SetActive(true);
                    float z = Vector2.Angle(from, new Vector2(1f, 0f));
                    if (Vector2.Angle(from, new Vector2(0f, 1f)) > 90f)
                    {
                        z *= -1f;
                    }
                    this.arrow.eulerAngles = new Vector3(0f, 0f, z);
                    this.distanceText.text = ((int) Vector3.Distance(this.cameraTarget.position, list[0])) + "m";
                }
                else
                {
                    this.arrow.gameObject.SetActive(false);
                }
                this.pointerImage.anchoredPosition = from;
            }
            else
            {
                this.pointerImage.gameObject.SetActive(false);
            }
        }
    }

    private void ShowDistance()
    {
        this.distanceText.enabled = true;
    }

    private void Start()
    {
        this.cameraTarget = UnityEngine.Object.FindObjectOfType<CameraTarget>().transform;
        this.pointerImage = this.arrow.parent.GetComponent<RectTransform>();
        this.borderSpace = Mathf.Max(this.pointerImage.sizeDelta.x, this.pointerImage.sizeDelta.y) / 2f;
        this.showPointerAnim = this.pointerImage.GetComponent<Animation>();
    }
}

