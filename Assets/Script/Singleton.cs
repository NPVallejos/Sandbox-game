using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main Purpose:
//  - Acts as an interface between the Builder.cs class and the Block.cs
// 	class
//	- This class manipulates the actual Block game objects in the game whereas
//	the Block.cs class acts as a skeleton class for all different types of
//	blocks and stores the actual item data
//------------------------------------------------------------------------------
// Other Important Information:
// 	- This script is attached to all block prefabs which are placed on the map
//	(i.e. the prefabs that are used to generate the map)
//  - Thus, this script holds the actual Block scriptable object
//------------------------------------------------------------------------------
// Why do we need to Instantiate scriptObj with scriptObj?
//	- scriptObj is simply a reference to the actual scriptableObject
//	for a particular block
//	- Because we want to manipulate the data held inside Block.cs we need to create
//	a copy of this scriptable object
//	- If we do not create this copy, then whenever we manipulate the data of a single
//	block in-game it will affect the data for ALL other similar blocks
public class Singleton : MonoBehaviour {
	public Block scriptObj;
	public SpriteRenderer sprRend;
	public BoxCollider2D col2D;

	void Awake() {
		sprRend = GetComponent<SpriteRenderer>();
		col2D = GetComponent<BoxCollider2D>();
		scriptObj = Instantiate(scriptObj);
	}

	void Start() {
		sprRend.sprite = scriptObj.sprArr[0];
		col2D.size = sprRend.sprite.bounds.size;
	}

	public void dealDamage(short dmg) {
		if(!scriptObj.dealDamage(dmg)) {
			Destroy();
		}
	}

	public void Destroy() {
		sprRend.sprite = scriptObj.sprArr[1];
		col2D.size = sprRend.sprite.bounds.size;
	}
}
