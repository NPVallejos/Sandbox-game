using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose:
// This provides the most generic class for all items
// All functions for other subclasses will go here
// **NOTE**
//   	The reason each function isn't abstract is because then they will have
// 		to be implemented in all subclasses - I only want to implement them
// 		in specific classes (i.e. attack would have to be implemented in Block
// 	    class, NaturalMaterial class, etc.)
//----------------------------------------------------
// Rules:
// An item can be:
//   - Weapon & Tool & Material
// An item cannot be:
//   - Weapon/Tool & Block
// Most variables will be left empty
//----------------------------------------------------
// Logic:
// Most tools ARE weapons:
//	 - An Axe is a Tool & Weapon
// However, there do exist tools that aren't weapons:
// 	 - A paint brush is a Tool but not a Weapon
[System.Serializable]
public class Item {
	public bool weapon, tool, block, material;

	// Within all items
	public string id;
	public short total;
	public Sprite[] sp_arr;
	public Item() {
		id = "";
		total = 0;
		sp_arr = null;
	}
	public Item(string id, short total, Sprite[] sp_arr) {
		this.id = id;
		this.total = total;
		this.sp_arr = sp_arr;
	}

	// Functions for NaturalMaterial class:
	// Functions for Block class:
	// Functions for Tool class:
	// Functions for Weapon class:
	public virtual void attack() {
		Debug.Log("N/A");
	}
}

// Note on Inheritance:
//	Inheritance in c# is very similar to c++
