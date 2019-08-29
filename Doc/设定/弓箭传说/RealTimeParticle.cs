using System;
using UnityEngine;

public class RealTimeParticle : MonoBehaviour
{
    [SerializeField]
    private bool withChildren = true;
    private ParticleSystem _particle;
    private ParticleSystem[] _particles;
    private int _particles_count;
    private float _deltaTime;
    private float _timeAtLastFrame;

    private void Awake()
    {
        this._particle = base.GetComponent<ParticleSystem>();
        if (this.withChildren)
        {
            this._particles = base.GetComponentsInChildren<ParticleSystem>();
            this._particles_count = this._particles.Length;
        }
    }

    private void Update()
    {
        this._deltaTime = Time.realtimeSinceStartup - this._timeAtLastFrame;
        this._timeAtLastFrame = Time.realtimeSinceStartup;
        if (Mathf.Abs(Time.timeScale) < 1E-06)
        {
            if (this.withChildren)
            {
                for (int i = 0; i < this._particles_count; i++)
                {
                    this._particles[i].Simulate(this._deltaTime, false, false);
                    this._particles[i].Play();
                }
            }
            else if (this._particle != null)
            {
                this._particle.Simulate(this._deltaTime, false, false);
                this._particle.Play();
            }
        }
    }
}

