using UnityEngine;
public class ACTORController : MonoBehaviour
{
    protected bool isGrounded = false;

    //public Transform groundCheckLeft;
    //public Transform groundCheckRight;

    Vector3 checkLeft = Vector3.zero;
    Vector3 checkRight = Vector3.zero;

    protected Rigidbody2D rigidbody = new Rigidbody2D();
    protected SpriteRenderer renderer = new SpriteRenderer();
    protected BoxCollider2D collide = new BoxCollider2D();

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collide = GetComponent<BoxCollider2D>();

        ActorStart();

        checkLeft = collide.offset;
        checkLeft.y += collide.size.y;
        checkRight = collide.offset;
        checkRight.x += collide.size.x;
        checkRight.y += collide.size.y;

        Debug.Log(collide.offset);
        Debug.Log(collide.size);
        Debug.Log(name + " actor is ready.");
    }

    public virtual void ActorStart()
    {
        Debug.Log("ACTOR HAS NO INIT");
    }

    private bool touchingLine(Vector3 b, Vector3 e)
    {
        Collider2D[] cl0 = Physics2D.OverlapCircleAll(b, 0.7f);
        Collider2D[] cl1 = Physics2D.OverlapCircleAll(e, 0.7f);
        return cl0.Length > 1 || cl1.Length > 1;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(checkLeft, checkRight);
    }

    private void Update()
    {
        //isGrounded = touchingLine(groundCheckLeft.transform.position, groundCheckRight.transform.position);
        isGrounded = touchingLine(checkLeft, checkRight);
        ActorUpdate();
    }

    public virtual void ActorUpdate()
    {

    }
}