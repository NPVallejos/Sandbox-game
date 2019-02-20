using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block : NaturalMaterial {
	public short health;
	public Block()
	: base() {
		health = 0;
	}
	public Block(string id, short total, Sprite[] sp_arr, short health)
	: base(id, total, sp_arr) {
		this.health = health;
	}
	// public new string name;
	// public short health;
	// public short total;
	// public Sprite[] sp_arr;
	// public BoxCollider2D col2D;
	//
	// public void damage(short amount) {
	// 	if(health > 0)
	// 		health--;
	// 	if(health <= 0)
	// 		; // 'destroy' the object
	// }
}
