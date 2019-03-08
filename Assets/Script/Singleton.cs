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
// Why do we need to Instantiate blockObj with blockObj?
//	- blockObj is simply a reference to the actual scriptableObject
//	for a particular block
//	- Because we want to manipulate the data held inside Block.cs we need to create
//	a copy of this scriptable object
//	- If we do not create this copy, then whenever we manipulate the data of a single
//	block in-game it will affect the data for ALL other similar blocks
public class Singleton : MonoBehaviour {
	public Block blockObj;
	public SpriteRenderer sprRend;
	public BoxCollider2D col2D;
	public bool destroyed;


	void Awake() {
		sprRend = GetComponent<SpriteRenderer>();
		col2D = GetComponent<BoxCollider2D>();
		blockObj = Instantiate(blockObj);
	}

	void Start() {
		sprRend.sprite = blockObj.sprArr[0];
		col2D.size = sprRend.sprite.bounds.size;
		destroyed = false;
	}

	public void dealDamage(short dmg) {
		if(!destroyed && !blockObj.dealDamage(dmg)) {
			Destroy();
		}
	}

	public void Destroy() {
		destroyed = true;
		sprRend.sprite = blockObj.sprArr[1];
		col2D.size = sprRend.sprite.bounds.size;
	}
}
