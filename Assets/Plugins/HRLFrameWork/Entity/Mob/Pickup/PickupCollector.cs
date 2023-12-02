using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollector : MonoBehaviour
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
            PickupItem pickupItem = other.GetComponent<PickupItem>();
            if (pickupItem != null)
            {
                if (pickupItem.info != null)
                {
                    switch (pickupItem.info.type)
                    {
                        case PickupType.Weapon:
                            owner.PickUpItem(pickupItem);
                            //SoundManager.PlayPickCoinClip();
                            break;
                    }
                }
            }
            Destroy(other.gameObject);
        }
    }
}
