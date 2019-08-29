namespace BestHTTP.Forms
{
    using BestHTTP;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class UnityForm : HTTPFormBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private WWWForm <Form>k__BackingField;

        public UnityForm()
        {
        }

        public UnityForm(WWWForm form)
        {
            this.Form = form;
        }

        public override void CopyFrom(HTTPFormBase fields)
        {
            base.Fields = fields.Fields;
            base.IsChanged = true;
            if (this.Form == null)
            {
                this.Form = new WWWForm();
                if (base.Fields != null)
                {
                    for (int i = 0; i < base.Fields.Count; i++)
                    {
                        HTTPFieldData data = base.Fields[i];
                        if (string.IsNullOrEmpty(data.Text) && (data.Binary != null))
                        {
                            this.Form.AddBinaryData(data.Name, data.Binary, data.FileName, data.MimeType);
                        }
                        else
                        {
                            this.Form.AddField(data.Name, data.Text, data.Encoding);
                        }
                    }
                }
            }
        }

        public override byte[] GetData() => 
            this.Form.get_data();

        public override void PrepareRequest(HTTPRequest request)
        {
            if (this.Form.get_headers().ContainsKey("Content-Type"))
            {
                request.SetHeader("Content-Type", this.Form.get_headers()["Content-Type"]);
            }
            else
            {
                request.SetHeader("Content-Type", "application/x-www-form-urlencoded");
            }
        }

        public WWWForm Form { get; set; }
    }
}

