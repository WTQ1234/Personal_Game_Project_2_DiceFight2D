using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;
using Sirenix.OdinInspector;

public class PickupItemManager : MonoSingleton<PickupItemManager>
{
    public bool start;

    public float tick_time = 0.3f;
    public float current_time = 0f;

    public int max_num_once = 3;

    public float each_precent = 0.2f;

    public PickupItem prefab;

    public List<PickupInfo> pickupInfo_list = new List<PickupInfo>();

    [ShowInInspector]
    public Dictionary<int, PickupInfo> pickup_dict;
    public List<int> weapon_white_list;
    public List<int> weapon_green_list;
    public List<int> weapon_gold_list;

    [Title("ÎäÆ÷³Ø¸ÅÂÊ")]
    public float weapon_precent = 1f;

    public float weapon_precent_white = 0.5f;
    public float weapon_precent_green = 0.3f;
    public float weapon_precent_gold = 0.2f;

    public float init_pos_X_range = 12;
    public float init_pos_Y = 15;
    public float init_pos_Z = 0;

    protected override void Awake()
    {
        pickup_dict = ConfigManager.Instance.GetAllInfo<PickupInfo>();
        foreach(var kv in pickup_dict)
        {
            if (kv.Value.type == PickupType.Weapon)
            {
                switch (kv.Value.weaponInfo.quality)
                {
                    case WeaponQuality.white:
                        weapon_white_list.Add(kv.Key);
                        break;
                    case WeaponQuality.green:
                        weapon_green_list.Add(kv.Key);
                        break;
                    case WeaponQuality.gold:
                        weapon_gold_list.Add(kv.Key);
                        break;
                }
            }
        }
    }

    void Update()
    {
        current_time += Time.deltaTime;
        if (current_time > tick_time)
        {
            current_time = 0f;
            Tick();
        }
    }

    [Button("Tick")]
    void Tick()
    {
        for (int i = 0; i < max_num_once; i++)
        {
            float ran = Random.Range(0f, 1f);
            if (ran > each_precent)
            {
                _CreatePickupItemFromSky();
            }
        }
    }

    public void CreatePickUpItemDrop(Vector3 pos, PickupInfo info,
        int extra_data_weapon_durable = -1)
    {
        PickupItem item = _CreatePickUpItem(info);
        item.transform.position = pos;
        item.extra_data_weapon_durable = extra_data_weapon_durable;
    }

    private void _CreatePickupItemFromSky()
    {
        PickupInfo info;
        float ran = Random.Range(0f, 1f);
        if (ran < weapon_precent_white)
        {
            int index = Random.Range(0, weapon_white_list.Count);
            info = pickup_dict[weapon_white_list[index]];
        }
        else if (ran < weapon_precent_white + weapon_precent_green)
        {
            int index = Random.Range(0, weapon_green_list.Count);
            info = pickup_dict[weapon_green_list[index]];
        }
        else
        {
            int index = Random.Range(0, weapon_gold_list.Count);
            info = pickup_dict[weapon_gold_list[index]];
        }
        PickupItem item = _CreatePickUpItem(info);
        float init_pos_x = Random.Range(-init_pos_X_range, init_pos_X_range);
        item.transform.position = new Vector3(transform.position.x + init_pos_x, init_pos_Y, init_pos_Z);
        item.extra_data_weapon_durable = -1;
    }

    private PickupItem _CreatePickUpItem(PickupInfo info)
    {
        PickupItem pickupItem = Instantiate<PickupItem>(prefab);
        pickupItem.SetPickupInfo(info);
        return pickupItem;
    }

    public void ClearPickUpItem()
    {
        Messenger.Instance.BroadcastMsg("ClearPickUp");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x - init_pos_X_range, init_pos_Y, init_pos_Z), new Vector3(transform.position.x + init_pos_X_range, init_pos_Y, init_pos_Z));
    }
}
