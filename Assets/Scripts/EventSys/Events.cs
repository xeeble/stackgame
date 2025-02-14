using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public enum GameEvents
{
    StopStack,
    GameStarted,
    GameOver,
    EnterLobby,
    CubeStacked,
    CubeSpawned,
    StateChanged,
}

public class Events : Singleton<Events>
{
    public delegate void GameEventListener(IGameEvent eventParam);
    public Dictionary<GameEvents,List<GameEventListener>> GameEventDictionary = new Dictionary<GameEvents, List<GameEventListener>>();

    public void AddListener(GameEvents gameEvent,GameEventListener gameEventListener)
    {
        List<GameEventListener> listeners;
        bool key = GameEventDictionary.TryGetValue(gameEvent, out listeners);
        if (key)
        {
            listeners.Add(gameEventListener);
        }
        else
        {
            GameEventDictionary.Add(gameEvent, new List<GameEventListener>() { gameEventListener });
        }
    }

    public void RemoveListener(GameEvents gameEvent, GameEventListener gameEventListener)
    {
        List<GameEventListener> listeners;
        bool key = GameEventDictionary.TryGetValue(gameEvent, out listeners);
        if (key)
        {
            listeners.Remove(gameEventListener);
        }
    }

    public void InvokeEvent(GameEvents gameEvent, IGameEvent eventParameters)
    {
        List<GameEventListener> listeners;
        bool key = GameEventDictionary.TryGetValue(gameEvent, out listeners);
        if (key)
        {
            foreach (GameEventListener item in listeners)
            {
                item(eventParameters);
            }
        }
    }

}
