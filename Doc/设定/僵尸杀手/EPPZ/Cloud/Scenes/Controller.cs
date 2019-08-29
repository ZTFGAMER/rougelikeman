namespace EPPZ.Cloud.Scenes
{
    using EPPZ.Cloud;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class Controller : MonoBehaviour
    {
        public Elements elements;

        private void AddConflictResolvingActions()
        {
            EPPZ.Cloud.Cloud.OnKeyChange("level", new Action<int>(this.ResolveConflictForLevel), 1);
            EPPZ.Cloud.Cloud.OnKeyChange("firstTrophy", new Action<bool>(this.ResolveConflictForFirstTrophy), 1);
            EPPZ.Cloud.Cloud.OnKeyChange("secondTrophy", new Action<bool>(this.ResolveConflictForSecondTrophy), 1);
            EPPZ.Cloud.Cloud.OnKeyChange("thirdTrophy", new Action<bool>(this.ResolveConflictForThirdTrophy), 1);
        }

        private void AddElementUpdatingActions()
        {
            EPPZ.Cloud.Cloud.OnKeyChange("name", delegate (string value) {
                this.elements.nameLabel.text = value;
                this.elements.nameLabelAnimation.Play("Blink");
            }, 2);
            EPPZ.Cloud.Cloud.OnKeyChange("sound", delegate (bool value) {
                this.elements.soundToggle.isOn = value;
                this.elements.soundToggleAnimation.Play("Blink");
            }, 2);
            EPPZ.Cloud.Cloud.OnKeyChange("volume", delegate (float value) {
                this.elements.volumeSlider.value = value;
                this.elements.volumeSliderAnimation.Play("Blink");
            }, 2);
            EPPZ.Cloud.Cloud.OnKeyChange("level", delegate (int value) {
                this.elements.levelDropdown.value = value;
                this.elements.levelDropdownAnimation.Play("Blink");
            }, 2);
            EPPZ.Cloud.Cloud.OnKeyChange("firstTrophy", delegate (bool value) {
                this.elements.firstTrophyToggle.isOn = value;
                this.elements.firstTrophyToggleAnimation.Play("Blink");
            }, 2);
            EPPZ.Cloud.Cloud.OnKeyChange("secondTrophy", delegate (bool value) {
                this.elements.secondTrophyToggle.isOn = value;
                this.elements.secondTrophyToggleAnimation.Play("Blink");
            }, 2);
            EPPZ.Cloud.Cloud.OnKeyChange("thirdTrophy", delegate (bool value) {
                this.elements.thirdTrophyToggle.isOn = value;
                this.elements.thirdTrophyToggleAnimation.Play("Blink");
            }, 2);
        }

        private EPPZ.Cloud.Cloud.Should OnCloudChange(string[] changedKeys, ChangeReason changeReason)
        {
            if (changeReason == ChangeReason.InitialSyncChange)
            {
                this.PopulateElementsFromCloud();
                return EPPZ.Cloud.Cloud.Should.StopUpdateKeys;
            }
            if (changeReason == ChangeReason.QuotaViolationChange)
            {
                return EPPZ.Cloud.Cloud.Should.StopUpdateKeys;
            }
            if (changeReason == ChangeReason.AccountChange)
            {
                this.PopulateElementsFromCloud();
                return EPPZ.Cloud.Cloud.Should.StopUpdateKeys;
            }
            return EPPZ.Cloud.Cloud.Should.UpdateKeys;
        }

        public void OnConflictResolutionToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                this.AddConflictResolvingActions();
            }
            else
            {
                this.RemoveConflictResolvingActions();
            }
        }

        private void OnDestroy()
        {
            EPPZ.Cloud.Cloud.onCloudChange = (EPPZ.Cloud.Cloud.OnCloudChange) Delegate.Remove(EPPZ.Cloud.Cloud.onCloudChange, new EPPZ.Cloud.Cloud.OnCloudChange(this.OnCloudChange));
        }

        public void OnFirstTrophyToggleValueChanged(bool isOn)
        {
            EPPZ.Cloud.Cloud.SetBoolForKey(isOn, "firstTrophy");
            EPPZ.Cloud.Cloud.Synchrnonize();
        }

        public void OnLevelDropDownValueChanged(int value)
        {
            EPPZ.Cloud.Cloud.SetIntForKey(value, "level");
            EPPZ.Cloud.Cloud.Synchrnonize();
        }

        public void OnNameInputFieldEndEdit(string text)
        {
            EPPZ.Cloud.Cloud.SetStringForKey(text, "name");
            EPPZ.Cloud.Cloud.Synchrnonize();
        }

        public void OnSecondTrophyToggleValueChanged(bool isOn)
        {
            EPPZ.Cloud.Cloud.SetBoolForKey(isOn, "secondTrophy");
            EPPZ.Cloud.Cloud.Synchrnonize();
        }

        public void OnSoundToggleValueChanged(bool isOn)
        {
            EPPZ.Cloud.Cloud.SetBoolForKey(isOn, "sound");
            EPPZ.Cloud.Cloud.Synchrnonize();
        }

        public void OnThirdTrophyToggleValueChanged(bool isOn)
        {
            EPPZ.Cloud.Cloud.SetBoolForKey(isOn, "thirdTrophy");
            EPPZ.Cloud.Cloud.Synchrnonize();
        }

        public void OnVolumeSliderEndDrag(BaseEventData eventData)
        {
            EPPZ.Cloud.Cloud.SetFloatForKey(this.elements.volumeSlider.value, "volume");
            EPPZ.Cloud.Cloud.Synchrnonize();
        }

        private void PopulateElementsFromCloud()
        {
            this.elements.nameLabel.text = EPPZ.Cloud.Cloud.StringForKey("name");
            this.elements.soundToggle.isOn = EPPZ.Cloud.Cloud.BoolForKey("sound");
            this.elements.volumeSlider.value = EPPZ.Cloud.Cloud.FloatForKey("volume");
            this.elements.levelDropdown.value = EPPZ.Cloud.Cloud.IntForKey("level");
            this.elements.firstTrophyToggle.isOn = EPPZ.Cloud.Cloud.BoolForKey("firstTrophy");
            this.elements.secondTrophyToggle.isOn = EPPZ.Cloud.Cloud.BoolForKey("secondTrophy");
            this.elements.thirdTrophyToggle.isOn = EPPZ.Cloud.Cloud.BoolForKey("thirdTrophy");
        }

        private void RemoveConflictResolvingActions()
        {
            EPPZ.Cloud.Cloud.RemoveOnKeyChangeAction("level", new Action<int>(this.ResolveConflictForLevel));
            EPPZ.Cloud.Cloud.RemoveOnKeyChangeAction("firstTrophy", new Action<bool>(this.ResolveConflictForFirstTrophy));
            EPPZ.Cloud.Cloud.RemoveOnKeyChangeAction("secondTrophy", new Action<bool>(this.ResolveConflictForSecondTrophy));
            EPPZ.Cloud.Cloud.RemoveOnKeyChangeAction("thirdTrophy", new Action<bool>(this.ResolveConflictForThirdTrophy));
        }

        private void ResolveConflictForFirstTrophy(bool value)
        {
            Debug.Log("ResolveConflictForFirstTrophy(" + value + ")");
            if (this.elements.firstTrophyToggle.isOn != value)
            {
                this.elements.firstTrophyToggle.isOn = this.elements.firstTrophyToggle.isOn || value;
                this.OnFirstTrophyToggleValueChanged(this.elements.firstTrophyToggle.isOn);
            }
        }

        private void ResolveConflictForLevel(int value)
        {
            Debug.Log("ResolveConflictForLevel(" + value + ")");
            if (this.elements.levelDropdown.value != value)
            {
                this.elements.levelDropdown.value = Math.Max(this.elements.levelDropdown.value, value);
                this.OnLevelDropDownValueChanged(this.elements.levelDropdown.value);
            }
        }

        private void ResolveConflictForSecondTrophy(bool value)
        {
            Debug.Log("ResolveConflictForSecondTrophy(" + value + ")");
            if (this.elements.secondTrophyToggle.isOn != value)
            {
                this.elements.secondTrophyToggle.isOn = this.elements.secondTrophyToggle.isOn || value;
                this.OnSecondTrophyToggleValueChanged(this.elements.secondTrophyToggle.isOn);
            }
        }

        private void ResolveConflictForThirdTrophy(bool value)
        {
            Debug.Log("ResolveConflictForThirdTrophy(" + value + ")");
            if (this.elements.thirdTrophyToggle.isOn != value)
            {
                this.elements.thirdTrophyToggle.isOn = this.elements.thirdTrophyToggle.isOn || value;
                this.OnThirdTrophyToggleValueChanged(this.elements.thirdTrophyToggle.isOn);
            }
        }

        private void Start()
        {
            EPPZ.Cloud.Cloud.onCloudChange = (EPPZ.Cloud.Cloud.OnCloudChange) Delegate.Combine(EPPZ.Cloud.Cloud.onCloudChange, new EPPZ.Cloud.Cloud.OnCloudChange(this.OnCloudChange));
            this.AddElementUpdatingActions();
            this.PopulateElementsFromCloud();
        }

        [Serializable]
        public class Elements
        {
            public InputField nameLabel;
            public Animation nameLabelAnimation;
            [Space]
            public Toggle soundToggle;
            public Animation soundToggleAnimation;
            [Space]
            public Slider volumeSlider;
            public Animation volumeSliderAnimation;
            [Space]
            public Dropdown levelDropdown;
            public Animation levelDropdownAnimation;
            [Space]
            public Toggle firstTrophyToggle;
            public Animation firstTrophyToggleAnimation;
            [Space]
            public Toggle secondTrophyToggle;
            public Animation secondTrophyToggleAnimation;
            [Space]
            public Toggle thirdTrophyToggle;
            public Animation thirdTrophyToggleAnimation;
        }
    }
}

