namespace Dxx.Util
{
    using System;

    public class AttackCallData : WeightRandomDataBase
    {
        public float hitratio;

        public AttackCallData(int bulletid, float hitratio, int weight) : base(bulletid)
        {
            this.hitratio = hitratio;
            base.weight = weight;
        }

        public override string ToString()
        {
            object[] args = new object[] { base.id, base.weight };
            return Utils.FormatString("DeadCallData id:{0}, weight:{1}", args);
        }
    }
}

