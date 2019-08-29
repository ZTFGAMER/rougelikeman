namespace Spine.Unity.Examples
{
    using System;
    using UnityEngine;

    public class MaterialPropertyBlockExample : MonoBehaviour
    {
        public float timeInterval = 1f;
        public Gradient randomColors = new Gradient();
        public string colorPropertyName = "_FillColor";
        private MaterialPropertyBlock mpb;
        private float timeToNextColor;

        private void Start()
        {
            this.mpb = new MaterialPropertyBlock();
        }

        private void Update()
        {
            if (this.timeToNextColor <= 0f)
            {
                this.timeToNextColor = this.timeInterval;
                Color color = this.randomColors.Evaluate(UnityEngine.Random.value);
                this.mpb.SetColor(this.colorPropertyName, color);
                base.GetComponent<MeshRenderer>().SetPropertyBlock(this.mpb);
            }
            this.timeToNextColor -= Time.deltaTime;
        }
    }
}

