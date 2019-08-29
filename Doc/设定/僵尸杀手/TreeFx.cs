using System;
using UnityEngine;

public class TreeFx : MonoBehaviour
{
    private ParticleSystem particles;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Survivor") && !this.particles.isPlaying)
        {
            this.particles.Play();
        }
    }

    private void Start()
    {
        this.particles = base.GetComponentInChildren<ParticleSystem>();
    }
}

