using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.UltimateCharacterController.Items;
using Opsive.UltimateCharacterController.Items.Actions;
using Opsive.UltimateCharacterController.Traits;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrushers.UCCSupport
{

    /// <summary>
    /// Saves an Opsive Ultimate Character Controller's position, attributes, and/or inventory.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Common/Save System/Opsive/UCC Saver")]
    [RequireComponent(typeof(UltimateCharacterLocomotion))]
    public class UCCSaver : Saver
    {
        public bool savePosition = true;
        public bool saveAttributes = true;
        public bool saveInventory = true;
        public bool debug = false;

        [Serializable]
        public class Data // Holds the character's save data.
        {
            public List<PositionData> positions = new List<PositionData>();
            public List<float> attributes = new List<float>();
            public List<ItemData> items = new List<ItemData>();
        }

        [Serializable]
        public class PositionData // Holds save data for a position in a specific scene.
        {
            public int scene;
            public Vector3 position;
            public Quaternion rotation;

            public PositionData(int _scene, Vector3 _position, Quaternion _rotation)
            {
                scene = _scene;
                position = _position;
                rotation = _rotation;
            }
        }

        [Serializable]
        public class ItemData // Holds the save data for an item in the character's inventory.
        {
            public int itemID;
            public int slot;
            public float count = 0;
            public bool equipped = false;
            public List<ItemActionData> itemActionData = new List<ItemActionData>();
        }

        [Serializable]
        public class ItemActionData // Holds the save data for an ItemAction.
        {
            public int id;
            public float count = 0; // extra not yet in clip
            public float consumableCount = 0; // in clip

            public ItemActionData() { }
            public ItemActionData(int _id, float _count, float _consumableCount)
            {
                id = _id;
                count = _count;
                consumableCount = _consumableCount;
            }
        }

        protected Data m_data = new Data();
        protected Data data { get { return m_data; } set { m_data = value; } }

        public override string RecordData()
        {
            if (data == null) data = new Data();

            // Save position:
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            var found = false;
            for (int i = 0; i < data.positions.Count; i++)
            {
                if (data.positions[i].scene == currentScene)
                {
                    found = true;
                    data.positions[i].position = transform.position;
                    data.positions[i].rotation = transform.rotation;
                    break;
                }
            }
            if (!found)
            {
                data.positions.Add(new PositionData(currentScene, transform.position, transform.rotation));
            }

            // Save attributes:
            if (saveAttributes)
            {
                data.attributes.Clear();
                var attributeManager = GetComponent<AttributeManager>();
                if (attributeManager == null)
                {
                    if (Debug.isDebugBuild) Debug.LogWarning("UCC Saver can't save attributes. " + name + " doesn't have an Attribute Manager.", this);
                }
                else
                {
                    for (int i = 0; i < attributeManager.Attributes.Length; i++)
                    {
                        data.attributes.Add(attributeManager.Attributes[i].Value);
                    }
                }
            }

            // Save inventory:
            if (saveInventory)
            {
                data.items.Clear();
                var inventory = GetComponent<InventoryBase>();
                if (inventory == null)
                {
                    if (Debug.isDebugBuild) Debug.LogWarning("UCC Saver can't save inventory. " + name + " doesn't have an Inventory component.", this);
                }
                else
                {
                    // Record equipped items:
                    var equipped = new ItemType[inventory.SlotCount];
                    for (int i = 0; i < inventory.SlotCount; i++)
                    {
                        var equippedItem = inventory.GetItem(i);
                        equipped[i] = (equippedItem != null) ? equippedItem.ItemType : null;
                    }

                    // Save items:
                    var items = inventory.GetAllItems();
                    for (int i = 0; i < items.Count; i++)
                    {
                        var item = items[i];
                        if (item == null || item.ItemType == null) continue;
                        var itemCount = inventory.GetItemTypeCount(item.ItemType);
                        if (itemCount <= 0) continue;
                        var isDuplicateItem = data.items.Find(x => x.itemID == item.ItemType.ID) != null;
                        var itemData = new ItemData();
                        itemData.itemID = item.ItemType.ID;
                        itemData.slot = item.SlotID;
                        itemData.count = 1;
                        itemData.equipped = item.ItemType == equipped[item.SlotID];
                        for (int j = 0; j < item.ItemActions.Length; j++)
                        {
                            var usableItem = item.ItemActions[j] as UsableItem;
                            if (usableItem == null) continue;
                            var itemActionData = new ItemActionData();
                            itemData.itemActionData.Add(itemActionData);
                            itemActionData.id = usableItem.ID;
                            itemActionData.count = isDuplicateItem ? 0 : inventory.GetItemTypeCount(usableItem.GetConsumableItemType());
                            itemActionData.consumableCount = usableItem.GetConsumableItemTypeCount();
                        }
                        data.items.Add(itemData);
                    }
                }
            }

            if (debug) Debug.Log(JsonUtility.ToJson(data, true)); // [DEBUG]

            var s = SaveSystem.Serialize(data);
            if (debug) Debug.Log("UCC Saver on " + name + " saving: " + s, this);
            return s;
        }

        public override void ApplyData(string s)
        {
            if (string.IsNullOrEmpty(s)) return;
            var newData = SaveSystem.Deserialize<Data>(s);
            if (newData == null)
            {
                if (Debug.isDebugBuild) Debug.LogWarning("UCC Saver on " + name + " received invalid data. Can't apply: " + s, this);
                return;
            }
            data = newData;
            var character = GetComponent<UltimateCharacterLocomotion>();

            // Restore position:
            if (savePosition)
            {
                if (CompareTag("Player") && SaveSystem.playerSpawnpoint != null)
                {
                    if (debug) Debug.Log("UCC Saver on " + name + " moving character to spawnpoint " + SaveSystem.playerSpawnpoint, this);
                    character.SetPositionAndRotation(SaveSystem.playerSpawnpoint.transform.position, SaveSystem.playerSpawnpoint.transform.rotation);
                }
                else
                {
                    var currentScene = SceneManager.GetActiveScene().buildIndex;
                    for (int i = 0; i < data.positions.Count; i++)
                    {
                        if (data.positions[i].scene == currentScene)
                        {
                            if (debug) Debug.Log("UCC Saver on " + name + " (tag=" + tag + ") restoring saved position " + data.positions[i].position, this);
                            character.SetPositionAndRotation(data.positions[i].position, data.positions[i].rotation);
                            break;
                        }
                    }
                }
            }

            // Restore attributes:
            if (saveAttributes)
            {
                var attributeManager = GetComponent<AttributeManager>();
                if (attributeManager == null)
                {
                    if (Debug.isDebugBuild) Debug.LogWarning("UCC Saver can't load attributes. " + name + " doesn't have an Attribute Manager.", this);
                }
                else
                {
                    if (debug) Debug.Log("UCC Saver on " + name + " restoring attributes", this);
                    var count = Mathf.Min(attributeManager.Attributes.Length, data.attributes.Count);
                    for (int i = 0; i < count; i++)
                    {
                        attributeManager.Attributes[i].Value = data.attributes[i];
                    }
                }
            }

            // Restore inventory:
            if (saveInventory)
            {
                var inventory = GetComponent<InventoryBase>();
                if (inventory == null)
                {
                    if (Debug.isDebugBuild) Debug.LogWarning("UCC Saver can't load inventory. " + name + " doesn't have an Inventory component.", this);
                }
                else
                {
                    // Clear inventory:
                    var items = inventory.GetAllItems();
                    for (int i = 0; i < items.Count; i++)
                    {
                        var item = items[i];
                        if (item != null && item.ItemType != null)
                        {
                            for (int j = 0; j < item.ItemActions.Length; j++)
                            {
                                var usableItem = item.ItemActions[j] as UsableItem;
                                if (usableItem != null)
                                {
                                    var consumableItemType = usableItem.GetConsumableItemType();
                                    if (consumableItemType != null)
                                    {
                                        var count = (int)inventory.GetItemTypeCount(consumableItemType);
                                        if (count > 0)
                                        {
                                            inventory.RemoveItem(consumableItemType, count, false);
                                        }
                                    }
                                }
                            }
                            inventory.RemoveItem(item.ItemType, item.SlotID, false);
                        }
                    }

                    var itemCollection = UCCUtility.FindItemCollection(character.gameObject);
                    if (itemCollection == null)
                    {
                        Debug.LogError("Error: Unable to find ItemCollection.");
                        return;
                    }

                    // Add saved items: (restore equipped items last so they end up equipped)
                    HashSet<ItemType> alreadyAddedItemTypes = new HashSet<ItemType>();
                    RestoreItems(false, itemCollection, inventory, alreadyAddedItemTypes);
                    RestoreItems(true, itemCollection, inventory, alreadyAddedItemTypes);
                }
            }
        }

        private void RestoreItems(bool restoreEquipped, ItemCollection itemCollection, InventoryBase inventory, HashSet<ItemType> alreadyAddedItemTypes)
        {
            for (int i = 0; i < data.items.Count; i++)
            {
                var itemData = data.items[i];
                if (itemData.count == 0) continue;
                if (itemData.equipped != restoreEquipped) continue;
                var itemType = UCCUtility.GetItemType(itemCollection, itemData.itemID);
                inventory.PickupItemType(itemType, itemData.count, itemData.slot, true, restoreEquipped);
                var item = inventory.GetItem(itemData.slot, itemType);
                if (item == null) continue;
                for (int j = 0; j < item.ItemActions.Length; j++)
                {
                    var usableItem = item.ItemActions[j] as UsableItem;
                    if (usableItem == null) continue;

                    for (int k = 0; k < itemData.itemActionData.Count; ++k)
                    {
                        if (usableItem.ID != itemData.itemActionData[k].id) continue;

#if ULTIMATE_CHARACTER_CONTROLLER_SHOOTER
                        var shootableWeapon = usableItem as ShootableWeapon;
                        if (shootableWeapon != null)
                        {
                            // Temporarily fill clip so inventory.PickupItemType doesn't auto-reload:
                            usableItem.SetConsumableItemTypeCount(shootableWeapon.ClipSize);
                        }
#endif
                        if (debug) Debug.Log("UCC Saver on " + name + " restoring item: " + usableItem.name +
                            " (" + itemData.itemActionData[k].consumableCount + "/" + itemData.itemActionData[k].count + ")", this);
                        var consumableItemType = usableItem.GetConsumableItemType();
                        if (itemData.itemActionData[k].count > 0 && !alreadyAddedItemTypes.Contains(consumableItemType))
                        {
                            inventory.PickupItemType(consumableItemType, itemData.itemActionData[k].count, -1, true, false);
                            alreadyAddedItemTypes.Add(consumableItemType);
                        }
                        usableItem.SetConsumableItemTypeCount(itemData.itemActionData[k].consumableCount);
                    }
                }
            }
        }
    }
}
