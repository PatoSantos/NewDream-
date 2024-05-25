using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get input from the arrow keys or WASD keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the new position
        //Vector3 newPosition = new Vector3(moveHorizontal, moveVertical, 0f) * moveSpeed * Time.deltaTime;

        rb.velocity = new Vector2(moveHorizontal, moveVertical) * moveSpeed;

        // Apply the new position to the object's transform
        //transform.position += newPosition;
    }
}
