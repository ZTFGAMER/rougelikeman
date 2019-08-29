namespace BestHTTP.Forms
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class HTTPFieldData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FileName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <MimeType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Text.Encoding <Encoding>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Text>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] <Binary>k__BackingField;

        public string Name { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }

        public System.Text.Encoding Encoding { get; set; }

        public string Text { get; set; }

        public byte[] Binary { get; set; }

        public byte[] Payload
        {
            get
            {
                if (this.Binary != null)
                {
                    return this.Binary;
                }
                if (this.Encoding == null)
                {
                    this.Encoding = System.Text.Encoding.UTF8;
                }
                byte[] bytes = this.Encoding.GetBytes(this.Text);
                this.Binary = bytes;
                return bytes;
            }
        }
    }
}

