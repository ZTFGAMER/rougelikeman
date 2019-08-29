namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Curve_curve : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Values>k__BackingField;
        private AnimationCurve curve;

        public Curve_curve Copy() => 
            new Curve_curve { 
                ID = this.ID,
                Notes = this.Notes,
                Values = this.Values
            };

        public AnimationCurve GetCurve()
        {
            if (this.curve == null)
            {
                this.InitCurve();
            }
            return this.curve;
        }

        private void InitCurve()
        {
            int length = this.Values.Length;
            Keyframe[] keys = new Keyframe[length];
            for (int i = 0; i < length; i++)
            {
                char[] separator = new char[] { ',' };
                string[] strArray = this.Values[i].Split(separator);
                keys[i] = new Keyframe(float.Parse(strArray[0]), float.Parse(strArray[1]));
                keys[i].inTangent = float.Parse(strArray[2]);
                keys[i].outTangent = float.Parse(strArray[3]);
            }
            this.curve = new AnimationCurve(keys);
        }

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Notes = base.readLocalString();
            this.Values = base.readArraystring();
            return true;
        }

        public int ID { get; private set; }

        public string Notes { get; private set; }

        public string[] Values { get; private set; }
    }
}

