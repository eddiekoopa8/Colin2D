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
    /// Hmmm IS this object Colin?...
    /// </summary>
    public static bool Verify(GameObject obj)
    {
        return obj.GetComponent<COLINControl>() != null;
    }
    
    public static COLINControl GetInstance(GameObject obj)
    {
        return obj.GetComponent<COLINControl>();
    }

    /// <summary>
    /// This function shouldn't be static..
    /// But whatever. Checks if the player is charging.
    /// </summary>
    public static bool Charging(GameObject obj)
    {
        return obj.GetComponent<COLINControl>().charging;
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

        charging = false;
    }

    /// <summary>
    /// Make the player charge.
    /// </summary>
    public void Charge()
    {
        charging = true;
        chargeTick = 0;
        rigidbody.velocityX = 0;
    }

    /// <summary>
    /// For direction handling.
    /// </summary>
    int DirectionMultiplier()
    {
        return (renderer.flipX ? -1 : 1);
    }

    /// <summary>
    /// This function is very anti-climatic.
    /// </summary>
    public void Knock()
    {
        PlayAnim("FALL");
        rigidbody.velocityX = 0;
        rigidbody.AddForce(rigidbody.transform.up * 8, ForceMode2D.Impulse);
        charging = false;
    }

    void FixedUpdate()
    {
        // Time charging
        if (charging)
        {
            if (chargeTick++ >= chargeTickMax)
            {
                if (rigidbody.velocityY == 0 && isGrounded == false)
                {
                    isGrounded = true;
                }
                if (isGrounded)
                {
                    // reset everything charge related (and velocity)
                    chargeTick = 0;
                    charging = false;
                    startCharging = false;
                    rigidbody.velocityX = 0;
                    rigidbody.velocity = Vector3.zero;
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
        bool PressedJump = Input.GetButtonDown("Jump"); // presed up
        bool HeldMoveKey = Input.GetButton("Horizontal"); // pressed movement
        float HeldMoveAxis = Input.GetAxis("Horizontal"); // held movement radius stuff
        bool HeldUpKey = Input.GetAxis("Vertical") > 0.1f; // if holding up

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
        if (charging) // if charging, only charge animation
        {
            PlayAnim("CHARGE");
        }
        else
        {
            if (isGrounded) // on land?
            {
                if (HeldMoveKey)
                {
                    PlayAnim("RUN"); // run animation on move
                }
                else
                {
                    PlayAnim("IDLE"); // idle animation when... idle
                }
            }
            else if (!isGrounded)
            {
                // TODO: `didSuperJump` is probably working now.
                // if so, replace the animation with super jump animation
                if (didSuperJump)
                {
                    PlayAnim("IDLE");
                }
                else
                {
                    // if falling, fall animation
                    if (rigidbody.velocity.y < 0.2)
                    {
                        PlayAnim("FALL");
                    }
                    else
                    {
                        // otherwise, jump animation
                        PlayAnim("JUMP");
                    }
                }
            }
        }

        // JUMPING
        if (PressedJump && isGrounded)
        {
            // High jump?
            if (HeldUpKey && !HeldMoveKey)
            {
                rigidbody.AddForce(rigidbody.transform.up * (jumpForce * 1.34f), ForceMode2D.Impulse);
                didSuperJump = true; // for animation
                isGrounded = false;
            }
            else
            {
                rigidbody.AddForce(rigidbody.transform.up * jumpForce, ForceMode2D.Impulse);
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
