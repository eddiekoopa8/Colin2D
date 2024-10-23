using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class COLINControl : ACTORController
{
    public float movingSpeed;
    public float jumpForce;

    private bool didSuperJump = false;

    private Animator animator;
    private FADERHandler fade;

    /// <summary>
    /// This plays an animation depending if the player is in ground or not
    /// </summary>
    /// <param name="name">Animation name</param>
    /// <param name="groundCheck">Check to see if either true or false</param>
    public void PlayAnimGround(string name, bool groundCheck)
    {
        bool result = true;
        if (isGrounded == groundCheck) animator.Play(name);
    }

    /// <summary>
    /// Play an animation
    /// </summary>
    /// <param name="name">Animation name</param>
    public void PlayAnim(string name)
    {
        animator.Play(name);
    }

    /// <summary>
    /// Colin's init function
    /// </summary>
    public override void ActorStart()
    {
        animator = GetComponent<Animator>();
        fade = GameObject.Find("FADERObject").GetComponent<FADERHandler>();
    }

    /// <summary>
    /// Colin's brain function.
    /// </summary>
    public override void ActorUpdate()
    {
        bool PressedJump = Input.GetButtonDown("Jump");
        bool HeldMoveKey = Input.GetButton("Horizontal");
        float HeldMoveAxis = Input.GetAxis("Horizontal");
        bool HeldUpKey = Input.GetAxis("Vertical") > 0.1f;

        // if there is a fader object
        if (fade)
        {
            // if it isnt done
            if (fade.FadeDone() == false)
            {
                // dont control the player yet
                return;
            }
        }

        // MOVE
        if (HeldMoveKey)
        {
            // Get direction
            Vector3 direction = transform.right * HeldMoveAxis;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);

            PlayAnimGround("ANIMRun", true);
        }
        else
        {
            PlayAnimGround("ANIMIdle", true);
        }

        // JUMP
        if (PressedJump && isGrounded)
        {
            if (HeldUpKey && !HeldMoveKey)
            {
                rigidbody.AddForce(transform.up * (jumpForce * 1.25f), ForceMode2D.Impulse);
                didSuperJump = true;
            }
            else
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        PlayAnimGround("ANIMIdle", false);

        // FLIP DIRECTION
        if (HeldMoveAxis > 0)
        {
            renderer.flipX = false;
        }
        else if (HeldMoveAxis < 0)
        {
            renderer.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
