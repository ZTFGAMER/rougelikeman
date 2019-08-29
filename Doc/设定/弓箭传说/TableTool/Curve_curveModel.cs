namespace TableTool
{
    using System;
    using UnityEngine;

    public class Curve_curveModel : LocalModel<Curve_curve, int>
    {
        private const string _Filename = "Curve_curve";

        protected override int GetBeanKey(Curve_curve bean) => 
            bean.ID;

        public AnimationCurve GetCurve(int id) => 
            base.GetBeanById(id).GetCurve();

        public AnimationCurve GetSin() => 
            this.GetCurve(0x30d42);

        protected override string Filename =>
            "Curve_curve";
    }
}

