using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;
//using PixelCrushers.DialogueSystem;

public class UIScreen_Bag : UIScreen
{
    public List<UI_Comp_Item_Slot> item_slots_list = new List<UI_Comp_Item_Slot>();

    protected override void Awake()
    {
        base.Awake();
        ////Messenger.Instance.AddListener("ItemChange", RefreshItems);
    }

    private void RefreshItems()
    {

    }
}
