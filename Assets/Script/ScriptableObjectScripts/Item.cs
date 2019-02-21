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
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject {
	public bool weapon, tool, block, material;

	// Within all items
	public string id;
	public short total;
	public Sprite[] sprArr;
	public Item() {
		id = "";
		total = 0;
		sprArr = null;
	}
	public Item(string id, short total, Sprite[] sprArr) {
		this.id = id;
		this.total = total;
		this.sprArr = sprArr;
	}

	// Functions for NaturalMaterial class:
	// Functions for Block class:
	// Functions for Tool class:
	public virtual void mine() {
		Debug.Log("N/A");
	}
	public virtual void chop() {
		Debug.Log("N/A");
	}
	// Functions for Weapon class:
	public virtual void attack() {
		Debug.Log("N/A");
	}

	public virtual void toString() {
		Debug.Log("id=" + id + "|total=" + total);
	}
}

// Note on Inheritance:
//	Inheritance in c# is very similar to c++
