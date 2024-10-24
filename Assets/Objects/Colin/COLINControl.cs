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

    private bool charging = false;
    private bool chargingCool = false;

    private bool startCharging = false;

    float chargeTickMax = 21f;
    float chargeTick = 0;

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

        charging = false;
    }

    public void Charge()
    {
        charging = true;
        chargeTick = 0;
        rigidbody.velocityX = 0;
    }

    int DirectionMultiplier()
    {
        return (renderer.flipX ? -1 : 1);
    }

    public void Knock()
    {
        PlayAnim("JUMP");
        rigidbody.velocityX = 0;
    }

    void FixedUpdate()
    {
        // Time charging
        if (charging)
        {
            if (chargeTick++ >= chargeTickMax)
            {
                if (rigidbody.velocityY == 0 || rigidbody.velocityX == 0)
                {
                    chargeTick = 0;
                    charging = false;
                    startCharging = false;
                    rigidbody.velocityX = 0;
                    chargingCool = true;
                }
            }
        }

        // Cool down after charge
        if (chargingCool)
        {
            if (chargeTick++ >= (chargeTickMax - 2) / 2)
            {
                chargeTick = 0;
                chargingCool = false;
            }
        }
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
        if (HeldMoveKey && !charging)
        {
            // Get direction
            Vector2 direction = rigidbody.transform.right * HeldMoveAxis;
            rigidbody.position = Vector3.MoveTowards(rigidbody.position, rigidbody.position + direction, movingSpeed * Time.deltaTime);
        }

        // CHARGE
        if (Input.GetButtonDown("Fire1") && !charging && !chargingCool)
        {
            Charge();
        }

        if (charging)
        {
            rigidbody.velocityX = (movingSpeed * 2) * DirectionMultiplier();
        }

        // ANIMATION
        if (charging)
        {
            PlayAnim("CHARGE");
        }
        else
        {
            if (isGrounded)
            {
                if (HeldMoveKey)
                {
                    PlayAnim("RUN");
                }
                else
                {
                    PlayAnim("IDLE");
                }
            }
            else if (!isGrounded)
            {
                if (didSuperJump)
                {
                    PlayAnim("JUMP");
                }
                else
                {
                    if (rigidbody.velocity.y < 0.2)
                    {
                        PlayAnim("FALL");
                    }
                    else
                    {
                        PlayAnim("JUMP");
                    }
                }
            }
        }

        if (PressedJump && isGrounded)
        {
            if (HeldUpKey && !HeldMoveKey)
            {
                rigidbody.AddForce(transform.up * (jumpForce * 1.34f), ForceMode2D.Impulse);
                didSuperJump = true;
                isGrounded = false;
            }
            else
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        if (isGrounded && didSuperJump && rigidbody.velocity.y < 0) didSuperJump = false;

        // FLIP DIRECTION
        if (!charging)
        {
            if (HeldMoveAxis > 0)
            {
                renderer.flipX = false;
            }
            else if (HeldMoveAxis < 0)
            {
                renderer.flipX = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
