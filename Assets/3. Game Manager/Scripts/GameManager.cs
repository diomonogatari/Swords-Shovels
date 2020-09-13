using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // keep track of the game state
    // generate other persistent systems
    // what level the game is currently in

    public GameObject[] SystemPrefabs;//a collection of things to keep track of
    private List<GameObject> instancedSystemPrefabs;

    private string currentLevelName = string.Empty;

    private static GameManager instance;

    List<AsyncOperation> loadOperationsList;

    protected override void Awake()
    {
        base.Awake();
        if(instance == null)
        {
            //I am the 1st being created!
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("[Game Manager] instance destroyed! There can only be one instance of GameManager!");
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);//never ever destroy my Game Manager pls :D

        instancedSystemPrefabs = new List<GameObject>();

        InstantiateSystemPrefabs();

        loadOperationsList = new List<AsyncOperation>();
        LoadLevel("Main");
    }

    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance; //store what is instantiated to stuff them into our manager list
        for(int i = 0; i< SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);//create the prefabs managers

            instancedSystemPrefabs.Add(prefabInstance);//add them to our management list
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for(int i = 0; i< instancedSystemPrefabs.Count; i++)
        {
            Destroy(instancedSystemPrefabs[i]);
        }
        instancedSystemPrefabs.Clear();//making sure and easy for GC
    }

    // load and unload game levels
    #region Loading Methods

    private void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (loadOperationsList.Contains(ao))
        {
            loadOperationsList.Remove(ao);

        }
        Debug.Log("Load Complete.");
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.Log("[Game Manager] Unable to Load level " + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        loadOperationsList.Add(ao);

        currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        var ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.Log("[Game Manager] Unable to Unload level " + levelName);
            return;
        }

        ao.completed += OnUnLoadOperationComplete;
        //currentLevelName = levelName;
    }

    private void OnUnLoadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete.");
        throw new System.NotImplementedException();
    }
    #endregion

}
