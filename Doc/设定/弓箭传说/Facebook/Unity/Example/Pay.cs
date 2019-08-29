namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using UnityEngine;

    internal class Pay : MenuBase
    {
        private string payProduct = string.Empty;

        private void CallFBPay()
        {
            int? quantityMin = null;
            FB.Canvas.Pay(this.payProduct, "purchaseitem", 1, quantityMin, null, null, null, null, new FacebookDelegate<IPayResult>(this.HandleResult));
        }

        protected override void GetGui()
        {
            base.LabelAndTextField("Product: ", ref this.payProduct);
            if (base.Button("Call Pay"))
            {
                this.CallFBPay();
            }
            GUILayout.Space(10f);
        }
    }
}

