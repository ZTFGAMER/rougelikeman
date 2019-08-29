using System;
using UnityEngine;

public class NewSurvivor : MonoBehaviour
{
    public SaveData.HeroData.HeroType heroType;
    private SurvivorHuman survivor;
    private Collider survivorCollider;
    private bool complete;
    [SerializeField]
    private AudioClip takeSound;

    private void OnTriggerEnter(Collider other)
    {
        if (((other.tag == "Survivor") && !this.complete) && (other.gameObject != this.survivor.gameObject))
        {
            this.survivor.transform.parent = null;
            this.survivor.enabled = true;
            this.survivorCollider.enabled = true;
            this.survivor.PlayWakeUpFx();
            SoundManager.Instance.PlaySound(this.takeSound, -1f);
            UnityEngine.Object.Destroy(base.gameObject);
            this.complete = true;
            GameManager.instance.newSurvivorsLeft--;
            DataLoader.Instance.AddPickedUpCount(this.heroType);
        }
    }

    private void Start()
    {
        this.survivor = UnityEngine.Object.Instantiate<SurvivorHuman>(DataLoader.Instance.GetSurvivorPrefab(this.heroType), base.transform).GetComponent<SurvivorHuman>();
        this.survivor.transform.localPosition = new Vector3(0f, 0f, -2.5f);
        this.survivor.body.rotation = new Quaternion(0f, 0.5f, 0f, 0f);
        this.survivor.enabled = false;
        this.survivorCollider = this.survivor.GetComponent<Collider>();
        this.survivorCollider.enabled = false;
    }
}

