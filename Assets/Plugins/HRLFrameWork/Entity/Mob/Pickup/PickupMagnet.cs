using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMagnet : MonoBehaviour
{
    public Player owner;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickupItem"))
        {
            if (owner == null)
            {
                return;
            }
            if (!owner.CanPickUpItem())
            {
                return;
            }
            PickupItem coinItem = other.GetComponent<PickupItem>();
            if (coinItem != null)
            {
                coinItem.Collect(owner);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float radius = GetComponent<CircleCollider2D>().radius;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
