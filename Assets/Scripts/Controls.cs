using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            switch (StackGameManager.currentState)
            {
                case GameEvents.StopStack:
                    
                    break;
                case GameEvents.GameStarted:
                    Events.Instance.InvokeEvent(GameEvents.StopStack, new StopStack());
                    break;
                case GameEvents.GameOver:
                    Events.Instance.InvokeEvent(GameEvents.GameStarted, new OnGameStart());
                    break;
                case GameEvents.EnterLobby:
                    Events.Instance.InvokeEvent(GameEvents.GameStarted, new OnGameStart());
                    break;
                default:
                    break;
            }

            
        }
    }
}
