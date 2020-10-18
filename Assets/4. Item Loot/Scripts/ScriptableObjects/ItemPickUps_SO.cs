using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypeDefinitions { HEALTH, WEALTH, MANA, WEAPON, ARMOR, BUFF, EMPTY };
public enum ItemArmorSubType { None, Head, Chest, Hands, Legs, Boots };

[CreateAssetMenu(fileName = "New Item Scriptable Object", menuName = "Spawnable Item/New Pick-up Scriptable Object", order = 1)]
public class ItemPickUps_SO : ScriptableObject
{
    /*The unique name of this item*/
    public string itemName = "New item";

    public ItemTypeDefinitions itemType = ItemTypeDefinitions.HEALTH;
    public ItemArmorSubType itemArmorSubType = ItemArmorSubType.None;
    public int itemAmount = 0;

    /*The probability of this item to drop*/
    public int spawnChanceWeight = 0;

    public Rigidbody itemSpawnObject = null;
    public Rigidbody weaponSlotObject = null;

    /*The icon that represents the image of the item while in inventory space*/
    public Sprite itemIcon = null;

    /*What material the item will use*/
    public Material itemMaterial = null;

    public bool isEquiped = false;
    public bool isInteractable = false;
    public bool isStorable = false;
    public bool isUnique = false;
    public bool isIndestructable = false;
    public bool isQuestItem = false;
    public bool isStackable = false;
    public bool destroyOnUse = false;
    public float itemWeight = 0f;
}