using UnityEngine;
public class ACTORController : MonoBehaviour
{
    protected bool isGrounded = false;

    public Transform groundCheckLeft;
    public Transform groundCheckRight;

    protected Rigidbody2D rigidbody = new Rigidbody2D();
    protected SpriteRenderer renderer = new SpriteRenderer();
    protected BoxCollider2D collide = new BoxCollider2D();

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collide = GetComponent<BoxCollider2D>();

        ActorStart();

        Debug.Log(name + " actor is ready.");
    }

    public virtual void ActorStart()
    {

    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckLeft.transform.position, 0.7f);
        isGrounded = colliders.Length > 1;

        colliders = Physics2D.OverlapCircleAll(groundCheckRight.transform.position, 0.7f);
        if (!isGrounded) isGrounded = colliders.Length > 1;

        ActorUpdate();
    }

    public virtual void ActorUpdate()
    {

    }
}