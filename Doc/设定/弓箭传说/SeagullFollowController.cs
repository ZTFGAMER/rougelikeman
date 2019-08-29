using System;
using UnityEngine;

public class SeagullFollowController : MonoBehaviour
{
    public float speed = 1f;
    public float dampTime = 2f;
    public Transform targetTransform;
    public float scatter;
    public float scatterSpeed = 1f;
    public float orthoCamScale = 1f;
    public bool isUseOffset;
    public Vector3 offset;
    private Animator animator;
    private int animFacingParam = Animator.StringToHash("Facing");

    private void Start()
    {
        this.animator = base.GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 position = this.targetTransform.position;
        if (this.isUseOffset)
        {
            position += this.offset * this.orthoCamScale;
        }
        position.x += this.scatter * Mathf.Sin(Time.time * this.scatterSpeed);
        Vector3 upwards = position - base.transform.position;
        Quaternion b = Quaternion.LookRotation(Vector3.forward, upwards);
        base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, this.dampTime * Time.deltaTime);
        Transform transform = base.transform;
        transform.position += ((base.transform.up * this.speed) * this.orthoCamScale) * Time.deltaTime;
        this.animator.SetFloat(this.animFacingParam, Vector3.Dot(base.transform.up, Vector3.down));
    }
}

