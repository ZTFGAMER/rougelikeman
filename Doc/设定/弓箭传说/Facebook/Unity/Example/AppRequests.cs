namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class AppRequests : MenuBase
    {
        private string requestMessage = string.Empty;
        private string requestTo = string.Empty;
        private string requestFilter = string.Empty;
        private string requestExcludes = string.Empty;
        private string requestMax = string.Empty;
        private string requestData = string.Empty;
        private string requestTitle = string.Empty;
        private string requestObjectID = string.Empty;
        private int selectedAction;
        private string[] actionTypeStrings = new string[] { "NONE", OGActionType.SEND.ToString(), OGActionType.ASKFOR.ToString(), OGActionType.TURN.ToString() };

        protected override void GetGui()
        {
            List<object> list2;
            if (base.Button("Select - Filter None"))
            {
                FB.AppRequest("Test Message", null, null, null, null, string.Empty, string.Empty, new FacebookDelegate<IAppRequestResult>(this.HandleResult));
            }
            if (base.Button("Select - Filter app_users"))
            {
                list2 = new List<object> { "app_users" };
                List<object> filters = list2;
                FB.AppRequest("Test Message", null, filters, null, 0, string.Empty, string.Empty, new FacebookDelegate<IAppRequestResult>(this.HandleResult));
            }
            if (base.Button("Select - Filter app_non_users"))
            {
                list2 = new List<object> { "app_non_users" };
                List<object> filters = list2;
                FB.AppRequest("Test Message", null, filters, null, 0, string.Empty, string.Empty, new FacebookDelegate<IAppRequestResult>(this.HandleResult));
            }
            base.LabelAndTextField("Message: ", ref this.requestMessage);
            base.LabelAndTextField("To (optional): ", ref this.requestTo);
            base.LabelAndTextField("Filter (optional): ", ref this.requestFilter);
            base.LabelAndTextField("Exclude Ids (optional): ", ref this.requestExcludes);
            base.LabelAndTextField("Filters: ", ref this.requestExcludes);
            base.LabelAndTextField("Max Recipients (optional): ", ref this.requestMax);
            base.LabelAndTextField("Data (optional): ", ref this.requestData);
            base.LabelAndTextField("Title (optional): ", ref this.requestTitle);
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MaxWidth(200f * base.ScaleFactor) };
            GUILayout.Label("Request Action (optional): ", base.LabelStyle, optionArray1);
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MinHeight(ConsoleBase.ButtonHeight * base.ScaleFactor), GUILayout.MaxWidth((float) (ConsoleBase.MainWindowWidth - 150)) };
            this.selectedAction = GUILayout.Toolbar(this.selectedAction, this.actionTypeStrings, base.ButtonStyle, optionArray2);
            GUILayout.EndHorizontal();
            base.LabelAndTextField("Request Object ID (optional): ", ref this.requestObjectID);
            if (base.Button("Custom App Request"))
            {
                OGActionType? selectedOGActionType = this.GetSelectedOGActionType();
                if (selectedOGActionType.HasValue)
                {
                    FB.AppRequest(this.requestMessage, selectedOGActionType.Value, this.requestObjectID, !string.IsNullOrEmpty(this.requestTo) ? ((IEnumerable<string>) this.requestTo.Split(new char[] { ',' })) : null, this.requestData, this.requestTitle, new FacebookDelegate<IAppRequestResult>(this.HandleResult));
                }
                else
                {
                    FB.AppRequest(this.requestMessage, !string.IsNullOrEmpty(this.requestTo) ? ((IEnumerable<string>) this.requestTo.Split(new char[] { ',' })) : null, !string.IsNullOrEmpty(this.requestFilter) ? this.requestFilter.Split(new char[] { ',' }).OfType<object>().ToList<object>() : null, !string.IsNullOrEmpty(this.requestExcludes) ? ((IEnumerable<string>) this.requestExcludes.Split(new char[] { ',' })) : null, new int?(!string.IsNullOrEmpty(this.requestMax) ? int.Parse(this.requestMax) : 0), this.requestData, this.requestTitle, new FacebookDelegate<IAppRequestResult>(this.HandleResult));
                }
            }
        }

        private OGActionType? GetSelectedOGActionType()
        {
            string str = this.actionTypeStrings[this.selectedAction];
            OGActionType sEND = OGActionType.SEND;
            if (str == sEND.ToString())
            {
                return 0;
            }
            OGActionType aSKFOR = OGActionType.ASKFOR;
            if (str == aSKFOR.ToString())
            {
                return 1;
            }
            OGActionType tURN = OGActionType.TURN;
            if (str == tURN.ToString())
            {
                return 2;
            }
            return null;
        }
    }
}

