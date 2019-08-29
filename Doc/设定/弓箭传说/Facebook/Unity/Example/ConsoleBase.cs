namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal class ConsoleBase : MonoBehaviour
    {
        private const int DpiScalingFactor = 160;
        private static Stack<string> menuStack = new Stack<string>();
        private string status = "Ready";
        private string lastResponse = string.Empty;
        private Vector2 scrollPosition = Vector2.zero;
        private float? scaleFactor;
        private GUIStyle textStyle;
        private GUIStyle buttonStyle;
        private GUIStyle textInputStyle;
        private GUIStyle labelStyle;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Texture2D <LastResponseTexture>k__BackingField;

        protected virtual void Awake()
        {
            Application.targetFrameRate = 60;
        }

        protected bool Button(string label)
        {
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinHeight(ButtonHeight * this.ScaleFactor), GUILayout.MaxWidth((float) MainWindowWidth) };
            return GUILayout.Button(label, this.ButtonStyle, optionArray1);
        }

        protected void GoBack()
        {
            if (menuStack.Any<string>())
            {
                SceneManager.LoadScene(menuStack.Pop());
            }
        }

        protected bool IsHorizontalLayout() => 
            (Screen.orientation == ScreenOrientation.LandscapeLeft);

        protected void LabelAndTextField(string label, ref string text)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MaxWidth(200f * this.ScaleFactor) };
            GUILayout.Label(label, this.LabelStyle, optionArray1);
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MaxWidth((float) (MainWindowWidth - 150)) };
            text = GUILayout.TextField(text, this.TextInputStyle, optionArray2);
            GUILayout.EndHorizontal();
        }

        protected void SwitchMenu(Type menuClass)
        {
            menuStack.Push(base.GetType().Name);
            SceneManager.LoadScene(menuClass.Name);
        }

        protected static int ButtonHeight =>
            (!Constants.IsMobile ? 0x18 : 60);

        protected static int MainWindowWidth =>
            (!Constants.IsMobile ? 700 : (Screen.width - 30));

        protected static int MainWindowFullWidth =>
            (!Constants.IsMobile ? 760 : Screen.width);

        protected static int MarginFix =>
            (!Constants.IsMobile ? 0x30 : 0);

        protected static Stack<string> MenuStack
        {
            get => 
                menuStack;
            set => 
                (menuStack = value);
        }

        protected string Status
        {
            get => 
                this.status;
            set => 
                (this.status = value);
        }

        protected Texture2D LastResponseTexture { get; set; }

        protected string LastResponse
        {
            get => 
                this.lastResponse;
            set => 
                (this.lastResponse = value);
        }

        protected Vector2 ScrollPosition
        {
            get => 
                this.scrollPosition;
            set => 
                (this.scrollPosition = value);
        }

        protected float ScaleFactor
        {
            get
            {
                if (!this.scaleFactor.HasValue)
                {
                    this.scaleFactor = new float?(Screen.dpi / 160f);
                }
                return this.scaleFactor.Value;
            }
        }

        protected int FontSize =>
            ((int) Math.Round((double) (this.ScaleFactor * 16f)));

        protected GUIStyle TextStyle
        {
            get
            {
                if (this.textStyle == null)
                {
                    this.textStyle = new GUIStyle(GUI.get_skin().get_textArea());
                    this.textStyle.set_alignment(TextAnchor.UpperLeft);
                    this.textStyle.set_wordWrap(true);
                    this.textStyle.set_padding(new RectOffset(10, 10, 10, 10));
                    this.textStyle.set_stretchHeight(true);
                    this.textStyle.set_stretchWidth(false);
                    this.textStyle.set_fontSize(this.FontSize);
                }
                return this.textStyle;
            }
        }

        protected GUIStyle ButtonStyle
        {
            get
            {
                if (this.buttonStyle == null)
                {
                    this.buttonStyle = new GUIStyle(GUI.get_skin().get_button());
                    this.buttonStyle.set_fontSize(this.FontSize);
                }
                return this.buttonStyle;
            }
        }

        protected GUIStyle TextInputStyle
        {
            get
            {
                if (this.textInputStyle == null)
                {
                    this.textInputStyle = new GUIStyle(GUI.get_skin().get_textField());
                    this.textInputStyle.set_fontSize(this.FontSize);
                }
                return this.textInputStyle;
            }
        }

        protected GUIStyle LabelStyle
        {
            get
            {
                if (this.labelStyle == null)
                {
                    this.labelStyle = new GUIStyle(GUI.get_skin().get_label());
                    this.labelStyle.set_fontSize(this.FontSize);
                }
                return this.labelStyle;
            }
        }
    }
}

