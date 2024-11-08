using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Assets.Scripts;
using System.Diagnostics.Contracts;

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

    bool dead = false;
    bool hadDied = false;

    bool HeldMoveKey = false;
    bool PressCharge = false;

    int health = 100;

    bool canHit = false;
    int invincible = 50;
    int invTick = 50;

    AudioSource chargeSnd;
    AudioSource jumpSnd;
    AudioSource dieSnd;
    AudioSource hurtSnd;

    public int Health { get { return health; } }

    /// <summary>
    /// Hmmm IS this object Colin?...
    /// </summary>
    /// <param name="obj">Game object</param>
    public static bool Verify(GameObject obj)
    {
        return obj.GetComponent<COLINControl>() != null;
    }

    public void PlaySound(AudioSource snd)
    {
        snd.gameObject.transform.position = transform.position;
        snd.Stop();
        snd.Play();
    }

    /// <summary>
    /// Hmmm IS this object Colin?...
    /// </summary>
    /// <param name="obj">Behaviour containing the game object</param>
    public static bool Verify(Behaviour obj)
    {
        return obj.gameObject.GetComponent<COLINControl>() != null;
    }

    /// <summary>
    /// Me when I GET you...
    /// </summary>
    /// <param name="obj">Game Object to be GET</param>
    public static COLINControl GetInstance(GameObject obj)
    {
        return obj.GetComponent<COLINControl>();
    }

    /// <summary>
    /// Me when I GET you...
    /// </summary>
    /// <param name="obj">Behaviour containing Game Object to be GET</param>
    public static COLINControl GetInstance(Behaviour obj)
    {
        return obj.gameObject.GetComponent<COLINControl>();
    }

    public void Hit(int damage)
    {
        if (canHit)
        {
            PlaySound(hurtSnd);
            health -= damage;
            if (health < 0) health = 0;
            canHit = false;
            invTick = 0;
        }
    }

    public void Heal(int nh)
    {
        health += nh;
        if (health >= 100)
        {
            health = 100;
        }
    }

    /// <summary>
    /// Checks if the player is charging.
    /// </summary>
    public bool Charging()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            return true;
        }
        else
        {
            return charging;
        }
    }

    public void Die()
    {
        dead = true;
    }

    public bool OnTop()
    {
        return isGrounded;
    }

    public void Bounce()
    {
        rigidbody.AddForce(rigidbody.transform.up * (jumpForce - 1f), ForceMode2D.Impulse);
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
        alwaysActive = true;

        animator = GetComponent<Animator>();
        fade = GameObject.Find("FADERObject").GetComponent<FADERHandler>();

        charging = false;

        chargeSnd = GameObject.Find("COLINAudioCharge").GetComponent<AudioSource>();
        jumpSnd = GameObject.Find("COLINAudioJump").GetComponent<AudioSource>();
        dieSnd = GameObject.Find("COLINAudioDie").GetComponent<AudioSource>();
        hurtSnd = GameObject.Find("COLINAudioHurt").GetComponent<AudioSource>();
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

        if (invTick++ >= invincible)
            {
            canHit = true;
            renderer.enabled = true;
        }
        else
        {
            renderer.enabled = !renderer.enabled; // flip bool
        }
    }

    /// <summary>
    /// Colin's brain function.
    /// </summary>
    public override void ActorUpdate()
    {
        bool PressedJump = Input.GetButtonDown("Jump"); // presed up
        HeldMoveKey = Input.GetButton("Horizontal"); // pressed movement
        float HeldMoveAxis = Input.GetAxis("Horizontal"); // held movement radius stuff
        bool HeldUpKey = Input.GetAxis("Vertical") > 0.1f; // if holding up
        PressCharge = Input.GetButtonDown("Fire1"); // pressed charge

        if (health <= 0)
        {
            Die();
        }

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

        if (dead)
        {
            if (hadDied == false)
            {
                PlaySound(dieSnd);

                PlayAnim("DEAD");

                LEVELBrain.GetInstance().PlayerDied();

                hadDied = true;
            }
            return;
        }

        // MOVE
        if (HeldMoveKey && !charging)
        {
            // Get direction
            Vector2 direction = rigidbody.transform.right * HeldMoveAxis;
            rigidbody.position = Vector3.MoveTowards(rigidbody.position, rigidbody.position + direction, movingSpeed * Time.deltaTime);
        }

        // CHARGE
        if (PressCharge && !charging && !chargingCool)
        {
            PlaySound(chargeSnd);
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
                    PlayAnim("JUMP");
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
                PlaySound(jumpSnd);
                rigidbody.AddForce(rigidbody.transform.up * (jumpForce * 1.34f), ForceMode2D.Impulse);
                didSuperJump = true; // for animation
                isGrounded = false;
            }
            else
            {
                PlaySound(jumpSnd);
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
