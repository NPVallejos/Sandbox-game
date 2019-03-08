using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class holds information for any type of block
// It derives ultimately from the ScriptableObject class (See Item.cs)
// So it can be used to create block assets quickly in the Editor
// The Singleton.cs class is a MonoBehaviour script that holds Block
// scriptableobject's - see Singleton.cs for more info
[System.Serializable]
[CreateAssetMenu(fileName = "New Block", menuName = "Block")]
public class Block : NaturalMaterial {
	public short health;

	public bool dealDamage(short dmg) {
		if(health > 0) {
			health -= dmg;
		}
		if(health == 0) {
			return false;
		}
		return true;
	}

	public Block() {
	//: base() {
		health = 0;
	}
	public Block(string id, short total, Sprite[] sp_arr, short health)
	: base(id, total, sp_arr) {
		this.health = health;
	}
	public Block(Block other)
	: base(other.id, other.total, other.sprArr) {
		this.health = other.health;
	}
	public override void toString() {
		base.toString();
		Debug.Log("health=" + health);
	}
}
