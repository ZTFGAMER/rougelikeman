namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Runtime.InteropServices;

    public class SpineAttachment : SpineAttributeBase
    {
        public bool returnAttachmentPath;
        public bool currentSkinOnly;
        public bool placeholdersOnly;
        public string skinField = string.Empty;
        public string slotField = string.Empty;

        public SpineAttachment(bool currentSkinOnly = true, bool returnAttachmentPath = false, bool placeholdersOnly = false, string slotField = "", string dataField = "", string skinField = "", bool includeNone = true, bool fallbackToTextField = false)
        {
            this.currentSkinOnly = currentSkinOnly;
            this.returnAttachmentPath = returnAttachmentPath;
            this.placeholdersOnly = placeholdersOnly;
            this.slotField = slotField;
            base.dataField = dataField;
            this.skinField = skinField;
            base.includeNone = includeNone;
            base.fallbackToTextField = fallbackToTextField;
        }

        public static Attachment GetAttachment(string attachmentPath, SkeletonData skeletonData)
        {
            Hierarchy hierarchy = GetHierarchy(attachmentPath);
            return (!string.IsNullOrEmpty(hierarchy.name) ? skeletonData.FindSkin(hierarchy.skin).GetAttachment(skeletonData.FindSlotIndex(hierarchy.slot), hierarchy.name) : null);
        }

        public static Attachment GetAttachment(string attachmentPath, SkeletonDataAsset skeletonDataAsset) => 
            GetAttachment(attachmentPath, skeletonDataAsset.GetSkeletonData(true));

        public static Hierarchy GetHierarchy(string fullPath) => 
            new Hierarchy(fullPath);

        [StructLayout(LayoutKind.Sequential)]
        public struct Hierarchy
        {
            public string skin;
            public string slot;
            public string name;
            public Hierarchy(string fullPath)
            {
                char[] separator = new char[] { '/' };
                string[] strArray = fullPath.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length == 0)
                {
                    this.skin = string.Empty;
                    this.slot = string.Empty;
                    this.name = string.Empty;
                }
                else
                {
                    if (strArray.Length < 2)
                    {
                        throw new Exception("Cannot generate Attachment Hierarchy from string! Not enough components! [" + fullPath + "]");
                    }
                    this.skin = strArray[0];
                    this.slot = strArray[1];
                    this.name = string.Empty;
                    for (int i = 2; i < strArray.Length; i++)
                    {
                        this.name = this.name + strArray[i];
                    }
                }
            }
        }
    }
}

