namespace BestHTTP.Examples
{
    using BestHTTP.Caching;
    using System;
    using UnityEngine;

    public sealed class CacheMaintenanceSample : MonoBehaviour
    {
        private DeleteOlderTypes deleteOlderType = DeleteOlderTypes.Secs;
        private int value = 10;
        private int maxCacheSize = 0x500000;

        private void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUILayout.Label("Delete cached entities older then", Array.Empty<GUILayoutOption>());
                GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinWidth(50f) };
                GUILayout.Label(this.value.ToString(), optionArray1);
                GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MinWidth(100f) };
                this.value = (int) GUILayout.HorizontalSlider((float) this.value, 1f, 60f, optionArray2);
                GUILayout.Space(10f);
                string[] textArray1 = new string[] { "Days", "Hours", "Mins", "Secs" };
                this.deleteOlderType = (DeleteOlderTypes) GUILayout.SelectionGrid((int) this.deleteOlderType, textArray1, 4, Array.Empty<GUILayoutOption>());
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(10f);
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUILayoutOption[] optionArray3 = new GUILayoutOption[] { GUILayout.Width(150f) };
                GUILayout.Label("Max Cache Size (bytes): ", optionArray3);
                GUILayoutOption[] optionArray4 = new GUILayoutOption[] { GUILayout.Width(70f) };
                GUILayout.Label(this.maxCacheSize.ToString("N0"), optionArray4);
                this.maxCacheSize = (int) GUILayout.HorizontalSlider((float) this.maxCacheSize, 1024f, 1.048576E+07f, Array.Empty<GUILayoutOption>());
                GUILayout.EndHorizontal();
                GUILayout.Space(10f);
                if (GUILayout.Button("Maintenance", Array.Empty<GUILayoutOption>()))
                {
                    TimeSpan deleteOlder = TimeSpan.FromDays(14.0);
                    switch (this.deleteOlderType)
                    {
                        case DeleteOlderTypes.Days:
                            deleteOlder = TimeSpan.FromDays((double) this.value);
                            break;

                        case DeleteOlderTypes.Hours:
                            deleteOlder = TimeSpan.FromHours((double) this.value);
                            break;

                        case DeleteOlderTypes.Mins:
                            deleteOlder = TimeSpan.FromMinutes((double) this.value);
                            break;

                        case DeleteOlderTypes.Secs:
                            deleteOlder = TimeSpan.FromSeconds((double) this.value);
                            break;
                    }
                    HTTPCacheService.BeginMaintainence(new HTTPCacheMaintananceParams(deleteOlder, (ulong) this.maxCacheSize));
                }
            });
        }

        private enum DeleteOlderTypes
        {
            Days,
            Hours,
            Mins,
            Secs
        }
    }
}

