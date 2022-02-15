/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.Core.InventoryCollections
{
    using Opsive.UltimateInventorySystem.Core.DataStructures;
    using Opsive.UltimateInventorySystem.UI.DataContainers;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using EventHandler = Opsive.Shared.Events.EventHandler;

    /// <summary>
    /// An ItemCollection that sends items to other ItemCollections by checking where it fits.
    /// </summary>
    [Serializable]
    public class ItemTransactionCollection : ItemCollection
    {
        [Tooltip(
            "The item collection where the item will be transferred to, Make sure to add restrictions to does collections.")]
        [SerializeField] protected List<string> m_ItemCollectionNames;
        [Tooltip(
            "Should an item that gets rejected The action that should be performed if the itemCollection is about to overflow.")]
        [SerializeField] protected bool m_OverflowBackToOrigin;
        [Tooltip("The actions performed on an item that was rejected from being added.")]
        [SerializeField] protected CategoryItemActionSet m_RejectedItemActions;
        [Tooltip(
            "Return the item amount that was actually added in the right item collection or return also the amount that was rejected (in case it was added externally)?")]
        [SerializeField] protected bool m_ReturnRealAddedItemAmount;

        protected ItemCollection m_AddingToCollection;

        /// <summary>
        /// Check if the item can be added to this item collection.
        /// </summary>
        /// <param name="itemInfo">The item info to add.</param>
        /// <returns>The item info to add.</returns>
        public override ItemInfo? AddItemCondition(ItemInfo itemInfo)
        {
            for (int i = 0; i < m_ItemCollectionNames.Count; i++) {
                var itemCollection = m_Inventory.GetItemCollection(m_ItemCollectionNames[i]);
                var result = itemCollection.AddItemCondition(itemInfo);
                if (result != null && result.Value.Amount != 0) {
                    m_AddingToCollection = itemCollection;
                    return itemInfo;
                }
            }

            m_AddingToCollection = null;
            return itemInfo;
        }

        /// <summary>
        /// Add the item to the item collection where it fits.
        /// </summary>
        /// <param name="itemInfo">The item info to add.</param>
        /// <param name="stackTarget">The item stack where it should be added.</param>
        /// <param name="notifyAdd">Notify that the item was added?</param>
        /// <returns></returns>
        protected override ItemInfo AddInternal(ItemInfo itemInfo, ItemStack stackTarget, bool notifyAdd = true)
        {
            if (m_AddingToCollection == null) {
                RejectItem(itemInfo, itemInfo);

                if (m_ReturnRealAddedItemAmount) { return (ItemInfo) (0, itemInfo); }

                return itemInfo;
            }

            var itemInfoToAdd = new ItemInfo(itemInfo.ItemAmount);
            var addedItemInfo = m_AddingToCollection.AddItem(itemInfoToAdd);

            if (addedItemInfo.Amount < itemInfo.Amount) {
                var amountNotAdded = itemInfo.Amount - addedItemInfo.Amount;
                var itemInfoRejected = (ItemInfo) (amountNotAdded, itemInfo);

                RejectItem(itemInfo, itemInfoRejected);
            }

            if (m_ReturnRealAddedItemAmount) { return addedItemInfo; } else {
                return (ItemInfo) (itemInfo.Amount, addedItemInfo);
            }
        }

        /// <summary>
        /// Reject the item when it cannot be added to the item collection.
        /// </summary>
        /// <param name="itemInfo">The original item info that was tried to be added.</param>
        /// <param name="itemInfoRejected">The item info, with the amount that was rejected</param>
        protected virtual void RejectItem(ItemInfo itemInfo, ItemInfo itemInfoRejected)
        {
            if (m_OverflowBackToOrigin && itemInfo.ItemCollection != null && itemInfo.ItemCollection != this) {
                var returnedItemInfo = itemInfo.ItemCollection.AddItem(itemInfoRejected);
            } else {
                Debug.Log("Rejected Item " + itemInfoRejected);
                m_RejectedItemActions?.UseAllItemActions(itemInfoRejected, m_Inventory.ItemUser);
            }

            EventHandler.ExecuteEvent<ItemInfo>(m_Inventory, EventNames.c_Inventory_OnRejected_ItemInfo,
                itemInfoRejected);
        }

        /// <summary>
        /// Remove the item.
        /// </summary>
        /// <param name="itemInfo">THe item to remove.</param>
        /// <returns>The item removed.</returns>
        protected override ItemInfo RemoveInternal(ItemInfo itemInfo)
        {
            var amountToRemove = itemInfo.Amount;
            var AmountRemoved = 0;
            ItemInfo itemInfoRemoved = new ItemInfo(itemInfo.ItemAmount);
            for (int i = 0; i < m_ItemCollectionNames.Count; i++) {
                var itemCollection = m_Inventory.GetItemCollection(m_ItemCollectionNames[i]);
                itemInfoRemoved = itemCollection.RemoveItem((amountToRemove, itemInfo));
                AmountRemoved += itemInfoRemoved.Amount;
                amountToRemove -= itemInfoRemoved.Amount;

                if (amountToRemove <= 0) { return (itemInfo.Amount, itemInfoRemoved); }
            }

            return (AmountRemoved, itemInfoRemoved);
        }

        /// <summary>
        /// Get the item info in the linked item collections.
        /// </summary>
        /// <param name="item">The item to get.</param>
        /// <returns>The item info.</returns>
        public override ItemInfo? GetItemInfo(Item item)
        {
            for (int i = 0; i < m_ItemCollectionNames.Count; i++) {
                var itemCollection = m_Inventory.GetItemCollection(m_ItemCollectionNames[i]);

                var itemInfo = itemCollection.GetItemInfo(item);

                if (itemInfo.HasValue) { return itemInfo; }
            }

            return null;
        }
    }
}