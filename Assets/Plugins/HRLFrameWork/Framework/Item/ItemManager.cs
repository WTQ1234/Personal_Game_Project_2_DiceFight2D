using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;
using Sirenix.OdinInspector;

[SerializeField]
public class Item
{
    public int id;
    public int num;
    public ItemInfo info;
}

public class ItemManager : MonoSingleton<ItemManager>
{
    [ShowInInspector]
    public List<Item> items = new List<Item>();

    public void AddItem(ItemInfo itemInfo, int num)
    {
        Item item = _GetItemByInfo(itemInfo);
        if (item == null)
        {
            item = new Item();
            item.id = items.Count;
            item.num = num;
            item.info = itemInfo;
            items.Add(item);
        }
        else
        {
            item.num += num;
        }
    }

    public void UseItem()
    {

    }

    private Item _GetItemById(int index)
    {
        return items[index];
    }

    private Item _GetItemByInfo(ItemInfo itemInfo)
    {
        foreach (Item item in items)
        {
            if (item.info.Id == itemInfo.Id)
            {
                return item;
            }
        }
        return null;
    }

    private int _GetItemIdByItemInfo(ItemInfo itemInfo)
    {
        Item item = _GetItemByInfo(itemInfo);
        if (item == null)
        {
            return -1;
        }
        return item.id;
    }
}
