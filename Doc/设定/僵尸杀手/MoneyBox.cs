using System;
using UnityEngine;

public class MoneyBox : MonoBehaviour
{
    [HideInInspector]
    public bool isBigCoin;
    private bool complete;
    [SerializeField]
    private ParticleSystem pickUpFx;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Survivor") && !this.complete)
        {
            if (!this.isBigCoin)
            {
                DataLoader.Instance.SavePickedMoneyBox(base.transform.position);
            }
            else
            {
                DataLoader.Instance.UpdateInGameMoneyCounter(MoneyCoinManager.instance.coinMoney * 10.0);
                UnityEngine.Object.Instantiate<TextMesh>(GameManager.instance.floatingTextPrefab, base.transform.position, Quaternion.identity, TransformParentManager.Instance.fx).text = (MoneyCoinManager.instance.coinMoney * 10.0).ToString();
            }
            this.pickUpFx.transform.parent = null;
            this.pickUpFx.Play();
            UnityEngine.Object.Destroy(this.pickUpFx.gameObject, 1f);
            UnityEngine.Object.Destroy(base.gameObject);
            this.complete = true;
        }
    }
}

