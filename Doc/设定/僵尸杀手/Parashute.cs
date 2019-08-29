using System;
using UnityEngine;

public class Parashute : MonoBehaviour
{
    private SurvivorHuman survivor;
    private Collider survivorCollider;
    private Rigidbody survivorRigidbody;
    private Rigidbody rigid;
    private CameraTarget cameraTarget;
    [SerializeField]
    private Transform survivorPlace;
    [SerializeField]
    private float speed = 10f;

    private void FixedUpdate()
    {
        if (base.transform.position.y <= 0f)
        {
            this.survivor.transform.parent = null;
            this.survivor.enabled = true;
            this.survivor.transform.rotation = new Quaternion();
            this.survivorCollider.enabled = true;
            this.survivorRigidbody.isKinematic = false;
            UnityEngine.Object.Destroy(base.gameObject);
        }
        this.rigid.velocity = Vector3.MoveTowards(Vector3.zero, (this.cameraTarget.transform.position - base.transform.position) * 10000f, 1f) * this.speed;
        float y = base.transform.eulerAngles.y;
        base.transform.LookAt(this.cameraTarget.transform);
        base.transform.eulerAngles = new Vector3(0f, Mathf.Lerp(y, base.transform.eulerAngles.y, 0.5f), 0f);
    }

    private void Start()
    {
        this.rigid = base.GetComponent<Rigidbody>();
        this.cameraTarget = UnityEngine.Object.FindObjectOfType<CameraTarget>();
        this.survivor = UnityEngine.Object.Instantiate<SurvivorHuman>(DataLoader.Instance.GetSurvivorPrefab(SpawnManager.instance.GetRandomSurvivorType()), this.survivorPlace).GetComponent<SurvivorHuman>();
        this.survivor.transform.localPosition = Vector3.zero;
        this.survivor.animator.SetBool("Rest", false);
        this.survivor.body.rotation = new Quaternion();
        this.survivor.enabled = false;
        this.survivorCollider = this.survivor.GetComponent<Collider>();
        this.survivorCollider.enabled = false;
        this.survivorRigidbody = this.survivor.GetComponent<Rigidbody>();
        this.survivorRigidbody.isKinematic = true;
    }
}

