using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[System.Serializable]//Visible in editor
public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState>
{
    /*T0 = currentIncoming GameState; T1 = previous GameState  */

}

public class GameManager : Singleton<GameManager>
{
    // keep track of the game state
    //PREGAME, RUNNING, PAUSED
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }
    public GameState CurrentGameState
    {
        get { return currentGameState; }
        private set { currentGameState = value; }//Only GameManager can write State
    }

    private GameState currentGameState = GameState.PREGAME;

    public EventGameState OnGameStateChanged;


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
        if (instance == null)
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
    #region Base LifeCycle
    private void Start()
    {
        DontDestroyOnLoad(gameObject);//never ever destroy my Game Manager pls :D

        instancedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();

        loadOperationsList = new List<AsyncOperation>();
    }
    private void Update()
    {
        if (currentGameState.Equals(GameState.PREGAME))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
        }
    }
    #endregion

    private void UpdateState(GameState state)
    {
        GameState previousGameState = currentGameState;
        currentGameState = state;

        switch (currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1f;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1f;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0f;
                break;
            default:
                Debug.LogWarning("[GameManager] tried to update GameState to a value not possible. Default entered");
                break;
        }

        OnGameStateChanged.Invoke(currentGameState, previousGameState);
    }

    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance; //store what is instantiated to stuff them into our manager list
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);//create the prefabs managers

            instancedSystemPrefabs.Add(prefabInstance);//add them to our management list
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < instancedSystemPrefabs.Count; i++)
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

            if (loadOperationsList.Count.Equals(0))//Loads are done
                UpdateState(GameState.RUNNING);
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

    public void StartGame()
    {
        LoadLevel("Main");
    }

    public void TogglePause()
    {
        //condition ? true : false;
        UpdateState(currentGameState.Equals(GameState.RUNNING) ? GameState.PAUSED : GameState.RUNNING);
    }
}
