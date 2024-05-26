using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wendigo : MonoBehaviour
{
    public enum State
    {
        PROWL,
        FOLLOW,
        CHASE
    }
    public Transform target;  // The target object to follow
    public Vector3 targetPosition;
    public Vector3 next;
    public float speed = 5f;  // Speed of the following movement
    Rigidbody2D rb;
    public LayerMask obstacleLayer;
    public bool lookingAtPlayer = false;
    public float positionTolerance = 1;
    public CollisionVision cv;
    Vector3 direction;
    public State state;

    private GridMap grid;
    private List<GridMap.Node> path;
    private int targetIndex;

    Vector2 directionToTarget;
    RaycastHit2D hit;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<GridMap>();
        targetPosition = target.position;
        state = State.PROWL;
    }

   

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case State.PROWL:
                if (path == null)
                {
                    do
                    {
                        targetPosition = new Vector3(UnityEngine.Random.Range(20.0f, 80.0f), UnityEngine.Random.Range(10.0f, 40.0f));
                        hit = Physics2D.CircleCast(targetPosition, grid.cellHeight, Vector2.zero, obstacleLayer);
                    } while (hit.collider != null && hit.collider.CompareTag("Building"));
                    //Debug.Log("Next pos: " + targetPosition);
                    do
                    {
                        UpdatePath();
                    } while (path[0].worldPosition == targetPosition);

                }
                //Debug.Log("TargetIndex: " + targetIndex);
                next = path[targetIndex].worldPosition;
                direction = next - transform.position;
                direction.Normalize();
                rb.velocity = direction * speed;
                if (Vector3.Distance(transform.position, next) <= 0.15f)
                {
                    targetIndex++;
                }
                if (Vector3.Distance(transform.position, targetPosition) <= 0.2f || targetIndex >= path.Count)
                {
                    do
                    {
                        targetPosition = new Vector3(UnityEngine.Random.Range(20.0f, 80.0f), UnityEngine.Random.Range(10.0f, 40.0f));
                        hit = Physics2D.CircleCast(targetPosition, grid.cellHeight, Vector2.zero, obstacleLayer);
                    } while (hit.collider != null && hit.collider.CompareTag("Building"));
                    UpdatePath();
                }
                break;
            case State.FOLLOW:
                //Debug.Log("TargetIndex: " + targetIndex);
                next = path[targetIndex > path.Count ? path.Count - 1 : targetIndex].worldPosition;
                direction = next - transform.position;
                direction.Normalize();
                rb.velocity = direction * speed;
                if (Vector3.Distance(transform.position, next) <= 0.15f)
                {
                    targetIndex++;
                }
                if (Vector3.Distance(transform.position, targetPosition) <= 2f || targetIndex >= path.Count)
                {
                    do
                    {
                        targetPosition = new Vector3(UnityEngine.Random.Range(20.0f, 80.0f), UnityEngine.Random.Range(10.0f, 40.0f));
                        hit = Physics2D.CircleCast(targetPosition, grid.cellHeight, Vector2.zero, obstacleLayer);
                    } while (hit.collider != null && hit.collider.CompareTag("Building"));
                    UpdatePath();
                    state = State.PROWL;
                    //Debug.Log(state);
                }
                break;

            case State.CHASE:
                //Debug.Log("TargetIndex: " + targetIndex);
                next = path[targetIndex > path.Count ? path.Count - 1 : targetIndex].worldPosition;
                direction = next - transform.position;
                direction.Normalize();
                rb.velocity = direction * speed;
                if (Vector3.Distance(transform.position, next) <= 2f)
                {
                    targetIndex++;
                }
                if (Vector3.Distance(target.position, targetPosition) > 2f)
                {
                    targetPosition = target.position;
                    UpdatePath();
                }
                if (targetIndex >= path.Count) {
                    UpdatePath();
                }
                break;
        }
        UpdateWendigo();
    }

    private void UpdateWendigo()
    {
        directionToTarget = target.position - transform.position;
        hit = Physics2D.Raycast(transform.position, directionToTarget, directionToTarget.magnitude, obstacleLayer);
        Debug.DrawLine(transform.position, target.position, hit.collider == null ? Color.green : Color.red);
        if (hit.collider != null && state == State.CHASE)
        {
            state = State.FOLLOW;
        }
        else if (hit.collider == null && state != State.CHASE)
        {
            state = State.CHASE;
            targetPosition = target.position;
            UpdatePath();
        }
    }

    void UpdatePath()
    {
        List<GridMap.Node> tempPath = grid.GetPath(transform.position, targetPosition);
        path = tempPath != null ? tempPath : path;
        Debug.Log(path[0].worldPosition);
        targetIndex = 0;
        //yield return new WaitForSeconds(0.5f);
    }

    bool PlayerVisible()
    {
        // Calculate the direction to the target
        Vector2 directionToTarget = target.position - transform.position;

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, directionToTarget.magnitude, obstacleLayer);

        // Check if the raycast hit an obstacl

        // Optional: Draw a debug line in the scene view to visualize the raycast
        Debug.DrawLine(transform.position, target.position, hit.collider == null ? Color.green : Color.red);

        if (hit.collider != null)
        {
            if (lookingAtPlayer)
            {
                //lastPos += hit.transform.position;
            }
            lookingAtPlayer = false;
        }
        else
        {
            lookingAtPlayer = true;
        }

        //Debug.Log(lookingAtPlayer);
        return lookingAtPlayer;
    }

    public void MoveRandom()
    {
        //Debug.Log("Lost player...");
        rb.velocity = Vector3.zero;
    }

    public void LookForPlayer()
    {
        //Debug.Log(targetIndex + " " + path.Count);
        if (path != null && targetIndex < path.Count)
        {
            this.targetPosition = target.transform.position;
            Vector3 targetPosition = path[targetIndex].worldPosition;
            float step = speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            direction = targetPosition - transform.position;
            direction.Normalize();
            rb.velocity = direction * speed;
            if (Vector3.Distance(transform.position, targetPosition) < 0.15f)
            {
                targetIndex++;
            }
        } else
        {
            direction = targetPosition - transform.position;
            direction.Normalize();
            rb.velocity = direction * speed;
        }
        /*if (cv.isNearBuilding())
        {
            direction = cv.getDetour() - transform.position;
        } 
        else
        {
            direction = lastPos - transform.position;
        }
        direction.Normalize();
        rb.velocity = direction * speed;

        if (cv.isNearBuilding() && Vector3.Distance(transform.position, cv.getDetour()) <= positionTolerance)
        {
            cv.nearBuilding = false;
        }*/
    }
}
