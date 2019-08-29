namespace BestHTTP.Examples
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GUIMessageList
    {
        private List<string> messages = new List<string>();
        private Vector2 scrollPos;

        public void Add(string msg)
        {
            this.messages.Add(msg);
            this.scrollPos = new Vector2(this.scrollPos.x, float.MaxValue);
        }

        public void Clear()
        {
            this.messages.Clear();
        }

        public void Draw()
        {
            this.Draw((float) Screen.width, 0f);
        }

        public void Draw(float minWidth, float minHeight)
        {
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinHeight(minHeight) };
            this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, optionArray1);
            for (int i = 0; i < this.messages.Count; i++)
            {
                GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MinWidth(minWidth) };
                GUILayout.Label(this.messages[i], optionArray2);
            }
            GUILayout.EndScrollView();
        }
    }
}

