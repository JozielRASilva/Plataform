using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour, IJump
{

    private Rigidbody2D rigidbody2d;

    public float Impulse = 3f;

    public LayerMask Ground;
    public Vector2 onGroundCheck;
    public float radius = 0.2f;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();        
    }

    void Update()
    {
        Jump();
    }

    public void Jump()
    {
        if (OnGround())
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector2 force = new Vector2(0, Impulse);
                rigidbody2d.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    public bool OnGround()
    {
        Vector2 checkPosition = new Vector2(
        transform.position.x + onGroundCheck.x,
        transform.position.y + onGroundCheck.y);

        Collider2D [] collider2Ds = Physics2D.OverlapCircleAll(checkPosition, radius, Ground);

        if(collider2Ds.Length != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 checkPosition = new Vector2(transform.position.x + onGroundCheck.x, transform.position.y + onGroundCheck.y);
        Gizmos.DrawWireSphere(checkPosition, radius);
    }

}
