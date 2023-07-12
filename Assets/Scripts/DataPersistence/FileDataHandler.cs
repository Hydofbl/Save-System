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

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
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

    public bool TryLoad()
    {
        GameData gameData = Load();

        return gameData != null;
    }

    public void Save(GameData data) 
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

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
}
