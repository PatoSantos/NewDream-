using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wendigo : MonoBehaviour
{
    public Transform target;  // The target object to follow
    public Vector3 lastPos;
    public Vector3 detour;
    public float speed = 5f;  // Speed of the following movement
    Rigidbody2D rb;
    public LayerMask obstacleLayer;
    public bool lookingAtPlayer = false;
    public float positionTolerance = 1;
    public CollisionVision cv;
    Vector3 direction;

    private GridMap grid;
    private List<GridMap.Node> path;
    private int targetIndex;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<GridMap>();
        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        while (true)
        {
            path = grid.GetPath(transform.position, lastPos);
            Debug.Log(path != null);
            targetIndex = 0;
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerVisible())
        {
            lastPos = target.transform.position;
            direction = lastPos - transform.position;
            direction.Normalize();
            rb.velocity = direction * speed;
        }
        else
        {
            //Debug.Log("Looking for player");
            LookForPlayer();
        }
        // Calculate the direction to the target
        

        // Calculate the step size based on speed and delta time
        //float step = speed * Time.deltaTime;

        // Move the object towards the target
        //transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        

        if (Vector3.Distance(transform.position, lastPos) <= positionTolerance)
        {
            MoveRandom();
        }
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
                lastPos += hit.transform.position;
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
        Debug.Log("Lost player...");
        rb.velocity = Vector3.zero;
    }

    public void LookForPlayer()
    {
        //Debug.Log(targetIndex + " " + path.Count);
        if (path != null && targetIndex < path.Count)
        {

            Vector3 targetPosition = path[targetIndex].worldPosition;
            float step = speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            direction = targetPosition - transform.position;
            direction.Normalize();
            rb.velocity = direction * speed;
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetIndex++;
            }
        } else
        {
            rb.velocity = Vector3.zero;
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
