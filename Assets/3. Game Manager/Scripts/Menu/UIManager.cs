using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu mainMenu;

    [SerializeField] private Camera dummyCamera;

    [SerializeField] private PauseMenu pauseMenu;


    private void Start()
    {
        //registor for GameState changes
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }
    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }
    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.StartGame();
        }
    }

    public void SetDummyCameraActive(bool active)
    {
        dummyCamera.gameObject.SetActive(active);
    }
}
