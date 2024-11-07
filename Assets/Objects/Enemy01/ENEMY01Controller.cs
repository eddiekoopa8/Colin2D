using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ENEMY01Controller : ACTORController
{
    int direction = 0;
    Animator animator;
    static int enemy_speed = 6;
    bool function = true;
    int deadTime = 0;
    int deadTimeMax = 40;
    // Start is called before the first frame update
    public override void ActorStart()
    {
        animator = GetComponent<Animator>();
        enabled = false;
    }

    private void FixedUpdate()
    {
        if (function == false)
        {
            if (deadTime++ >= deadTimeMax)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    public override void ActorUpdate()
    {
        if (function == false)
        {
            rigidbody.velocityX = 0;
            return;
        }

        if (rigidbody.velocityX >= 0 && direction == 0)
        {
            direction = 1;
        }

        if (rigidbody.velocityX == 0 && direction == 1)
        {
            direction = 0;
        }

        if (direction == 0)
        {
            renderer.flipX = false;
        }
        else if (direction == 1)
        {
            renderer.flipX = true;
        }

        if (direction == 0)
        {
            rigidbody.velocityX = -enemy_speed;
        }
        else if (direction == 1)
        {
            rigidbody.velocityX = enemy_speed;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (function == false)
        {
            return;
        }
        if (COLINControl.Verify(collision.gameObject))
        {
            if (COLINControl.GetInstance(collision.gameObject).Charging())
            {
                COLINControl.GetInstance(collision.gameObject).Knock();
                animator.Play("DEAD");
                function = false;
                collide.isTrigger = true;
                collide.enabled = false;
                rigidbody.gravityScale = 0;
                rigidbody.velocityX = 0;
            }

            if (function == false)
            {
                return;
            }

            if (COLINControl.GetInstance(collision.gameObject).OnTop())
            {
                COLINControl.GetInstance(collision.gameObject).Bounce();
                COLINControl.GetInstance(collision.gameObject).Hit(15);
                rigidbody.velocityX *= 2;

            }
        }
    }
}


