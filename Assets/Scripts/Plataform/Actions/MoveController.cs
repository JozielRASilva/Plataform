using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Iinput))]
public class MoveController : MonoBehaviour, IMove
{
    private Rigidbody2D rigidbody2d;

    public float speed = 5f;

    public bool direction;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }



    public void Move(Vector2 directions)
    {
        if (directions.x != 0)
        {
            rigidbody2d.velocity = new Vector2(directions.x * speed, rigidbody2d.velocity.y);

            if (rigidbody2d.velocity.x > 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                direction = true;
            }
            else if (rigidbody2d.velocity.x < 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
                direction = false;
            }

        }
        else
        {
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
        }
    }



}
