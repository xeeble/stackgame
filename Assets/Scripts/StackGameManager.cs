using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGameManager : Singleton<StackGameManager>
{
    public static MovingCube CurrentCube { get; private set; }
    [SerializeField]
    private CubeSpawner[] CubeSpawners;
    private int spawnerIndex = 0;
    private CubeSpawner currentSpawner;
    private bool canSpawn = true;
    [Range(0f,30f)]
    [SerializeField] private float spawnDelay = 1.5f;

    public static GameEvents currentState { get; private set; }

    private void OnEnable()
    {
        Events.Instance.AddListener(GameEvents.StopStack,HandleStackStop);
        Events.Instance.AddListener(GameEvents.GameOver, HandleGameOver);
        Events.Instance.AddListener(GameEvents.GameStarted, HandleGameStarted);
        ChangeGameState(GameEvents.EnterLobby);
        //StartGame();
    }

    public static GameEvents ChangeGameState(GameEvents newState)
    {
        var oldState = currentState;
        currentState = newState;
        Events.Instance.InvokeEvent(GameEvents.StateChanged, new OnStateChanged(newState, oldState));
        Debug.Log(currentState);
        return currentState;
    }
    // Update is called once per frame
    public void HandleStackStop(IGameEvent gameEvent)
    {
        Debug.Log("Handle Stack Stop");
        if (currentState == GameEvents.GameStarted)
        {
            MovingCube.CurrentCube.StopMoving();
            StartCoroutine(SpawnCube());
        } 
    }

    public void HandleGameOver(IGameEvent gameEvent)
    {
        Debug.Log("Handle Game Over");
        DestroyCubes();
        ChangeGameState(GameEvents.EnterLobby);
    }

    private static void DestroyCubes()
    {
        var cubes = FindObjectsOfType<MovingCube>();
        foreach (var item in cubes)
        {
            if (item.isBase)
            {
                CurrentCube = item;
            }
            else
            {
                item.ClearCube();
            }
        }
    }

    public void HandleGameStarted(IGameEvent gameEvent)
    {
        Debug.Log("Handle Game Started");
        if (currentState != GameEvents.GameStarted)
        {
            DestroyCubes();
            StartGame();
        }
    }
    private void StartGame()
    {
        ChangeGameState(GameEvents.GameStarted);
        StartCoroutine(SpawnCube());
    }

    IEnumerator SpawnCube()
    {
        canSpawn = false;
        spawnerIndex = spawnerIndex == 0 ? 1 : 0;
        currentSpawner = CubeSpawners[spawnerIndex];
        currentSpawner.SpawnCube();
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
    private void FixedUpdate()
    {
        if (currentState == GameEvents.EnterLobby)
        {
            if (canSpawn)
            {
                StartCoroutine(SpawnCube());
            }
        }
    }

    private void OnDisable()
    {
        Events.Instance.RemoveListener(GameEvents.StopStack,HandleStackStop);
        Events.Instance.RemoveListener(GameEvents.GameOver, HandleGameOver);
    }
}
