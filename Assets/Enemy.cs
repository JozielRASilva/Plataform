using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColliderArea))]
[RequireComponent(typeof(MoveController))]
public class Enemy : MonoBehaviour
{

    SpriteRenderer sprite;

    public int playerInt;
    public LayerMask enemyInt;

    public LayerMask isSolid;

    ColliderArea colliders;

    public bool direction;

    public bool spawned;

    MoveController move;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        Physics2D.IgnoreLayerCollision(playerInt, gameObject.layer);
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        colliders = GetComponent<ColliderArea>();

        move = GetComponent<MoveController>();
    }

    private void Update()
    {

        Walk();

    }

    private void Walk()
    {
        Vector4 distances = Checkdirections();

        if (direction)
        {

            move.Move(Vector2.right);

            if (distances.y <= transform.localScale.x / 2 + 0.05f)
            {
                direction = false;
            }


        }
        else
        {
            move.Move(Vector2.left);

            if (distances.x <= transform.localScale.x / 2 + 0.05f)
            {
                direction = true;
            }

        }
    }

    private Vector4 Checkdirections()
    {

        Vector4 direcao = new Vector4();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, isSolid);
        if (hit)
        {
            Debug.DrawLine(hit.point, transform.position, Color.red);
            direcao.x = Vector2.Distance(hit.point, transform.position);
        }
        hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, isSolid);
        if (hit)
        {
            Debug.DrawLine(hit.point, transform.position, Color.red);
            direcao.y = Vector2.Distance(hit.point, transform.position);
        }
        hit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, isSolid);
        if (hit)
        {
            Debug.DrawLine(hit.point, transform.position, Color.red);
            direcao.z = Vector2.Distance(hit.point, transform.position);
        }
        hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, isSolid);
        if (hit)
        {
            Debug.DrawLine(hit.point, transform.position, Color.red);
            direcao.w = Vector2.Distance(hit.point, transform.position);
        }

        return direcao;
    }


}
