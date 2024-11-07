using UnityEngine;

public class MOVINGController : MonoBehaviour
{
    public Transform platform; // reference for the platform
    public Transform startPoint; // reference for the start point
    public Transform endPoint; // reference for the end point
    public float speed = 1.5f; // speed between points
    public int direction = 1; // direction of travel 1 or -1

    // Function to show the lines between points
    private void OnDrawGizmos()
    {
        // just for
        if (platform != null && startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.position);
        }
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 target = currentMovementTarget();
        platform.position = Vector2.MoveTowards(platform.position, target, speed * Time.deltaTime);
        float distance = (target - (Vector2)platform.position).magnitude;
        if (distance <= 0.1f)
        {
            direction *= -1;
        }
    }

    // function to set the direction of travel
    Vector2 currentMovementTarget()
    {
        if (direction == 1)
        {
            return startPoint.position;
        }
        else
        {
            return endPoint.position;
        }
    }


    // Ensure player becomes a child of the platform on collision
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == COLINControl.Verify(col.gameObject))
        {
            col.gameObject.transform.SetParent(platform.transform, true);
        }
    }

    // Ensure player stops following the platform when exiting collision
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject == COLINControl.Verify(col.gameObject))
        {
            if (col.gameObject.transform.parent == platform.transform)
            {
                col.gameObject.transform.SetParent(null, true);
            }
        }
    }


}


