using System;
using UnityEngine;

public class MoneyCoin : MonoBehaviour
{
    private bool complete;
    [SerializeField]
    private ParticleSystem pickUpFx;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Survivor") && !this.complete)
        {
            DataLoader.Instance.UpdateInGameMoneyCounter(MoneyCoinManager.instance.coinMoney);
            UnityEngine.Object.Instantiate<TextMesh>(GameManager.instance.floatingTextPrefab, base.transform.position, Quaternion.identity, TransformParentManager.Instance.fx).text = MoneyCoinManager.instance.coinMoney.ToString();
            this.pickUpFx.transform.parent = null;
            this.pickUpFx.Play();
            UnityEngine.Object.Destroy(this.pickUpFx.gameObject, 1f);
            UnityEngine.Object.Destroy(base.gameObject);
            this.complete = true;
        }
    }
}

