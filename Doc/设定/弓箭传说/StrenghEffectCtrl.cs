using System;
using UnityEngine;

public class StrenghEffectCtrl : MonoBehaviour
{
    public ParticleSystem particle;
    private ParticleSystem.ShapeModule shape;
    private SkinnedMeshRenderer mesh;

    public void InitMesh(EntityBase entity)
    {
        this.shape = this.particle.shape;
        this.mesh = entity.m_Body.Body.GetComponent<SkinnedMeshRenderer>();
        this.shape.mesh = this.mesh.sharedMesh;
    }
}

