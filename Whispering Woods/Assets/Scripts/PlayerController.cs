using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int curHp;
    public int maxHp;
    public float moveSpeed;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 facingDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 vel = new Vector2(x, y);

        if (vel.magnitude != 0)
        {
            facingDirection = vel;
        }

        UpdateSpriteDirection();


        rb.velocity = vel * moveSpeed;
    }

    private void UpdateSpriteDirection()
    {
        if (facingDirection == Vector2.right)
        {
            sr.flipX = false;
        }
        else if (facingDirection == Vector2.left)
        {
            sr.flipX = true;
        }
    }
}
