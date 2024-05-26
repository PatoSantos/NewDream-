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
        CHASE,
        INACTIVE
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

    public AudioSource audioSource;

    public float visionRange = 5f;

    public string[] layerMasks = { "Player", "Buildings" };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
                        targetPosition = new Vector3(UnityEngine.Random.Range(10.0f, 90.0f), UnityEngine.Random.Range(5.0f, 45.0f));
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
                    StartCoroutine(EndChaseAudio());
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
            case State.INACTIVE:
                rb.velocity = Vector3.zero;
                break;
        }
        UpdateWendigo();
    }

    private void UpdateWendigo()
    {
        if (GameManager.Instance.isPanelActive)
        {
            state = State.INACTIVE;
        }
        else
        {
            if (state == State.INACTIVE)
            {
                state = State.PROWL;
                path = null;
                StartCoroutine(EndChaseAudio());
            }
            directionToTarget = target.position - transform.position;
            hit = Physics2D.Raycast(transform.position, directionToTarget, visionRange, LayerMask.GetMask(layerMasks));
            Debug.DrawLine(transform.position, target.position, hit.collider == null ? Color.green : Color.red);
            //if (hit.collider != null) { Debug.Log(hit.collider.tag); }
            if (hit.collider != null && hit.collider.CompareTag("Building") && state == State.CHASE)
            {
                state = State.FOLLOW;
            }
            else if (hit.collider != null && hit.collider.CompareTag("Player") && state != State.CHASE)
            {
                state = State.CHASE;
                targetPosition = target.position;
                UpdatePath();
                if (!audioSource.isPlaying)
                {
                    StartChaseAudio();
                }
            }
        }
    }

    public void StartChaseAudio()
    {
        audioSource.volume = 1.0f;
        audioSource.Play();
        GameManager.Instance.PlayScream();
    }

    IEnumerator EndChaseAudio()
    {
        while (audioSource.volume > 0.1f)
        {
            audioSource.volume -= (1f / 1.5f)*0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.Stop();
    }

    void UpdatePath()
    {
        List<GridMap.Node> tempPath = grid.GetPath(transform.position, targetPosition);
        path = tempPath != null ? tempPath : path;
        //Debug.Log(path[0].worldPosition);
        targetIndex = 0;
        //yield return new WaitForSeconds(0.5f);
    }   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
