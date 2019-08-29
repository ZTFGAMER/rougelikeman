namespace Spine
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public abstract class Attachment
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;

        protected Attachment(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", "name cannot be null");
            }
            this.Name = name;
        }

        public override string ToString() => 
            this.Name;

        public string Name { get; private set; }
    }
}

