namespace LitJson
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct ArrayMetadata
    {
        private Type element_type;
        private bool is_array;
        private bool is_list;
        public Type ElementType
        {
            get
            {
                if (this.element_type == null)
                {
                    return typeof(JsonData);
                }
                return this.element_type;
            }
            set => 
                (this.element_type = value);
        }
        public bool IsArray
        {
            get => 
                this.is_array;
            set => 
                (this.is_array = value);
        }
        public bool IsList
        {
            get => 
                this.is_list;
            set => 
                (this.is_list = value);
        }
    }
}

