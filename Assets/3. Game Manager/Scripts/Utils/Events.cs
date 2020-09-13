using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Events
{
    [System.Serializable]//Visible in editor
    public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState>
    {
        /*T0 = currentIncoming GameState; T1 = previous GameState  */
    }
    [System.Serializable]
    public class EventFadeComplete : UnityEvent<bool>
    {//bool states fade in or out
        //fade out represents true
        //fade in is false

    }
}
