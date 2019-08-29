namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using System.Collections;
    using System.Linq;
    using UnityEngine;

    internal abstract class MenuBase : ConsoleBase
    {
        private static ShareDialogMode shareDialogMode;

        protected MenuBase()
        {
        }

        private void AddBackButton()
        {
            GUI.set_enabled(ConsoleBase.MenuStack.Any<string>());
            if (base.Button("Back"))
            {
                base.GoBack();
            }
            GUI.set_enabled(true);
        }

        private void AddDialogModeButton(ShareDialogMode mode)
        {
            bool flag = GUI.get_enabled();
            GUI.set_enabled(flag && (mode != shareDialogMode));
            if (base.Button(mode.ToString()))
            {
                shareDialogMode = mode;
                FB.Mobile.ShareDialogMode = mode;
            }
            GUI.set_enabled(flag);
        }

        private void AddDialogModeButtons()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            IEnumerator enumerator = Enum.GetValues(typeof(ShareDialogMode)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    this.AddDialogModeButton((ShareDialogMode) current);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void AddLogButton()
        {
            if (base.Button("Log"))
            {
                base.SwitchMenu(typeof(LogView));
            }
        }

        private void AddStatus()
        {
            GUILayout.Space(5f);
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinWidth((float) ConsoleBase.MainWindowWidth) };
            GUILayout.Box("Status: " + base.Status, base.TextStyle, optionArray1);
        }

        protected abstract void GetGui();
        protected void HandleResult(IResult result)
        {
            if (result == null)
            {
                base.LastResponse = "Null Response\n";
                LogView.AddLog(base.LastResponse);
            }
            else
            {
                base.LastResponseTexture = null;
                if (!string.IsNullOrEmpty(result.Error))
                {
                    base.Status = "Error - Check log for details";
                    base.LastResponse = "Error Response:\n" + result.Error;
                }
                else if (result.Cancelled)
                {
                    base.Status = "Cancelled - Check log for details";
                    base.LastResponse = "Cancelled Response:\n" + result.RawResult;
                }
                else if (!string.IsNullOrEmpty(result.RawResult))
                {
                    base.Status = "Success - Check log for details";
                    base.LastResponse = "Success Response:\n" + result.RawResult;
                }
                else
                {
                    base.LastResponse = "Empty Response\n";
                }
                LogView.AddLog(result.ToString());
            }
        }

        protected void OnGUI()
        {
            if (base.IsHorizontalLayout())
            {
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            }
            GUILayout.Label(base.GetType().Name, base.LabelStyle, Array.Empty<GUILayoutOption>());
            this.AddStatus();
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                Vector2 scrollPosition = base.ScrollPosition;
                scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
                base.ScrollPosition = scrollPosition;
            }
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinWidth((float) ConsoleBase.MainWindowFullWidth) };
            base.ScrollPosition = GUILayout.BeginScrollView(base.ScrollPosition, optionArray1);
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (this.ShowBackButton())
            {
                this.AddBackButton();
            }
            this.AddLogButton();
            if (this.ShowBackButton())
            {
                GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MinWidth((float) ConsoleBase.MarginFix) };
                GUILayout.Label(GUIContent.none, optionArray2);
            }
            GUILayout.EndHorizontal();
            if (this.ShowDialogModeSelector())
            {
                this.AddDialogModeButtons();
            }
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            this.GetGui();
            GUILayout.Space(10f);
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        protected virtual bool ShowBackButton() => 
            true;

        protected virtual bool ShowDialogModeSelector() => 
            false;
    }
}

