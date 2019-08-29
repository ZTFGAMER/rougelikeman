namespace Facebook.Unity.Example
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class LogView : ConsoleBase
    {
        private static string datePatt = "M/d/yyyy hh:mm:ss tt";
        private static IList<string> events = new List<string>();

        public static void AddLog(string log)
        {
            events.Insert(0, $"{DateTime.Now.ToString(datePatt)}
{log}
");
        }

        protected void OnGUI()
        {
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            if (base.Button("Back"))
            {
                base.GoBack();
            }
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                Vector2 scrollPosition = base.ScrollPosition;
                scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
                base.ScrollPosition = scrollPosition;
            }
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinWidth((float) ConsoleBase.MainWindowFullWidth) };
            base.ScrollPosition = GUILayout.BeginScrollView(base.ScrollPosition, optionArray1);
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.ExpandHeight(true), GUILayout.MaxWidth((float) ConsoleBase.MainWindowWidth) };
            GUILayout.TextArea(string.Join("\n", events.ToArray<string>()), base.TextStyle, optionArray2);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}

