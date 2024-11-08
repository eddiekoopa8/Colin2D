using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CREATEBrain : MonoBehaviour
{
    Animator animator;
    Collider2D collider;
    Rigidbody2D body;
    bool function = true;
    int deadTime = 0;
    int deadTimeMax = 20;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (function == false)
        {
            if (deadTime++ >= deadTimeMax)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (function == false) { return; }
        if (COLINControl.Verify(col.gameObject))
        {
            Debug.Log("COL!");
            if (COLINControl.GetInstance(col.gameObject).Charging())
            {
                Debug.Log("COL GOOD!");
                COLINControl.GetInstance(col.gameObject).Knock();
                GetComponent<AudioSource>().Play();
                animator.Play("DEAD");
                collider.isTrigger = true;
                collider.enabled = false;
                body.gravityScale = 0;
                function = false;
            }
        }
    }
}
