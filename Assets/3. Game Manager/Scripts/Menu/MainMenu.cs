using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Function that can receive animation events
    // Functions to play the fade in/out animations

    [SerializeField] private Animation mainMenuAnimator;
    [SerializeField] private AnimationClip fadeOutAnimation;
    [SerializeField] private AnimationClip fadeInAnimation;

    public Events.EventFadeComplete OnMainMenuFadeComplete; //We will register this event to know when it is complete

    private void Start()
    {
        //registor for event
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    public void OnFadeOutComplete()
    {
        OnMainMenuFadeComplete.Invoke(true);
    }

    public void OnFadeInComplete()
    {
        OnMainMenuFadeComplete.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);
    }

    public void FadeIn()
    {
        mainMenuAnimator.Stop();//stop any animations that are running;
        mainMenuAnimator.clip = fadeInAnimation;//set what to play
        mainMenuAnimator.Play();//play it

    }
    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);

        mainMenuAnimator.Stop();//stop any animations that are running;
        mainMenuAnimator.clip = fadeOutAnimation;//set what to play
        mainMenuAnimator.Play();//play it
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState.Equals(GameManager.GameState.PREGAME) && currentState.Equals(GameManager.GameState.RUNNING))
        {
            FadeOut();
        }

        if (!previousState.Equals(GameManager.GameState.PREGAME) && currentState.Equals(GameManager.GameState.PREGAME))
        {
            FadeIn();
        }
    }
}
