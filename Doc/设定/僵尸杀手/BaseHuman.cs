using System;
using UnityEngine;

public class BaseHuman : MonoBehaviour
{
    protected Rigidbody rigid;
    protected Transform targetMove;
    protected Transform targetRotation;
    [SerializeField]
    public Transform body;
    [SerializeField]
    public Animator animator;
    [SerializeField]
    protected Transform healthBar;
    public int countHealth = 100;
    [HideInInspector]
    public int maxCountHealth;
    protected bool isLookAtTarget;
    private float moveForce = 5f;
    private Vector3 previousPosition;
    private int framesStay;
    private Vector3 previousTargetPos;
    private int framesTargetStay;
    protected ParticleSystem takeDamageFx;
    protected bool isMoving;
    [SerializeField]
    protected AudioClip[] deathSounds;

    private void Awake()
    {
        this.rigid = base.GetComponent<Rigidbody>();
        this.maxCountHealth = this.countHealth;
        this.previousPosition = base.transform.position;
        this.HideHpbar();
    }

    private void CalculateBodyRotation()
    {
        this.isLookAtTarget = false;
        if (this.targetRotation != null)
        {
            Vector3 from = this.targetRotation.position - base.transform.position;
            if ((this.rigid.velocity == Vector3.zero) && (this.targetRotation == this.targetMove))
            {
                from = -from;
            }
            float b = Vector3.Angle(from, base.transform.forward);
            if (Vector3.Angle(from, base.transform.right) > 90f)
            {
                b *= -1f;
            }
            this.body.transform.eulerAngles = new Vector3(0f, Mathf.LerpAngle(this.body.transform.eulerAngles.y, b, 0.5f), 0f);
            if (Mathf.Abs(Mathf.DeltaAngle(this.body.transform.eulerAngles.y, b)) < 2f)
            {
                this.isLookAtTarget = true;
            }
        }
    }

    protected void CheckMoving()
    {
        if (Vector3.Distance(this.previousPosition, base.transform.position) < 0.1f)
        {
            if (this.framesStay < 3)
            {
                this.framesStay++;
            }
            if (this.framesStay >= 3)
            {
                this.isMoving = false;
                SoundManager.Instance.PlayStepsSound(false);
            }
        }
        else
        {
            if (this.framesStay > 0)
            {
                this.framesStay--;
            }
            if (this.framesStay <= 0)
            {
                this.isMoving = true;
                SoundManager.Instance.PlayStepsSound(true);
            }
        }
        this.previousPosition = base.transform.position;
    }

    protected virtual void FixedUpdate()
    {
        this.CheckMoving();
        if (!this.isMoving && !this.IsTargetMoving())
        {
            this.rigid.velocity = Vector3.zero;
        }
        else
        {
            this.rigid.velocity = Vector3.MoveTowards(Vector3.zero, this.targetMove.transform.position - base.transform.position, 1f) * this.moveForce;
        }
        if (this.animator != null)
        {
            if (!this.isMoving)
            {
                this.animator.SetBool("Run", false);
            }
            else
            {
                this.animator.SetBool("Run", true);
            }
        }
    }

    protected void HideHpbar()
    {
        if (this.healthBar != null)
        {
            this.healthBar.gameObject.SetActive(false);
        }
    }

    protected bool IsTargetMoving()
    {
        if (Vector3.Distance(this.previousTargetPos, this.targetMove.position) < 0.1f)
        {
            if (this.framesTargetStay < 3)
            {
                this.framesTargetStay++;
            }
        }
        else if (this.framesTargetStay > 0)
        {
            this.framesTargetStay--;
        }
        this.previousTargetPos = this.targetMove.position;
        if (this.framesTargetStay >= 3)
        {
            return false;
        }
        return true;
    }

    public virtual int TakeDamage(int damage)
    {
        if (this.countHealth > 0)
        {
            this.countHealth -= damage;
            if (this.healthBar != null)
            {
                base.CancelInvoke("HideHpbar");
                this.healthBar.gameObject.SetActive(true);
                base.Invoke("HideHpbar", 2f);
                this.healthBar.localScale = new Vector3(((float) this.countHealth) / ((float) this.maxCountHealth), this.healthBar.localScale.y, this.healthBar.localScale.z);
            }
            if (((damage > 0) && (this.takeDamageFx != null)) && !this.takeDamageFx.isPlaying)
            {
                this.takeDamageFx.Play();
            }
        }
        return this.countHealth;
    }

    protected virtual void Update()
    {
        this.CalculateBodyRotation();
    }
}

