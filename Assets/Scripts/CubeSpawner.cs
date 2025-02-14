using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private MovingCube cubePrefab;
    [SerializeField]
    private MoveDirection moveDirection;
    private float spawnDelay = 2.5f;
    private bool canSpawn = true;
     
    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);
        Events.Instance.InvokeEvent(GameEvents.CubeSpawned, null);

        if (MovingCube.LastCube != null && MovingCube.LastCube.gameObject != GameObject.Find("Base"))
        {
            float x = moveDirection == MoveDirection.X ? transform.position.x : MovingCube.LastCube.transform.position.x;
            float z = moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.LastCube.transform.position.z;
            cube.transform.position = new Vector3(x,
                MovingCube.LastCube.transform.position.y + MovingCube.LastCube.transform.localScale.y/2f + cubePrefab.transform.localScale.y/2,
                z);
        }
        else
        {
            cube.transform.position = transform.position;
        }
        cube.MoveDirection = moveDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position,cubePrefab.transform.localScale);
    }
}