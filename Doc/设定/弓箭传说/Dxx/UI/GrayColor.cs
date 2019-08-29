namespace Dxx.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [ExecuteInEditMode, AddComponentMenu("UI/Effects/Gray Color"), RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class GrayColor : UIBehaviour
    {
        public Material hueMaterial;
        private Graphic m_Graphic;

        protected override void OnDisable()
        {
            base.OnDisable();
            this.graphic.set_material((Material) null);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.graphic.set_material(this.hueMaterial);
            this.graphic.get_material().SetColor("_Color", this.graphic.get_color());
        }

        public Graphic graphic
        {
            get
            {
                if (this.m_Graphic == null)
                {
                    this.m_Graphic = base.GetComponent<Graphic>();
                }
                return this.m_Graphic;
            }
        }
    }
}

