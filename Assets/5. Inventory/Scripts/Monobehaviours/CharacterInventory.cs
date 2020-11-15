using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
    #region Variable Declarations
    public static CharacterInventory instance;

    public CharacterStats charStats;
    GameObject foundStats;

    public Image[] hotBarDisplayHolders = new Image[4];
    public GameObject InventoryDisplayHolder;
    public Image[] inventoryDisplaySlots = new Image[30];

    int inventoryItemCap = 20;
    int idCount = 1;
    bool addedItem = true;

    public Dictionary<int, InventoryEntry> itemsInInventory = new Dictionary<int, InventoryEntry>();
    public InventoryEntry itemEntry;
    #endregion

    #region Initializations
    void Start()
    {
        instance = this;
        itemEntry = new InventoryEntry(0, null, null);
        itemsInInventory.Clear();

        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();

        foundStats = GameObject.FindGameObjectWithTag("Player");
        charStats = foundStats.GetComponent<CharacterStats>();
    }
    #endregion

    void Update()
    {
        #region Watch for Hotbar Keypresses - Called by Character Controller Later
        //Checking for a hotbar key to be pressed
        // TODO: Add some keypresses
        #endregion

        //Check to see if the item has already been added - Prevent duplicate adds for 1 item
        if (!addedItem)
        {
            TryPickUp();
        }
    }

    // TODO: Add functions
    
    public void StoreItem(ItemPickUp itemToStore)
    {
        addedItem = false;

        if((charStats.characterDefinition.currentEncumbrance+
            itemToStore.itemDefinition.itemWeight)>= charStats.characterDefinition.maxEncumbrance) //check for the encumbrance before picking
        {//if the character can grab it, do it
            itemEntry.invEntry = itemToStore; //register a new inv entry
            itemEntry.stackSize = 1; //it occupies 1 stack
            itemEntry.hbSprite = itemToStore.itemDefinition.itemIcon; //update slot sprite

            itemToStore.gameObject.SetActive(false); //deactivate the item from the floor, it's in our pocket now :)
        }
        //else nothing
    }

    void TryPickUp()
    {
        bool itsInInv = true;
        
        //Check if the item to be stored was properly inserted into the inventory
        if (itemEntry.invEntry)
        {
            if(itemsInInventory.Count == 0) //Do we have any items in the inventory already? - if not, add the item
            {
                addedItem = AddItemToInv(addedItem);
            }
            else //We have stuff inside so...
            {
                if (itemEntry.invEntry.itemDefinition.isStackable) //Can I stack what I picked up?
                {
                    foreach(KeyValuePair<int,InventoryEntry> ie in itemsInInventory)
                    {
                        //Does this stackable item exist my inventory?
                        if(itemEntry.invEntry.itemDefinition == ie.Value.invEntry.itemDefinition)
                        {
                            //Add 1 to the stack and destroy new instance
                            ie.Value.stackSize += 1;
                            AddItemToHotBar(ie.Value);
                            itsInInv = true;
                            Destroy(itemEntry.invEntry.gameObject);
                            break;
                        }
                        //If item does not exist already in inventory then continue here
                        else
                        {
                            itsInInv = false;
                        }
                    }
                }
                //If Item is not stackable then continue here
                else
                {
                    itsInInv = false;

                    //If no space and item is not stackable - say inventory full
                    if (itemsInInventory.Count == inventoryItemCap)
                    {
                        itemEntry.invEntry.gameObject.SetActive(true);
                        Debug.Log("Inventory is Full");
                    }
                }
                //Check if there is space in inventory - if yes, continue here
                if (!itsInInv)
                {
                    addedItem = AddItemToInv(addedItem);
                    itsInInv = true;
                }
            }
        }
        //else do nothing
    }

    bool AddItemToInv(bool finishedAdding)
    {
        //We add into a specific count a new item that we picked up
        itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, Instantiate(itemEntry.invEntry),itemEntry.hbSprite));

        Destroy(itemEntry.invEntry.gameObject);

        FillInventoryDisplay();
        AddItemToHotBar(itemsInInventory[idCount]);

        idCount = IncreaseID(idCount);

        #region Reset itemEntry

        itemEntry.invEntry = null;
        itemEntry.stackSize = 0;
        itemEntry.hbSprite = null;

        #endregion

        finishedAdding = true;

        return finishedAdding;
    }

    int IncreaseID(int currentID)
    {
        int newID = 1;

        for (int itemCount = 1; itemCount <= itemsInInventory.Count; itemCount++)
        {
            if (itemsInInventory.ContainsKey(newID))
            {
                newID += 1;
            }
            else return newID;
        }

        return newID;
    }

    private void AddItemToHotBar(InventoryEntry itemForHotBar)
    {

    }

    void DisplayInventory()
    {

    }

    void FillInventoryDisplay()
    {

    }

    public void TriggerItemUse(int itemToUseID)
    {

    }
}