using Game.Core.Traps;
using Game.UI.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Traps
{
    public class TrapItemEditor : MonoBehaviour
    {
        private PlayerType playerType;
        private Dictionary<ItemType, TrapsItem> itemData;
        public int MAXCOUNT;
        public List<TrapsItem> canUseItem;
        public List<TrapsItem> placeItems;
        public TrapsItem currentItem;
        public int lastSelected;
        public int selectedIndex;
        private SubView_Editor editorUI;
        private float CDTime;
        private float currentCD;
        private void Awake()
        {
            itemData = new Dictionary<ItemType, TrapsItem>();
            CDTime = 1f;
            currentCD = 0f;
            for (int i = 0; i < 7; i++)
            {
                ItemType itemType = (ItemType)i;
                GameObject item = transform.Find($"{itemType}").gameObject;
                TrapsItem trap = item.GetComponent<TrapsItem>();
                item.SetActive(false);
                itemData.Add(itemType, trap);
                //Debug.Log($"{itemType}������Ʒ:{itemPrefab.name}");
            }

            MAXCOUNT = 3;
            lastSelected = 0;
            selectedIndex = 0;

            Init_GetItem();
        }

        private void Init_GetItem()
        {
            editorUI = (UI.ViewManager.GetView(UI.UIViewType.Combat) as CombatUI).view_Editor;
            canUseItem = new List<TrapsItem>();
            placeItems = new List<TrapsItem>();
            for (int i = 0; i < MAXCOUNT; i++)
            {
                TrapsItem item;
                do
                {
                    item = GetItem((ItemType)RollNumber.RandomValue());
                }
                while (canUseItem.Contains(item));
                canUseItem.Add(item);
            }
        }
        /// <summary>
        /// ��ȡ�ɷ��õ���, ����������Ʒ
        /// </summary>
        /// <param name="itemType">��Ʒ����</param>
        /// <returns>������Ʒ</returns>
        public TrapsItem GetItem(ItemType itemType)
        {
            TrapsItem item = itemData[itemType];
            return item;
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="changeItemCode">�л���Ʒ����</param>
        /// <param name="placeItemCode">������Ʒ����</param>
        public void OpenPlayerInput(KeyCode changeItemCode, KeyCode placeItemCode)
        {
            switch(changeItemCode)
            {
                case KeyCode.S: { playerType = PlayerType.Player_2; }break;
                case KeyCode.DownArrow: { playerType = PlayerType.Player_1; }break;
            }
            StartCoroutine(PlayerInput(changeItemCode, placeItemCode));
        }
        IEnumerator PlayerInput(KeyCode changeItemCode, KeyCode placeItemCode)
        {
            //������ȴ���ΪInEditor, ����Ŀǰ����ReadyEnter
            yield return new WaitUntil(() => CombatManager.Instance.fightState == FightState.InEditor);
            CheckItemsInfo();
            ChangeItemClicked();
            while (CombatManager.Instance.fightState == FightState.InEditor)
            {
                currentCD += Time.deltaTime;
                if (Input.GetKeyDown(changeItemCode) & currentCD > CDTime) //�л�����
                    ChangeItemClicked();

                if (Input.GetKeyDown(placeItemCode) & currentCD > CDTime) //���õ���
                    PlaceItemClicked();

                yield return null;
            }

            //���༭��ģʽ������ �л���Ʒ������ƻᱻ����, ��ʼ����δ�����õĵ��߲������Ѿ����õĵ���
            yield return ClearUnUseItemAndEnablePlaceItem();
        }
        private void ChangeItemClicked()
        {
            lastSelected = selectedIndex;
            selectedIndex++;
            if (canUseItem.Count != 0)
            {
                if (selectedIndex == canUseItem.Count)
                    selectedIndex = 0;
                canUseItem[lastSelected].body.SetActive(false);
                canUseItem[selectedIndex].body.SetActive(true);
                currentItem = canUseItem[selectedIndex];
            }
            currentCD = 0f;
        }
        private void PlaceItemClicked()
        {
            if (canUseItem.Count != 0 && currentItem != null) //������ڿɷ��õĵ���
            {
                TrapsItem placeItem = canUseItem[selectedIndex];
                Vector3 origPos = placeItem.body.transform.position;
                Quaternion rotation = placeItem.body.transform.rotation;
                Vector3 origScale = placeItem.body.transform.lossyScale;

                placeItem.body.transform.parent = null;

                placeItem.body.transform.position = origPos;
                placeItem.body.transform.rotation = rotation;
                placeItem.body.transform.localScale = origScale;

                currentItem = null;
                canUseItem.Remove(placeItem);
                placeItems.Add(placeItem);
                placeItem.ResetColliderState();
                CombatManager.Instance.AddTrapItem(placeItem);
                placeItem.isPlace = true;//��ɷ���

                lastSelected = 0;
                selectedIndex = 0;

                lastSelected = selectedIndex;
                selectedIndex++;
                if (canUseItem.Count != 0)
                {
                    if (selectedIndex == canUseItem.Count)
                        selectedIndex = 0;
                    canUseItem[lastSelected].body.SetActive(false);
                    canUseItem[selectedIndex].body.SetActive(true);
                    currentItem = canUseItem[selectedIndex];
                }

                CheckItemsInfo();
                currentCD = 0f;
            }

        }
        private void CheckItemsInfo()
        {
            if (canUseItem.Count <= 0)
            {
                editorUI.SetCurrentItemInfo(playerType, "Traps have been placed");
                return;
            }
            string message = "Traps��";
            for (int i = 0; i < canUseItem.Count; i++)
            {
                message += canUseItem[i].name+"��";
            }
            editorUI.SetCurrentItemInfo(playerType, message);
        }
        private void OnDestroy()
        {
            currentItem = null;
            canUseItem.Clear();
            placeItems.Clear();
            StopAllCoroutines();
        }
        /// <summary>
        /// ���û��ʹ�õĵ��߲������Ѿ����õĵ���
        /// </summary>
        IEnumerator ClearUnUseItemAndEnablePlaceItem()
        {
            currentItem = null;
            for (int i = 0; i < placeItems.Count; i++)
                placeItems[i].UnLock();

            for (int i = 0; i < placeItems.Count; i++)
            {
                placeItems.Remove(placeItems[i]);
            }
            yield return null;

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
                count = transform.childCount;
            }
            Destroy(gameObject);
        }
    }
}

public enum ItemType
{
    /// <summary>
    /// �ش�
    /// </summary>
    GroundSpike,
    /// <summary>
    /// ����
    /// </summary>
    Landmine,
    /// <summary>
    /// ֩����
    /// </summary>
    SpiderWeb,
    /// <summary>
    /// ���ɵ�
    /// </summary>
    SpringWasher,

    /// <summary>
    /// ·��1
    /// </summary>
    BigBarrier,
    /// <summary>
    /// ·��2
    /// </summary>
    SmallBarrier,
    /// <summary>
    /// ľ��
    /// </summary>
    WoodBox,
}