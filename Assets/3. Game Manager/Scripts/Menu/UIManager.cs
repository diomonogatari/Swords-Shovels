using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu mainMenu;

    [SerializeField] private Camera dummyCamera;

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.StartGame();
            //mainMenu.FadeOut();//Call the animation
        }
    }

    public void SetDummyCameraActive(bool active)
    {
        dummyCamera.gameObject.SetActive(active);
    }
}
