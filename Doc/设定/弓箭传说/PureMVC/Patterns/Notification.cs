namespace PureMVC.Patterns
{
    using PureMVC.Interfaces;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Serializable]
    public class Notification : INotification
    {
        private StringBuilder strTemp;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object <Body>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FileName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FuncName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LineNumber>k__BackingField;

        public Notification(string name) : this(name, null, null)
        {
        }

        public Notification(string name, object body) : this(name, body, null)
        {
        }

        public Notification(string name, object body, string type)
        {
            this.strTemp = new StringBuilder();
            this.Name = name;
            this.Body = body;
            this.Type = type;
        }

        public void getDebugInfo()
        {
            StackFrame frame = new StackTrace(true).GetFrame(5);
            if (frame != null)
            {
                this.FileName = frame.GetFileName();
                this.FuncName = frame.GetMethod().Name;
                this.LineNumber = frame.GetFileLineNumber();
            }
        }

        public override string ToString()
        {
            this.strTemp.Clear();
            this.strTemp.AppendFormat("Notification Name: {0}", this.Name);
            this.strTemp.AppendFormat("{0}Body:{1}", Environment.NewLine, (this.Body != null) ? this.Body.ToString() : "null");
            if (this.Type == null)
            {
            }
            this.strTemp.AppendFormat("{0}Type:{1}", Environment.NewLine, "null");
            return this.strTemp.ToString();
        }

        public string Name { get; private set; }

        public object Body { get; set; }

        public string Type { get; set; }

        public string FileName { get; private set; }

        public string FuncName { get; private set; }

        public int LineNumber { get; private set; }
    }
}

