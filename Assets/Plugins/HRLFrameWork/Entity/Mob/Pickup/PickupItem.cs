using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class PickupItem : Entity
{
    public PickupInfo info;

    public int extra_data_weapon_durable = -1;

    public float max_fall_speed = 4f;

    public float speed = 3;
    public bool isCollected;
    public Entity collector;

    private Rigidbody2D rigidbody2D;

    public GameObject effect_white;
    public GameObject effect_green;
    public GameObject effect_gold;
    public GameObject effect_red;

    protected override void Awake()
    {
        base.Awake();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if (isCollected)
        {
            if (collector != null)
            {
                float distance = (transform.position - collector.transform.position).sqrMagnitude;

                transform.position = Vector3.MoveTowards(transform.position,
                    collector.transform.position, speed * Time.deltaTime);
            }
        }

        if (rigidbody2D != null)
        {
            if (rigidbody2D.velocity.y < - max_fall_speed)
            {
                rigidbody2D.gravityScale = 0;
            }
        }

        if (transform.position.y < -20)
        {
            LogHelper.Info("Destroy", "Destroy GameObject", transform.name, transform.position);
            Destroy(gameObject);
        }
    }

    public void Collect(Entity _collector)
    {
        isCollected = true;
        collector = _collector;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void SetPickupInfo(PickupInfo _pickupInfo)
    {
        info = _pickupInfo;

        if (_pickupInfo.weaponInfo != null)
        {
            switch(_pickupInfo.weaponInfo.quality)
            {
                case WeaponQuality.white:
                    effect_white.gameObject.SetActive(true);
                    effect_green.gameObject.SetActive(false);
                    effect_gold.gameObject.SetActive(false);
                    effect_red.gameObject.SetActive(false);
                    break;
                case WeaponQuality.green:
                    effect_white.gameObject.SetActive(false);
                    effect_green.gameObject.SetActive(true);
                    effect_gold.gameObject.SetActive(false);
                    effect_red.gameObject.SetActive(false);
                    break;
                case WeaponQuality.gold:
                    effect_white.gameObject.SetActive(false);
                    effect_green.gameObject.SetActive(false);
                    effect_gold.gameObject.SetActive(true);
                    effect_red.gameObject.SetActive(false);
                    break;
                case WeaponQuality.red:
                    effect_white.gameObject.SetActive(false);
                    effect_green.gameObject.SetActive(false);
                    effect_gold.gameObject.SetActive(false);
                    effect_red.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public void SetSprite()
    {

    }
}
