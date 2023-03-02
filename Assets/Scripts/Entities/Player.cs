using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Animator animator;
    Rigidbody2D rb;
    float lastDirectionx = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveDirection = new Vector2(Mathf.Clamp(Mathf.Round(rb.velocity.x / 3f), -1f, 1f), Input.GetAxisRaw("Vertical"));

        lastDirectionx = moveDirection.x != 0 ? moveDirection.x : lastDirectionx;

        animator.transform.parent.rotation = Quaternion.Euler(0, lastDirectionx < 0 ? 180f : 0f, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playAnimation("SwingRight");
        }

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            playAnimation("WalkRight");
        }
        else
        {
            playAnimation("idle");
        }
    }

    void playAnimation(string name)
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName(name) && !animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            animator.Play(name);
        }
    }
}
