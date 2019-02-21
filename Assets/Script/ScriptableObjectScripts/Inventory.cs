using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The inventory is simply a list of Item scriptable objects
//----------------------------------------------------------
// Casting rules:
//  - Only need to cast an inventory element to a specific class when
//  the variable/function being accessed is not within the Item class
//  - This is a bit of a hassle but it'll do for now
//  - This means that when the Add(Item i) function is called, it is important
//  that you cast the object to its specific class
//      - ex) Say I want to add a tool to the inventory:
//          - inv.Add((Tool)toolToBeAdded)
//----------------------------------------------------------
// So why wrap our inventory in a class?
//  We could just skip this class implementation and put
//      List<Item> inv = new List<Item>();
//  inside Builder.cs but then our inventory would not be a scriptable object
//  With our inventory wrapped inside a class that derives from ScriptableObject class
//  we can actually see that the inventory is saved in our assets folder even whent the game is closed
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class Inventory : ScriptableObject {
    public List<Item> inv = new List<Item>();
    public int currentItem;
    //--------------------------------------------------------------------------
    // Inventory functions
    public void cycleUp() {
        if(currentItem < inv.Count - 1) {
            currentItem++;
        }
        else {
            currentItem = 0;
        }
        Debug.Log(currentItem);
    }

    public void cycleDown() {
        if(currentItem > 0) {
            currentItem--;
        }
        else {
            currentItem = inv.Count - 1;
        }
        Debug.Log(currentItem);
    }

    public void add(Item i) {
        inv.Add(i);
    }

    public void reset() {
        currentItem = 0;
    }
    //--------------------------------------------------------------------------
    // Block related functions
    public bool isBlock() {
        return inv[currentItem].block;
    }
    //--------------------------------------------------------------------------
    // Tool/Weapon related functions
    public bool checkItemFunction(short function) {
        if(inv[currentItem].tool) {
            if(((Tool)inv[currentItem]).function == function) {
                return true;
            }
        }
        return false;
    }

    public short getToolPower() {
        return ((Tool)inv[currentItem]).power;
    }
}
