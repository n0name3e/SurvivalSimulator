using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyPatrolling : MonoBehaviour
{
    public float speed;
    public Transform point1;
    public Transform point2;
    public Vector3 target;

    private bool isPoint1 = true;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = point1.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(target.x, 0, target.z)
        );

        if (distance < 0.5f) // Close enough
        {
            SwapPoints();
            return;
        }

        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0; // Don't move up/down

        // Set velocity, but keep existing gravity/vertical velocity
        rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, direction.z * speed);
    }
    private void SwapPoints()
    {
        isPoint1 = !isPoint1;
        target = isPoint1 ? point1.position : point2.position;
    }
}
