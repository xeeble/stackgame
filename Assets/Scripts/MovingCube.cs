using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public MovingCube BaseCube;
    public MoveDirection MoveDirection { get; internal set; }
    [Range(0f,60f)]
    public float TimeToCheckBounds = 1f;

    [SerializeField]
    public Color[] colors;
    public bool isBase;
    private bool isStacked = false;

    private BlockMovement movement;
    private float timer;

    // Start is called before the first frame update
    void OnEnable()
    {
        CurrentCube = this;
        movement = GetComponent<BlockMovement>();
        if (movement == null)
        {
            gameObject.AddComponent<BlockMovement>();
            movement = GetComponent<BlockMovement>();
            movement.Stop();
        }
       
        GetComponent<Renderer>().material.color = GetRandomColour();
        if (LastCube == null)
        {
            LastCube = BaseCube;// GameObject.Find("Base").GetComponent<MovingCube>();
        }

        transform.localScale = new Vector3(LastCube.transform.localScale.x,transform.localScale.y,LastCube.transform.localScale.z);
    }

    private Color GetRandomColour()
    {
        var c = colors[UnityEngine.Random.Range(0, colors.Length - 1)];
        return c;
    }

    private bool CheckForGameOver()
    {
        if (StackGameManager.currentState == GameEvents.GameStarted)
        {
            float hangover = GetHangover();
            //check if the cube is past the previous cube
            float max = MoveDirection == MoveDirection.X ? LastCube.transform.localScale.x : LastCube.transform.localScale.z;
            if (Mathf.Abs(hangover) >= max)
            {
                LastCube = null;
                CurrentCube = null;
                Events.Instance.InvokeEvent(GameEvents.GameOver, new OnGameOver());
                return true;
            }
        }
        return false;
    }

    private void CheckOutsideBounds()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            Debug.Log("checking out of bounds");
            if (StackGameManager.currentState == GameEvents.GameStarted)
            {
                CheckForGameOver();
            }
            else
            {
                ClearCube();
            }
        }

    }
    public void StopMoving()
    {
        if (!isStacked)
        {
            movement.Stop();
            float hangover = GetHangover();
            //check for game over

            if (CheckForGameOver())
            {
                return;
            }

            //determine which side is the hangover
            float direction = hangover > 0 ? 1f : -1f;

            if (MoveDirection == MoveDirection.X)
            {
                SplitCubeOnX(hangover, direction);

            }
            else
            {
                SplitCubeOnZ(hangover, direction);
            }

            LastCube = this;
            //notify cube was successfully stacked

            Events.Instance.InvokeEvent(GameEvents.CubeStacked, null);
        }
        
    }

    private float GetHangover()
    {
        if (MoveDirection == MoveDirection.X)
        {
            return transform.position.x - LastCube.transform.position.x;
        }
        else
        {
            return transform.position.z - LastCube.transform.position.z;
        }
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newXsize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newXsize;

        float newXPosition = LastCube.transform.position.x + (hangover / 2);
        //set size
        transform.localScale = new Vector3(newXsize, transform.localScale.y, transform.localScale.z);
        //set position
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXsize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockXPosition, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZsize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZsize;

        float newZPosition = LastCube.transform.position.z + (hangover / 2);
        //set size
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZsize);
        //set position
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZsize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;
        
        SpawnDropCube(fallingBlockZPosition, fallingBlockSize);
    }

   
    private void SpawnDropCube(float fallingBlockPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (MoveDirection == MoveDirection.X)
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockPosition, transform.position.y, transform.position.z);
        }
        else
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPosition);
        }
        
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
        Destroy(cube.gameObject, 1f);
    }

    private void LateUpdate()
    {
        if (movement != null)
        {
            movement.MoveDirection = MoveDirection;
            movement.Move();
            
        }
        timer += Time.deltaTime;

        if(timer >= TimeToCheckBounds)
        {
            CheckOutsideBounds();
        }
    }

    public void ClearCube()
    {
        Destroy(this.gameObject);
    }
}
