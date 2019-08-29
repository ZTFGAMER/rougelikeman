namespace FC.Tool
{
    using System;
    using UnityEngine;

    public class FPSDisplay : MonoBehaviour
    {
        private float deltaTime;

        private void OnGUI()
        {
            int width = Screen.width;
            int height = Screen.height;
            GUIStyle style = new GUIStyle();
            Rect rect = new Rect(0f, 0f, (float) width, (float) ((height * 2) / 100));
            style.set_alignment(TextAnchor.UpperLeft);
            style.set_fontSize((height * 2) / 100);
            style.get_normal().set_textColor(new Color(0f, 0f, 0.5f, 1f));
            float num3 = this.deltaTime * 1000f;
            float num4 = 1f / this.deltaTime;
            string str = $"{num3:0.0} ms ({num4:0.} fps)";
            GUI.Label(rect, str, style);
        }

        private void Update()
        {
            this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * 0.1f;
        }
    }
}

