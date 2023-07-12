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

    public FileDataHandler fileDataHandler;
    public static DataPersistenceManager Instance{get; private set;}

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager. Newer one deleted.");
            //Destroy(gameObject);
        }
        Instance = this;

        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
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

    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
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

        fileDataHandler.Save(gameData);
    }

    public void LoadGame() 
    {
        gameData = fileDataHandler.Load();

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

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
