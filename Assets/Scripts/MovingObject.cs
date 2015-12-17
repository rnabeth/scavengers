using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer; //To check colision

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime; //To make our movement calculation more efficient

    // Use this for initialization
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime; //So we can use it by multiplying instead of diving, which is more efficient computationally
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position; //Implicitly convert Vector3 to Vector2 discarding the Z axis
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false; //To make sure that when we are casting our ray we are not going to hit our own collider
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude; //sqrMagnitude is computationally cheaper than magnitude

        while (sqrRemainingDistance > float.Epsilon) //Epsilon is a very small number, almost zero
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null; //Wait for a frame before reevaluating the condition of the loop
        }
    }

    //T is used to specify the type of component we expect our unit to interact with in case it's blocked
    //In case of enemy this is a player
    //In case of a player this is going to be walls so the player can attack and destroy them
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null) //Moving object is blocked and hit something that he can interact with
            OnCantMove(hitComponent);
    }

    //Player is going to need to be able to interact with walls
    //Enemy is going to need to be able to interact with the player
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}
