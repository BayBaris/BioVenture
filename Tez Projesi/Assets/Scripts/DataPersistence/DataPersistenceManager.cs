using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    public static DataPersistenceManager instance { get; private set; }
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("Found more than one Data Persistence Manager in scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(fileName);
        
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded called!");
        this.dataPersistenceObjects = FindAllPersistencesObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Unloaded Called!");
        SaveGame();
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load any saved data from a file using the data handler..
        this.gameData = dataHandler.Load();

        // If no data can be loaded, initialize to new game
        if(this.gameData == null)
        {
            Debug.Log("No data was found! A New Game needs to be started before data can be loaded!");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("Level Score: " + gameData.levelScore2);
    }
    public void SaveGame()
    {
        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        Scene sceneName = SceneManager.GetActiveScene();

        if(!sceneName.name.Equals("Main Menu"))
        {
            if (!sceneName.name.Equals("Cinematic"))
            {
                if (!sceneName.name.Equals("Quiz Scene"))
                { 
                    gameData.currentSceneName = sceneName.name;
                }
            }
        }
        //if we don't have any data to save, Log warning here
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // Save game data to a file using data handler...
        dataHandler.Save(gameData);
        Debug.Log("Data Saved!");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPersistence> FindAllPersistencesObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public string GetSavedSceneName()
    {
        if(gameData == null)
        {
            Debug.LogError("Try t� get scene name but data was null!");
            return null;
        }
        return gameData.currentSceneName;
    }
}
