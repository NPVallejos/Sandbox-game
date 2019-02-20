using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class Inventory : ScriptableObject {
    public List<Item> inventory = new List<Item>();

    public void Add(Item i) { 
        inventory.Add(i);
    }

    public Item Get(short index) {
        return inventory[index];
    }
}
