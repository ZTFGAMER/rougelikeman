namespace Spine.Unity.Examples
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class EquipButtonExample : MonoBehaviour
    {
        public EquipAssetExample asset;
        public EquipSystemExample equipSystem;
        public Image inventoryImage;

        private void MatchImage()
        {
            if (this.inventoryImage != null)
            {
                this.inventoryImage.set_sprite(this.asset.sprite);
            }
        }

        private void OnValidate()
        {
            this.MatchImage();
        }

        private void Start()
        {
            this.MatchImage();
            base.GetComponent<Button>().onClick.AddListener(() => this.equipSystem.Equip(this.asset));
        }
    }
}

