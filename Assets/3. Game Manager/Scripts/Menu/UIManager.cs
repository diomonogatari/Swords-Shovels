using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu mainMenu;

    [SerializeField] private Camera dummyCamera;

    [SerializeField] private PauseMenu pauseMenu;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        //registor for GameState changes
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
    }
    private void HandleMainMenuFadeComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete.Invoke(fadeOut);
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

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }




    public void SetDummyCameraActive(bool active)
    {
        dummyCamera.gameObject.SetActive(active);
    }
}
