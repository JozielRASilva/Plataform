using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderArea : MonoBehaviour
{

    public List<ColSquare> squares;
    public List<ColCircle> circles;

    public bool showGizmos = true;

    public LayerMask canCallCollider;

    private void Update()
    {
        
    }

    public bool isColliding()
    {

        bool collide = false;
        foreach (var square in squares)
        {

            Collider2D [] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + square.position, square.size, 1f, canCallCollider);

            if (colliders.Length > 0)
            {
                square.color = Color.red;
                collide = true;
            }
            else if (colliders.Length == 0)
            {
                square.color = Color.white;
            }

        }

        foreach (var circle in circles)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + circle.position, circle.radius, canCallCollider);

            if (colliders.Length > 0)
            {
                circle.color = Color.red;
                collide = true;
            }
            else if (colliders.Length == 0)
            {
                circle.color = Color.white;
            }
        }

        return collide;
    }

    public List<Collider2D> GetColliding()
    {

        List<Collider2D> colliders2D = new List<Collider2D>();
        foreach (var square in squares)
        {

            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + square.position, square.size, 1f, canCallCollider);

            if (colliders.Length > 0)
            {
                square.color = Color.red;
                foreach (var col in colliders)
                {
                    colliders2D.Add(col);
                }
            }
            else if (colliders.Length == 0)
            {
                square.color = Color.white;
            }

        }

        foreach (var circle in circles)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + circle.position, circle.radius, canCallCollider);

            if (colliders.Length > 0)
            {
                circle.color = Color.red;
                foreach (var col in colliders)
                {
                    colliders2D.Add(col);
                }
            }
            else if (colliders.Length == 0)
            {
                circle.color = Color.white;
            }
        }

        return colliders2D;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            foreach(var square in squares)
            {
                Gizmos.color = square.color;
                Gizmos.DrawWireCube((Vector2)transform.position + square.position, square.size);
            }

            foreach (var circle in circles)
            {

                Gizmos.color = circle.color;
                Gizmos.DrawWireSphere(circle.position + (Vector2)transform.position, circle.radius);

            }

        }
    }


}

[System.Serializable]
public class Col
{
    public Vector2 position;
    public Color color = Color.white;
}
[System.Serializable]
public class ColSquare : Col
{
    public Vector2 size;
}
[System.Serializable]
public class ColCircle : Col
{
    public float radius;
}