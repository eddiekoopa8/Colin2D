using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPRNGBrain : MonoBehaviour
{
    int cooldownMax = 12;
    int cooldown = 0;
    public float BounceForce = 40f;
    void FixedUpdate()
    {
        if (cooldown < cooldownMax)
        {
            cooldown++;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D body = col.gameObject.GetComponent<Rigidbody2D>();
        if (body != null && cooldown >= cooldownMax)
        {
            body.velocityY = 0;
            body.AddForce(body.transform.up * BounceForce, ForceMode2D.Impulse);
            GetComponent<Animator>().Play("Bounce");
            GetComponent<AudioSource>().Play();
            cooldown = 0;
        }
    }
}
