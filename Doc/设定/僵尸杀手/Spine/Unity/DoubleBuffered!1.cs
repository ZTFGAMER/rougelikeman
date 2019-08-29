namespace Spine.Unity
{
    using System;

    public class DoubleBuffered<T> where T: new()
    {
        private readonly T a;
        private readonly T b;
        private bool usingA;

        public DoubleBuffered()
        {
            this.a = Activator.CreateInstance<T>();
            this.b = Activator.CreateInstance<T>();
        }

        public T GetCurrent() => 
            (!this.usingA ? this.b : this.a);

        public T GetNext()
        {
            this.usingA = !this.usingA;
            return (!this.usingA ? this.b : this.a);
        }
    }
}

