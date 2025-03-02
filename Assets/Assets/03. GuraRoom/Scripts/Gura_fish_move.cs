using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_fish_move : MonoBehaviour
{
    public float minSpeed = 0.5f; // Minimum movement speed
    public float maxSpeed = 1.5f; // Maximum movement speed
    public float minZ = -18f;     // Minimum Z-axis position
    public float maxZ = -5f;      // Maximum Z-axis position
    private float speed;
    private bool movingForward = true;

    void Start()
    {
        // Assign a random speed within the range
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        // Move the fish forward or backward along the Z-axis based on speed and direction
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // If the fish reaches the forward or backward limits of the Z-axis, change direction
        if (transform.position.z > maxZ && movingForward)
        {
            Flip();
        }
        else if (transform.position.z < minZ && !movingForward)
        {
            Flip();
        }
    }

    // Function to flip the fish 180 degrees
    void Flip()
    {
        movingForward = !movingForward;
        transform.Rotate(0f, 180f, 0f); // Rotate the fish by 180 degrees along the Y-axis
        speed = Random.Range(minSpeed, maxSpeed); // Randomize speed when changing direction
    }
}
