using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour, IPlataform
{
    Collider2D[] colliders;

    void Start()
    {
        colliders = GetComponents<Collider2D>();    
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        OnOver(collision.collider);
    }

    

    public IEnumerator PassDown(Collider2D other, float timeToDown)
    {

        foreach (var collider in colliders)
        {
            Physics2D.IgnoreCollision(collider, other, true);
        }

        yield return new WaitForSeconds(timeToDown);

        foreach (var collider in colliders)
        {
            Physics2D.IgnoreCollision(collider, other, false);
        }

    }

    public void OnOver(Collider2D collider) {}



}
