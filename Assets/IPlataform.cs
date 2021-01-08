using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlataform
{

    IEnumerator PassDown(Collider2D other, float timeToDown);
    void OnOver(Collider2D collider);
}
