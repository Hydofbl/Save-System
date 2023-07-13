using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;

    public string selectedProfileId = "";
    public FileDataHandler fileDataHandler;
    public static DataPersistenceManager Instance{get; private set;}

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager. Newer one deleted.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        selectedProfileId = fileDataHandler.GetMostRecentlyUpdatedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called after OnEnable and before Start method.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistanceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        selectedProfileId = newProfileId;
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach(IDataPersistance obj in dataPersistanceObjects)
        {
            obj.SaveData(ref gameData);
        }

        gameData.lastUpdated = DateTime.Now.ToBinary();

        fileDataHandler.Save(gameData, selectedProfileId);
    }

    public void LoadGame() 
    {
        gameData = fileDataHandler.Load(selectedProfileId);

        if(gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        foreach(IDataPersistance obj in dataPersistanceObjects)
        {
            obj.LoadData(gameData);
        }
    }

    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return fileDataHandler.LoadAllProfiles();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
