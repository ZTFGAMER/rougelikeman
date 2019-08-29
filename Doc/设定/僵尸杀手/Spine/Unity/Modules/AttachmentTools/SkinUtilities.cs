namespace Spine.Unity.Modules.AttachmentTools
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class SkinUtilities
    {
        public static void Append(this Skin destination, Skin source)
        {
            source.CopyTo(destination, true, false, true);
        }

        public static void Clear(this Skin skin)
        {
            skin.Attachments.Clear();
        }

        public static void CopyTo(this Skin source, Skin destination, bool overwrite, bool cloneAttachments, bool cloneMeshesAsLinked = true)
        {
            Dictionary<Skin.AttachmentKeyTuple, Attachment> attachments = source.Attachments;
            Dictionary<Skin.AttachmentKeyTuple, Attachment> dictionary2 = destination.Attachments;
            if (cloneAttachments)
            {
                if (overwrite)
                {
                    foreach (KeyValuePair<Skin.AttachmentKeyTuple, Attachment> pair in attachments)
                    {
                        dictionary2[pair.Key] = pair.Value.GetClone(cloneMeshesAsLinked);
                    }
                }
                else
                {
                    foreach (KeyValuePair<Skin.AttachmentKeyTuple, Attachment> pair2 in attachments)
                    {
                        if (!dictionary2.ContainsKey(pair2.Key))
                        {
                            dictionary2.Add(pair2.Key, pair2.Value.GetClone(cloneMeshesAsLinked));
                        }
                    }
                }
            }
            else if (overwrite)
            {
                foreach (KeyValuePair<Skin.AttachmentKeyTuple, Attachment> pair3 in attachments)
                {
                    dictionary2[pair3.Key] = pair3.Value;
                }
            }
            else
            {
                foreach (KeyValuePair<Skin.AttachmentKeyTuple, Attachment> pair4 in attachments)
                {
                    if (!dictionary2.ContainsKey(pair4.Key))
                    {
                        dictionary2.Add(pair4.Key, pair4.Value);
                    }
                }
            }
        }

        public static Attachment GetAttachment(this Skin skin, string slotName, string keyName, Skeleton skeleton)
        {
            int slotIndex = skeleton.FindSlotIndex(slotName);
            if (skeleton == null)
            {
                throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
            }
            if (slotIndex == -1)
            {
                throw new ArgumentException($"Slot '{slotName}' does not exist in skeleton.", "slotName");
            }
            return skin.GetAttachment(slotIndex, keyName);
        }

        public static Skin GetClone(this Skin original)
        {
            Skin skin = new Skin(original.name + " clone");
            Dictionary<Skin.AttachmentKeyTuple, Attachment> attachments = skin.Attachments;
            foreach (KeyValuePair<Skin.AttachmentKeyTuple, Attachment> pair in original.Attachments)
            {
                attachments[pair.Key] = pair.Value;
            }
            return skin;
        }

        public static Skin GetClonedSkin(this Skeleton skeleton, string newSkinName, bool includeDefaultSkin = false, bool cloneAttachments = false, bool cloneMeshesAsLinked = true)
        {
            Skin destination = new Skin(newSkinName);
            Skin defaultSkin = skeleton.data.DefaultSkin;
            Skin skin = skeleton.skin;
            if (includeDefaultSkin)
            {
                defaultSkin.CopyTo(destination, true, cloneAttachments, cloneMeshesAsLinked);
            }
            if (skin != null)
            {
                skin.CopyTo(destination, true, cloneAttachments, cloneMeshesAsLinked);
            }
            return destination;
        }

        public static bool RemoveAttachment(this Skin skin, int slotIndex, string keyName) => 
            skin.Attachments.Remove(new Skin.AttachmentKeyTuple(slotIndex, keyName));

        public static bool RemoveAttachment(this Skin skin, string slotName, string keyName, Skeleton skeleton)
        {
            int slotIndex = skeleton.FindSlotIndex(slotName);
            if (skeleton == null)
            {
                throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
            }
            if (slotIndex == -1)
            {
                throw new ArgumentException($"Slot '{slotName}' does not exist in skeleton.", "slotName");
            }
            return skin.RemoveAttachment(slotIndex, keyName);
        }

        public static void SetAttachment(this Skin skin, int slotIndex, string keyName, Attachment attachment)
        {
            skin.AddAttachment(slotIndex, keyName, attachment);
        }

        public static void SetAttachment(this Skin skin, string slotName, string keyName, Attachment attachment, Skeleton skeleton)
        {
            int slotIndex = skeleton.FindSlotIndex(slotName);
            if (skeleton == null)
            {
                throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
            }
            if (slotIndex == -1)
            {
                throw new ArgumentException($"Slot '{slotName}' does not exist in skeleton.", "slotName");
            }
            skin.AddAttachment(slotIndex, keyName, attachment);
        }

        public static Skin UnshareSkin(this Skeleton skeleton, bool includeDefaultSkin, bool unshareAttachments, AnimationState state = null)
        {
            Skin newSkin = skeleton.GetClonedSkin("cloned skin", includeDefaultSkin, unshareAttachments, true);
            skeleton.SetSkin(newSkin);
            if (state != null)
            {
                skeleton.SetToSetupPose();
                state.Apply(skeleton);
            }
            return newSkin;
        }
    }
}

