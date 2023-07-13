using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    } 

    public GameData Load(string profileId)
    {
        // base case
        if(profileId == null)
        {
            return null;
        }
        
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        GameData loadedData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader sr = new StreamReader(stream)) 
                    {
                        dataToLoad = sr.ReadToEnd();
                    }
                }

                // Deserialize
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e) 
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public bool TryLoad(string profileId)
    {
        GameData gameData = Load(profileId);

        return gameData != null;
    }

    public void Save(GameData data, string profileId) 
    {
        // base case
        if(profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        try
        {
            // Create Dir
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write the serialized data to the file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles() 
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos) 
        {
            string profileId = dirInfo.Name;

            // Check if data file exist
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if(!File.Exists(fullPath))
            {
                Debug.LogWarning($"Skipping directory when loading all profiles because it does not contain any data: {profileId}");
                continue;
            }

            // if data file exist, then load the game data
            GameData profileData = Load(profileId);

            // Check if profile data loaded correctly
            if(profileData != null) 
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogWarning($"Failed to load profile {profileId}");
            }
        }

        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecentProfileId = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach(KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;
            
            // skip this entry if the gamedata is null
            if(gameData == null)
            {
                continue;
            }

            if(mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                if(newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }
        }

        return mostRecentProfileId;
    }
}
