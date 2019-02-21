using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Functions:
// 	- mine/dig = pic axe = 0
//	- chop = axe = 1
// --------------------------
// Tiers are based on the type of material used to make the object:
//  - 1 = wood tier
//  - 2 = stone tier
//	- 3 = copper tier
//  - 4 = iron tier
// 	- 5 = silver tier
//	- 6 = gold tier
//	- 7 = diamond tier
//	- etc.
// For now we are going to start small and only handle natural materials
// The idea is that each tier can involve several natural materials
// These materials can also be other things like:
// 	- soul
//	- cloud
//	- lava
//	- meteor
//	- sand
//  - water
//	- dirt
//	- robot parts
//	- lizard legs
//  - etc.
// Also, tools may not even need to specify a tier so maybe they shouldnt be here
// Miscellaneous stuff like a paint brush
// However, fishing rods (for example) should be tiered so lets leave this here
[System.Serializable]
[CreateAssetMenu(fileName = "New Tool", menuName = "Tool")]
public class Tool : NaturalMaterial {
	public short function;
	public short tier;
	public short power;
	public Tool() {
	//: base() {
		function = 0;
		tier = 0;
	}
	public Tool(string id, short total, Sprite[] sp_arr, short function, short tier) 
	: base(id, total, sp_arr) {
		this.function = function;
		this.tier = tier;
	}
	public override void mine() {
		Debug.Log("Mined!");
	}
	public override void chop() {
		Debug.Log("Chopped!");
	}
	public override void toString() {
		base.toString();
		Debug.Log("function=" + function + "|tier=" + tier);
	}
}
