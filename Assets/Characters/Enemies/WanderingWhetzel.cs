using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingWhetzel : WhetzelController
{
    private float timeSinceLastMoveDirectionChange = 0;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        moveDirection = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
        Move();
    }

    void Move()
    {
        if (!followingFabot)
        {
            if (Time.time - timeSinceLastMoveDirectionChange > 3)
            {
                moveDirection = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
                timeSinceLastMoveDirectionChange = Time.time;
            }
        }
        else
        {
            moveDirection = new Vector2(fabot.transform.position.x - transform.position.x, fabot.transform.position.y - transform.position.y).normalized;

        }
        rigidBody2D.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);

    }
}
