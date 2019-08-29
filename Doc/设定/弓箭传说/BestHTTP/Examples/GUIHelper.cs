namespace BestHTTP.Examples
{
    using System;
    using UnityEngine;

    public static class GUIHelper
    {
        private static GUIStyle centerAlignedLabel;
        private static GUIStyle rightAlignedLabel;
        public static Rect ClientArea;

        public static void DrawArea(Rect area, bool drawHeader, Action action)
        {
            Setup();
            GUI.Box(area, string.Empty);
            GUILayout.BeginArea(area);
            if (drawHeader)
            {
                DrawCenteredText(SampleSelector.SelectedSample.DisplayName);
                GUILayout.Space(5f);
            }
            if (action != null)
            {
                action();
            }
            GUILayout.EndArea();
        }

        public static void DrawCenteredText(string msg)
        {
            Setup();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.FlexibleSpace();
            GUILayout.Label(msg, centerAlignedLabel, Array.Empty<GUILayoutOption>());
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void DrawRow(string key, string value)
        {
            Setup();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label(key, Array.Empty<GUILayoutOption>());
            GUILayout.FlexibleSpace();
            GUILayout.Label(value, rightAlignedLabel, Array.Empty<GUILayoutOption>());
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void Setup()
        {
            if (centerAlignedLabel == null)
            {
                centerAlignedLabel = new GUIStyle(GUI.get_skin().get_label());
                centerAlignedLabel.set_alignment(TextAnchor.MiddleCenter);
                rightAlignedLabel = new GUIStyle(GUI.get_skin().get_label());
                rightAlignedLabel.set_alignment(TextAnchor.MiddleRight);
            }
        }
    }
}

