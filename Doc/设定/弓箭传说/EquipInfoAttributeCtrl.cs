using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipInfoAttributeCtrl : MonoBehaviour
{
    public float pery = 40f;
    public List<Text> mList;
    private LocalSave.EquipOne mEquipData;

    public void UpdateUI(LocalSave.EquipOne one)
    {
        this.mEquipData = one;
    }

    public class UpExcute
    {
        public static string GetChangeValue(string value1, string value2)
        {
            switch (value1.Substring(value1.Length - 1, 1))
            {
                case "%":
                {
                    value1 = value1.Substring(0, value1.Length - 1);
                    value2 = value2.Substring(0, value2.Length - 1);
                    int.TryParse(value1, out int num);
                    int.TryParse(value2, out int num2);
                    object[] objArray1 = new object[] { num2 - num };
                    return Utils.FormatString("{0}%", objArray1);
                }
                case "f":
                {
                    value1 = value1.Substring(0, value1.Length - 1);
                    value2 = value2.Substring(0, value2.Length - 1);
                    int.TryParse(value1, out num);
                    int.TryParse(value2, out num2);
                    object[] objArray2 = new object[] { ((float) (num2 - num)) / 1000f };
                    return Utils.FormatString("{0}", objArray2);
                }
            }
            int.TryParse(value1, out num);
            int.TryParse(value2, out num2);
            object[] args = new object[] { num2 - num };
            return Utils.FormatString("{0}", args);
        }

        public static string GetValue(string value)
        {
            string str = value.Substring(value.Length - 1, 1);
            if (str == null)
            {
                return value;
            }
            if (str != "f")
            {
                return value;
            }
            value = value.Substring(0, value.Length - 1);
            int.TryParse(value, out int num);
            object[] args = new object[] { ((float) num) / 1000f };
            return Utils.FormatString("{0}", args);
        }
    }
}

