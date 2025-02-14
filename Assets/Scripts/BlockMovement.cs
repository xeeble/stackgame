using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    public MoveDirection MoveDirection { get; internal set; }
    
    public void Move()
    {
        if (MoveDirection == MoveDirection.Z)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }
        
    }

    public void Stop()
    {
        moveSpeed = 0f;
    }

    
}
