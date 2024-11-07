using System.Drawing;
using UnityEngine;
public class ACTORController : MonoBehaviour
{
    protected bool isGrounded = false;
    protected bool isLeft = false;
    protected bool isRight = false;

    protected bool alwaysActive = false;

    //public Transform groundCheckLeft;
    //public Transform groundCheckRight;

    Vector3 groundLeft = Vector3.zero;
    Vector3 groundRight = Vector3.zero;

    Vector3 leftUp = Vector3.zero;
    Vector3 leftDown = Vector3.zero;

    Vector3 rightUp = Vector3.zero;
    Vector3 rightDown = Vector3.zero;

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

        if (alwaysActive == false)
        {
            enabled = false;
        }
    }

    public virtual void ActorStart()
    {
        Debug.Log("ACTOR HAS NO INIT");
    }

    private bool touchingLine(Vector3 b, Vector3 e)
    {
        Collider2D[] cl0;
        Collider2D[] cl1;

        cl0 = Physics2D.OverlapCircleAll(b, 0.7f);
        cl1 = Physics2D.OverlapCircleAll(e, 0.7f);

        return cl0.Length > 1 || cl1.Length > 1;
    }

    private void Update()
    {
        if (isActiveAndEnabled == false)
        {
            return;
        }
        // This part took so long to figure out. the axis in unity is so confusing. I still am not 100% how this fully works.

        // For ground
        groundLeft = rigidbody.position + collide.offset;
        groundLeft.x -= 1.3f;
        groundLeft.y -= collide.size.y - 2.8f;

        groundRight = groundLeft;
        groundRight.x += collide.size.x - (1.1f * 2) - 0.3f;

        // For left
        leftUp = rigidbody.position + collide.offset;
        leftUp.x -= 1.2f;
        leftDown = leftUp;
        leftDown.y -= collide.size.y - 2.6f;

        // For left
        rightUp = rigidbody.position + collide.offset;
        rightUp.x += collide.size.x - (1.1f * 2) - 0.2f;
        rightDown = rightUp;
        rightDown.y -= collide.size.y - 2.6f;

        /*if (GameObject.Find("ILoveDebugging") != null && GameObject.Find("VeryNotPainful") != null)
        {
            GameObject.Find("ILoveDebugging").transform.position = groundLeft;
            GameObject.Find("VeryNotPainful").transform.position = groundRight;
        }*/

        isGrounded = touchingLine(groundLeft, groundRight);
        isLeft = touchingLine(leftUp, leftDown);
        isRight = touchingLine(rightUp, rightDown);

        // DEBUGGING PURPOSES
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("INFO");
            Debug.Log("========================");
            Debug.Log("rigid " + rigidbody.position);
            Debug.Log("groundLeft " + groundLeft);
            Debug.Log("groundRight " + groundRight);
            Debug.Log("collide.offset " + collide.offset);
            Debug.Log("collide.size " + collide.size);
            Debug.Log("isGrounded " + isGrounded);
            Debug.Log("rigidbody.velocity " + rigidbody.velocity);
            Debug.Log("========================");
        }

        ActorUpdate();
    }

    public virtual void ActorUpdate()
    {

    }

    void OnBecameVisible()
    {
        if (alwaysActive == false)
        {
            enabled = true;
        }
    }
}