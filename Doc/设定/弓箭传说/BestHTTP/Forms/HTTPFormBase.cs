namespace BestHTTP.Forms
{
    using BestHTTP;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class HTTPFormBase
    {
        private const int LongLength = 0x100;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<HTTPFieldData> <Fields>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsChanged>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <HasBinary>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <HasLongValue>k__BackingField;

        public void AddBinaryData(string fieldName, byte[] content)
        {
            this.AddBinaryData(fieldName, content, null, null);
        }

        public void AddBinaryData(string fieldName, byte[] content, string fileName)
        {
            this.AddBinaryData(fieldName, content, fileName, null);
        }

        public void AddBinaryData(string fieldName, byte[] content, string fileName, string mimeType)
        {
            if (this.Fields == null)
            {
                this.Fields = new List<HTTPFieldData>();
            }
            HTTPFieldData item = new HTTPFieldData {
                Name = fieldName
            };
            if (fileName == null)
            {
                item.FileName = fieldName + ".dat";
            }
            else
            {
                item.FileName = fileName;
            }
            if (mimeType == null)
            {
                item.MimeType = "application/octet-stream";
            }
            else
            {
                item.MimeType = mimeType;
            }
            item.Binary = content;
            this.Fields.Add(item);
            bool flag = true;
            this.IsChanged = flag;
            this.HasBinary = flag;
        }

        public void AddField(string fieldName, string value)
        {
            this.AddField(fieldName, value, Encoding.UTF8);
        }

        public void AddField(string fieldName, string value, Encoding e)
        {
            if (this.Fields == null)
            {
                this.Fields = new List<HTTPFieldData>();
            }
            HTTPFieldData item = new HTTPFieldData {
                Name = fieldName,
                FileName = null
            };
            if (e != null)
            {
                item.MimeType = "text/plain; charset=" + e.WebName;
            }
            item.Text = value;
            item.Encoding = e;
            this.Fields.Add(item);
            this.IsChanged = true;
            this.HasLongValue |= value.Length > 0x100;
        }

        public virtual void CopyFrom(HTTPFormBase fields)
        {
            this.Fields = new List<HTTPFieldData>(fields.Fields);
            this.IsChanged = true;
            this.HasBinary = fields.HasBinary;
            this.HasLongValue = fields.HasLongValue;
        }

        public virtual byte[] GetData()
        {
            throw new NotImplementedException();
        }

        public virtual void PrepareRequest(HTTPRequest request)
        {
            throw new NotImplementedException();
        }

        public List<HTTPFieldData> Fields { get; set; }

        public bool IsEmpty =>
            ((this.Fields == null) || (this.Fields.Count == 0));

        public bool IsChanged { get; protected set; }

        public bool HasBinary { get; protected set; }

        public bool HasLongValue { get; protected set; }
    }
}

