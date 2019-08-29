using System;
using UnityEngine;

public class BossZombie : ZombieHuman
{
    private static ParticleSystem deadFx;
    [SerializeField]
    public string myNameIs;

    protected override void Start()
    {
        base.Start();
        DataLoader.gui.RefreshBossHealth(base.maxCountHealth, base.countHealth, this.myNameIs);
    }

    public override int TakeDamage(int damage)
    {
        if (base.countHealth > 0)
        {
            base.TakeDamage(damage);
            if (base.countHealth <= 0)
            {
                if (GameManager.instance.currentGameMode == GameManager.GameModes.GamePlay)
                {
                    WavesManager.instance.BossDead();
                }
                if (GameManager.instance.currentGameMode == GameManager.GameModes.DailyBoss)
                {
                    WavesManager.instance.StopIt();
                }
                DataLoader.Instance.SaveKilledBoss(base.power, this.myNameIs);
                if (deadFx == null)
                {
                    deadFx = UnityEngine.Object.Instantiate<ParticleSystem>(GameManager.instance.prefabBossDeathFx);
                }
                deadFx.transform.position = new Vector3(base.transform.position.x, deadFx.transform.position.y, base.transform.position.z);
                deadFx.Play();
            }
            DataLoader.gui.RefreshBossHealth(base.maxCountHealth, base.countHealth, this.myNameIs);
        }
        return base.countHealth;
    }
}

