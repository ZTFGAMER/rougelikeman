using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GPGSCloudSave : MonoBehaviour
{
    private static bool mSaving;
    public static bool syncWithCloud;
    [CompilerGenerated]
    private static ConflictCallback <>f__mg$cache0;
    [CompilerGenerated]
    private static Action<SavedGameRequestStatus, ISavedGameMetadata> <>f__mg$cache1;
    [CompilerGenerated]
    private static Action<SavedGameRequestStatus, ISavedGameMetadata> <>f__mg$cache2;
    [CompilerGenerated]
    private static Action<SavedGameRequestStatus, byte[]> <>f__mg$cache3;

    public static void CloudSync(bool loadFromCloud)
    {
        Debug.Log("CloudSync, loadFromCloud - " + loadFromCloud);
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.Log("Authenticated, start sync");
            mSaving = !loadFromCloud;
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new ConflictCallback(GPGSCloudSave.ResolveConflict);
            }
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Action<SavedGameRequestStatus, ISavedGameMetadata>(GPGSCloudSave.SavedGameOpened);
            }
            ((PlayGamesPlatform) Social.Active).SavedGame.OpenWithManualConflictResolution("PlayerData.dat", DataSource.ReadCacheOrNetwork, true, <>f__mg$cache0, <>f__mg$cache1);
        }
    }

    private static byte[] GetBinary(SaveData savesCollection)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream serializationStream = new MemoryStream();
        formatter.Serialize(serializationStream, savesCollection);
        return serializationStream.GetBuffer();
    }

    private static SaveData GetCollection(byte[] data)
    {
        if (data.Length == 0)
        {
            return new SaveData();
        }
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream serializationStream = new MemoryStream(data);
        return (SaveData) formatter.Deserialize(serializationStream);
    }

    private static void ResolveConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
    {
        Debug.Log("Resolving conflict");
        if (originalData == null)
        {
            resolver.ChooseMetadata(unmerged);
        }
        else if (unmergedData == null)
        {
            resolver.ChooseMetadata(original);
        }
        else
        {
            SaveData collection = GetCollection(originalData);
            SaveData data2 = GetCollection(unmergedData);
            if (collection.gamesPlayed > data2.gamesPlayed)
            {
                resolver.ChooseMetadata(original);
            }
            else
            {
                resolver.ChooseMetadata(unmerged);
            }
        }
    }

    public static void SavedGameLoaded(SavedGameRequestStatus status, byte[] data)
    {
        Debug.Log("SavedGameLoaded");
        if (status == SavedGameRequestStatus.Success)
        {
            if (data.Length <= 0)
            {
                Debug.Log("data in cloud is empty");
                syncWithCloud = true;
                return;
            }
            SaveData collection = GetCollection(data);
            collection.CheckNewData();
            SaveManager.Save<SaveData>(collection, StaticConstants.PlayerSaveDataPath);
            DataLoader.dataUpdateManager.UpdateAfterConnect();
            DataLoader.playerData = collection;
            SetTutorialsComplete();
            if (DataLoader.gui != null)
            {
                DataLoader.gui.UpdateMenuContent();
            }
            Debug.Log("SaveGameLoaded, success=" + status);
        }
        else
        {
            Debug.LogWarning("Error reading game: " + status);
        }
        syncWithCloud = true;
    }

    public static void SavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        Debug.Log("SavedGameOpened");
        if (status == SavedGameRequestStatus.Success)
        {
            if (mSaving)
            {
                byte[] binary = GetBinary(DataLoader.playerData);
                Debug.Log("Saving to " + game);
                SavedGameMetadataUpdate updateForMetadata = new SavedGameMetadataUpdate.Builder().Build();
                if (<>f__mg$cache2 == null)
                {
                    <>f__mg$cache2 = new Action<SavedGameRequestStatus, ISavedGameMetadata>(GPGSCloudSave.SavedGameWritten);
                }
                ((PlayGamesPlatform) Social.Active).SavedGame.CommitUpdate(game, updateForMetadata, binary, <>f__mg$cache2);
            }
            else
            {
                if (<>f__mg$cache3 == null)
                {
                    <>f__mg$cache3 = new Action<SavedGameRequestStatus, byte[]>(GPGSCloudSave.SavedGameLoaded);
                }
                ((PlayGamesPlatform) Social.Active).SavedGame.ReadBinaryData(game, <>f__mg$cache3);
            }
        }
        else
        {
            Debug.LogWarning("Error opening game: " + status);
            syncWithCloud = true;
        }
    }

    public static void SavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        Debug.Log("SavedGameWritten");
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Game " + game.Description + " written");
        }
        else
        {
            Debug.LogWarning("Error saving game: " + status);
        }
        syncWithCloud = true;
    }

    private static void SetTutorialsComplete()
    {
        PlayerPrefs.SetInt(StaticConstants.TutorialCompleted, 1);
        PlayerPrefs.SetInt(StaticConstants.UpgradeTutorialCompleted, 1);
        PlayerPrefs.SetInt(StaticConstants.AbilityTutorialCompleted, 1);
        PlayerPrefs.Save();
    }
}

