using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPlataform : MonoBehaviour
{

    public float timeToDown = 0.2f;

    private Collider2D collider2d;

    private void Start()
    {
        collider2d = GetComponent<Collider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnOver(collision.collider);
    }

    public void OnInteract(Collider2D collider)
    {
        if (collider.CompareTag("Plataform"))
        {
            IPlataform plataform = collider.GetComponent<IPlataform>();

            StartCoroutine(plataform.PassDown(collider2d, timeToDown));

        }
    }

    public void OnOver(Collider2D collider)
    {
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            OnInteract(collider);

        }
    }

}
