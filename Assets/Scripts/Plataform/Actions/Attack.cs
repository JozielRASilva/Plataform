using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(StatsValue))]
public class Attack : MonoBehaviour
{

    public ColliderArea areaOnLeft;
    public ColliderArea areaOnRight;

    MoveController move;

    Stats stats;

    private void Start()
    {
        move = GetComponent<MoveController>();
        stats = GetComponent<Stats>();
    }

    private void Update()
    {
        if (move.direction)
        {
            if (areaOnRight != null)
            {
                if (areaOnRight.isColliding())
                {
                    List<Collider2D> collider2Ds = areaOnRight.GetColliding();
                    foreach (var col in collider2Ds)
                    {
                        DoAttack(col.gameObject.GetComponent<Stats>());
                    }
                }
            }

        }
        else
        {
            if (areaOnLeft != null)
            {
                if (areaOnLeft.isColliding())
                {
                    List<Collider2D> collider2Ds = areaOnLeft.GetColliding();
                    foreach (var col in collider2Ds)
                    {
                        DoAttack(col.gameObject.GetComponent<Stats>());
                    }
                }
            }

        }
    }

    public void DoAttack(Stats other)
    {
        other.TakeDamage(stats.damage.GetValue());
    }


}
