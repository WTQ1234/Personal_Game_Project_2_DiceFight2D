using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HRL
{
    public class UI_Comp_Item_Slot : MonoBehaviour
    {
        public Image img_icon;

        public Item item;

        public bool isEmpty
        {
            get { return (item != null); }
        }

        private void Start()
        {
            Refresh();
        }

        public void SetItem(Item item)
        {
            this.item = item;
            Refresh();
        }

        private void Refresh()
        {
            if (item != null)
            {
                img_icon.gameObject.SetActive(true);
                img_icon.sprite = item.info.Icon;
            }
            else
            {
                img_icon.gameObject.SetActive(false);
            }
        }
    }
}